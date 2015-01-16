using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

public class ScriptSequenceDataEditor : MotinAssemblyFoldListEditor  {

	public ScriptSequenceDataEditor( ):base()
	{
		UpdateCreateMenu();
	}
	
	public ScriptSequenceDataEditor(EditorWindow host):base(host)
	{
		//namespaceList.Add("MotinScripts");
		UpdateCreateMenu();
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
		/*
		items.Add(new GUIContent("ScriptLevelUpData"));
		items.Add(new GUIContent("ScriptLeveDownData"));
		items.Add(new GUIContent("ScriptResetLevelData"));
		items.Add(new GUIContent("ScriptKillSelfData"));
		items.Add(new GUIContent("ScriptKillPiecesData"));
		items.Add(new GUIContent("ScriptAddTurnsData"));
		items.Add(new GUIContent("ScriptAddCurrencyData"));
		items.Add(new GUIContent("ScriptSetPieceData"));
		items.Add(new GUIContent("ScriptTurnTriggeredSequenceData"));
		items.Add(new GUIContent("ScriptSeasonTriggeredSecuenceData"));
		items.Add(new GUIContent("ScriptGamecontrollerSpawnFxData"));
		
		items.Add(new GUIContent("ScriptPlayAnimAnimatedObjectData"));
	*/
		menuItems = items.ToArray();
	}
	
	
	protected override MotinEditor CreateEditor (System.Type dataType)
	{
		/*
		if(dataType == typeof(ScriptLevelUpData))
		{
			Debug.Log("Create ScriptTargetedDataEditor editor");
			return new ScriptTargetedDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptLeveDownData))
		{
			return new ScriptTargetedDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptResetLevelData))
		{
			return new ScriptTargetedDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptKillPiecesData))
		{
			return new ScriptTargetedDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptKillSelfData))
		{
			return new MotinDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptTurnTriggeredSequenceData))
		{
			Debug.Log("Create TurnTriggeredSecuence editor");
			return new ScriptSequenceDataEditor(hostEditorWindow);
		}
		else if(dataType == typeof(ScriptSeasonTriggeredSecuenceData))
		{
			Debug.Log("Create SeasonTriggeredSecuence editor");
			return new ScriptSequenceDataEditor(hostEditorWindow);
		}
		*/
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
	
