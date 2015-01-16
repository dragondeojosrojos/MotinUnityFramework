#if UNITY_ANDROID

using UnityEngine;
using System.Collections;
using System;

public class PushNativeBridgeAndroid : IPushNativeBridge {
	
	
	private AndroidJavaObject _AndroidPlugin;
	private AndroidJavaObject AndroidPlugin
	{
		get
		{
			if (_AndroidPlugin == null)
			{
				_AndroidPlugin = new AndroidJavaObject("com.motingames.gcm.GcmManager");
			}
			return _AndroidPlugin;
		}
	}
	
	public PushNativeBridgeAndroid()
	{
		
	}
	
	public void Initialize (string goName,string data)
	{
		AndroidPlugin.CallStatic("Initialize",goName,data);
	}
	
	public void RegisterDevice()
	{
		AndroidPlugin.CallStatic("RegisterDevice");
	}
	

	
}

#endif
