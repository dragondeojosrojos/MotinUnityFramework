using UnityEngine;
using System.Collections;

using System.Runtime.InteropServices;

public class TwitterBinding  {

	private static string PublicKey = "Rtb7kOlYXSP04OfAkYg2g";
	private static string SecretKey = "CZ4xAAdGmS77LNHfL0trq289m1s5uWuBbKcmkyF55k";

#if UNITY_WEBPLAYER || UNITY_EDITOR
	public static void ShowTwitter(string defaultMessage)
	{
	}
#elif UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern void ShowTwitter(string defaultMessage);
#elif UNITY_ANDROID

	public static void ShowTwitter(string defaultMessage)
	{
		AndroidJavaClass twitterClass = new AndroidJavaClass("com.motingames.twitter.TwitterManager");
		AndroidJavaObject twitterManager =  twitterClass.CallStatic<AndroidJavaObject>("sharedManager");

		if(twitterManager.GetRawObject().ToInt32()==0)
			Debug.LogError("Twitter Manager is NUll");


		twitterManager.Call("Init",PublicKey,SecretKey);
		twitterManager.Call("Show",defaultMessage);


	}

#endif

}
