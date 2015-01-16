using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SMSManager : MonoBehaviour {

	// Field descriptor #8 I
	public  const int RESULT_OK = -1;
	// Field descriptor #8 I
	public  const int RESULT_ERROR_GENERIC_FAILURE = 1;
	
	// Field descriptor #8 I
	public  const int RESULT_ERROR_RADIO_OFF = 2;
	
	// Field descriptor #8 I
	public  const int RESULT_ERROR_NULL_PDU = 3;
	
	// Field descriptor #8 I
	public  const int RESULT_ERROR_NO_SERVICE = 4;

	// Field descriptor #8 I
	public  const int RESULT_ERROR_USER_CANCEL = 5;




	private static SMSManager sharedInstance = null;
	public bool dontDestroy = false;
	public System.Action<string,string> onSMSRecieved = null;
	public System.Action<int> onSMSSent = null;
	bool isQuitting = false;
	ISMSNativeBridge nativeBridge = null;


	public static SMSManager sharedManager()
	{
		return sharedInstance;
	}

	void Awake()
	{
		if(sharedInstance==null)
		{
			//			Debug.Log("OpenkitManager Init");
			isQuitting = false;
			sharedInstance = this;
			if (Application.isPlaying && dontDestroy) {
				DontDestroyOnLoad( gameObject );
			}
		}
		else
		{
			if(sharedInstance!=this)
			{
				Debug.Log("SMSManager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}

		#if UNITY_ANDROID && !UNITY_EDITOR
		nativeBridge = new SMSNativeBridgeAndroid();
		#elif UNITY_IPHONE && !UNITY_EDITOR
		nativeBridge = new SMSNativeBridgeDummy(); //Implementar ios
		#else
		nativeBridge = new SMSNativeBridgeDummy();
		#endif

		Initialize(this.gameObject.name);

	}

	void OnDisable()
	{
		if(isQuitting)
			sharedInstance=null;

		//Debug.Log("DISABLE OPEN KIT");
	}

	void OnApplicationQuit() {
		isQuitting = true;
		//Debug.Log("QUITTING OPENKIT");
	}

	void Initialize (string goName)
	{
		nativeBridge.Initialize(goName);
	}
	
	public void StartSmsListening ()
	{
		nativeBridge.StartSmsListening();
	}
	
	public void StopSmsListening ()
	{
		nativeBridge.StopSmsListening();
	}
	
	public void SendSms (string number, string content)
	{
		nativeBridge.SendSms(number,content);
	}


	public void OnSmsRecieved(string jsonData)
	{
		Debug.Log("SMS RECIEVED \n" + jsonData);
		Hashtable jsonparams =  MiniJSON.jsonDecode(jsonData) as Hashtable;

		if(jsonparams==null)
			Debug.Log("jsonparams ES NUUUUUULLLLLLL");

	
		if(onSMSRecieved!=null)
			onSMSRecieved(jsonparams["number"].ToString(),jsonparams["content"].ToString());

	}

	public void OnSmsSent(string jsonData)
	{
		Debug.Log("SMS SENT CALLBACK \n" + jsonData);
		Hashtable jsonparams =  MiniJSON.jsonDecode(jsonData) as Hashtable;
		
		if(jsonparams==null)
			Debug.Log("jsonparams ES NUUUUUULLLLLLL");
		
		
		if(onSMSSent!=null)
			onSMSSent( int.Parse(jsonparams["resultCode"].ToString()));
		
	}

}
