using UnityEngine;
using UnityEditor;
using System.Collections;
using MotinGames;

[CustomEditor(typeof(MotinSoundManager))]
public class MotinSoundManagerInspector : Editor {


	public	MotinSoundManager soundManager= null;
	MotinSoundManagerEditor soundManagerEditor = null;

	public bool defaultFoldout = true;


	void OnEnable()
	{
		soundManager = (MotinSoundManager)target;

		if(soundManagerEditor==null)
		{
			soundManagerEditor = new MotinSoundManagerEditor(); 
		}

	}
	void OnDisable()
	{
		soundManagerEditor = null;
		soundManager = null;
	}

	public override void OnInspectorGUI()
    {
		soundManagerEditor.target = soundManager;
		soundManagerEditor.Draw(new Rect(0,0,300,1000));
		//defaultFoldout = MotinEditorUtils.DrawDefaultEditorFoldout(defaultFoldout,this);	

		//defaultFoldout = MotinEditorUtils.DrawDefaultEditorFoldout(defaultFoldout,this);	

	}
	/*
	List<string> textLines = new List<string>();
	List<string> defineNames = new List<string>();
	List<string> fileNames = new List<string>();
	List<string> extensionNames = new List<string>();
	protected void GenerateData()
	{
		//Object[] loadedAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/Audio");
		
	
		defineNames.Clear();
		fileNames.Clear();
		textLines.Clear();
		extensionNames.Clear();


		DirectoryInfo info = new DirectoryInfo("Assets/Resources/Audio");
		System.IO.FileInfo[] fileInfo = info.GetFiles();
		int i =0;
		foreach (System.IO.FileInfo file in fileInfo)
		{
			{
				if(file.Extension== ".wav" || file.Extension== ".mp3" || file.Extension== ".ogg") {
					defineNames.Add(file.Name.Replace(file.Extension,"" ).ToUpper());
					fileNames.Add(file.Name.Replace(file.Extension,"" ));
					extensionNames.Add(file.Extension);
					//Debug.Log("Asset " + file.FullName +  " extension " + file.Extension);


					i++;
				}
			}
		}

		WriteHeader();
		WriteDefines();
		WritePaths();
		WriteFooter();


		System.IO.File.WriteAllLines(Application.dataPath +  "/MotinGames/SoundManager/SoundDefinitions.cs", textLines.ToArray());

		AssetDatabase.Refresh();

		soundManager.UpdateSoundProps();
	}

	void WriteHeader()
	{
		textLines.Add ("using UnityEngine;");
		textLines.Add ("public class SoundDefinitions");
		textLines.Add ("{");
			
	}
	void WriteDefines()
	{
		textLines.Add ("	public enum Sounds");
		textLines.Add ("	{");

		int i =0;
		foreach(string define in defineNames)
		{
			textLines.Add("		" + defineNames[i] + "=" + i.ToString() + ",");
			i++;
		}
		textLines.Add("		COUNT=" + i.ToString());
		textLines.Add ("	}");
	}
	void WritePaths()
	{
		textLines.Add ("	public static readonly string[] SoundPaths = ");
		textLines.Add ("	{");
		
		int i =0;
		foreach(string define in defineNames)
		{
			textLines.Add("		\"" + fileNames[i] + "\",");
			i++;
		}
		textLines.Add ("	};");


		textLines.Add ("	public static readonly string[] SoundExtensions = ");
		textLines.Add ("	{");
		
		i =0;
		foreach(string define in defineNames)
		{
			textLines.Add("		\"" + extensionNames[i] + "\",");
			i++;
		}
		textLines.Add ("	};");
	}
	void WriteFooter()
	{
		textLines.Add ("}");
		
	}
	*/
}
	
	