package com.motingames.gcm;

import java.io.IOException;
import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import org.json.JSONObject;

import android.os.AsyncTask;
import android.util.Log;

import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.unity3d.player.UnityPlayer;

public class GcmManager {
	static final String DEBUG_TAG = "GcmManager";
	
	private static final int GCM_REG_RESULT_CODE_OK = 0;	
	private static final int GCM_REG_RESULT_CODE_ERROR = 1;	
	
	private static final String ON_GCM_REGISTERED = "OnRegisteredToService";	
	
	
	private static GcmManager sharedInstance = null;
	
	private String gameObjectName = "";
	private String senderId = "";

	public static GcmManager sharedManager() {
		if (sharedInstance == null) {
			sharedInstance = new GcmManager();
		}
		return sharedInstance;
	}

	//To call from unity
	public static void Initialize(String goName,String sender_id)
	{
		GcmManager.sharedManager().initialize(goName,sender_id);
	}
	
	public static void RegisterDevice()
	{
		GcmManager.sharedManager().registerInBackground();
	}
	
	
	//Unity callbacks
	private void onRegisteredToGCM(String jsonResult)
	{
		Log.d(DEBUG_TAG, "GcmManager::onRegisteredToGCM: " + jsonResult);
		UnityPlayer.UnitySendMessage(gameObjectName, ON_GCM_REGISTERED, jsonResult);
	}
	
	//Class methods
	public void initialize(String goName,String sender_id) {
		gameObjectName = goName;
		senderId = sender_id;
	}
	
	public void registerInBackground() {
		Log.d(DEBUG_TAG, "GcmManager::registerInBackground");
        new AsyncTask<Void, Void, String>() {
            @Override
            protected String doInBackground(Void... params) {
            	int resultCode =0;
                String data = "";
                try {
                    //if (gcm == null) {
                	GoogleCloudMessaging gcm = GoogleCloudMessaging.getInstance(UnityPlayer.currentActivity.getApplicationContext());
                    //}
                    String regid = gcm.register(senderId);
                    
                    resultCode = GCM_REG_RESULT_CODE_OK;
                    data = regid;
                    
                    Log.d(DEBUG_TAG, "Device registered, registration ID=" + regid);
                    //msg = "Device registered, registration ID=" + regid;

                    // You should send the registration ID to your server over HTTP, so it
                    // can use GCM/HTTP or CCS to send messages to your app.
                   // sendRegistrationIdToBackend(regid);

                    // For this demo: we don't need to send it because the device will send
                    // upstream messages to a server that echo back the message using the
                    // 'from' address in the message.

                    // Persist the regID - no need to register again.
                    //storeRegistrationId(context, regid);
                } catch (IOException ex) {
                	resultCode = GCM_REG_RESULT_CODE_ERROR;
                	data = "Error :" + ex.getMessage();
                    // If there is an error, don't just keep trying to register.
                    // Require the user to click a button again, or perform
                    // exponential back-off.
                }
                
                Map<String, Serializable> keyValues = new HashMap<String, Serializable>();
                keyValues.put("resultCode",resultCode);
                keyValues.put("data",data);
                return  (new JSONObject(keyValues)).toString();
            }

            @Override
            protected void onPostExecute(String jsonResult) {
            	onRegisteredToGCM(jsonResult);
            }
        }.execute(null, null, null);
    }

}
