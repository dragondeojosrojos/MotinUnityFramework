using UnityEngine;
using System.Collections;
using System;
public class MotinDrmManager : MonoBehaviour {
	

	private const string KEY_UNLOCK_STATUS = "MotinDrmUnlockStatus";
	private const string KEY_LAST_TEST = "MotinDrmLastTestTime";

	private static MotinDrmManager sharedInstance = null;
	public string drmURL = "http://motingames.pcriot.com/Minigames/Backend/MotinTest.php";
	public bool dontDestroy = false;
	bool isQuitting = false;

	
	public static MotinDrmManager sharedManager()
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
				Debug.Log("MotinDrmManager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}
		
	
		Initialize(this.gameObject.name);
		
	}
	
	void OnDisable()
	{
		if(isQuitting)
		{
			StopAllCoroutines();
			sharedInstance=null;
		}
		
		//Debug.Log("DISABLE OPEN KIT");
	}
	
	void OnApplicationQuit() {
		isQuitting = true;
		//Debug.Log("QUITTING OPENKIT");
	}

	void Start()
	{
		System.DateTime registeredTime = System.DateTime.Parse(PlayerPrefs.GetString(KEY_LAST_TEST));
		//if(/*(System.DateTime.UtcNow-  registeredTime ).Days >=1 || */!isUnlocked())
		//{
			StartCoroutine(TestDrm());
		//}
	}

	public bool isUnlocked()
	{
		if(PlayerPrefs.HasKey(KEY_UNLOCK_STATUS))
		{
			if(PlayerPrefs.GetInt(KEY_UNLOCK_STATUS)==1)
				return true;
			else
				return false;
		}

		return true;
	}
	
	void Initialize (string goName)
	{
		if(!PlayerPrefs.HasKey(KEY_UNLOCK_STATUS))
		{
			PlayerPrefs.SetInt(KEY_UNLOCK_STATUS,1);
			PlayerPrefs.Save();
		}

		if(!PlayerPrefs.HasKey(KEY_LAST_TEST))
		{
			PlayerPrefs.SetString(KEY_LAST_TEST,"2000-01-01 00:00:00");
			PlayerPrefs.Save();
		}
	}

	IEnumerator TestDrm()
	{


		WWW request = null; 
		
		try{
			request = new WWW(drmURL);
		}
		catch (Exception e)
		{
			Debug.Log("Exception " + e.Message);
			//if(OnFailed!=null)
			//{
			//	OnFailed();
				yield break;
			//}
		}
		
		while(!request.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		
		//Debug.Log ("REQUES T DONE " + gameRequest.text);
		if(request.error !=null /*&& request.error.Length >0*/)
		{
			Debug.LogError("Cant connect to motin server");
			//if(OnFailed!=null)
			//{
			//	OnFailed();
				yield break;
			//}
			
		}
		
		Hashtable parameters = MiniJSON.jsonDecode(request.text) as Hashtable;

		if(parameters==null)
		{
			Debug.LogError("Parameters is Null " + request.text);
			yield break;
		}

		if(int.Parse(parameters["resultCode"].ToString()) ==1)
			PlayerPrefs.SetInt(KEY_UNLOCK_STATUS,1);
		else
			PlayerPrefs.SetInt(KEY_UNLOCK_STATUS,0);


		Debug.Log ("MOTIN DRM UPDATED " + int.Parse(parameters["resultCode"].ToString()));
		PlayerPrefs.SetString(KEY_LAST_TEST,DateTime.UtcNow.ToString() );
		PlayerPrefs.Save();
		//DeviceManager.sharedManager().ShowNotification("Registrado " + email);
		//Debug.Log("INSTALL DATE IS " + parameters["message"].ToString());
		
		//if(OnCompleted!=null)
		//	OnCompleted();
	}

}
