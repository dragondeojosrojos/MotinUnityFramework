using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MotinGames
{

	[CustomEditor(typeof(MotinWindowManager))]
	public class MotinWindowManagerInspector : Editor {

		private MotinWindowManager windowManager = null;
		private bool defaultInspectorFoldout = false;
		public override void OnInspectorGUI()
	    {

			windowManager = (MotinWindowManager)target;
			GUILayout.BeginVertical(GUI.skin.box);

			if(GUILayout.Button("Update Windows Array"))
			{
				windowManager.FindWindows();
			}
			if(GUILayout.Button("Generate Window Enum"))
			{
				GenerateWindowsEnum();
			}
			if(GUILayout.Button("Dismiss All Windows"))
			{
				windowManager.DismissAllWindows();
			}
			if(GUILayout.Button("Show All Windows"))
			{
				windowManager.ShowAllWindows();
			}

			if(windowManager.childWindows!=null && windowManager.childWindows.Length>0)
			{
				foreach(MotinWindow tmpWindow in windowManager.childWindows)
				{
					if(tmpWindow==null)
						continue;
					
					GUILayout.BeginHorizontal(GUI.skin.box);
					GUILayout.Label(tmpWindow.name);
					if(GUILayout.Button("Show"))
					{
						tmpWindow.ShowInmediate();
					}
					if(GUILayout.Button("Dismiss"))
					{
						tmpWindow.DismissInmediate();
					}
					GUILayout.EndHorizontal();
				}
			}

			GUILayout.EndVertical();

			defaultInspectorFoldout = MotinEditorUtils.DrawDefaultEditorFoldout(defaultInspectorFoldout,this);
			if (GUI.changed)
			{
				EditorUtility.SetDirty(windowManager);
			}
		}

		void GenerateWindowsEnum()
		{
			List<string> windowNames = new List<string>();
			foreach(MotinWindow tmpWindow in windowManager.childWindows)
			{
				windowNames.Add(tmpWindow.name);
			}
		

			MotinEditorUtils.WriteDefinesFile("MOTIN_WINDOWS","MotinGames",AppSettings.GeneratedSourcePath + windowManager.definesNamesPath,windowNames);
		}


	}
}