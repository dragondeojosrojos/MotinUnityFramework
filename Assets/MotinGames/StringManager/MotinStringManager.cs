using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Text;


[ExecuteInEditMode]
public class MotinStringManager : MonoBehaviour {
	
	
	public delegate void StringManagerDelegate(LANG language);
	public StringManagerDelegate OnLanguageChanged=null;
	public void raiseOnLanguageChanged()
	{
		if(OnLanguageChanged!=null)
		{
			OnLanguageChanged(currentLang);
		}
	}
	
	
	public LANG 	defaultLang;
	Dictionary<MotinStrings,string>	stringList = new Dictionary<MotinStrings, string>();
	LANG 	currentLang  = LANG.COUNT;
	public LANG currentLanguage
	{
		get{ return currentLang;}
	}


	bool isQuitting = false;
	protected static MotinStringManager sharedInstance = null;
	public static MotinStringManager sharedManager()
	{
		return sharedInstance;
	}
	

	void Awake()
	{
		if(sharedInstance==null)
		{
			Debug.Log("STRING MANAGER INIT");
			sharedInstance = this;
			if (Application.isPlaying) {
				DontDestroyOnLoad( gameObject );
			}
		}
		else
		{
			if(sharedInstance!=this)
			{
				Debug.Log("String Manager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}
		isQuitting = false;
		SetLanguage(defaultLang);
		
	}
	void OnApplicationQuit() {
		isQuitting = true;
//		Debug.Log("QUITTING STRING MANAGER");
	}
	void Start()
	{
//		Debug.LogWarning("String Manager Start");
		//SetLanguage(defaultLang);
	}
	
	void OnEnable()
	{
	//	Debug.LogWarning("String Manager onenable");
		//if(Application.isEditor)
			UpdateFromFile();
	}
	void OnDisable()
	{
		if(isQuitting)
			sharedInstance=null;
		//Debug.Log("DISABLE String Manager");
	}
	
	public string getString(MotinStrings stringKey)
	{
//		Debug.Log("getString " + stringKey.ToString() + " count " + stringList.Count);
		if(stringKey== MotinStrings.STRING_COUNT)
		{
			return "No String Setted";
		}
		string result="";
		if(!stringList.TryGetValue(stringKey,out result))
		{
			Debug.LogWarning("String not found "+ stringKey.ToString());
			result = "Invalid:"+ stringKey.ToString();
		}
		return result;
	}
	public string getString(string stringName)
	{
		return getString(MotinUtils.StringToEnum<MotinStrings>(stringName));
	}
	
	public void SetNextLanguage()
	{
		if((int)currentLang == (int)LANG.COUNT-1)
		{
			SetLanguage((LANG)0);
			return;
		}
		
		SetLanguage((LANG)((int)currentLang+1));
	}
	public void SetPrevLanguage()
	{
		if((int)currentLang == 0)
		{
			SetLanguage((LANG)(LANG.COUNT-1));
			return;
		}
		
		SetLanguage((LANG)((int)currentLang-1));
	}
	public void SetLanguage(LANG newLang)
	{
		if(currentLang==newLang)
			return;
		
		currentLang = newLang;
		UpdateFromFile();
		
	}
	public void UpdateFromFile()
	{
		stringList.Clear();
		
		TextAsset asset = Resources.Load ("strings/"+ currentLang.ToString()) as TextAsset;
		if(asset==null)
		{
			Debug.LogError("StringsFile not found " + "strings/"+ currentLang.ToString());
			return;
		}
		
		Stream s = new MemoryStream (asset.bytes);
		BinaryReader br = new BinaryReader (s);
		
		int nStrings = br.ReadInt32();	
		//Debug.Log("String Count " + nStrings);

		int len;

//		int charIndex = 0;
		
		bool isAsianFont = (currentLang == LANG.CH || 
	                            currentLang == LANG.JA ||
	                            currentLang == LANG.KO);

		//char[] charBuffer;
		byte[] byteBuffer;
		for (int i=0; i< nStrings; i++) 
		{
	
			
			//stringRange.location = INDEX;
			//stringRange.length = 4;
			
			len =br.ReadInt32();
			//Debug.Log("String len" + len);
			
			//INDEX += 4 ;//+ 2;
			 
			//charBuffer = new char[len];
			byteBuffer = new byte[len];
			byte readByte;
			
			for (int j = 0; j < len; j+=1) 
			{
				readByte = br.ReadByte();
				//charBuffer[j] = (char)readByte;
				byteBuffer[j]  = readByte;
				//Debug.Log("char: " + charBuffer[j]);

			}
//			Debug.Log("string: " + Encoding.UTF8.GetString(byteBuffer));
			stringList.Add((MotinStrings)i,Encoding.UTF8.GetString(byteBuffer));
			//stringList.Add((MotinStrings)i,new string(charBuffer,0,len));

		}
		raiseOnLanguageChanged();
	}
}
