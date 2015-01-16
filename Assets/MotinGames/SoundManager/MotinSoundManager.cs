
//#if ( UNITY_WEBPLAYER || UNITY_MAC && !UNITY_EDITOR)
	#define DOWNLOAD_SOUNDS 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

namespace MotinGames
{
	namespace SoundManagerData
	{
		[System.Serializable]
		public class SoundProperties : MotinData
		{
			[MotinEditorSoundEnumField]
			public string 	SoundId;
			public float 	volumen;
			public SoundProperties()
			{
				name = SoundDefinitions.Sounds.COUNT.ToString();
				SoundId = SoundDefinitions.Sounds.COUNT.ToString();
				volumen = 1;
			}
			public SoundProperties(string defName,float vol)
			{
				name = defName;
				SoundId = defName;
				volumen = vol;
			}
			
		}
	}
public class SoundPlayInstance
{
	public SoundPlayInstance()
	{

	}

	public MotinSoundManager.SoundType soundType;
	public SoundManagerData.SoundProperties soundProps = null;
	public float instanceVolumen = 1.0f;
	public AudioSource	audioSource = null;

}


public class MotinSoundManager : MonoBehaviour {

	const string KEY_VOLUME_FX 		= "SoundManager_Volume_Fx";
	const string KEY_VOLUME_MUSIC 	= "SoundManager_Volume_Music";

	public enum SoundType
	{
		FX,
		MUSIC,
	}

	public bool	enableDebug = false;
	public List<SoundManagerData.SoundProperties>	soundProps = new List<SoundManagerData.SoundProperties>();
	public bool  downloadSounds = false;
	public int   defaultAudioSources =10;

	List<SoundPlayInstance> soundPlayingInstances = new List<SoundPlayInstance>();

	bool isQuitting = false;
	public delegate void AudioEvent( );
	public delegate void AudioSourceEvent(AudioSource source);
	
	protected class EventData
	{
	
		public EventData(AudioEvent callback,AudioSource	source,int intValue=0)
		{
			eventDelegate = callback;
			audioSource = source;
			intData = intValue;
		}
		public AudioEvent	eventDelegate;
		public AudioSource	audioSource;
		public int 			intData;
	}
	
	
	protected Dictionary<int,WWW> downloadingSounds = new Dictionary<int, WWW>();
	
	protected Dictionary<int,AudioClip> loadedSounds = new Dictionary<int, AudioClip>();
	
	protected List<AudioSource>			freeSources = new List<AudioSource>();
	protected List<AudioSource>			audioSources = new List<AudioSource>();


	protected SoundPlayInstance			musicSource = null;

	protected int previusMusicId = -1;

	

	float musicVolume_ = 1;

	string basePath = "";

	public float musicVolume
	{
		get{
			return musicVolume_;
		}
		set{
			musicVolume_ = value;
			UpdateVolumeInstances(SoundType.MUSIC);
		}
	}
	float fxVolume_ = 1;
	public float fxVolume
	{
		get{
			return fxVolume_;
		}
		set{
			fxVolume_ = value;
			UpdateVolumeInstances(SoundType.FX);
		}
	}




	// Use this for initialization
	
	private static MotinSoundManager sharedInstance = null;
	public static MotinSoundManager sharedManager()
	{
		return sharedInstance;
	}
	
	void Awake()
	{
		isQuitting = false;
		if(sharedInstance==null)
		{
			//Debug.Log("SOUND MANAGER INIT");
			sharedInstance = this;
			if (Application.isPlaying) {
				DontDestroyOnLoad( gameObject );
			}
		}
		else
		{
			if(sharedInstance!=this)
			{
				//Debug.Log("Sound Manager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}


		if(downloadSounds)
		{
#if UNITY_WEBPLAYER
				basePath = "http://motingames.pcriot.com/Bioma/Audio/";
#elif UNITY_STANDALONE
			if(Application.platform == RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.OSXEditor )
			{
				basePath ="file://"+ Application.dataPath+ "/../../Audio/";
			}
			else if(Application.platform == RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				basePath = "file://"+ Application.dataPath+ "/../Audio/";
			}
			else
			{
				basePath = "Audio/";
			}
#else
			basePath = "Audio/";
#endif
		}
		else
		{
			basePath = "Audio/";
		}
		Debug.Log("Sound path " + basePath);

		UpdateSoundProps();
		
	}

	void OnApplicationQuit() {
		isQuitting=true;
		//Debug.Log("QUITTING");
	}
	void OnDisable()
	{
		if(isQuitting)
		{
			StopAllCoroutines();
			sharedInstance = null;
		}
	}

	void Log(string text)
	{
		if(enableDebug)
			Debug.Log(text);
	}
	void Start()
	{
		InitPlayerPrefs();
		LoadPlayerPrefs();
		InitAudioSources();
		LoadAllClips();
		StartCoroutine(SoundsInstancesCoroutine());
	}

	protected void InitPlayerPrefs()
	{
		if(!PlayerPrefs.HasKey(KEY_VOLUME_FX))
		{
			PlayerPrefs.SetFloat(KEY_VOLUME_FX,1.0f);
		}
		if(!PlayerPrefs.HasKey(KEY_VOLUME_MUSIC))
		{
			PlayerPrefs.SetFloat(KEY_VOLUME_MUSIC,1.0f);
		}
	}
	protected void LoadPlayerPrefs()
	{
		if(PlayerPrefs.HasKey(KEY_VOLUME_FX))
		{
			fxVolume =  PlayerPrefs.GetFloat(KEY_VOLUME_FX);
		}
		if(PlayerPrefs.HasKey(KEY_VOLUME_MUSIC))
		{
			musicVolume = PlayerPrefs.GetFloat(KEY_VOLUME_MUSIC);
		}
	}
	public void SavePlayerPrefs()
	{
		PlayerPrefs.SetFloat(KEY_VOLUME_FX,fxVolume);
		PlayerPrefs.SetFloat(KEY_VOLUME_MUSIC,musicVolume);
		PlayerPrefs.Save();
	}

	protected void InitAudioSources()
	{
		//Debug.Log("SoundManager InitAudioSources");
		AudioSource tmpSource = null;
		for(int i = 0 ; i < defaultAudioSources; i++)
		{
			tmpSource = this.gameObject.AddComponent<AudioSource>();
			audioSources.Add(tmpSource);
			freeSources.Add(tmpSource);
		}
	}
	

	public void PlayFx(SoundDefinitions.Sounds soundId)
	{
		PlayFx((int)soundId);
	}
	public void PlayFx(SoundDefinitions.Sounds soundId,float volume)
	{
		PlayFx((int)soundId,volume);
	}
	public void PlayFx(int soundId)
	{
		PlaySound(soundId,false,1,SoundType.FX);
	}
	public void PlayFx(int soundId,float volume)
	{
		PlaySound(soundId,false,volume,SoundType.FX);
	}


	public void PlayMusic(string musicName,float fadeOutTime = 0.5f,float fadeInTime = 2f,float soundVolume =1,bool crossFade = false,bool loop = true)
	{
		PlayMusic( (int) MotinUtils.StringToEnum<SoundDefinitions.Sounds>(musicName), fadeOutTime , fadeInTime , soundVolume, crossFade,loop);
	}
	                      
	public void PlayMusic(int soundId,float fadeOutTime = 0.5f,float fadeInTime = 2f,float soundVolume =1,bool crossFade = false,bool loop = true)
	{
		//Debug.Log ("PLAY MUSIC " + soundId);
		if(musicSource!=null )
		{
			if( musicSource.audioSource.clip == GetAudioClip(soundId))
				return;

			//previusMusicId = Gec

			if(!crossFade)
			{
				pendingMusicId = soundId;
				pendingfadeIn = fadeInTime;
				pendingLoop = loop;
				pendingMusicInstanceVolume = soundVolume;
				StopSound(musicSource,fadeOutTime,PlayPendingMusic);
				return;
			}
			else
			{
				StopSound(musicSource,fadeOutTime);
			}
		}
		
		musicSource = PlaySound(soundId,loop,soundVolume,SoundType.MUSIC,fadeInTime);
	
	}
	
	protected int pendingMusicId = 0;
	protected bool pendingLoop = true;
	protected float pendingfadeIn = 0;
	protected float pendingMusicInstanceVolume = 0;

	public void PlayPendingMusic()
	{
		
		musicSource = PlaySound(pendingMusicId,pendingLoop,pendingMusicInstanceVolume,SoundType.MUSIC,pendingfadeIn);
	}
	
	public void StopMusic(float fadeOutTime = 0.5f)
	{
		//Debug.Log("SoundManager StopMusic");
		if(musicSource!=null )
		{
			StopSound(musicSource,fadeOutTime);
		}
	}
				
	public SoundPlayInstance PlaySound(int soundId,bool loop,float volume,SoundType soundType, float fadeTime =0)
	{
		if(soundId==-1)
			return null;

		
		Log ("SoundManager PlaySound " + soundProps[soundId].SoundId + "\nProps volume " + soundProps[soundId].volumen + "\ninstance Volume " +  volume + "\nFinal Volume " + volume*soundProps[soundId].volumen);

		AudioClip tmpClip = GetAudioClip(soundId);
		if(tmpClip==null)
			return null;

		AudioSource source = GetFreeAudioSource();
		source.clip = tmpClip;
		source.loop = loop;



		if(fadeTime>0)
		{
			source.volume =0;
			HOTween.To(source,fadeTime,new TweenParms().Prop("volume",volume*soundProps[soundId].volumen* ( soundType == SoundType.FX? fxVolume : musicVolume),false).Ease(EaseType.EaseInOutSine));
		}
		else
		{
			source.volume = volume*soundProps[soundId].volumen* ( soundType == SoundType.FX? fxVolume : musicVolume);
			
		}

		SoundPlayInstance newPlayInstance = new SoundPlayInstance();
		newPlayInstance.audioSource = source;
		newPlayInstance.soundProps = soundProps[soundId];
		newPlayInstance.soundType = soundType;
		newPlayInstance.instanceVolumen = volume;
		newPlayInstance.audioSource.Play();

		soundPlayingInstances.Add(newPlayInstance);

		return newPlayInstance;
	}
	
	public void StopSound(SoundPlayInstance playingInstance,float fadeTime=0,AudioEvent callback =null)
	{
		Log("SoundManager StopSound");
		if(fadeTime>0)
		{
			
			HOTween.To(playingInstance.audioSource,fadeTime,new TweenParms().Prop("volume",0,false).Ease(EaseType.EaseInOutSine).OnComplete(OnSourceFadeOut,new EventData(callback,playingInstance.audioSource) ));
		}
		else
		{
			playingInstance.audioSource.Stop();
			playingInstance.audioSource.clip = null;
			if(callback!=null)
				callback();
		}
	}
	public void OnSourceFadeOut(TweenEvent data)
	{
		Log("SoundManager OnSourceFadeOut");
		EventData eventData = (EventData) data.parms[0];
		eventData.audioSource.Stop();
		eventData.audioSource.clip = null;
		if(eventData.eventDelegate!=null)
				eventData.eventDelegate();
	}

	protected void UpdateVolumeInstances(SoundType soundType)
	{
		foreach(SoundPlayInstance playingInstance in soundPlayingInstances)
		{
			if(soundType == playingInstance.soundType )
				playingInstance.audioSource.volume = playingInstance.instanceVolumen*playingInstance.soundProps.volumen* ( playingInstance.soundType == SoundType.FX? fxVolume : musicVolume);
		}
	}

	protected AudioSource GetFreeAudioSource()
	{
		AudioSource tmpSource = null;
		if(freeSources.Count==0)
		{
			tmpSource =  this.gameObject.AddComponent<AudioSource>();
			audioSources.Add(tmpSource);
			freeSources.Add(tmpSource);
		}
		tmpSource = freeSources[0];
		freeSources.RemoveAt(0);

		return tmpSource;
		
	}
	/*
	public int GetClipIndex(AudioClip clip)
	{
		AudioClip tmpClip = null;
		if(loadedSounds.TryGetValue(soundId,out tmpClip))
		{
			return tmpClip;
		}
		else
		{
			return LoadAudioClip(soundId);
		}
	}
	*/
	public AudioClip GetAudioClip(int soundId)
	{
		AudioClip tmpClip = null;
		if(loadedSounds.TryGetValue(soundId,out tmpClip))
		{
			return tmpClip;
		}
		else
		{
			return LoadAudioClip(soundId);
		}
	}
	protected virtual AudioClip LoadAudioClip(int soundId)
	{
		if(soundId>=(int)SoundDefinitions.Sounds.COUNT)
			return null;
		
		if(loadedSounds.ContainsKey(soundId))
			return loadedSounds[soundId];
		
		AudioClip tmpClip = null;

		if(downloadSounds)
		{
			string clipUrl;
#if UNITY_WEBPLAYER
			clipUrl =  basePath + SoundDefinitions.SoundPaths[soundId]+ ".wav";
#else
			clipUrl = basePath + SoundDefinitions.SoundPaths[soundId]+ SoundDefinitions.SoundExtensions[soundId];
#endif


				WWW webClass = new WWW(clipUrl);

			//tmpClip = webClass.GetAudioClip(false);
			downloadingSounds.Add(soundId,webClass);
			
		}
		else
		{
			tmpClip = (AudioClip)Resources.Load(basePath + SoundDefinitions.SoundPaths[soundId]);
		}

		if(downloadSounds)
		{
			return null;
		}

		if(tmpClip!=null)
			loadedSounds.Add(soundId,tmpClip);
		
		return tmpClip;
	}
	
	protected void LoadAllClips()
	{
		Log("Load All Clips");
		for(int i = 0 ;i <(int) SoundDefinitions.Sounds.COUNT;i++)
		{
			LoadAudioClip(i);
		}
		if(downloadSounds)
			StartCoroutine(DownloadingSoundsCoroutine());
	}

	IEnumerator DownloadingSoundsCoroutine()
	{
		bool stillDownloading = true;
		while(stillDownloading)
		{
			stillDownloading = false;
			foreach(KeyValuePair<int, WWW> entry in downloadingSounds)
			{

				if(entry.Value.progress!=1)
				{
					stillDownloading = true;
					//if(entry.Key == (int)SoundDefinitions.Sounds.BIOMA_S_MENU_BACK || entry.Key ==  (int)SoundDefinitions.Sounds.BIOMA_S_MENU_OK)
					//	Debug.Log("Downloading " + entry.Value.url + " " + entry.Value.progress);
					//downloadingSounds.Remove(entry.Key);
					//return false;
				}
				else
				{
					if(!loadedSounds.ContainsKey(entry.Key))
					{
						Debug.Log("Sound Downloaded " + entry.Value.url + " " + entry.Value.progress);
						loadedSounds.Add(entry.Key,entry.Value.GetAudioClip(false));
					}

				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		downloadingSounds.Clear();
		Log("ALL SOUNDS DOWNLOADED");
	}

	public bool AreClipsDownloaded()
	{
		return (loadedSounds.Count == (int)SoundDefinitions.Sounds.COUNT);
		/*
		if(downloadSounds)
		{
			foreach(KeyValuePair<int, WWW> entry in downloadingSounds)
			{
				if(entry.Value.progress!=1)
				{
					Debug.Log("Downloading " + entry.Value.url + " " + entry.Value.progress);
					//downloadingSounds.Remove(entry.Key);
					return false;
				}
			}
			downloadingSounds.Clear();
		}
		return true;
		*/
		
	}
	
	public void UpdateSoundProps()
	{
		if(soundProps==null)
				soundProps = new List<SoundManagerData.SoundProperties>();
	
		string[] soundDefines = MotinUtils.EnumNames<SoundDefinitions.Sounds>();

		bool found; 
		SoundManagerData.SoundProperties auxProp =null;
		for(int i = 0 ; i < soundDefines.Length-1;i++)
		{
			found = false;
			for(int x = 0; x < soundProps.Count;x++)
			{
				if(soundProps[x].SoundId == soundDefines[i])
				{
					if(x !=i)
					{
						auxProp = soundProps[i];
						soundProps[i] = soundProps[x];
						soundProps[x] =auxProp ;
					}
					found = true;
					break;
				}
			}
			if(!found)
			{
				soundProps.Insert(i,new SoundManagerData.SoundProperties(soundDefines[i],1));
			}
		}
		for(int i = soundDefines.Length-1 ; i < soundProps.Count;i++)
		{
			soundProps.RemoveAt(i);
		}

	}

	IEnumerator SoundsInstancesCoroutine()
	{
		int i =0;
		while(true)
		{
			for(i=0; i < soundPlayingInstances.Count;i++)
			{
				if(!soundPlayingInstances[i].audioSource.isPlaying)
				{
					soundPlayingInstances[i].audioSource.clip = null;
					freeSources.Add (soundPlayingInstances[i].audioSource);
					soundPlayingInstances.RemoveAt(i);
					i--;
				}
			}

			yield return new WaitForSeconds(0.1f);
		}


	}

}
}
