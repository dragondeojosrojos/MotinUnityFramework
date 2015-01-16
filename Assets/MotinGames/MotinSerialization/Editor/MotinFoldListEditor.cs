
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MotinGames
{

public class MotinFoldListEditor  : MotinEditor {
	
	protected GUIContent[] menuItems = new GUIContent[0];
	//estaba creando los menues
	
	
	protected int handleId = 0;
	protected int leftBarWidth_ = 150;
	
	protected int minLeftBarWidth = 150;
	public int leftBarWidth { get { return leftBarWidth_; } set { leftBarWidth_ = Mathf.Max(value, minLeftBarWidth); } }

	protected  string searchFilter = "";
	
	//protected System.Type dataType = null;
	
	protected  List<object> 	objectList_ = null;
	//protected  List<MotinSerializableData> allEntityData = new List<MotinSerializableData>();
	protected  List<object> filteredObjects = new List<object>();
	protected  object _selectedObject;
	protected  int selectedObjectIndex = -1;
	
	protected  List<MotinEditor> 	motinEditors_ = new List<MotinEditor>();
	protected  List<MotinEditor> 	filteredEditors_ = new List<MotinEditor>();
	
	protected bool cancelUpdate = false;
	
	public void UpdateEditors()
	{
		if(objectList==null  || objectList.Count==0)
			return;
		
		motinEditors_.Clear();
		MotinEditor editor;
		foreach(object data in objectList_)
		{
			CreateInitializedEditor(data.GetType(),data);
		}
	}
	protected override void targetUpdated ()
	{
		base.targetUpdated ();
		
		if( MotinUtils.IsTypeDerivedFrom(target.GetType(),typeof(ScriptSequenceData)))
			objectList =((ScriptSequenceData)target).childDatas.Cast<object>().ToList();
	}
	protected virtual void objectListUpdated()
	{
		UpdateEditors();
				
		searchFilter = "";
		OrderList();

		FilterList();
	
			
		if (selectedObjectIndex != -1 && selectedObjectIndex < objectList_.Count)
		{
			selectedObject = objectList_[selectedObjectIndex];
		}
		else
		{
			selectedObject = null;
		}
				
		Repaint();
	}
	object selectedObject
	{
		get { return _selectedObject; }
		set 
		{
			selectedObjectIndex = -1;
			if (value != null)
			{
				for (int i = 0; i < objectList.Count; ++i)
				{
					if (objectList[i] == value)
					{
						_selectedObject = value;
						selectedObjectIndex = i;
						break;
					}
				}
			}
			if (selectedObjectIndex == -1)
			{
				if (value != null) Debug.LogError("Unable to find clip");
				_selectedObject = null;
			}
		}
	}

	public List<object> objectList
	{
		get{
			return objectList_;
		}
		set{

			if(value!=null)
			{ 
				objectList_ =value;
				objectListUpdated();
			}
		}
	}
	
	public MotinFoldListEditor( ):base()
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	
	public MotinFoldListEditor(EditorWindow host): base(host)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
		//dataType = type;
		//dataEditor = editor;
	}
	
	public virtual void OrderList()
	{
		//if (searchFilter.Length == 0)
			filteredObjects = (from data in objectList  select data).ToList();
		
		//else
		//	filteredObjects = (from data in objectList where  Contains(data.name, searchFilter)  select data).ToList();
		
		//if (searchFilter.Length == 0)
			filteredEditors_ = (from data in motinEditors_  select data).ToList();
		//else
		//	filteredEditors_ = (from data in motinEditors_ where  Contains( ((MotinSerializableData)data.target).name, searchFilter)  select data).ToList();
	}

	public virtual void UpdateCreateMenu()
	{
		
	}
	public void ObjectSelectionChanged(MotinData data, int direction)
	{
		int selectedId = -1;
		for (int i = 0; i < filteredObjects.Count; ++i)
		{
			if (filteredObjects[i] == data)
			{
				selectedId = i;
				break;
			}
		}
		if (selectedId != -1)
		{
			int newSelectedId = selectedId + direction;
			if (newSelectedId >= 0 && newSelectedId < filteredObjects.Count)
			{
				selectedObject = filteredObjects[newSelectedId];	
			}
		}
		Repaint();
	}
	
	protected virtual string UniqueDataName(List<object> allData, string baseName)
	{
		return baseName;
	}
	
	public static bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	
	public void FilterList()
	{
		if(objectList==null || objectList.Count==0)
		{
			filteredObjects.Clear();
			filteredEditors_.Clear();
			return;
		}
		
		filteredObjects = new List<object>(objectList.Count);
		filteredEditors_ = new List<MotinEditor>(objectList.Count);
		OrderList();	
	
		//Debug.Log("FilterList  " + filteredObjects.Count);
	}
	
	public void OnDeletePressedCallback(MotinEditor editor)
	{
		int index = objectList_.IndexOf(editor.target);
		objectList_.RemoveAt(index);
		motinEditors_.RemoveAt(index);
		/*
		if(objectList_.Count ==0)
			CreateNewData();
		*/
		
		FilterList();
		
		if(objectList.Count==0)
		{
			selectedObjectIndex =-1;
			selectedObject = null;
		}
		else
		{
			selectedObjectIndex =0;
			selectedObject = objectList[selectedObjectIndex];
		}
		RaiseOnDataChanged();
		
		cancelUpdate = true;
		Repaint();
		
	
	}
	
	
	public void OnUpPressedCallback(MotinEditor editor)
	{

		int index = objectList_.IndexOf(editor.target);
		Debug.Log("On Up Pressed " + editor.editorName + " " + index);
		if(index<=0)
			return;
		
		
		object auxData = objectList_[index -1];
		objectList_[index -1]  =  objectList_[index];
		objectList_[index] = auxData;

		MotinEditor auxEditor = motinEditors_[index -1];
		motinEditors_[index -1]  =  motinEditors_[index];
		motinEditors_[index] = auxEditor;

		FilterList();
		RaiseOnDataChanged();
		
		cancelUpdate = true;
		Repaint();
	}
	public virtual void OnDataChangedEditorCallback(MotinEditor editor)
	{
		RaiseOnDataChanged();
	}
	public void OnDownPressedCallback(MotinEditor editor)
	{
		int index = objectList_.IndexOf((MotinData)editor.target);
		
		if(index+1>=objectList_.Count)
			return;
		
		
		object auxData = objectList_[index +1];
		objectList_[index +1]  =  objectList_[index];
		objectList_[index] = auxData;

		MotinEditor auxEditor = motinEditors_[index +1];
		motinEditors_[index +1]  =  motinEditors_[index];
		motinEditors_[index] = auxEditor;

		FilterList();
		RaiseOnDataChanged();
		cancelUpdate = true;
		Repaint();
		
		
	}
	protected virtual MotinEditor CreateInitializedEditor(Type dataType,object target)
	{
		MotinEditor editor = CreateEditor(dataType);
		editor.OnDeletePressed+=OnDeletePressedCallback;
		editor.OnUpPressed += OnUpPressedCallback;
		editor.OnDownPressed += OnDownPressedCallback;
		editor.OnDataChangedEditor += OnDataChangedEditorCallback;
		editor.showListToolbar = true;
		editor.target = target;
		motinEditors_.Add(editor);
		return editor;
	}
	protected virtual MotinEditor CreateEditor(Type dataType)
	{
		//Debug.Log (" DATA TYPE " + dataType.Name);
		Type editorType = Types.GetType(dataType.Name + "Editor","Assembly-CSharp-Editor");
		if(editorType!=null)
		{
			MotinEditor editor = (MotinEditor)System.Activator.CreateInstance(editorType);
			return editor;
		}
	
		if(MotinUtils.IsTypeDerivedFrom(dataType,typeof(MotinData)))
		{
			return new MotinDataEditor(hostEditorWindow);
		}
		
		return new MotinEditor(hostEditorWindow);
		
	}

	protected virtual bool DoCreateMenu(int selected)
	{
		return false;
	}
	protected void DrawCreateMenu()
	{
		if(menuItems == null || menuItems.Length ==0)
			return;
		
		// Create Button
		GUIContent createButton = new GUIContent("Create");
		Rect createButtonRect = GUILayoutUtility.GetRect(createButton, EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(false));
		if (GUI.Button(createButtonRect, createButton, EditorStyles.toolbarDropDown))
		{
			GUIUtility.hotControl = 0;
			EditorUtility.DisplayCustomMenu(createButtonRect, menuItems, -1, 
				delegate(object userData, string[] options, int selected) {
					if (selected >= 0)
					{
							if(DoCreateMenu(selected))
							{
								RaiseOnDataChanged();
								Repaint();
							}
					}
				}
				, null);
		}
	}
	
	protected override void DrawToolbarButtons ()
	{
		// Create Button
		
		// LHS
		GUILayout.BeginHorizontal(GUILayout.Width(150));

		// Filter box
		if (objectList != null)
		{
			GUILayout.Space(8);
			string newSearchFilter = GUILayout.TextField(searchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
			if (newSearchFilter != searchFilter)
			{
				searchFilter = newSearchFilter;
				FilterList();
			}
			if (searchFilter.Length > 0)
			{
				if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(false)))
				{
					searchFilter = "";
					FilterList();
				}
			}
			else
			{
				GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
			}
		}
		
		GUILayout.EndHorizontal();
		
		// RHS
		//GUILayout.Label(editorName);
		//GUILayout.FlexibleSpace();
		DrawCreateMenu();
	
	}

	protected override void DoDraw()
	{ 
		cancelUpdate = false;
		//MotinEditor[] editors = motinEditors_.ToArray();
		if (objectList != null )
		{
			//GUILayout.BeginVertical(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
			DrawFields();
			listScroll = GUILayout.BeginScrollView(listScroll,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
				GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		
				int index =0;
				//Debug.Log("FILTERED DATAS " + filteredObjects.Count);
				foreach (object data in filteredObjects)
				{
				    GUILayout.BeginHorizontal(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
					//motinEditors_[index].target = data;
				
					DrawItem(index);
				
				   	GUILayout.EndHorizontal();
					index++;
					
					if(cancelUpdate)
						break;
				}
				
				GUILayout.EndVertical();
			GUILayout.EndScrollView();
			//GUILayout.EndVertical();
		}

	}
	
	Vector2 listScroll = Vector2.zero;
	protected virtual void DrawItem(int index)
	{
		filteredEditors_[index].Draw(new Rect(0,0,editorRect.width,600));
	}
	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		
		if(field.Name== "")
		{
			return true;
		}
		return base.DrawField (value, field);
	}


}
}



	
