
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TwitterManager : MonoBehaviour {

	private static TwitterManager sharedInstance = null;
	public bool dontDestroy = false;
	bool isQuitting = false;
	ITwitterNativeBridge nativeBridge = null;

	private static string PublicKey = "pqpq4QiClTT5hSljIuBUng";
	private static string SecretKey = "yBmo1e9BAkoCySbj5Ej1xVC5F2gbYGkL1jRExvbq0";
	
	public static TwitterManager sharedManager()
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
		nativeBridge = new TwitterNativeBridgeAndroid();
		#elif UNITY_IPHONE && !UNITY_EDITOR
		nativeBridge = new TwitterNativeBridgeDummy(); //Implementar ios
		#else
		nativeBridge = new TwitterNativeBridgeDummy();
		#endif
		
		Initialize(this.gameObject.name,PublicKey,SecretKey);
		
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
	
	void Initialize (string goName,string publicKey, string secretKey)
	{
		nativeBridge.Initialize(goName,publicKey,secretKey);
	}

	System.Action OnTwitterCompletedCallback = null;
	public void Show(string message,System.Action OnCompleted)
	{
		OnTwitterCompletedCallback = OnCompleted;
		nativeBridge.Show(message);
	}

	public void OnTwitterCompleted(string result)
	{
		if(OnTwitterCompletedCallback!=null)
			OnTwitterCompletedCallback();

		OnTwitterCompletedCallback = null;
	}

}
