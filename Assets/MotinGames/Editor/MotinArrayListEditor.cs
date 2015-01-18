
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
	public bool overwriteFileOrder = false;
	List<MotinData> motinDataList = new List<MotinData>();
	protected override void objectListUpdated ()
	{
		motinDataList = objectList_.Cast<MotinData>().ToList();
		if(motinDataList==null)
				motinDataList = new List<MotinData>();

		base.objectListUpdated ();

	}

	protected override MotinEditor CreateInitializedEditor (System.Type dataType, object target, int index = -1)
	{
		MotinDataEditor newEditor = (MotinDataEditor) base.CreateInitializedEditor (dataType, target,index);
		newEditor.OnDataNameChanged += DataNameChanged;
		return newEditor;
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

			if (searchFilter.Length == 0)
				filteredObjects = (from data in motinDataList  orderby data.name select data).Cast<object>().ToList();
			else
				filteredObjects = (from data in motinDataList where  Contains(data.name, searchFilter) orderby data.name select data).Cast<object>().ToList();

			if(overwriteFileOrder)
			{
				motinEditors_ = (from meditor in motinEditors_  orderby ((MotinData)meditor.target).name select meditor).ToList();
				objectList_ = motinDataList.Cast<object>().ToList();
			}
		}
		else
		{
			if (searchFilter.Length == 0)
				filteredObjects = (from data in motinDataList   select data).Cast<object>().ToList();
			else
				filteredObjects = (from data in motinDataList where  Contains(data.name, searchFilter)  select data).Cast<object>().ToList();
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
	/*
	protected override void DrawToolbarButtons ()
	{
		// Create Button
		
		// LHS
		GUILayout.BeginHorizontal(GUILayout.Width(leftBarWidth - 6));
		
		GUIContent createButton = new GUIContent("Create");
		Rect createButtonRect = GUILayoutUtility.GetRect(createButton, EditorStyles.miniButton, GUILayout.ExpandWidth(false));
		if (GUI.Button(createButtonRect, createButton, EditorStyles.miniButton) && motinDatas != null)
		{
			
			selectedData = CreateNewData();
			dataEditor.target = selectedData;		
			Repaint();
		}
		
		GUIContent duplicateButton = new GUIContent("Duplicate");
		Rect duplicateButtonRect = GUILayoutUtility.GetRect(duplicateButton, EditorStyles.miniButton, GUILayout.ExpandWidth(false));
		if (GUI.Button(duplicateButtonRect, duplicateButton, EditorStyles.miniButton) && motinDatas != null)
		{
			selectedData = DuplicateSelectedData();
			dataEditor.target = selectedData;		
			Repaint();
		}
		
		
		// Filter box
		if (motinDatas != null)
		{
			GUILayout.Space(8);
			string newSearchFilter = GUILayout.TextField(searchFilter, MotinEditorSkin.ToolbarSearch, GUILayout.ExpandWidth(true));
			if (newSearchFilter != searchFilter)
			{
				searchFilter = newSearchFilter;
				FilterDatas();
			}
			if (searchFilter.Length > 0)
			{
				if (GUILayout.Button("", MotinEditorSkin.ToolbarSearchClear, GUILayout.ExpandWidth(false)))
				{
					searchFilter = "";
					FilterDatas();
				}
			}
			else
			{
				GUILayout.Label("", MotinEditorSkin.ToolbarSearchRightCap);
			}
		}
		
		GUILayout.EndHorizontal();
	}
	
	
	*/
	/*
	void TrimClips()
	{
		if (motinDatas.Count < 1)
			return;
		
		int validCount = motinDatas.Count;
		while (validCount > 0 )
			--validCount;
		
		motinDatas.RemoveRange(validCount, motinDatas.Count - validCount);
		
		if (motinDatas.Count == 0)
		{
			motinDatas.Add(CreateNewData());
			FilterDatas();
		}
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
		listScroll = GUILayout.BeginScrollView(listScroll, GUILayout.Width(leftBarWidth));
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
		
		listScrollEditor = GUILayout.BeginScrollView(listScrollEditor,GUILayout.ExpandWidth(true) , GUILayout.ExpandHeight(true));
		if(selectedObject!=null)
		{ 
				motinEditors_[selectedObjectIndex].Draw(new Rect(0,0,editorRect.width,editorRect.height) ,childEditorsExpandHeight );
			//DrawItem(selectedObjectIndex);
			//			Debug.Log(" DRAW EDITOR " + selectedData.name);
			//dataEditor.target = selectedData;
			//dataEditor.Draw(new Rect(0,0,editorRect.width,editorRect.height)  );
		}  
		GUILayout.EndScrollView();
		
	}

	public void DataNameChanged(MotinEditor editor)
	{
		FilterList();
		UpdateSelectedObjectIndex();
		Repaint();
	}
}

}


