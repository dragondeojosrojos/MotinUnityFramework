#if UNITY_ANDROID

using UnityEngine;
using System.Collections;


public class TwitterNativeBridgeAndroid : ITwitterNativeBridge {

	private AndroidJavaObject _TwitterAndroidPlugin;
	private AndroidJavaObject TwitterAndroidPlugin
	{
		get
		{
			if (_TwitterAndroidPlugin == null)
			{
				_TwitterAndroidPlugin = new AndroidJavaObject("com.motingames.twitter.TwitterManager");
			}
			return _TwitterAndroidPlugin;
		}
	}
	
	public TwitterNativeBridgeAndroid()
	{
		
	}
	
	public void Initialize (string goName, string publicKey, string secretKey)
	{
		TwitterAndroidPlugin.CallStatic("Initialize", goName,publicKey,secretKey);
	}
	
	public void Show (string message)
	{
		TwitterAndroidPlugin.CallStatic("Show",message);
	}
	
}

#endif