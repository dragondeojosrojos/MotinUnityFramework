using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MotinGames
{

public class MotinArrayBaseEditor  : MotinEditor {

	public System.Action<MotinData,int> OnInitializeData = null;
	
	public bool childEditorsExpandHeight = true;
	public List<Type> 	typesList = new List<Type>();
	public List<string> namespaceList = new List<string>();
	
	protected Type[] 	   menuTypes = new Type[0];
	protected GUIContent[] menuItems = new GUIContent[0];

			
	protected  string searchFilter = "";

	protected  List<object> 	objectList_ =  new List<object>();
	protected  List<object> 	filteredObjects = new List<object>();
	protected  object 			_selectedObject;
	protected  int 				selectedObjectIndex = -1;
	
	//protected  List<MotinEditor> 	motinEditors_ = new List<MotinEditor>();

	protected  List<MotinEditor> 	filteredEditors_ = new List<MotinEditor>();
	
	protected bool cancelUpdate = false;
	
	public void UpdateEditors()
	{
		if(objectList==null  || objectList.Count==0)
			return;
		
		//motinEditors_.Clear();
		MotinEditor auxEditor;
		int editorIndex = 0;
		for(int i = 0 ; i < objectList_.Count; i ++)
		{
			if(i >= motinEditors_.Count)
			{
				CreateInitializedEditor(objectList_[i].GetType(),objectList_[i]);
				continue;
			}
			
			editorIndex = GetEditorIndex(objectList_[i]);
			if(editorIndex ==-1)
			{
				CreateInitializedEditor(objectList_[i].GetType(),objectList_[i],i);
			}
			else if(editorIndex == i)
			{
				continue;
			}
			else
			{
				auxEditor = motinEditors_[i];
				motinEditors_[i] =  motinEditors_[editorIndex];
				motinEditors_[editorIndex] =  auxEditor;
			}
		}
		
		for(int i = motinEditors_.Count-1 ; i >= objectList_.Count; i --)
		{
			motinEditors_.RemoveAt(i);
		}
			/*
		foreach(object data in objectList_)
		{
			CreateInitializedEditor(data.GetType(),data);
		}
		*/
	}
		/*
	protected int GetEditorIndex ( object targetObject)
	{
		for(int i = 0 ; i < motinEditors_.Count; i ++)
		{
			if(motinEditors_[i].target == targetObject)
				return i;
		}
		return -1;
	}
	*/
	protected override void targetUpdated ()
	{
		//base.targetUpdated ();

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

		//if (selectedObjectIndex != -1 && selectedObjectIndex < objectList_.Count)
		if (selectedObject != null )
		{
			selectedObject = selectedObject;
		}
		else
		{
			selectedObject = null;
		}
		
		Repaint();
	}
	
	protected void UpdateSelectedObjectIndex()
	{
		if (selectedObject != null)
		{
			for (int i = 0; i < objectList.Count; ++i)
			{
				if (objectList[i] == selectedObject)
				{
					selectedObjectIndex = i;
					break;
				}
			}
		}
		else
		{
			selectedObjectIndex = -1;
		}
	}

	protected object selectedObject
	{
		get { return _selectedObject; }
		set 
		{
			selectedObjectIndex = -1;
			if (value != null)
			{
				for (int i = 0; i < filteredObjects.Count; ++i)
				{
					if (filteredObjects[i] == value)
					{
						_selectedObject = value;
						selectedObjectIndex = i;
						break;
					}
				}
			}
			if (selectedObjectIndex == -1)
			{
				if (value != null) Debug.Log("Unable to find selected object");
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
		//UpdateCreateMenu();
	}

	public MotinArrayBaseEditor(EditorWindow host,System.Type type): base(host,null)
	{
		AddType(type);
		//UpdateCreateMenu();
	}
	public MotinArrayBaseEditor(string NamespaceName ):base()
	{
		AddNamespace(NamespaceName);
		//UpdateCreateMenu();
	}
	
	public MotinArrayBaseEditor(EditorWindow host,string NamespaceName): base(host,null)
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
		List<Type> foundTypes = new List<Type>();

		foreach(string namespaceName in namespaceList)
		{
			
			types = MotinUtils.GetTypesInNamespace(System.Reflection.Assembly.Load("Assembly-CSharp"),namespaceName);
			foreach(Type type in types)
			{
				foundTypes.Add(type);
			}
		}

		foreach(Type type in typesList)
		{
			foundTypes.Add(type);
		}

		MotinEditorClassCreateMenuName[] createMenuAttr = null;
		foreach(Type type in foundTypes)
		{
			createMenuAttr = (MotinEditorClassCreateMenuName[])type.GetCustomAttributes(typeof(MotinEditorClassCreateMenuName),false);
			if(createMenuAttr!=null && createMenuAttr.Length>0)
			{
				Items.Add(new GUIContent(createMenuAttr[0].name));
			}
			else
			{
				Items.Add(new GUIContent(type.FullName));	
			}
		}

		this.menuItems = Items.ToArray();
		this.menuTypes = foundTypes.ToArray();
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
		
		//FilterList();
		objectListUpdated();
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
		

		RaiseOnEditorChanged();
		
		//RaiseOnDataChanged();
		

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

			RaiseOnEditorChanged();
		//RaiseOnDataChanged();
		

		Repaint();
	}
		/*
	public virtual void OnDataChangedEditorCallback(MotinEditor editor)
	{
		RaiseOnDataChanged();
	}
	*/
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

		RaiseOnEditorChanged();
		//FilterList();
		//RaiseOnDataChanged();

		Repaint();
		
		
	}

	
	protected override MotinEditor CreateInitializedEditor (MotinEditor newEditor, object editorTarget, int index)
	{
		MotinEditor editor =  base.CreateInitializedEditor (newEditor, editorTarget, index);
		if(editor==null)
			return null;

		editor.OnDeletePressed+=OnDeletePressedCallback;
		editor.OnUpPressed += OnUpPressedCallback;
		editor.OnDownPressed += OnDownPressedCallback;
		editor.showListToolbar = true;

		return editor;

	}
	
	protected virtual bool DoCreateMenu(int selected)
	{
//		Debug.Log("DoCreateMenu  " + selected);
		object newData = null;
		//MotinEditor newEditor =null;
		
		newData = MotinDataManager.InstanceFromName(menuTypes[selected].FullName);
		
		
		if(newData!=null)
		{
			InitializeCreatedObject(newData);
			objectList_.Add(newData);
			UpdateTargetList();
			if(OnInitializeData!=null)
					OnInitializeData((MotinData)newData,objectList_.Count-1);
			
			CreateInitializedEditor(newData.GetType(),newData);

			objectListUpdated();
			//if(selectedObject ==null)
			selectedObject =newData;
			
			
			//FilterList();
			return true;
		}

		return false;
		//RaiseOnDataChanged();
		

	}
	void UpdateTargetList()
	{
		
		System.Type listType = typeof(List<>);
		Type[] genericArgs = target_.GetType().GetGenericArguments();
		Type concreteType = listType.MakeGenericType(genericArgs);
		object newList = Activator.CreateInstance(concreteType);
		
		foreach(object obj in objectList_)
		{
			concreteType.GetMethod("Add").Invoke(newList,new object[]{obj});

		}
		
		target_ = newList;
	}
	protected virtual void InitializeCreatedObject(object newObject)
	{
       // if(newObject.GetType().IsSubclassOf(typeof(MotinData)))
		//	((MotinData)newObject).SetDefaultValues();
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
						RaiseOnEditorChanged();
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
	

		float contentHeight =0;
	protected override void DoDraw()
	{ 
		cancelUpdate = false;
		
		//MotinEditor[] editors = motinEditors_.ToArray();
		if (objectList != null )
		{
			//GUILayout.BeginVertical(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
			//DrawFields();
				listScroll = GUILayout.BeginScrollView(listScroll,GUILayout.ExpandWidth(true),  GUILayout.Height(contentHeight));
				GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
				contentHeight =0;
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
			/*
			GUILayout.BeginHorizontal();
			if(parentEditor!=null)
				GUILayout.Space(10);
*/
			filteredEditors_[index].Draw(new Rect(0,0,editorRect.width,filteredEditors_[index].editorContentRect.height),false);
			contentHeight+=filteredEditors_[index].editorContentRect.height;

			//GUILayout.EndHorizontal();
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




