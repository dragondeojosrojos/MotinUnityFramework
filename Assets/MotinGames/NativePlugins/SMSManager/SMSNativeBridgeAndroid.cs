#if UNITY_ANDROID

using UnityEngine;
using System.Collections;
using System;

public class SMSNativeBridgeAndroid : ISMSNativeBridge {


	private AndroidJavaObject _SMSAndroidPlugin;
	private AndroidJavaObject SMSAndroidPlugin
	{
		get
		{
			if (_SMSAndroidPlugin == null)
			{
				_SMSAndroidPlugin = new AndroidJavaObject("com.motingames.SMSManager.SMSManager");
			}
			return _SMSAndroidPlugin;
		}
	}

	public SMSNativeBridgeAndroid()
	{

	}

	public void Initialize (string goName)
	{
		SMSAndroidPlugin.CallStatic("Initialize",goName);
	}

	public void StartSmsListening ()
	{
		SMSAndroidPlugin.CallStatic("StartSmsListening");
	}

	public void StopSmsListening ()
	{
		SMSAndroidPlugin.CallStatic("StopSmsListening");
	}

	public void SendSms (string number, string content)
	{
		SMSAndroidPlugin.CallStatic("SendSms",number,content);
	}
	
}

#endif
