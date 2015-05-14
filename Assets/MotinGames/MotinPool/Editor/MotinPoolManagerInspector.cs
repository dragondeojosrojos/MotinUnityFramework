using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

	[CustomEditor(typeof(MotinPoolManager))]
	public class MotinPoolManagerInspector : Editor {

		public	MotinPoolManager poolManager= null;

		MotinPoolManagerEditor poolManagerEditor = null;
		
		void OnEnable()
		{
			if(poolManager==null)
			{
				poolManager = (MotinPoolManager)target;
			}

			if(poolManagerEditor == null)
			{
				poolManagerEditor = new MotinPoolManagerEditor();
				poolManagerEditor.OnEditorChanged+=OnEditorChanged;
				poolManagerEditor.target = poolManager;
			}

		}
		void OnDisable()
		{
			if(poolManagerEditor != null)
			{
				poolManagerEditor.OnEditorChanged-=OnEditorChanged;
				poolManagerEditor.Destroy();
				poolManagerEditor = null;
			}
			if(poolManager!=null)
			{
				poolManager = null;
			}
		}

		public override void OnInspectorGUI()
		{
			poolManagerEditor.Draw(new Rect(0,0,300,300));
		}

		void OnEditorChanged()
		{
			EditorUtility.SetDirty(poolManager);
			AssetDatabase.SaveAssets();
		}
		
	}
}
