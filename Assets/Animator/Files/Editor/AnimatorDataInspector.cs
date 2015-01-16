using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
[CustomEditor(typeof(AnimatorData))]
public class AnimatorDataInspector : Editor {

	const string defaultName = "MotinAnimatorData";
	public	AnimatorData animatorData= null;
	

	public override void OnInspectorGUI()
    {
		/*
		animatorData = (AnimatorData)target;
		
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		
			if(MotinEditorUtils.IsPrefab(animatorData.gameObject))
			{
				if (GUILayout.Button("Create Data Folder", GUILayout.MinWidth(120)))
				{
					if (animatorData.name == defaultName)
					{
						EditorUtility.DisplayDialog("Invalid name", "Please rename before proceeding", "Ok");
					}
					else
					{
						//animatorData.assetPath = AssetDatabase.GetAssetPath(animatorData);
						//string dataFolder = Path.GetDirectoryName(animatorData.assetPath ) + "/" + animatorData.name + "_DATA"; 
						//if(!System.IO.Directory.Exists(dataFolder))
						//{
						//	AssetDatabase.CreateFolder(Path.GetDirectoryName(animatorData.assetPath ), animatorData.name + "_DATA");
						//}
						//animatorData.dataPath = dataFolder;
					}
					//string path = AssetDatabase.GetAssetPath(animatorData);
					
					//Debug.Log("AssetPath " +Path.GetPathRoot(path));
					//Debug.Log("AssetPath " +Path.GetDirectoryName(path));
					//AMTake newTake =  animatorData.addTake();
					//AssetDatabase.AddObjectToAsset(newTake,animatorData);
					//EditorUtility.SetDirty(animatorData);
					//AssetDatabase.SaveAssets();
				}
			}
			else
			{
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Open Editor...", GUILayout.MinWidth(120)))
				{
					if (animatorData.name == defaultName)
					{
						EditorUtility.DisplayDialog("Invalid Entity Definition name", "Please rename entity definition before proceeding", "Ok");
					}
					else
					{
						MotinAnimatorTimeline.window.animatorData = animatorData;
						MotinAnimatorTimeline v = EditorWindow.GetWindow( typeof(MotinAnimatorTimeline), false, "Motin Animatior Timeline" ) as MotinAnimatorTimeline;
						//Debug.Log("Animator data editor adata");
						//v.aData	 = animatorData;
					}
				}
				
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
				if (GUILayout.Button("Commit changes", GUILayout.MinWidth(120)))
				{
				
				}
				
			}
			MotinEditorUtils.DrawDefaultEditorFoldout(true,this);
		
		
        EditorGUILayout.EndVertical();

		GUILayout.Space(64);
		*/
	}
	
	
	
	

}
