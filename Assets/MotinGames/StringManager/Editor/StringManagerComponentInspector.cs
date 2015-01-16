using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
[CustomEditor(typeof(StringManagerComponent))]
public class StringManagerComponentInspector : Editor {
	
	const string defaultGameplaySettingsName = "GameplaySettings";
	
	public string[] allStringNames = null;
	public List<string> filteredStringNames =null;
	//string selectedMotinString;
	public StringManagerComponent stringComponent = null;
	public override void OnInspectorGUI()
    {
		stringComponent = (StringManagerComponent)target;
		Initialize();
		GUILayout.BeginVertical( GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

		bool newForeceUpperCase = EditorGUILayout.Toggle("Force Upper Case",stringComponent.forceUpperCase);
		if(newForeceUpperCase!=stringComponent.forceUpperCase)
		{
			stringComponent.forceUpperCase = newForeceUpperCase;
			stringComponent.UpdateString();
		}
		if(GUILayout.Button("UPDATE LABEL"))
		{
			stringComponent.UpdateString();
		}
		
		MotinStrings tmpString = (MotinStrings)EditorGUILayout.EnumPopup("String: ",MotinUtils.StringToEnum<MotinStrings>(selectedString));
		if(tmpString.ToString() != selectedString)
		{
			//selectedMotinString = tmpString;
			selectedString = tmpString.ToString(); 
			//stringComponent.motinStringName = selectedString;
			stringComponent.SetString(MotinUtils.StringToEnum<MotinStrings>(selectedString));
			
			EditorUtility.SetDirty(stringComponent);
			AssetDatabase.SaveAssets();
		}
		
		if (allStringNames != null)
		{
			GUILayout.Space(8);
			string newSearchFilter = GUILayout.TextField(searchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
			if (newSearchFilter != searchFilter)
			{
				searchFilter = newSearchFilter;
				FilterStrings();
			}
			if (searchFilter.Length > 0)
			{
				if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(true)))
				{
					searchFilter = "";
					FilterStrings();
				}
			}
			else
			{
				GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
			}
		}
		DrawList();
		GUILayout.Space(20);
		GUILayout.EndVertical();
		
	}
	
	public void Initialize()
	{
		if(allStringNames==null)
		{
			allStringNames =MotinUtils.EnumNames<MotinStrings>();
			//filteredStringNames = new List<string>();
		}
		//selectedMotinString = stringComponent.motinStringName;
		if(stringComponent.motinStringName.Length==0)
		{
			stringComponent.motinStringName = ((MotinStrings)0).ToString();
		}
		selectedString =stringComponent.motinStringName;
		FilterStrings();
	}
	
	Vector2 listScroll = Vector2.zero;
	string  selectedString;
	int _leftBarWidth = 200;
	int minLeftBarWidth = 150;
	int leftBarWidth { get { return _leftBarWidth; } set { _leftBarWidth = Mathf.Max(value, minLeftBarWidth); } }

	void DrawList()
	{
		listScroll = GUILayout.BeginScrollView(listScroll,GUILayout.ExpandWidth(true),GUILayout.MinHeight(400));
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
				stringComponent.SetString(MotinUtils.StringToEnum<MotinStrings>(selectedString));
				GUIUtility.keyboardControl = 0;
				EditorUtility.SetDirty(stringComponent);
				AssetDatabase.SaveAssets();
				Repaint();
			}
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		//Rect viewRect = GUILayoutUtility.GetLastRect();
		//leftBarWidth = (int)tk2dGuiUtility.DragableHandle(4819283, 
		//viewRect, leftBarWidth, tk2dGuiUtility.DragDirection.Horizontal);
	}
	
	public bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	string searchFilter = "";
	
	void FilterStrings()
	{
		//Debug.Log("FilterEntityDatas count "  + allEntityData.Count);
		filteredStringNames = new List<string>(allStringNames.Length);
		if (searchFilter.Length == 0)
			filteredStringNames = (from data in allStringNames  orderby data select data).ToList();
		else
			filteredStringNames = (from data in allStringNames where  Contains(data, searchFilter) orderby data select data).ToList();
		
		//Debug.Log("FilterEntityDatas  " + filteredDatas.Count);
	}

}
