using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class StringManagerComponent : MonoBehaviour {
	
	public MotinLabelBaseWrapper		motinLabel = null;
	//protected MotinStrings		motinString = (MotinStrings)0;
	
	public string			motinStringName = "STRING_COUNT";
	public string 			labelName		= "motinLabel";

	public bool 			forceUpperCase;
	// Use this for initialization
	protected MotinStringManager stringManager = null;
	void Awake()
	{
		//Debug.LogWarning("StringManagerComponent Awake");
		this.tag = "StringManagerLabel";
	}
	
	void OnEnable()
	{
		Initialize();

		UpdateString();
	}
	
	void OnDisable()
	{
		//Debug.LogWarning("StringManagerComponent  ondisable");
		if(stringManager!=null)
		{
			stringManager.OnLanguageChanged-=OnLangChanged;
			stringManager = null;
		}
	}
	void OnDestroy()
	{
		//Debug.LogWarning("StringManagerComponent on Destroy");
		
	}
	
	protected void Initialize()
	{
		if(motinLabel==null)
		{
			motinLabel = GetComponent<MotinLabelBaseWrapper>();
		}
		//Debug.LogWarning("StringManagerComponent  on Enable");
		if(stringManager==null)
		{

			if(Application.isEditor)
			{
				//tk2dLabel = GetComponent<tk2dTextMesh>();
				//motinString = MotinUtils.StringToEnum<MotinStrings>(motinStringName);
				
				GameObject managerObj = GameObject.FindGameObjectWithTag("StringManager");
				if(managerObj==null)
				{
					Debug.LogError("String Manager is Missing");
					return;
				}
				
				stringManager = managerObj.GetComponent<MotinStringManager>();
			}
			else
			{
				stringManager = MotinStringManager.sharedManager();
			}

			if(stringManager == null)
			{
				Debug.LogError("String Manager is Null");
				return;
			}

			stringManager.OnLanguageChanged+=OnLangChanged;
		}

	}
	
	public void OnLangChanged(LANG language)
	{
		UpdateString();
	}
	public void SetString(string  stringName)
	{
		motinStringName = stringName;
		UpdateString();
	}
	public void SetString(MotinStrings stringKey)
	{
		//motinString = stringKey;
		SetString(stringKey.ToString());
	
	}
	public void UpdateString()
	{
		Initialize();
		if(stringManager!=null)
		{
			string newString ="";
			if(motinStringName=="STRING_COUNT")
			{
				motinLabel.text = "";
			}
			else
			{
				newString = stringManager.getString(MotinUtils.StringToEnum<MotinStrings>(motinStringName));
				if(motinLabel.maxChars<newString.Length)
				{
					motinLabel.maxChars = newString.Length;
				}

				if(forceUpperCase)
					motinLabel.text = newString.ToUpper();
				else
					motinLabel.text = newString;


			}

		}
	}
}
