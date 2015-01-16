
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class PushManager : MonoBehaviour {
	

	
	private static PushManager sharedInstance = null;
	private const int MAX_RETRIES = 5; 
	private const int BASE_RETRY_TIME = 5; 

	private const string KEY_EMAIL = "pushManagerEmail"; 
	private const string KEY_TOKEN = "pushToken"; 
	private const string KEY_REGISTERED_TO_SERVER = "registeredToServer"; 

	private const string URL_REGISTRATION = "/php/registerGcmDevice.php"; 

	public bool dontDestroy = false;
	public string backendURL = ""; //http://motingames.enjuego.com:8084
	public string androidSenderId ="";


	bool isQuitting = false;
	IPushNativeBridge nativeBridge = null;
	//string userEmail = "";

	
	
	public static PushManager sharedManager()
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
				Debug.Log("PushManager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		nativeBridge = new PushNativeBridgeAndroid();
		#elif UNITY_IPHONE && !UNITY_EDITOR
		nativeBridge = new PushNativeBridgeDummy(); //Implementar ios
		#else
		nativeBridge = new PushNativeBridgeDummy();
		#endif
		
		Initialize(this.gameObject.name);
		
	}
	void Start()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
			RegisterDevice();
		#endif
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
		#if UNITY_ANDROID
			nativeBridge.Initialize(goName,androidSenderId);
		#else
		nativeBridge.Initialize(goName,androidSenderId);
		#endif
	}



	public string Token
	{
		get
		{
			if(isRegisteredToService())
				return PlayerPrefs.GetString(KEY_TOKEN);

			return null;
		}
	}

	public bool isRegistered()
	{
		if(isEmailRegistered() && isRegisteredToService() && isRegisteredToServer())
		{
			return true;
		}
		
		return false;
		
	}

	public bool isEmailRegistered()
	{
		if(PlayerPrefs.HasKey(KEY_EMAIL))
		{
			return true;
		}
		
		return false;
		
	}

	public bool isRegisteredToService()
	{
		if(PlayerPrefs.HasKey(KEY_TOKEN))
		{
			return true;
		}

		return false;

	}
	public bool isRegisteredToServer()
	{
		if(PlayerPrefs.HasKey(KEY_REGISTERED_TO_SERVER))
		{
			if(PlayerPrefs.GetInt(KEY_REGISTERED_TO_SERVER)==1)
				return true;
		}
		
		return false;
		
	}
	public void RegisterDevice()
	{
		if(isRegistered())
		{
			Debug.Log("PushManager isRegistered true");
			return;
		}

		if(!isEmailRegistered())
		{
			if(!RegisterEmail())
				return;//No Google account
		}

		if(!isRegisteredToService())
		{
			RegisterToService();
			return;
		}

		if(!isRegisteredToServer())
			RegisterToServer();
	}
	bool RegisterEmail()
	{
		string email = DeviceManager.sharedManager().GetEmail();
		if(email == null)
			return false;

		if(email == "")
			return false;

		PlayerPrefs.SetString(KEY_EMAIL,email);
		return true;
	}
	void RegisterToService()
	{
		if(!isEmailRegistered())
			return;
		
		StartCoroutine(RegisterToServiceCoroutine());
		
	}

	bool registerToServiceCompleted = false;
	string tmpJsonResult = "";
	IEnumerator RegisterToServiceCoroutine()
	{
		Debug.Log("PushManager::RegisterToService");
		int currentWaitTime = BASE_RETRY_TIME;
		Hashtable jsonparams = null;
		tmpJsonResult = "";
		for(int i =0 ; i <MAX_RETRIES;i++)
		{
			Debug.Log("PushManager::RegisterToServiceCoroutine Attempt " + i.ToString());
			registerToServiceCompleted = false;
			jsonparams = null;
			nativeBridge.RegisterDevice();
			while(!registerToServiceCompleted)
			{
				yield return new WaitForSeconds(0.1f);
			}

			jsonparams =  MiniJSON.jsonDecode(tmpJsonResult) as Hashtable;
			
			if(jsonparams==null)
			{
				Debug.Log("jsonparams ES NUUUUUULLLLLLL");
				yield return new WaitForSeconds(currentWaitTime);
				currentWaitTime*=2;
				continue;
			}
			
			if(int.Parse(jsonparams["resultCode"].ToString())==1)
			{
				Debug.Log("PushManager::OnRegisteredToService ERROR " + jsonparams["data"].ToString());
				yield return new WaitForSeconds(currentWaitTime);
				currentWaitTime*=2;
				continue;
			}

			break;

		}

		PlayerPrefs.SetString(KEY_TOKEN,jsonparams["data"].ToString());
		PlayerPrefs.Save();
		RegisterToServer();

	}

	public void OnRegisteredToService(string jsonData)
	{
		Debug.Log("PushManager::OnRegisteredToService:\n" + jsonData);
		tmpJsonResult = jsonData;
		registerToServiceCompleted = true;
	}

	void RegisterToServer()
	{
		if(!isRegisteredToService())
			return;

		StartCoroutine(RegisterToServerCoroutine());

	}
	
	IEnumerator RegisterToServerCoroutine()
	{
		int currentWaitTime = BASE_RETRY_TIME;
		WWW request = null; 
		Hashtable parameters = null;
		Hashtable registrationParams = new Hashtable();
		registrationParams.Add("regId",Token);
		registrationParams.Add("email",PlayerPrefs.GetString(KEY_EMAIL));
		string requestUrl = backendURL + URL_REGISTRATION + "?" + BuildQueryString(registrationParams);
		Debug.Log("PushManager::RegisterToServerCoroutine RequestUrl  " + requestUrl);
		bool hasError = false;
		for(int i =0 ; i <MAX_RETRIES;i++)
		{
			Debug.Log("PushManager::RegisterToServerCoroutine Attempt " + i.ToString());
			hasError = false;
			parameters = null;

			try{
				request = new WWW(requestUrl);
			}
			catch (Exception e)
			{
				Debug.Log("PushManager::RegisterToServerCoroutine " + e.Message);
				hasError = true;

			}
			if(hasError)
			{
				yield return new WaitForSeconds(currentWaitTime);
				currentWaitTime*=2;
				continue;
			}
			while(!request.isDone)
			{
				yield return new WaitForSeconds(0.1f);
			}

			//Debug.Log ("REQUES T DONE " + gameRequest.text);
			if(request.error !=null /*&& request.error.Length >0*/)
			{
				Debug.Log("PushManager::RegisterToServerCoroutine Error: " + request.error);
				yield return new WaitForSeconds(currentWaitTime);
				currentWaitTime*=2;
				continue;
			}

			parameters = MiniJSON.jsonDecode(request.text) as Hashtable;
			
			if(int.Parse(parameters["resultCode"].ToString()) ==1)
			{
				Debug.Log("PushManager::OnRegisteredToService Error :\n" + parameters["message"].ToString());
				yield return new WaitForSeconds(currentWaitTime);
				currentWaitTime*=2;
				continue;
			}
			
			PlayerPrefs.SetInt(KEY_REGISTERED_TO_SERVER,1);
			PlayerPrefs.Save();
			break;
		}

	}

	private string BuildQueryString(Hashtable parameters)
	{
		string Querystring = "";

		foreach(DictionaryEntry entry in parameters)
		{
			Querystring += entry.Key.ToString() + "=" + entry.Value.ToString() + "&";
		}

		if(Querystring.Length>0)
			Querystring.Remove(Querystring.Length-1);

		return Querystring;

	}
	
}
