using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MotinGames
{

public class MotinArrayBaseEditor  : MotinEditor {

	public bool childEditorsExpandHeight = true;
	public List<Type> 	typesList = new List<Type>();
	public List<string> namespaceList = new List<string>();
	protected GUIContent[] menuItems = new GUIContent[0];

			
	protected  string searchFilter = "";

	protected  List<object> 	objectList_ =  new List<object>();
	protected  List<object> 	filteredObjects = new List<object>();
	protected  object 			_selectedObject;
	protected  int 				selectedObjectIndex = -1;
	
	protected  List<MotinEditor> 	motinEditors_ = new List<MotinEditor>();
	protected  List<MotinEditor> 	filteredEditors_ = new List<MotinEditor>();
	
	protected bool cancelUpdate = false;
	
	public void UpdateEditors()
	{
		if(objectList==null  || objectList.Count==0)
			return;
		
		motinEditors_.Clear();
		//MotinEditor editor;
		foreach(object data in objectList_)
		{
			CreateInitializedEditor(data.GetType(),data);
		}
	}
	protected override void targetUpdated ()
	{
		base.targetUpdated ();

			//Debug.Log("Target Type = " + target.GetType().ToString() );
		if(target.GetType().IsGenericType && target.GetType().GetGenericTypeDefinition() == typeof(List<>))
			objectList =((IList)target).Cast<object>().ToList();

		

		//if( MotinUtils.IsTypeDerivedFrom(target.GetType(),typeof(ScriptSequenceData)))
		
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
	protected object selectedObject
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
				
			}
			else
			{
				objectList_ = new List<object>();
			}
			objectListUpdated();
		}
	}
	public MotinArrayBaseEditor( ):base()
	{
		UpdateCreateMenu();
	}
	public MotinArrayBaseEditor(System.Type type ):base()
	{
		AddType(type);
		UpdateCreateMenu();
	}

	public MotinArrayBaseEditor(EditorWindow host,System.Type type): base(host)
	{
		AddType(type);
		UpdateCreateMenu();
	}
	public MotinArrayBaseEditor(string NamespaceName ):base()
	{
		AddNamespace(NamespaceName);
		UpdateCreateMenu();
	}
	
	public MotinArrayBaseEditor(EditorWindow host,string NamespaceName): base(host)
	{
		AddNamespace(NamespaceName);
		UpdateCreateMenu();
	}

	public void AddType(System.Type type)
	{
		if(type ==null  || typesList.Contains(type))
			return;
			
		typesList.Add(type);
		UpdateCreateMenu();
	}

	public void AddNamespace(string value)
	{
		if(value ==null || value== "" || namespaceList.Contains(value))
			return;

		namespaceList.Add(value);
		UpdateCreateMenu();
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
		System.Type[] types = null;
		List<GUIContent> Items = new List<GUIContent>();
		foreach(string namespaceName in namespaceList)
		{
			
			types = MotinUtils.GetTypesInNamespace(System.Reflection.Assembly.Load("Assembly-CSharp"),namespaceName);
			foreach(Type type in types)
			{
				Items.Add(new GUIContent(type.FullName));
			}
		}

		foreach(Type type in typesList)
		{
			Items.Add(new GUIContent(type.FullName));
		}

		this.menuItems = Items.ToArray();
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

		cancelUpdate = true;
		objectListUpdated();

		RaiseOnDataChanged();
		

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
		
		//FilterList();

		cancelUpdate = true;
		objectListUpdated();

		RaiseOnDataChanged();
		

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
		

		cancelUpdate = true;
		objectListUpdated();

		//FilterList();
		RaiseOnDataChanged();

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
		Debug.Log("DoCreateMenu  " + selected);
		object newData = null;
		//MotinEditor newEditor =null;
		
		newData = MotinDataManager.InstanceFromName(menuItems[selected].text);
		
		
		if(newData!=null)
		{
			InitializeCreatedObject(newData);
			objectList_.Add(newData);
			
			CreateInitializedEditor(newData.GetType(),newData);
			objectListUpdated();
			//FilterList();
			return true;
		}

		return false;
		//RaiseOnDataChanged();
		

	}
	protected virtual void InitializeCreatedObject(object newObject)
	{
        if(newObject.GetType().IsSubclassOf(typeof(MotinData)))
			((MotinData)newObject).SetDefaultValues();
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
		//	childEditorsExpandHeight = EditorGUILayout.Toggle ("expand child",childEditorsExpandHeight);
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
			//DrawFields();
			listScroll = GUILayout.BeginScrollView(listScroll,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
			
			//int index =0;
			//Debug.Log("FILTERED DATAS " + filteredObjects.Count);
			for(int i = 0 ; i < filteredObjects.Count;i++)
			{
				GUILayout.BeginHorizontal(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true)/*, GUILayout.ExpandHeight(true)*/);
				//motinEditors_[index].target = data;
				
				DrawItem(i);
				
				GUILayout.EndHorizontal();
				//index++;
				
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
			filteredEditors_[index].Draw(new Rect(0,0,editorRect.width,600),childEditorsExpandHeight);
	}
	/*
	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		if(field.Name== "")
		{
			return true;
		}
		return base.DrawField (value, field);
	}
	*/
	
	
}
}




