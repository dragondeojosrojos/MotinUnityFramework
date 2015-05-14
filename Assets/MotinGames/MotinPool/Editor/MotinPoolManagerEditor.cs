using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

	public class MotinPoolManagerEditor : MotinEditor {

		public	MotinPoolManager poolManager= null;

		MotinArrayListEditor poolDatasEditor = null;
		
		List<MotinData> dataList =null;

		protected override void targetUpdated ()
		{
			//base.targetUpdated ();
			poolManager = (MotinPoolManager)target_;
			dataList = new List<MotinData>( poolManager.poolDatas);
			poolDatasEditor.target = dataList;
		}

		protected override void Initialize ()
		{
			base.Initialize ();
			this.editorName = "Motin Pool Manager Editor";

			poolDatasEditor = new MotinArrayListEditor(hostEditorWindow,typeof(MotinGameObjectPoolData));
			poolDatasEditor.editorName = "Prefabs Editor";
			poolDatasEditor.orderDatas = true;
			poolDatasEditor.overwriteFileOrder = true;
			poolDatasEditor.OnEditorChanged+=OnPrefabEditorDataChanged;


		}
		public override void Destroy ()
		{
			base.Destroy ();
			poolDatasEditor.OnEditorChanged-=OnPrefabEditorDataChanged;
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
			poolManager.poolDatas = poolDatasEditor.objectList.Cast<MotinGameObjectPoolData>().ToArray();

			RaiseOnEditorChanged();
		}




	}
}
