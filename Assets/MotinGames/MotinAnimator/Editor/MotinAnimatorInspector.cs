using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
[CustomEditor(typeof(MotinAnimator))]
public class MotinAnimatorInspector : Editor {

	const string defaultName = "MotinAnimator";
	public	MotinAnimator motinAnimator= null;
	public bool defaultFoldout = true;
	
	protected string[] TakeNames =  null; 
	protected int selectedTake =0;
	
	public bool windowOpened = false;
	
	
	
	
 	protected virtual void OnEnable()
	{
		motinAnimator = (MotinAnimator)target;
		
		if(motinAnimator.animatorData!=null)
			motinAnimator.Load(motinAnimator.animatorData);
		
		 UpdateTakeNames();
	}
	
	public void UpdateTakeNames()
	{
		if(motinAnimator.animatorData!=null && motinAnimator.animatorData.takes!=null)
		{
			List<string> names = new List<string>();
			foreach(MotinTakeData take in motinAnimator.animatorData.takes	)
			{
				names.Add(take.name);
			}
				TakeNames = names.ToArray();
				if(selectedTake >= names.Count)
					selectedTake =0;
					
			}
	}
	public override void OnInspectorGUI()
    {
		motinAnimator = (MotinAnimator)target;
		
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		
			if(MotinEditorUtils.IsPrefab(motinAnimator.gameObject))
			{
				MotinEditorUtils.DrawDefaultEditorFoldout(true,this);
			}
			else
			{
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Open Editor...", GUILayout.MinWidth(120)))
				{
					if (motinAnimator.name == defaultName)
					{
						EditorUtility.DisplayDialog("Invalid Entity Definition name", "Please rename entity definition before proceeding", "Ok");
					}
					else
					{
						if(motinAnimator.animatorData ==null)
						{
							Debug.Log("motinAnimator.animatorData ==null");
							motinAnimator.animatorData = new MotinAnimatorData();
							EditorUtility.SetDirty(motinAnimator);
							AssetDatabase.SaveAssets();
						}
						if (MotinAnimatorTimeline.window !=null)
						{
							MotinAnimatorTimeline.window.setMotinAnimator(motinAnimator);	
							//EditorUtility.DisplayDialog("Hay una ventana abierta!", "Ya hay una ventana abierta de Motin Animator Timeline, por favor cerrar antes de continuar o puede haber efectos imprevistos y potencialmente catastroficos, posiblemente daño cerebral irreparable, sera fixieado a la brevedad, muchas gracias", "Ok");
						
						}
						else
						{
							
							//MotinAnimatorTimeline.setMotinAnimator(motinAnimator);
							MotinAnimatorTimeline v = EditorWindow.GetWindow( typeof(MotinAnimatorTimeline), false, "Motin Animatior Timeline" ) as MotinAnimatorTimeline;
							MotinAnimatorTimeline.window.setMotinAnimator(motinAnimator);	
							MotinAnimatorTimeline.window.OnWindowClosed+=OnWindowClosed;
							windowOpened = true;
						
					}
					//Debug.Log("Animator data editor adata");
						//v.aData	 = animatorData;
					}
				}
				
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			
			
				if(windowOpened==false && TakeNames !=null && TakeNames.Length>0)
				{
					
				
					int setTake =  EditorGUILayout.Popup("set Take" ,selectedTake,TakeNames);
					if(setTake!=selectedTake)
					{
						selectedTake= setTake;
						motinAnimator.PreviewValue(TakeNames[selectedTake],true,0);
					}
				
					if(GUILayout.Button("Update preview"))
					{
						motinAnimator.PreviewValue(TakeNames[selectedTake],true,0);
					}
				}
			
				if(defaultFoldout==false)
				{
					if (GUILayout.Button("Show Default Inspector", GUILayout.MinWidth(120)))
					{
						defaultFoldout = true;
					}
				}
				else
				{
					if (GUILayout.Button("Hide Default Inspector", GUILayout.MinWidth(120)))
					{
						defaultFoldout = false;
					}
					MotinEditorUtils.DrawDefaultEditorFoldout(true,this);
				}
			
			}
		
		
		
        EditorGUILayout.EndVertical();

		GUILayout.Space(64);
	}
	public void OnWindowClosed()
	{
		windowOpened =false;
		motinAnimator.Load(motinAnimator.animatorData);
		UpdateTakeNames();
	}
	
	// Menu entries
	[MenuItem("Assets/Create/Motin/MotinAnimator/Animated Object", false, 10000)]
    static void DoAnimatorDataCreate()
    {
		string path = MotinEditorUtils.CreateNewPrefab(defaultName);
        if (path.Length != 0)
        {
            GameObject go = new GameObject();
            /*AnimatorData newData = */ go.AddComponent<MotinAnimator>();
            
	        MotinEditorUtils.SetGameObjectActive(go, false);

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
			Object p = EditorUtility.CreateEmptyPrefab(path);
            EditorUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#else
			Object p = PrefabUtility.CreateEmptyPrefab(path);
            PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#endif
			
            GameObject.DestroyImmediate(go);

			// Select object
			Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
        }
    }	
	

}
