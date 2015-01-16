using UnityEngine;
using System.Collections;

public interface IPushNativeBridge  {
	
	void Initialize(string goName,string data);
	
	void RegisterDevice();
	
}
