using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

public class MotinDataEditorWindow : EditorWindow  {
	

	public string currentFilePath =  "";
	public EditorWindow window = null;
	
	protected List<MotinData> loadedData = new List<MotinData>();

	protected MotinArrayBaseEditor dataEditor = null;
	

	protected virtual void OnEnable()
	{
		//Debug.Log("ON ENABLE ENTITY WINDOW");
		if(window == null)
		{
			this.autoRepaintOnSceneChange = true;
			
			// Create clip editor
			if (dataEditor == null)
			{
				CreateDataEditor();
			}
			
		}
		
		window = this;
		
		if(currentFilePath.Length>0)
			Load(currentFilePath);
		// Create menu items
		
		//clipEditor.animOps = animOps;
	}
	
	protected virtual void CreateDataEditor()
	{
		dataEditor =new MotinArrayBaseEditor();
	}
	
	public void Load(string path)
	{
		currentFilePath = path;
		MotinData[] loadedDefs =  MotinDataManager.Load(path);
		loadedData.Clear();
		if(loadedDefs!=null)
		{

			for(int i = 0 ; i < loadedDefs.Length ; i++)
			{
				loadedData.Add(loadedDefs[i]);
			}
		}
		
		OnDataLoaded();

		if(dataEditor!=null)
			dataEditor.target = loadedData;
		
		EditorUtility.SetDirty(this);
	}

	protected virtual void OnDataLoaded()
	{

	}
	/*
	void MirrorEntityDatas()
	{
		if (motinDatas == null) return;

		allEntityData.Clear();
		if (motinDatas != null)
		{
			foreach (MotinData data in motinDatas)
			{
				//BiomaEntityData d =(BiomaEntityData) MotinDataManager.CreateData("BiomaEntityData");
				//d.CopyFrom(data);
//				Debug.Log("Mirror Data " + data.name);
				allEntityData.Add(data);
			}
		}
	}
	*/
	void OnDisable()
	{
		//Debug.Log("ON DISABLE");
		OnDestroy();
	}

	void OnDestroy()
	{
//		Debug.Log("ON DESTRY");
		window = null;


		if (dataEditor != null)
			dataEditor = null;
	}
	
	
	void Commit()
	{
		if(dataEditor==null)
			return;

		if (dataEditor.objectList.Count == 0) return;
		//Debug.Log("Commiting Entity Definitions");
		// Handle duplicate names
		string dupNameString = "";
		HashSet<string> duplicateNames = new HashSet<string>();
		HashSet<string> names = new HashSet<string>();

		List<MotinData> motinDatas = dataEditor.objectList.Cast<MotinData>().ToList();
		foreach (MotinData data in motinDatas)
		{
			//if (data.Empty) continue;
			if (names.Contains(data.name)) { duplicateNames.Add(data.name); dupNameString += data.name + " "; continue; }
			names.Add(data.name);
		}
		if (duplicateNames.Count > 0)
		{
			int res = EditorUtility.DisplayDialogComplex("Commit",
				"Duplicate names found in entity definition library. You won't be able to select duplicates in the interface.\n" +
				"Duplicates: " + dupNameString, 
				"Auto-rename",
				"Cancel",
				"Force commit");

			if (res == 1) return; // cancel
			if (res == 0)
			{
				// auto rename
				HashSet<string> firstOccurances = new HashSet<string>();
				foreach (MotinData data in motinDatas)
				{
					//if (data.Empty) continue;
					string name = data.name;
					if (duplicateNames.Contains(name))
					{
						if (!firstOccurances.Contains(name))
						{
							firstOccurances.Add(name);
						}
						else
						{
							// find suitable name
							int i = 1;
							string n = "";
							do 
							{
								n = string.Format("{0} {1}", name, i++);
							} while (names.Contains(n));
							name = n;

							names.Add(name);
							data.name = name;
						}
					}
				}
				
				Repaint();
			}
		}

		//EditorUtility.SetDirty(motinDatas);
		
		MotinDataManager.Save(currentFilePath,motinDatas);
		
		loadedData = motinDatas;
		if(dataEditor!=null)
		{
			dataEditor.FilterList();	
		}


		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		if(dataEditor!=null && dataEditor.objectList!=null)
		{	
			dataEditor.OrderList();
			dataEditor.FilterList();
		}
		//Debug.Log("Save Assets " );
		OnCommited();
	}

	protected List<string> GetDataNames()
		{
			List<string > dataNames = new List<string>();
			foreach(MotinData data in loadedData)
			{
				dataNames.Add(data.name);
			}

			return dataNames;
		}

	protected virtual void OnCommited()
		{

		}
	void DrawToolbar()
	{
		if(dataEditor==null)
			return;

		GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width((int)window.position.width));
		
		// RHS
		GUILayout.FlexibleSpace();
		
		if (dataEditor.objectList.Count != 0 && GUILayout.Button("Commit", EditorStyles.toolbarButton))
			Commit();
		
		GUILayout.EndHorizontal();
	}
	

	
	public void OnGUI()
	{ 

		GUILayout.BeginVertical(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
		
		DrawToolbar();
		
		if ( dataEditor!=null)
		{
			dataEditor.Draw(new Rect(0,0,window.position.width,window.position.height),true);
		} 
		
		GUILayout.EndVertical();
		
	}
	
	
}

}