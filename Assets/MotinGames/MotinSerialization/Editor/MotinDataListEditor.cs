using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

public class MotinSerializableDataListEditor  : MotinEditor {
	

	protected int handleId = 0;
	protected int leftBarWidth_ = 150;
	
	protected int minLeftBarWidth = 150;

	public int leftBarWidth { get { return leftBarWidth_; } set { leftBarWidth_ = Mathf.Max(value, minLeftBarWidth); } }

	public bool orderDatas = true;

	protected  string searchFilter = "";
	
	protected System.Type dataType = null;
	[SerializeField]
	protected  List<MotinData> 	motinDatas_ = null;
	//protected  List<MotinSerializableData> allEntityData = new List<MotinSerializableData>();
	[SerializeField]
	protected  List<MotinData> filteredDatas = new List<MotinData>();
	[SerializeField]
	protected  MotinData _selectedData;
	protected  int selectedDataId = -1;
	[SerializeField]
	protected MotinDataEditor dataEditor_ = null;
	
	public MotinDataEditor dataEditor
	{
		
		get{
			return dataEditor_;
		}
		set{
			if(dataEditor!=null)
			{
				dataEditor.OnDataNameChanged -= DataNameChanged;
				dataEditor.OnDeletePressed -= DataDeleted;
				dataEditor.OnUpPressed -= OnDataUpPressed;
				dataEditor.OnDownPressed -= OnDataDownPressed;
				dataEditor.OnDataChanged	-= OnDataChangedCallback;
				dataEditor = null;
			}
			
			dataEditor_ = value;
			if(dataEditor_!=null)
			{
				dataEditor.OnDataNameChanged += DataNameChanged;
				dataEditor.OnDeletePressed += DataDeleted;
				dataEditor.OnUpPressed += OnDataUpPressed;
				dataEditor.OnDownPressed += OnDataDownPressed;
				dataEditor.OnDataChanged += OnDataChangedCallback;
				dataEditor.showListToolbar = true;
				dataEditor.hostEditorWindow = this.hostEditorWindow;
			}
			if(motinDatas!=null)
				dataEditor.target = selectedData;
		}
	}
	public List<MotinData> motinDatas
	{
		get{
			return motinDatas_;
		}
		set{

			if(value!=null)
			{ 
				motinDatas_ =value;
				target_ = value;
				searchFilter = "";
				OrderDatas();

				if(motinDatas_.Count ==0)
					CreateNewData();
				
				FilterDatas();
	
			
				if (selectedDataId != -1 && selectedDataId < motinDatas_.Count)
				{
					selectedData = motinDatas_[selectedDataId];
				}
				else
				{
					selectedData = null;
				}
				
				Repaint();
			}
		}
	}
	/*
	public static MotinSerializableDataListEditor Create(EditorWindow host,MotinDataEditor editor,System.Type type)
        {
			MotinSerializableDataListEditor motinEditor = new MotinSerializableDataListEditor();
			//motinEditor.motinDatas = datas;
			motinEditor.hostEditorWindow = host;
			motinEditor.dataType = type;
			motinEditor.dataEditor =editor;
			motinEditor.initialize();
            return motinEditor;
        }
	*/
	
	public MotinSerializableDataListEditor(EditorWindow host,MotinDataEditor editor,System.Type type): base(host)
	{
		handleId = MotinEditorUtils.GetUniqueInteger();
		dataType = type;
		dataEditor = editor;
		
	}
	
	public virtual void OrderDatas()
	{
		
	}
	
	/*
	protected void initialize()
	{	
		//Debug.Log("ON ENABLE ENTITY WINDOW");
		if(hostEditorWindow!=null)
		{
			//CreateMenu();
			
			// Create clip editor
			if (dataEditor == null)
			{
				CreateDataEditor();	
			}

		}

		//if(currentFilePath.Length>0)
		//	Load(currentFilePath);
		// Create menu items
		
		//clipEditor.animOps = animOps;
	}
		
	public virtual void CreateDataEditor()
	{
		dataEditor =new MotinDataEditor();
	}
	 */
	
	/*
	public override void Destroy()
	{
		motinDatas = null;
		searchFilter = "";
		selectedData = null;
		selectedDataId =0;
		
		if (dataEditor != null)
			dataEditor.Destroy();
		
		base.Destroy();
	}
	 */
	
	public void OnDataChangedCallback( )
	{
		RaiseOnDataChanged();
	}
	public void DataNameChanged(MotinEditor editor)
	{
		FilterDatas();
		Repaint();
	}

	public void DataDeleted(MotinEditor editor)
	{
		//entityData.Clear();
		MotinData data = (MotinData)editor.target;
		motinDatas.Remove(data);
		
		if(motinDatas_.Count ==0)
			CreateNewData();
		
		FilterDatas();
		
		selectedDataId =0;
		selectedData = motinDatas[selectedDataId];
		RaiseOnDataChanged();
		Repaint();
	}
	public void OnDataDownPressed(MotinEditor editor)
	{
		int index = motinDatas_.IndexOf((MotinData)editor.target);
		
		if(index+1>=motinDatas_.Count)
			return;

		MotinData auxData = motinDatas_[index +1];
		motinDatas_[index +1]  =  motinDatas_[index];
		motinDatas_[index] = auxData;
		
		int selectedId = -1;
		for (int i = 0; i < filteredDatas.Count; ++i)
		{
			if (filteredDatas[i] == motinDatas_[index +1])
			{
				selectedId = i;
				break;
			}
		}

		selectedData = filteredDatas[selectedId];	



		FilterDatas();

		RaiseOnDataChanged();

		Repaint();

	}
	public void OnDataUpPressed(MotinEditor editor)
	{
		int index = motinDatas_.IndexOf((MotinData)editor.target);
		
		if(index-1<=0)
			return;
		
		MotinData auxData = motinDatas_[index -1];
		motinDatas_[index -1]  =  motinDatas_[index];
		motinDatas_[index] = auxData;
		
		int selectedId = -1;
		for (int i = 0; i < filteredDatas.Count; ++i)
		{
			if (filteredDatas[i] == motinDatas_[index -1])
			{
				selectedId = i;
				break;
			}
		}
		
		selectedData = filteredDatas[selectedId];	
		
		
		
		FilterDatas();
		
		RaiseOnDataChanged();
		
		Repaint();

	}
	
	string UniqueDataName(List<MotinData> allData, string baseName)
		{
			bool found = false;
			for (int i = 0; i < motinDatas.Count; ++i)
			{
				if (motinDatas[i].name == baseName) 
				{ 
					found = true; 
					break; 
				}
			}
			if (!found) return baseName;

			string uniqueName = baseName + " ";
			int uniqueId = 1;
			for (int i = 0; i < motinDatas.Count; ++i)
			{
				string uname = uniqueName + uniqueId.ToString();
				if (motinDatas[i].name == uname)
				{
					uniqueId++;
					i = -1;
					continue;
				}
			}
			uniqueName = uniqueName + uniqueId.ToString();
			return uniqueName;
		}
	
	MotinData selectedData
	{
		get { return _selectedData; }
		set 
		{
			selectedDataId = -1;
			if (value != null)
			{
				for (int i = 0; i < motinDatas.Count; ++i)
				{
					if (motinDatas[i] == value)
					{
						_selectedData = value;
						selectedDataId = i;
						break;
					}
				}
			}
			if (selectedDataId == -1)
			{
				if (value != null) Debug.LogError("Unable to find clip");
				_selectedData = null;
			}
		}
	}

	
	public static bool Contains(string s, string text) { return s.ToLower().IndexOf(text.ToLower()) != -1; }
	
	public void FilterDatas()
	{
		if(motinDatas==null || motinDatas.Count==0)
		{
			filteredDatas.Clear();
			return;
		}
		
		filteredDatas = new List<MotinData>(motinDatas.Count);
		
		OrderFilterDatas();
		
		//Debug.Log("FilterDatas  " + filteredDatas.Count);
	}
	public virtual void OrderFilterDatas()
	{

		if(orderDatas)
		{
			if (searchFilter.Length == 0)
				filteredDatas = (from data in motinDatas  orderby data.name select data).ToList();
			else
				filteredDatas = (from data in motinDatas where  Contains(data.name, searchFilter) orderby data.name select data).ToList();
		}
		else
		{
			if (searchFilter.Length == 0)
				filteredDatas = (from data in motinDatas   select data).ToList();
			else
				filteredDatas = (from data in motinDatas where  Contains(data.name, searchFilter)  select data).ToList();
		}
	}

	
	MotinData CreateNewData()
	{
		// Find a unique name
		string uniqueName = UniqueDataName(motinDatas, "New Data");
		
		MotinData data = MotinDataManager.InstanceDataFromName(  dataType.ToString() );
		data.name = uniqueName;
		
	
		
		bool inserted = false;
		
		if (!inserted)
			motinDatas.Add(data);

		searchFilter = "";
		FilterDatas();
		RaiseOnDataChanged();
		return data;
	}
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
		
		/*
		// RHS
		GUILayout.FlexibleSpace();
		
		if (selectedData != null)
		{ 
			
			if(GUILayout.Button("Delete", EditorStyles.toolbarButton))
			{
				DataDeleted(selectedData,0);
			}
			
		}
		*/
	}
	
	

	void TrimClips()
	{
		if (motinDatas.Count < 1)
			return;

		int validCount = motinDatas.Count;
		while (validCount > 0 /*&& !allEntityData[validCount - 1].Empty*/)
			--validCount;
		
		motinDatas.RemoveRange(validCount, motinDatas.Count - validCount);

		if (motinDatas.Count == 0)
		{
			motinDatas.Add(CreateNewData());
			FilterDatas();
		}
	}
	
	protected override void DoDraw()
	{ 
		
		if (motinDatas != null )
		{
			GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
			DrawList();
			DoDrawEditor();
			
			GUILayout.EndHorizontal();
		}

	}
	
	Vector2 listScroll = Vector2.zero;
	void DrawList()
	{
		
		GUILayout.BeginVertical(MotinEditorSkin.SC_ListBoxBG, GUILayout.Width(leftBarWidth), GUILayout.ExpandHeight(true));
		listScroll = GUILayout.BeginScrollView(listScroll, GUILayout.Width(leftBarWidth));
		foreach (MotinData data in filteredDatas)
		{
			// 0 length name signifies inactive clip
			if (data.name.Length == 0) continue;

			bool selected = selectedData == data;
			bool newSelected = GUILayout.Toggle(selected, data.name, MotinEditorSkin.SC_ListBoxItem, GUILayout.ExpandWidth(true));
			if (newSelected != selected && newSelected == true)
			{
				selectedData = data; 
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
	
	Vector2 listScrollEditor = Vector2.zero;
	protected virtual void DoDrawEditor()
	{
	
		listScrollEditor = GUILayout.BeginScrollView(listScrollEditor,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		if(dataEditor!=null && selectedData!=null)
		{ 
//			Debug.Log(" DRAW EDITOR " + selectedData.name);
			dataEditor.target = selectedData;
			dataEditor.Draw(new Rect(0,0,editorRect.width,editorRect.height)  );
		}  
		GUILayout.EndScrollView();
		
	}
}
}



