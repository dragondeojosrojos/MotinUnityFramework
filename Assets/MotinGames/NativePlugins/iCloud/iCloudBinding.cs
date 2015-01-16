using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class iCloudBinding : MonoBehaviour 
{
	static bool isAvaiable = false;
	
	[DllImport("__Internal")]
	public static extern void iCloudInitialize();
	public static void Initialize()
	{
	   if( Application.platform != RuntimePlatform.IPhonePlayer )
		{
			return;
		}
		
	   iCloudInitialize();
	}
	[DllImport("__Internal")]
	public static extern bool iCloudIsAvaiable();
	public static bool IsAvaiable()
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
		{
			isAvaiable = false;
			return isAvaiable;
		}
		isAvaiable = iCloudIsAvaiable();
	   return isAvaiable;
	}
	
	[DllImport("__Internal")]
	public static extern bool iCloudContainsKey(string key);
	public static bool ContainsKey(string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return false;
	
		if(!isAvaiable)
			return false;
		
	   return iCloudContainsKey(key);
	}
	
		
	[DllImport("__Internal")]
	public static extern string iCloudGetString(string key);
	public static string GetString(string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return "";
				
		if(!isAvaiable)
			return "";
	   return iCloudGetString(key);
	}
		
	[DllImport("__Internal")]
	public static extern void iCloudSetString(string value,string key);
	public static  void SetString(string value,string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;
		if(!isAvaiable)
			return ;
	   iCloudSetString(value,key);
	    
	}
	
	[DllImport("__Internal")]
	public static extern int iCloudGetInt(string key);
	public static int GetInt(string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return 0;
		if(!isAvaiable)
			return 0;
	  	return iCloudGetInt(key);
	}
	
	[DllImport("__Internal")]
	public static extern void iCloudSetInt(int value,string key);
	public static  void SetInt(int value,string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;
		if(!isAvaiable)
			return ;
	    iCloudSetInt(value,key);
	}
		
	[DllImport("__Internal")]
	public static extern float iCloudGetFloat(string key);
		public static float GetFloat(string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return 0;
		if(!isAvaiable)
			return 0f;
		
		return  iCloudGetFloat(key);
	}
		
	[DllImport("__Internal")]
	public static extern void iCloudSetFloat(float value,string key);
	public static void SetFloat(float value,string key)
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;
		
		if(!isAvaiable)
			return ;
	   iCloudSetFloat(value,key);
	}
		
	[DllImport("__Internal")]
	public static extern void iCloudSynchronize();
	public static  void Synchronize()
	{
		 if( Application.platform != RuntimePlatform.IPhonePlayer )
			return;
		
		if(!isAvaiable)
			return ;
	  iCloudSynchronize();
	}

}
