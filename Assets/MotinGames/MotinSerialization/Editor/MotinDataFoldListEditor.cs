
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace MotinGames
{

public class MotinDataFoldListEditor  : MotinFoldListEditor {

	protected override void targetUpdated ()
	{
		base.targetUpdated ();
		editorName =  ((MotinData)target).name;
		
	}

	protected override void objectListUpdated ()
	{
//		motinDatas_ = objectList_.Cast<MotinSerializableData>() as List<MotinSerializableData>;
		base.objectListUpdated();
		
	}

	public MotinDataFoldListEditor( ):base()
	{
		//handleId = MotinEditorUtils.GetUniqueInteger();
	}
	
	public MotinDataFoldListEditor(EditorWindow host): base(host)
	{
	
	}
	
	 protected override string UniqueDataName(List<object> allData, string baseName)
		{
			bool found = false;
			for (int i = 0; i < allData.Count; ++i)
			{
				if ( ((MotinData)allData[i]).name == baseName) 
				{ 
					found = true; 
					break; 
				}
			}
			if (!found) return baseName;

			string uniqueName = baseName + " ";
			int uniqueId = 1;
			for (int i = 0; i < allData.Count; ++i)
			{
				string uname = uniqueName + uniqueId.ToString();
				if (((MotinData)allData[i]).name == uname)
				{
					uniqueId++;
					i = -1;
					continue;
				}
			}
			uniqueName = uniqueName + uniqueId.ToString();
			return uniqueName;
		}
	
	public override void OrderList()
	{
		if (searchFilter.Length == 0)
			filteredObjects = (from data in objectList_  select data).ToList();
		else
			filteredObjects = (from data in objectList_ where  Contains( ((MotinData)data).name, searchFilter)  select data).ToList();
		
		if (searchFilter.Length == 0)
			filteredEditors_ = (from data in motinEditors_  select data).ToList();
		else
			filteredEditors_ = (from data in motinEditors_ where  Contains( ((MotinData)data.target).name, searchFilter)  select data).ToList();
	}

	
	MotinData CreateNewData()
	{
		// Find a unique name
		/*
		string uniqueName = UniqueDataName(motinDatas, "New Data");
		
		MotinData data = (MotinData)MotinDataManager.InstanceDataFromName ( dataType.ToString() );
		data.name = uniqueName;
		
	
		
		bool inserted = false;
		
		if (!inserted)
			motinDatas.Add(data);

		searchFilter = "";
		FilterDatas();
		return data;
*/
		return null;
	}
	

	
}
}



	
