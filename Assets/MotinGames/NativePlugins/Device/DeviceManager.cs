using UnityEngine;
using System.Collections;
//using System.Runtime.InteropServices;

public class DeviceManager : MonoBehaviour
{	
	private static DeviceManager sharedInstance = null;
	public bool dontDestroy = false;

	
	bool isQuitting = false;
	IDeviceNativeBridge nativeBridge = null;
	
	
	public static DeviceManager sharedManager()
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
				Debug.Log("DeviceManager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		nativeBridge = new DeviceNativeBridgeAndroid();
		#elif UNITY_IPHONE && !UNITY_EDITOR
		nativeBridge = new DeviceNativeBridgeDummy(); //Implementar ios
		#else
		nativeBridge = new DeviceNativeBridgeDummy();
		#endif


		
		Initialize(gameObject.name);
		
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
		nativeBridge.Initialize(this.gameObject.name);
	}

	public void ShowNotification(string message)
	{
		nativeBridge.ShowNotification(message);
	}

	public void ShowLoading(string message)
	{
		nativeBridge.ShowLoading(message);
	}

	public void DismissLoading()
	{
		nativeBridge.DismissLoading();
	}

	public string GetNetworkName()
	{
		return nativeBridge.GetNetworkName();
	}

	public void DebugLog(string messaje)
	{
		Debug.Log("ANDROID_LOG " + messaje);
	}

	public string GetEmail()
	{
		return nativeBridge.GetEmail();
	}

	System.Action<string> OnAccountPickerCallback =null;
	public void ShowAccountPicker(System.Action<string> accountPickerCallback)
	{
		OnAccountPickerCallback = accountPickerCallback;
		nativeBridge.ShowAccountPicker();
	}

	public void OnAccountPickerResult(string email)
	{
		if(OnAccountPickerCallback!=null)
			OnAccountPickerCallback(email);
	}

	public void Vibrate(int time)
	{
		nativeBridge.Vibrate(time);
	}

	/*
	static DeviceManager instance=null;
	//Delegates
	public delegate void OnAlertViewButtonClickedEventHandler(int index);
	public OnAlertViewButtonClickedEventHandler OnAlertViewButtonClicked=null;	
	
	public static DeviceManager sharedManager()
	{
		return instance;
	}
	void Awake()
	{
		if(instance==null)
			instance = this;
	}

	[DllImport("__Internal")]
	private static extern void _iOSPluginShowLoadingView();
	public static void ShowLoadingView()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_iOSPluginShowLoadingView();
	}
	
	[DllImport("__Internal")]
	private static extern void _iOSPluginHideLoadingView();
	public static void HideLoadingView()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_iOSPluginHideLoadingView();
	}
	[DllImport("__Internal")]
	private static extern void _iOSPluginshowAlertView(string title,string message,string  buttonTitle,bool pauseUnity);
	public static void ShowAlertView(string title,string message,string  buttonTitle,bool pauseUnity)
	{
#if UNITY_EDITOR
		if(DeviceManager.sharedManager().OnAlertViewButtonClicked!=null)
		{
			DeviceManager.sharedManager().OnAlertViewButtonClicked(0);
		}
#elif UNITY_IPHONE 
		_iOSPluginshowAlertView( title, message,  buttonTitle, pauseUnity);
#endif
	}
	
	public void AlertViewButtonClicked(string clickedButtonIndex)
	{
		if(OnAlertViewButtonClicked!=null)
		{
			OnAlertViewButtonClicked(int.Parse(clickedButtonIndex));
		}
	}
	*/
}
