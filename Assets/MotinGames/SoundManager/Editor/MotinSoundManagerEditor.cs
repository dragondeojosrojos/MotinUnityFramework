using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MotinGames;
public class MotinSoundManagerEditor : MotinEditor {


	MotinSoundManager soundManager = null;
	MotinArrayListEditor	listEditor = null;

	protected override void Initialize ()
	{
		base.Initialize ();
		listEditor = new MotinArrayListEditor("MotinGames.SoundManagerData");
		listEditor.OnDataChanged+= OnSoundPropsEditorChanged;

	}
	void OnSoundPropsEditorChanged()
	{
		soundManager.soundProps = listEditor.objectList.Cast<MotinGames.SoundManagerData.SoundProperties>().ToList();
	}
	protected override void targetUpdated ()
	{
		base.targetUpdated ();
		soundManager = (MotinSoundManager)target;
		soundManager.UpdateSoundProps();

	}

	protected override void DoDraw ()
	{
		GUILayout.BeginVertical();
		
		if(GUILayout.Button("Generate Data"))
		{
			GenerateData();
			EditorUtility.SetDirty(soundManager);
			AssetDatabase.SaveAssets();
		}
		if(GUILayout.Button("Update Props"))
		{
			soundManager.UpdateSoundProps();
			EditorUtility.SetDirty(soundManager);
			AssetDatabase.SaveAssets();
			
		}
		
		DrawFields();
		

		GUILayout.EndVertical();
	}

	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		if(field.Name=="soundProps")
		{
			listEditor.target = soundManager.soundProps;
			listEditor.Draw(new Rect(0,0,editorRect.width,600)); 
			return true;
		}

		return base.DrawField (value, field);
	}

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
}
