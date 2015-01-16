using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

public class MotinPoolManagerEditor : MotinEditor {

	public	MotinPoolManager poolManager= null;

	MotinDataListEditor poolDatasEditor = null;
	
	List<MotinData> dataList =null;

	protected override void targetUpdated ()
	{
		base.targetUpdated ();
		poolManager = (MotinPoolManager)target_;
		dataList = new List<MotinData>( poolManager.poolDatas);
		poolDatasEditor.motinDatas = dataList;
	}

	protected override void Initialize ()
	{
		base.Initialize ();
		this.editorName = "Motin Pool Manager Editor";

		poolDatasEditor = new MotinDataListEditor(new MotinDataEditor(),typeof(MotinGameObjectPoolData));
		poolDatasEditor.editorName = "Prefabs Editor";
		poolDatasEditor.OnDataChanged+=OnPrefabEditorDataChanged;


	}
	public override void Destroy ()
	{
		base.Destroy ();
		poolDatasEditor.OnDataChanged-=OnPrefabEditorDataChanged;
		poolDatasEditor = null;
		dataList = null;
	}

	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		if(field.Name == "poolDatas")
		{
			poolDatasEditor.Draw(editorRect);
			return true;
		}

		return base.DrawField (value, field);
	}

	void OnPrefabEditorDataChanged()
	{
		poolManager.poolDatas = poolDatasEditor.motinDatas.Cast<MotinGameObjectPoolData>().ToArray();
		EditorUtility.SetDirty(poolManager);
	}




}
}
