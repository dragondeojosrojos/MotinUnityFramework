using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*
namespace MotinGames
{

	public class ScriptSequenceDataEditor : MotinArrayBaseEditor  {


		public ScriptSequenceDataEditor( ):base()
		{
			pdateCreateMenu();
		}
		public ScriptSequenceDataEditor(System.Type type ):base(type)
		{
			pdateCreateMenu();
		}
		public ScriptSequenceDataEditor(EditorWindow host,System.Type type ):base(host,type)
		{
			pdateCreateMenu();
		}
		public ScriptSequenceDataEditor(string NamespaceName ):base(NamespaceName)
		{
			pdateCreateMenu();
		}
		public ScriptSequenceDataEditor(EditorWindow host,string NamespaceName): base(host,NamespaceName)
		{
			pdateCreateMenu();
		}


		protected override void targetUpdated ()
		{
			base.targetUpdated ();
			//this.editorName = "SECUENCE: " + ((ScriptSequenceData)target).name;
		}

		public override void UpdateCreateMenu ()
		{

			List<GUIContent> items = new List<GUIContent>();
			items.Add(new GUIContent("ScriptSequenceData"));
			menuItems = items.ToArray();
		}

		protected override MotinEditor CreateEditor (System.Type dataType)
		{

			return base.CreateEditor (dataType);
		}

		
		public override void OnDataChangedEditorCallback (MotinEditor editor)
		{
			
			if(MotinUtils.IsTypeDerivedFrom(editor.target.GetType(),typeof(ScriptSequenceData)))
			{
				//Debug.Log("");
				((ScriptSequenceData)editor.target).childDatas =((ScriptSequenceDataEditor)editor).objectList.Cast<MotinData>().ToList();
				//entityData.scriptSequences.childDatas  = scriptSequencesEditor.objectList.Cast<MotinSerializableData>().ToList();
			}
			base.OnDataChangedEditorCallback (editor);
		}
	

	}

}
*/
	
