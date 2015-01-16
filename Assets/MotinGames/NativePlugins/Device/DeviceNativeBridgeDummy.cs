using UnityEngine;
using System.Collections;


public class DeviceNativeBridgeDummy : IDeviceNativeBridge {
	
	
	public DeviceNativeBridgeDummy()
	{
		
	}

	public void Initialize(string goName)
	{

	}

	public void ShowNotification (string message)
	{
		Debug.Log(message);
	}
	public void ShowLoading (string message)
	{
		Debug.Log("DeviceNativeBridgeDummy ShowLoading " + message);
	}

	public void DismissLoading ()
	{
		Debug.Log("DeviceNativeBridgeDummy DismissLoading ");
	}

	public string GetNetworkName()
	{
		Debug.Log("DeviceNativeBridgeDummy GetNetworkName ");
		return "Test";
	}
	public string GetEmail()
	{
		Debug.Log("DeviceNativeBridgeDummy GetEmail ");
		return "chandias.juan@gmail.com";
	}
	public void ShowAccountPicker()
	{
		DeviceManager.sharedManager().OnAccountPickerResult("chandias.juan@gmail.com");

	}
	public void Vibrate(int time)
	{
		Debug.Log("DeviceNativeBridgeDummy Vibrate ");
	}
}