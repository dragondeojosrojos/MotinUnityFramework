using UnityEngine;
using System.Collections;

public interface ISMSNativeBridge  {

	void Initialize(string goName);
	
	void StartSmsListening();
	
	void StopSmsListening();
	
	void SendSms(string number,string content);

}
