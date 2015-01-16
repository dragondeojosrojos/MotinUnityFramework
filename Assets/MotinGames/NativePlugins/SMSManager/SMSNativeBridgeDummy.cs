using UnityEngine;
using System.Collections;


public class SMSNativeBridgeDummy : ISMSNativeBridge {
	

	public SMSNativeBridgeDummy()
	{

	}
	
	public void Initialize (string goName)
	{
		Debug.Log("SMSNativeBridgeDummy Initialize " + goName);
	}
	
	public void StartSmsListening ()
	{
		Debug.Log("SMSNativeBridgeDummy StartSmsListening");
	}
	
	public void StopSmsListening ()
	{
		Debug.Log("SMSNativeBridgeDummy StopSmsListening");
	}
	
	public void SendSms (string number, string content)
	{
		Debug.Log("SMSNativeBridgeDummy SendSms " + number  + " " + content);
	}
	
}