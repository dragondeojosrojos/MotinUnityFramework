using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace MotinGames
{

[CustomEditor(typeof(MotinWindow))]
public class MotinWindowInspector : Editor {
	
	public MotinWindow motinWindow = null;
	bool defaultFoldout = false;

	protected virtual void OnDisable()
	{
		motinWindow = null;
	}

	public override void OnInspectorGUI()
    {

		GetReference();

		DoDraw();

		defaultFoldout = MotinEditorUtils.DrawDefaultEditorFoldout(defaultFoldout,this);

		if (GUI.changed)
		{
			EditorUtility.SetDirty(motinWindow);
		}
	}

	protected virtual void GetReference()
	{
		motinWindow = (MotinWindow)target;
	}


	protected virtual void DoDraw()
	{
		//motinWindow.windowFsm = (PlayMakerFSM)EditorGUILayout.ObjectField("WindowFsm",motinWindow.windowFsm,typeof(PlayMakerFSM));
		//motinWindow.windowBox = (GameObject)EditorGUILayout.ObjectField("Window Object",motinWindow.windowBox,typeof(GameObject));
		motinWindow.defaultShow =EditorGUILayout.Toggle("Show by default",motinWindow.defaultShow);
		//motinWindow.updateAnchors = EditorGUILayout.Toggle("Update Anchors " ,motinWindow.updateAnchors);
		//motinWindow.windowAnchor = (MotinWindow.WindowAnchors)EditorGUILayout.EnumPopup("Anchor:",motinWindow.windowAnchor);
		//motinWindow.windowSize = EditorGUILayout.Vector2Field("Size:",motinWindow.windowSize);
	}
}
}