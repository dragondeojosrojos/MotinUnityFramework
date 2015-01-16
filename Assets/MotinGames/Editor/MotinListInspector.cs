using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MotinListInspector  {
	// "Create" menu
	GUIContent[] menuItems = new GUIContent[0];
	int minLeftBarWidth = 300;
	int	_leftBarWidth =200;
	int leftBarWidth { get { return _leftBarWidth; } set { _leftBarWidth = Mathf.Max(value, minLeftBarWidth); } }
	public object target;
	string searchFilter = "";
	protected List<string> allNames = new List<string>();
	protected  List<string> filteredNames = new List<string>();
	protected  int selectedIndex =0;
	
	public MotinListInspector()
    {
		//target = Target;
		Init();
	}
	public void Draw()
	{
		DrawToolbar();
		
		if(allNames.Count>0)
		{
			if(selectedIndex<0)
			{
				selectedIndex=0;
			}
			GUILayout.BeginHorizontal();
			DrawList();
			DrawSelectedItemInspector();
			GUILayout.EndHorizontal();
		}
	}
	
	public void Init()
	{
		List<GUIContent> menuItems = new List<GUIContent>();
		menuItems.Add(new GUIContent("Currency"));
			//LoadEntityFile();
		this.menuItems = menuItems.ToArray();
		UpdateNames();
		FilterString();
		if(allNames.Count==0)
		{
			selectedIndex = -1;
		}
		else
		{
			if(selectedIndex>allNames.Count)
			{
				selectedIndex = allNames.Count-1;
			}
		}
	}
	
	
	public bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	public void FilterString()
	{
		//Debug.Log("FilterEntityDatas count "  + allEntityData.Count);
		filteredNames = new List<string>(allNames.Count);
		if (searchFilter.Length == 0)
			filteredNames = (from data in allNames  orderby data select data).ToList();
		else
			filteredNames = (from data in allNames where  Contains(data, searchFilter) orderby data select data).ToList();
		
		//Debug.Log("FilterEntityDatas  " + filteredDatas.Count);
	}
	public virtual void UpdateNames()
	{
			
	}
	public virtual void SelectedChanged()
	{
		
	}
	public virtual void OnDeleteSelected()
	{

	}
	public virtual void DrawSelectedItemInspector()
	{
		
		if(target!=null)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			
				if(GUILayout.Button("delete"))
				{
					target = null;
					OnDeleteSelected();
					UpdateNames();
					FilterString();
					selectedIndex = Mathf.Clamp(selectedIndex,-1,-1+allNames.Count);
					SelectedChanged();
					return;
				}
			EditorGUILayout.EndHorizontal();
			
			System.Reflection.FieldInfo[] fields = target.GetType().GetFields();
			EditorGUI.indentLevel++;
			foreach(System.Reflection.FieldInfo field in fields)
			{
			  if(field.IsPublic && !field.IsStatic)
			  {
				DrawField(field);
			  }         
			 }
			EditorGUILayout.EndVertical();
		}
	}
	public virtual void DrawField(System.Reflection.FieldInfo field)
	{
		MotinEditorUtils.DrawDefaultField(target,field);
	}
	
	public virtual void OnCreateClick()
	{
		
	}
	public virtual void Commit()
	{
		
	}
	
	public virtual void Revert()
	{
		
	}
	
	
	protected string UniqueName(string baseName)
		{
			bool found = false;
			for (int i = 0; i < allNames.Count; ++i)
			{
				if (allNames[i] == baseName) 
				{ 
					found = true; 
					break; 
				}
			}
			if (!found) return baseName;

			string uniqueName = baseName + " ";
			int uniqueId = 1;
			for (int i = 0; i < allNames.Count; ++i)
			{
				string uname = uniqueName + uniqueId.ToString();
				if (allNames[i] == uname)
				{
					uniqueId++;
					i = -1;
					continue;
				}
			}
			uniqueName = uniqueName + uniqueId.ToString();
			return uniqueName;
		}
	
	void DrawToolbar()
	{
		GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
		
		// LHS
			GUILayout.BeginHorizontal(GUILayout.Width(leftBarWidth - 6));
			
				// Create Button
				GUIContent createButton = new GUIContent("Create");
				Rect createButtonRect = GUILayoutUtility.GetRect(createButton, EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(false));
				if (GUI.Button(createButtonRect, createButton, EditorStyles.toolbarDropDown) )
				{
					GUIUtility.hotControl = 0;
					EditorUtility.DisplayCustomMenu(createButtonRect, menuItems, -1, 
						delegate(object userData, string[] options, int selected) {
							if (selected == 0)
							{
									OnCreateClick();
									UpdateNames();
									FilterString();
									//clipEditor.InitForNewClip();
									//Repaint();
							}
						
						}
						, null);
				}
				
				// Filter box
				if (allNames.Count>0)
				{
					GUILayout.Space(8);
					string newSearchFilter = GUILayout.TextField(searchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
					if (newSearchFilter != searchFilter)
					{
						searchFilter = newSearchFilter;
						FilterString();
					}
					if (searchFilter.Length > 0)
					{
						if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(false)))
						{
							searchFilter = "";
							FilterString();
						}
					}
					else
					{
						GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
					}
				}
		
			GUILayout.EndHorizontal();

		// RHS
		GUILayout.FlexibleSpace();
		
		if (allNames.Count>0 && GUILayout.Button("Revert", EditorStyles.toolbarButton))
			Revert();
		
		if (allNames.Count>0 && GUILayout.Button("Commit", EditorStyles.toolbarButton))
			Commit();
		
		GUILayout.EndHorizontal();
	}
	
	Vector2 listScroll = Vector2.zero;
	void DrawList()
	{
		listScroll = GUILayout.BeginScrollView(listScroll, GUILayout.Width(leftBarWidth));
		GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.Height(400));
		int index =0;
		foreach (string data in filteredNames)
		{
			// 0 length name signifies inactive clip
			if (data.Length == 0) continue;

			bool selected = selectedIndex == allNames.IndexOf(data);
			bool newSelected = GUILayout.Toggle(selected, data, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
			if (newSelected != selected && newSelected == true)
			{
				selectedIndex = allNames.IndexOf(data);
				GUIUtility.keyboardControl = 0;
				SelectedChanged();
//				Repaint();
			}
			index++;
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();

		Rect viewRect = GUILayoutUtility.GetLastRect();
		leftBarWidth = (int)MotinGuiUtility.DragableHandle(4819283, 
			viewRect, leftBarWidth, 
			MotinGuiUtility.DragDirection.Horizontal);
	}
/*
	void TrimClips()
	{
		if (allNames.Count < 1)
			return;

		int validCount = allNames.Count;
		while (validCount > 0 )
			--validCount;
		
		allNames.RemoveRange(validCount, allNames.Count - validCount);

		if (allNames.Count == 0)
		{
			allNames.Add("");
			FilterString();
		}
	}
*/
}
