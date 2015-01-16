using UnityEngine;
using System.Collections;


public class PushNativeBridgeDummy : IPushNativeBridge {
	
	
	public PushNativeBridgeDummy()
	{
		
	}
	
	public void Initialize (string goName,string data)
	{
		Debug.Log("PushNativeBridgeDummy Initialize " + goName);
	}
	
	public void RegisterDevice ()
	{
		Debug.Log("PushNativeBridgeDummy RegisterToService ");
	}
}