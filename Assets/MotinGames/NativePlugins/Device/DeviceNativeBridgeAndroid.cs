#if UNITY_ANDROID


using UnityEngine;
using System.Collections;


public class DeviceNativeBridgeAndroid : IDeviceNativeBridge {
	
	private AndroidJavaObject _DeviceAndroidPlugin;
	private AndroidJavaObject DeviceAndroidPlugin
	{
		get
		{
			if (_DeviceAndroidPlugin == null)
			{
				_DeviceAndroidPlugin = new AndroidJavaObject("com.motingames.device.DeviceManager");
			}
			return _DeviceAndroidPlugin;
		}
	}



	public DeviceNativeBridgeAndroid()
	{
		
	}

	public void Initialize(string goName)
	{
		DeviceAndroidPlugin.CallStatic("Initialize", goName);
	}

	public void ShowNotification (string message)
	{
		//Debug.Log ("DeviceNativeBridgeAndroid ShowLoading" );
		DeviceAndroidPlugin.CallStatic("ShowNotification", message);
	}
	public void ShowLoading (string message)
	{
		//Debug.Log ("DeviceNativeBridgeAndroid ShowLoading" );
		DeviceAndroidPlugin.CallStatic("ShowLoading", message);
	}
	
	public void DismissLoading ()
	{
		DeviceAndroidPlugin.CallStatic("DismissLoading");
	}
	public string GetNetworkName()
	{
		return DeviceAndroidPlugin.CallStatic<string>("GetNetworkName");
	}
	public string GetEmail()
	{
		return DeviceAndroidPlugin.CallStatic<string>("GetEmail");
	}
	public void ShowAccountPicker()
	{
		DeviceAndroidPlugin.CallStatic("ShowAccountPicker");
	}

	public void Vibrate(int time)
	{
		DeviceAndroidPlugin.CallStatic("Vibrate", time);
	}
}

#endif