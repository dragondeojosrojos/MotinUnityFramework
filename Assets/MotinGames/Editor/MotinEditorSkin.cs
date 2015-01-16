using UnityEngine;
using UnityEditor;
using System.Collections;

public class MotinEditorSkin  {

	private static GUISkin	editorSkin = null;
	static bool isProSkin;
	
	// Sprite collection editor styles
	public static void Init()
	{
		isProSkin = EditorGUIUtility.isProSkin;
		if(editorSkin==null)
		{
			if(isProSkin)
			{
				editorSkin = (GUISkin)(Resources.LoadAssetAtPath("Assets/MotinGames/Editor/Skin/MotinPro.guiskin", typeof(GUISkin)));
			}
			else
			{
				editorSkin = (GUISkin)(Resources.LoadAssetAtPath("Assets/MotinGames/Editor/Skin/MotinFree.guiskin", typeof(GUISkin)));
			}


		}
	}
	

	public static GUIStyle GetStyle(string name) {
		return editorSkin.GetStyle(name);
	}
	
	
	public static GUIStyle SC_InspectorBG { get { Init(); return GetStyle("SC_InspectorBG"); } }
	public static GUIStyle SC_InspectorHeaderBG { get { Init(); return GetStyle("SC_InspectorHeaderBG"); } }
	public static GUIStyle SC_ListBoxBG { get { Init(); return GetStyle("SC_ListBoxBG"); } }
	public static GUIStyle SC_ListBoxItem { get { Init(); return GetStyle("SC_ListBoxItem"); } }
	public static GUIStyle SC_ListBoxSectionHeader { get { Init(); return GetStyle("SC_ListBoxSectionHeader"); } }	
	public static GUIStyle SC_BodyBackground { get { Init(); return GetStyle("SC_BodyBackground"); } }	
	public static GUIStyle SC_DropBox { get { Init(); return GetStyle("SC_DropBox"); } }	
	
	public static GUIStyle ToolbarSearch { get { Init(); return GetStyle("ToolbarSearch"); } }
	public static GUIStyle ToolbarSearchClear { get { Init(); return GetStyle("ToolbarSearchClear"); } }
	public static GUIStyle ToolbarSearchRightCap { get { Init(); return GetStyle("ToolbarSearchRightCap"); } }
	
	public static GUIStyle Anim_BG { get { Init(); return GetStyle("AnimBG"); } }
	public static GUIStyle Anim_Trigger { get { Init(); return GetStyle("AnimTrigger"); } }
	public static GUIStyle Anim_TriggerSelected { get { Init(); return GetStyle("AnimTriggerDown"); } }
	
	public static GUIStyle MoveHandle { get { Init(); return GetStyle("MoveHandle"); } }
	public static GUIStyle RotateHandle { get { Init(); return GetStyle("RotateHandle"); } }
	
	public static GUIStyle WhiteBox { get { Init(); return GetStyle("WhiteBox"); } }
	public static GUIStyle Selection { get { Init(); return GetStyle("Selection"); } }
}