using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[CustomEditor(typeof(MotinStringManager))]
public class StringManagerInspector : Editor {
	
	const string defaultGameplaySettingsName = "GameplaySettings";
	
	public List<StringManagerComponent>	allLabels = new List<StringManagerComponent>();
	public List<StringManagerComponent>	filteredLabels = new List<StringManagerComponent>();
	
	public StringManagerComponent selectedLabel = null;
	
	//string selectedMotinString;
	public MotinStringManager stringManager = null;
	public override void OnInspectorGUI()
    {
		stringManager = (MotinStringManager)target;
		UpdateLabelObjects();
		
		
		GUILayout.BeginVertical(GUI.skin.box,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			if (GUILayout.Button("Reload Strings File", GUILayout.ExpandWidth(true)))
			{
				stringManager.UpdateFromFile();
			}
			if (GUILayout.Button("Update Labels", GUILayout.ExpandWidth(true)))
			{
				UpdateLabelObjects();
			}
		
		GUILayout.EndHorizontal();
		
		LANG	newLang =(LANG) EditorGUILayout.EnumPopup("Language:",stringManager.defaultLang);
		if(newLang!=stringManager.defaultLang)
		{
			stringManager.defaultLang = newLang;
			stringManager.SetLanguage(stringManager.defaultLang);
			
			EditorUtility.SetDirty(stringManager);
			AssetDatabase.SaveAssets();
		}
	
		GUILayout.Label("Labels Found " + allLabels.Count);
		if (allLabels.Count > 0)
		{
			GUILayout.Space(8);
			string newSearchFilter = GUILayout.TextField(searchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
			if (newSearchFilter != searchFilter)
			{
				searchFilter = newSearchFilter;
				FilterLabels();
			}
			if (searchFilter.Length > 0)
			{
				if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(true)))
				{
					searchFilter = "";
					FilterLabels();
				}
			}
			else
			{
				GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
			}
		}
		
		DrawList();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUI.skin.box,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
		DrawSelectedComponent();
		
		GUILayout.EndVertical();

	}
	public void UpdateLabelObjects()
	{
		
		if(allStringNames==null)
		{
			allStringNames =MotinUtils.EnumNames<MotinStrings>();
			//filteredStringNames = new List<string>();
		}
		//selectedMotinString = stringComponent.motinStringName;
		
		FilterStrings();

		allLabels.Clear();

		StringManagerComponent[]	foundObjects =(StringManagerComponent[])Resources.FindObjectsOfTypeAll(typeof(StringManagerComponent));
		//StringManagerComponent	tmpText;
		foreach(StringManagerComponent obj in foundObjects)
		{
			//tmpText = obj.GetComponent<StringManagerComponent>();
			//if(tmpText!=null)
			//{
			if(PrefabUtility.GetPrefabParent(obj) == null && PrefabUtility.GetPrefabObject(obj) != null) 
				continue;
			
				allLabels.Add(obj);
			//}
		}
		FilterLabels();
		
		
	}
	
	Vector2 listScroll = Vector2.zero;
	//string  selectedString;
	int _leftBarWidth = 200;
	int minLeftBarWidth = 150;
	int leftBarWidth { get { return _leftBarWidth; } set { _leftBarWidth = Mathf.Max(value, minLeftBarWidth); } }
	void DrawList()
	{
		//int selectedIndex =0;
		listScroll = GUILayout.BeginScrollView(listScroll,GUILayout.ExpandWidth(true),GUILayout.Height(250));
		//GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.Height(2000));
		GUILayout.BeginVertical( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		foreach (StringManagerComponent data in filteredLabels)
		{
			// 0 length name signifies inactive clip
			if (data.labelName.Length == 0) continue;

			bool selected = selectedLabel == data;
			bool newSelected = GUILayout.Toggle(selected, data.labelName, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
			if (newSelected != selected && newSelected == true)
			{
				selectedLabel = data;
				//selectedMotinString = MotinUtils.StringToEnum<MotinStrings>(selectedString);
				//selectedLabel = GetStringManagerComponent(selectedString);
				//stringComponent.motinStringName = selectedString;
				GUIUtility.keyboardControl = 0;
				
				
				Repaint();
			}
			//selectedIndex++;
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		//Rect viewRect = GUILayoutUtility.GetLastRect();
		//leftBarWidth = (int)tk2dGuiUtility.DragableHandle(4819283, 
		//viewRect, leftBarWidth, tk2dGuiUtility.DragDirection.Horizontal);
	}
	
	public bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	string searchFilter = "";
	
	
	
	public string[] allStringNames = null;
	public List<string> filteredStringNames =null;
	public string selectedString;
	string stringSearchFilter = "";
	void DrawSelectedComponent()
	{
		if(selectedLabel==null)
			return;
		
		if(selectedLabel.motinStringName.Length==0)
		{
			selectedLabel.motinStringName = ((MotinStrings)0).ToString();
		}
		selectedString =selectedLabel.motinStringName;
		
		GUILayout.BeginVertical( GUILayout.ExpandWidth(true));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Label Name:");
		selectedLabel.labelName = GUILayout.TextField(selectedLabel.labelName);
		if(GUILayout.Button("Ping"))
		{
			EditorGUIUtility.PingObject(selectedLabel);
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("CURRENT STRING: " + selectedString);
		
		if (allStringNames.Length > 0)
		{
			GUILayout.Space(8);
			string newSearchFilter = GUILayout.TextField(stringSearchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
			if (newSearchFilter != stringSearchFilter)
			{
				stringSearchFilter = newSearchFilter;
				FilterStrings();
			}
			if (stringSearchFilter.Length > 0)
			{
				if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(true)))
				{
					stringSearchFilter = "";
					FilterStrings();
				}
			}
			else
			{
				GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
			}
		}
		
		DrawMotinStringList();
		GUILayout.EndVertical();
	}
	Vector2 stringListScroll = Vector2.zero;
	void DrawMotinStringList()
	{
		
		stringListScroll = GUILayout.BeginScrollView(stringListScroll,GUILayout.ExpandWidth(true),GUILayout.Height(150));
		//GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.Height(2000));
		GUILayout.BeginVertical( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		foreach (string data in filteredStringNames)
		{
			// 0 length name signifies inactive clip
			if (data.Length == 0) continue;

			bool selected = selectedString == data;
			bool newSelected = GUILayout.Toggle(selected, data, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
			if (newSelected != selected && newSelected == true)
			{
				selectedString = data;
				//selectedMotinString = MotinUtils.StringToEnum<MotinStrings>(selectedString);
				selectedLabel.SetString( MotinUtils.StringToEnum<MotinStrings>(selectedString));
				
				EditorUtility.SetDirty(selectedLabel);
				AssetDatabase.SaveAssets();
				GUIUtility.keyboardControl = 0;
				Repaint();
			}
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		//Rect viewRect = GUILayoutUtility.GetLastRect();
		//leftBarWidth = (int)tk2dGuiUtility.DragableHandle(4819283, 
		//viewRect, leftBarWidth, tk2dGuiUtility.DragDirection.Horizontal);
	}
	
	
	void FilterStrings()
	{
		//Debug.Log("FilterEntityDatas count "  + allEntityData.Count);
		filteredStringNames = new List<string>(allStringNames.Length);
		if (stringSearchFilter.Length == 0)
			filteredStringNames = (from data in allStringNames  orderby data select data).ToList();
		else
			filteredStringNames = (from data in allStringNames where  Contains(data, stringSearchFilter) orderby data select data).ToList();
		
		//Debug.Log("FilterEntityDatas  " + filteredDatas.Count);
	}
	
	void FilterLabels()
	{
		//Debug.Log("FilterEntityDatas count "  + allEntityData.Count);
		filteredLabels = new List<StringManagerComponent>(allLabels.Count);
		if (searchFilter.Length == 0)
			filteredLabels = (from data in allLabels  orderby data.labelName select data).ToList();
		else
			filteredLabels = (from data in allLabels where  Contains(data.labelName, searchFilter) orderby data.labelName select data).ToList();
		
		//Debug.Log("FilterEntityDatas  " + filteredDatas.Count);
	}
	
	
}