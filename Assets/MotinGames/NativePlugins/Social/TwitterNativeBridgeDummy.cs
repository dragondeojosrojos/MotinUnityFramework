using UnityEngine;
using System.Collections;


public class TwitterNativeBridgeDummy : ITwitterNativeBridge {
	
	
	public TwitterNativeBridgeDummy()
	{
		
	}

	public void Initialize (string goName, string publicKey, string secretKey)
	{
		Debug.Log("TwitterNativeBridgeDummy Initialize");
	}

	public void Show (string message)
	{
		Debug.Log("TwitterNativeBridgeDummy Show " + message);
	}

}