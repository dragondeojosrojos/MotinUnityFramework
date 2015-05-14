
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

public class MotinArrayListEditor  : MotinArrayBaseEditor {

	protected int handleId = 0;
	protected int leftBarWidth_ = 150;
	
	protected int minLeftBarWidth = 50;
	
	public int leftBarWidth { get { return leftBarWidth_; } set { leftBarWidth_ = Mathf.Max(value, minLeftBarWidth); } }
	
	
	public bool orderDatas = true;
	public bool overwriteFileOrder = true;
	List<MotinData> motinDataList = new List<MotinData>();
	protected override void objectListUpdated ()
	{
		motinDataList = objectList_.Cast<MotinData>().ToList();
		if(motinDataList==null)
				motinDataList = new List<MotinData>();

		base.objectListUpdated ();

	}
	

	protected override MotinEditor CreateInitializedEditor (MotinEditor newEditor, object editorTarget, int index)
	{
		MotinDataEditor editor = (MotinDataEditor) base.CreateInitializedEditor (newEditor, editorTarget, index);
		if(editor == null)
			return null;
		editor.OnDataNameChangedWithEditor += DataNameChanged;
		return editor;
	}

	public MotinArrayListEditor( ):base()
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	public MotinArrayListEditor(System.Type type ):base(type)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	public MotinArrayListEditor(EditorWindow host,System.Type type ):base(host,type)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	public MotinArrayListEditor(string NamespaceName ):base(NamespaceName)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	public MotinArrayListEditor(EditorWindow host,string NamespaceName): base(host,NamespaceName)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
	}
	
	string UniqueDataName(List<MotinData> allData, string baseName)
	{
		

		bool found = false;
		for (int i = 0; i < motinDataList.Count; ++i)
		{
			if (motinDataList[i].name == baseName) 
			{ 
				found = true; 
				break; 
			}
		}
		if (!found) return baseName;
		
		string uniqueName = baseName + " ";
		int uniqueId = 1;
		for (int i = 0; i < motinDataList.Count; ++i)
		{
			string uname = uniqueName + uniqueId.ToString();
			if (motinDataList[i].name == uname)
			{
				uniqueId++;
				i = -1;
				continue;
			}
		}
		uniqueName = uniqueName + uniqueId.ToString();
		return uniqueName;
	}

	public static bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	

	public override void OrderList()
	{
		
		if(orderDatas)
		{
			motinDataList = (from data in motinDataList  orderby data.name select data).ToList();
			motinEditors_ = (from meditor in motinEditors_  orderby ((MotinData)meditor.target).name select meditor).ToList();
			
			if (searchFilter.Length == 0)
			{
				filteredObjects = (from data in motinDataList  orderby data.name select data).Cast<object>().ToList();
				filteredEditors_ = (from meditor in motinEditors_  orderby ((MotinData)meditor.target).name select meditor).ToList();
			}
			else
			{
				filteredObjects = (from data in motinDataList where  Contains(data.name, searchFilter) orderby data.name select data).Cast<object>().ToList();
				filteredEditors_ = (from meditor in motinEditors_ where Contains(((MotinData)meditor.target).name,searchFilter)  orderby ((MotinData)meditor.target).name select meditor).ToList();
			}
		

			if(overwriteFileOrder)
			{
				objectList_ = motinDataList.Cast<object>().ToList();
			}
		}
		else
		{
			if (searchFilter.Length == 0)
			{
				filteredObjects = (from data in motinDataList   select data).Cast<object>().ToList();
				filteredEditors_ = (from meditor in motinEditors_  select meditor).ToList();
			}
			else
			{
				filteredObjects = (from data in motinDataList where  Contains(data.name, searchFilter)  select data).Cast<object>().ToList();
				filteredEditors_ = (from meditor in motinEditors_ where Contains(((MotinData)meditor.target).name,searchFilter) select meditor).ToList();
			}
		}
	}
	
	protected override void InitializeCreatedObject (object newObject)
	{
		base.InitializeCreatedObject (newObject);
		((MotinData)newObject).name =  UniqueDataName(motinDataList, "New Data");
	}
	/*
	MotinData DuplicateSelectedData()
	{
		if(selectedData == null)
			return null;
		// Find a unique name
		string uniqueName = UniqueDataName(motinDatas,selectedData.name);
		
		MotinData data = MotinDataManager.InstanceDataFromName(  dataType.ToString() );
		
		data.CopyFromData(selectedData);
		data.name = uniqueName;
		
		
		
		bool inserted = false;
		
		if (!inserted)
			motinDatas.Add(data);
		
		searchFilter = "";
		FilterDatas();
		RaiseOnDataChanged();
		return data;
	}
	*/

	protected override void DoDraw()
	{ 
		
		if (motinDataList != null )
		{
			GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
			DrawList();
			DoDrawEditor();
			GUILayout.EndHorizontal();
		}
		
	}
	
	Vector2 listScroll = Vector2.zero;
	MotinData tmpListData = null;
	void DrawList()
	{
		
		GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.Width(leftBarWidth), GUILayout.ExpandHeight(true));
		listScroll = GUILayout.BeginScrollView(listScroll, GUILayout.Width(leftBarWidth),GUILayout.ExpandHeight(true));
		foreach (object data in filteredObjects)
		{
			tmpListData = (MotinData)data;
			// 0 length name signifies inactive clip
			if (tmpListData.name.Length == 0) continue;
			
			bool selected = selectedObject == data;
			bool newSelected =	DrawListItem(selected,tmpListData);
			//bool newSelected = GUILayout.Toggle(selected, tmpListData.name, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
			if (newSelected != selected && newSelected == true)
			{
				selectedObject = data; 
				GUIUtility.keyboardControl = 0;
				
				Repaint();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
		
		
		Rect viewRect = GUILayoutUtility.GetLastRect();
		leftBarWidth = (int)MotinGuiUtility.DragableHandle(handleId, 
		                                                   viewRect, leftBarWidth, 
		                                                   MotinGuiUtility.DragDirection.Horizontal);
		
	}
	
	protected virtual bool DrawListItem(bool selected,MotinData data)
	{
		return  GUILayout.Toggle(selected, data.name, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
	}

	Vector2 listScrollEditor = Vector2.zero;
	protected virtual void DoDrawEditor()
	{
		
		
		if(selectedObject!=null)
		{ 
				listScrollEditor = GUILayout.BeginScrollView(listScrollEditor,false,true,GUILayout.ExpandWidth(true) , GUILayout.ExpandHeight(true));
				filteredEditors_[selectedObjectIndex].Draw(new Rect(0,0,editorRect.width,filteredEditors_[selectedObjectIndex].editorContentRect.height) ,false );
				GUILayout.EndScrollView();
			//DrawItem(selectedObjectIndex);
			//			Debug.Log(" DRAW EDITOR " + selectedData.name);
			//dataEditor.target = selectedData;
			//dataEditor.Draw(new Rect(0,0,editorRect.width,editorRect.height)  );
		}  
		else
		{
				listScrollEditor = GUILayout.BeginScrollView(listScrollEditor,GUILayout.ExpandWidth(true) , GUILayout.ExpandHeight(true));
				GUILayout.EndScrollView();
		}
		
		
	}

	public void DataNameChanged(MotinEditor editor)
	{
		FilterList();
		UpdateSelectedIndex();
		Repaint();
	}
}

}


