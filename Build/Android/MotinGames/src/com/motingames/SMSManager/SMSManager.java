package com.motingames.SMSManager;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import org.json.JSONObject;

import com.motingames.device.DeviceManager;
import com.unity3d.player.UnityPlayerNativeActivity;

import android.app.Activity;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.telephony.SmsManager;
import android.telephony.SmsMessage;
import android.util.Log;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

public class SMSManager {
	static final String DEBUG_TAG = "SMSManager";
	private static final String ON_SMS_RECIEVED = "OnSmsRecieved";
	private static final String ON_SMS_SENT = "OnSmsSent";
	
	private static SMSManager sharedInstance = null;

	BroadcastReceiver smsSentReceiver, smsDeliveredReceiver;
	private IncomingSms incomingReciever = null;
	private String gameObjectName = "";

	public static SMSManager sharedManager() {
		if (sharedInstance == null) {
			sharedInstance = new SMSManager();
		}
		return sharedInstance;
	}

	//To call from unity
	public static void Initialize(String goName)
	{
		SMSManager.sharedManager().initialize(goName);
	}
	public static void StartSmsListening()
	{
		SMSManager.sharedManager().startSmsListening();
	}
	public static void StopSmsListening()
	{
		SMSManager.sharedManager().stopSmsListening();
	}
	public static void SendSms(String number,String content)
	{
		SMSManager.sharedManager().sendSms(number,content);
	}
	
	//Unity callbacks
	public void onRecievedCallback(String number,String content)
	{
		Map<String, Serializable> params = new HashMap<String, Serializable>();
		params.put("number",number);
		params.put("content", content);
		String message = new JSONObject(params).toString();
		UnityPlayer.UnitySendMessage(gameObjectName, ON_SMS_RECIEVED, message);
	}
	public void onSMSSentCallback(int resultCode)
	{
		Map<String, Serializable> params = new HashMap<String, Serializable>();
		params.put("resultCode",resultCode);
		String message = new JSONObject(params).toString();
		
		UnityPlayer.UnitySendMessage(gameObjectName, ON_SMS_SENT, message);
	}
	
	//Class methods
	public void initialize(String goName) {
		gameObjectName = goName;
	}

	public void startSmsListening() {
		if (incomingReciever != null)
			return;

		incomingReciever = new IncomingSms();

		IntentFilter iFilter = new IntentFilter("android.provider.Telephony.SMS_RECEIVED");
		iFilter.setPriority(999);
		UnityPlayer.currentActivity.registerReceiver(incomingReciever,
				iFilter);

		// Toast.makeText(this, "Registered broadcast receiver",
		// Toast.LENGTH_SHORT)
		// .show();
	}

	public void stopSmsListening() {
		if (incomingReciever == null)
			return;

		UnityPlayer.currentActivity.unregisterReceiver(incomingReciever);
		incomingReciever = null;

	}

	

	public void sendSms(String number,String content)
	{
		
		smsSentReceiver=new BroadcastReceiver() {
            
            @Override
            public void onReceive(Context arg0, Intent arg1) {
                // TODO Auto-generated method stub
            	Log.d(DEBUG_TAG,"REsult Code " + getResultCode());
            	SMSManager.sharedManager().onSMSSentCallback(getResultCode());
            	
            	/*
                switch (getResultCode()) {
                case Activity.RESULT_OK:
                	Log.d(DEBUG_TAG, "SMS has been sent");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "SMS has been sent", Toast.LENGTH_SHORT).show();
                    break;
                case SmsManager.RESULT_ERROR_GENERIC_FAILURE:
                	Log.d(DEBUG_TAG, "Generic Failure");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "Generic Failure", Toast.LENGTH_SHORT).show();
                    break;
                case SmsManager.RESULT_ERROR_NO_SERVICE:
                	Log.d(DEBUG_TAG, "No Service");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "No Service", Toast.LENGTH_SHORT).show();
                    break;
                case SmsManager.RESULT_ERROR_NULL_PDU:
                	Log.d(DEBUG_TAG, "Null PDU");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "Null PDU", Toast.LENGTH_SHORT).show();
                    break;
                case SmsManager.RESULT_ERROR_RADIO_OFF:
                	Log.d(DEBUG_TAG, "Radio Off");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "Radio Off", Toast.LENGTH_SHORT).show();
                    break;
                case 5:
                	
                	break;
                default:
                   
                    break;
                }
               */
                
                
            }
        };
        smsDeliveredReceiver=new BroadcastReceiver() {
            
            @Override
            public void onReceive(Context arg0, Intent arg1) {
                // TODO Auto-generated method stub
                switch(getResultCode()) {
                case Activity.RESULT_OK:
                	Log.d(DEBUG_TAG,"SMS Delivered");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "SMS Delivered", Toast.LENGTH_SHORT).show();
                    break;
                case Activity.RESULT_CANCELED:
                	Log.d(DEBUG_TAG,"SMS not delivered");
                    Toast.makeText(UnityPlayer.currentActivity.getBaseContext(), "SMS not delivered", Toast.LENGTH_SHORT).show();
                    break;
                }
            }
        };
		
        UnityPlayer.currentActivity.registerReceiver(smsSentReceiver, new IntentFilter("SMS_SENT"));
        UnityPlayer.currentActivity.registerReceiver(smsDeliveredReceiver, new IntentFilter("SMS_DELIVERED"));
		
        
        PendingIntent piSent=PendingIntent.getBroadcast(UnityPlayer.currentActivity, 0, new Intent("SMS_SENT"), 0);
        PendingIntent piDelivered=PendingIntent.getBroadcast(UnityPlayer.currentActivity, 0, new Intent("SMS_DELIVERED"), 0);
        
        SmsManager.getDefault().sendTextMessage(number, null, content, piSent, piDelivered);
		
	}
	
	
	
	//internal class
	public class IncomingSms extends BroadcastReceiver {

		// Get the object of SmsManager
		final SmsManager sms = SmsManager.getDefault();

		public void onReceive(Context context, Intent intent) {

			// Retrieves a map of extended data from the intent.
			final Bundle bundle = intent.getExtras();

			try {

				if (bundle != null) {

					final Object[] pdusObj = (Object[]) bundle.get("pdus");

					for (int i = 0; i < pdusObj.length; i++) {

						SmsMessage currentMessage = SmsMessage
								.createFromPdu((byte[]) pdusObj[i]);
						String phoneNumber = currentMessage
								.getDisplayOriginatingAddress();

						String senderNum = phoneNumber;
						String message = currentMessage.getDisplayMessageBody();

						//Log.i("SmsReceiver", "senderNum: " + senderNum
						//		+ "; message: " + message);

						DeviceManager.sharedManager().UnityLog("senderNum: " + senderNum
								+ "; message: " + message);
						
						SMSManager.sharedManager().onRecievedCallback(
								senderNum, message);

						// Show Alert
						// int duration = Toast.LENGTH_LONG;
						// Toast toast = Toast.makeText(context,
						// "senderNum: "+ senderNum + ", message: " + message,
						// duration);
						// toast.show();

					} // end for loop
				} // bundle is null

			} catch (Exception e) {
				DeviceManager.sharedManager().UnityLog("Exception smsReceiver" + e);
				Log.e("SmsReceiver", "Exception smsReceiver" + e);

			}
		}
	}

}
