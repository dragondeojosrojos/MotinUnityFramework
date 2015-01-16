using UnityEngine;
using System.Collections;

public interface IDeviceNativeBridge  {

	void Initialize(string goName);
	void ShowNotification(string message);
	void ShowLoading(string message);
	void DismissLoading();
	string GetNetworkName();
	void ShowAccountPicker();
	string GetEmail();
	void Vibrate(int time);
	
}