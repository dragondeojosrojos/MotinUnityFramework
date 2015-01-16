
using UnityEngine;
using System.Collections;

public interface ITwitterNativeBridge  {
	
	void Initialize(string goName,string publicKey,string secretKey);
	void Show(string message);
	
}