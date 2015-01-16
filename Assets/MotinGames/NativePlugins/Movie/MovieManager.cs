
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class MovieManager : MonoBehaviour
{	
	static MovieManager instance=null;

	public static MovieManager sharedManager()
	{
		return instance;
	}
	void Awake()
	{
		if(instance==null)
			instance = this;
	}
	
	
	[DllImport("__Internal")]
	private static extern void Movie_PlayMovie(string url);
	public void PlayMovie(string url)
	{
		#if UNITY_IPHONE 
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			Movie_PlayMovie(url);
		#endif
	}
	
	[DllImport("__Internal")]
	private static extern void Movie_Play();
	public void Play()
	{
		#if UNITY_IPHONE 
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			Movie_Play();
		#endif
	}
	
	[DllImport("__Internal")]
	private static extern void Movie_Stop();
	public void Stop()
	{
#if UNITY_IPHONE 
		Movie_Stop( );
#endif
	}
	
}
