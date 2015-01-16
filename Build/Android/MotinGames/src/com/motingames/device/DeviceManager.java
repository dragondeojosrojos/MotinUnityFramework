package com.motingames.device;




import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import android.os.Vibrator;
import org.json.JSONObject;

import com.google.android.gms.auth.GoogleAuthUtil;
import com.unity3d.player.UnityPlayer;

import android.accounts.Account;
import android.accounts.AccountManager;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.Window;
import android.widget.Toast;

public class DeviceManager {

	
	static final String DEBUG_TAG = "DeviceManager";
	private static final String UNITY_DEBUG_LOG = "DebugLog";
	private static final String UNITY_ON_ACCOUNT_PICKER_RESULT = "OnAccountPickerResult";
	
	private static DeviceManager sharedInstance = null;
	private String gameObjectName = "";
	ProgressDialog	progressDialog = null;
	String progressTempMessage ;
	String notificationTempMessage ;
	//AccountPickerActivity accountPicker = null;
			
	
	
	public static DeviceManager sharedManager() {
		if (sharedInstance == null) {
			sharedInstance = new DeviceManager();
		}
		return sharedInstance;
	}
	
	//To call from unity
	public static void Initialize(String goName)
	{
		DeviceManager.sharedManager().initialize(goName);
	}

	public static void ShowNotification(String message)
	{
		Log.d(DEBUG_TAG, "Show Notification "  + message);
		DeviceManager.sharedManager().showNotification(message);
	}
	public static void ShowLoading(String message)
	{
		Log.d(DEBUG_TAG, "Show Loading");
		DeviceManager.sharedManager().showLoading(message);
	}

	public static void DismissLoading()
	{
		Log.d(DEBUG_TAG, "dismissLoading");
		DeviceManager.sharedManager().dismissLoading();
	}
	
	public static void ShowAccountPicker()
	{
		Log.d(DEBUG_TAG, "Show Account Picker");
		DeviceManager.sharedManager().showAccountPicker();
	}
	
	public static String GetEmail()
	{
		return DeviceManager.sharedManager().getEmail();
	}
	
	public static String GetNetworkName()
	{
		return DeviceManager.sharedManager().getNetworkName();
	}
	public static void Vibrate(int time)
	{
		 DeviceManager.sharedManager().vibrate(time);
	}
	
	//Unity callbacks
	public void onAccountPickerResult(String email)
	{
		//if(accountPicker==null)
		//	Log.e(DEBUG_TAG, "Account Picker is Null");
		
		//accountPicker.FinishActivity();
		UnityPlayer.UnitySendMessage(gameObjectName, UNITY_ON_ACCOUNT_PICKER_RESULT, email);
	}
	
	//Class methods
	public void initialize(String goName) {
		gameObjectName = goName;
	}

	public void UnityLog(String log)
	{
		Map<String, Serializable> params = new HashMap<String, Serializable>();
		params.put("message",log);
		String message = new JSONObject(params).toString();
		UnityPlayer.UnitySendMessage(gameObjectName, UNITY_DEBUG_LOG, message);
	}
	
	public void showNotification(String message)
	{
		notificationTempMessage = message;
		UnityPlayer.currentActivity.runOnUiThread(new Runnable(){
		    public void run(){
		    	 Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), notificationTempMessage, Toast.LENGTH_SHORT).show();
		    	 notificationTempMessage = null;
		    }
		});
	}
	
	public void showLoading(String message)
	{
		progressTempMessage = message;

		UnityPlayer.currentActivity.runOnUiThread(new Runnable(){
		    public void run(){
		    	if(progressDialog==null)
				{
					progressDialog = new ProgressDialog(UnityPlayer.currentActivity);
				
				
					progressDialog.setMessage(progressTempMessage);
					progressDialog.setCancelable(false);
					progressDialog.setCanceledOnTouchOutside(false);
					progressDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		    		progressDialog.show();
				}
		    }
		});
		
	}
	public void dismissLoading()
	{
		if(progressDialog==null)
		{
			return;
		}
	
		progressDialog.dismiss();
		progressDialog = null;
	}
	
	public String getEmail()
	{
		Account[] accounts = AccountManager.get(UnityPlayer.currentActivity).getAccountsByType(GoogleAuthUtil.GOOGLE_ACCOUNT_TYPE);
		if(accounts == null || accounts.length ==0)
			return "";
		
		return accounts[0].name;
		
	}
	
	public String getNetworkName()
	{
		TelephonyManager manager = (TelephonyManager)UnityPlayer.currentActivity.getSystemService(Context.TELEPHONY_SERVICE);
		String carrierName = manager.getNetworkOperatorName();
		return carrierName;
	}
	
	public void showAccountPicker()
	{
		
		
		
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				//if(accountPicker!=null)
				//	Log.e(DEBUG_TAG, "Account opicker Already created");
				
				Intent newIntent = new Intent(UnityPlayer.currentActivity, AccountPickerActivity.class);
				UnityPlayer.currentActivity.startActivity(newIntent);
			}
		});
	}
	
	
	public void vibrate(int time)
	{
		Vibrator v = (Vibrator) UnityPlayer.currentActivity.getSystemService(Context.VIBRATOR_SERVICE);
		 // Vibrate for 500 milliseconds
		 v.vibrate(time);
	}

}
