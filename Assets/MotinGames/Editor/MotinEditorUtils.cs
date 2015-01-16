using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class MotinEditorUtils {
	
	public static System.Random randomGenerator = null;
	/*
	public static void DrawLevelFileGUI(LevelFile levelfile)
	{
		EditorGUILayout.BeginVertical();
			if(levelfile==null)
			{
				EditorGUILayout.LabelField("Level Data","NULL");
				
			}
			else
			{
		
				levelfile.chapterId = EditorGUILayout.IntField("chapterId",levelfile.chapterId);
				levelfile.levelId = EditorGUILayout.IntField("levelId",levelfile.levelId);
				levelfile.levelName = EditorGUILayout.TextField("levelName",levelfile.levelName);
				levelfile.TimeGold = EditorGUILayout.FloatField("TimeGold",levelfile.TimeGold);
				levelfile.TimeSilv = EditorGUILayout.FloatField("TimeSilv",levelfile.TimeSilv);
				levelfile.countdownAudio = EditorGUILayout.TextField("countdownAudio",levelfile.countdownAudio);
				levelfile.missionCompleteAudio = EditorGUILayout.TextField("missionCompleteAudio",levelfile.missionCompleteAudio);
				levelfile.missionFailAudio = EditorGUILayout.TextField("missionFailAudio",levelfile.missionFailAudio);
			}
	
	//public List<SpawnEntityData> defaultSpawns = new List<SpawnEntityData>();
		EditorGUILayout.EndVertical();
	}
	*/
	
	public static int GetUniqueInteger()
	{
 	   if(randomGenerator==null)
		{
	  	  randomGenerator = new System.Random();
		}
		
	   int result =  randomGenerator.Next();
//		Debug.Log("GENERATED " + result);
	   return result;
	  
	}
	
	public static string DrawStringPopup (string label, string selectedName, string[] names )
	{
		
		if(selectedName ==null || names ==null || names.Length==0)
		{
			return "";
		}

		int selectedIndex = names.Length-1;
		for(int i = 0; i < names.Length;i++)
		{
			if(selectedName== names[i])
			{
				selectedIndex =i;
				break;
			}
		}
		
		selectedIndex =  EditorGUILayout.Popup(label,selectedIndex,names);
		//if(selectedEntityIndex==names.Length-1)
		//{
		//	return "";
		//}
		//else
		//{
			return names[selectedIndex];
		//}

	}
	
	public static void DrawInspectorTitle(string title,Color backColor)
	{
		GUI.backgroundColor = backColor;
		GUILayout.BeginHorizontal(GUI.skin.box);
				GUILayout.FlexibleSpace();
				GUILayout.Label(title);
				GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
	}
	

	public static void DrawInspectorTitle(string title,string description,Color backColor)
	{
		GUI.backgroundColor = backColor;
		GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label(title);
					GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		
			if(description!="")
			{
				GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label(description.ToLower());
						GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		GUILayout.EndVertical();
		GUI.backgroundColor = Color.white;
	}
	
	public static bool DrawDefaultEditorFoldout(bool open,Editor targetEditor)
	{
		EditorGUILayout.BeginVertical();
			open = EditorGUILayout.Foldout(open,"Default Inspector");
			if(open)
			{
				EditorGUI.indentLevel =1;
				targetEditor.DrawDefaultInspector();
				EditorGUI.indentLevel = 0;
			}
		EditorGUILayout.EndVertical();
		return open;
	}
	
	/*
	public static int DrawAnimationClipField(string fieldName,tk2dSpriteAnimation animationLib,string[] clipNames, int currentClipId)
	{
		currentClipId = ClampClipId(animationLib, currentClipId);
		int newClipId = EditorGUILayout.Popup(fieldName, currentClipId, clipNames);
		return newClipId;

		
	}
*/
	
	public static bool DrawList(string fieldName,IList list,bool foldout = true)
	{
		//bool changed = false;
		foldout = EditorGUILayout.Foldout(foldout,fieldName);
		if(foldout)
		{
			EditorGUI.indentLevel++;
			
			GUILayout.BeginHorizontal();
					int count = EditorGUILayout.IntField("count:" ,list.Count);
					if(GUILayout.Button("+"))
					{
						list.Add(list[list.Count-1]);
					}
				GUILayout.EndHorizontal();
			if(count!=list.Count)
			{
				if(count<list.Count)
				{
					while(count<list.Count)
					{
						list.RemoveAt(list.Count-1);
					}
				}
				else if(count>list.Count)
				{
					while(count>list.Count)
					{
						list.Add(list[list.Count-1]);
					}
				}
				//changed = true;
			}
			//EditorGUI.indentLevel++;
			int index =0;
			foreach(object data in list)
			{
				GUILayout.BeginHorizontal();
					GUILayout.Label("item " + index);
					if(GUILayout.Button("-"))
					{
						list.RemoveAt(index);
						return foldout;
					}
				GUILayout.EndHorizontal();
				
				DrawClassInspector(data);
				
				index++;
			}
			//EditorGUI.indentLevel--;
			
			EditorGUI.indentLevel--;
		}
		//GUI.backgroundColor = Color.white;
	   return foldout;;
	}
	
	public static void DrawClassInspector( object value)
	{
		  // Debug.Log("MOTINSCRIPT EDITOR DRAWFIELDS "  + name);
		   EditorGUILayout.Separator(); 
		   System.Reflection.FieldInfo[] fields = value.GetType().GetFields();
		   EditorGUI.indentLevel++;
		   foreach(System.Reflection.FieldInfo field in fields)
		   {
		      if(field.IsPublic && !field.IsStatic)
		      {
				DrawDefaultField(value,field);
		      }         
		   }
		 
		   EditorGUI.indentLevel--;
	}
	public static bool IsFieldAList(System.Reflection.FieldInfo field)
	{
		return field.FieldType.GetInterface(typeof(IList<>).FullName) != null;
	}
	
	public static void DrawDefaultField(object objInstance,System.Reflection.FieldInfo field)
	{
		if(field.FieldType == typeof(int))
		{
			field.SetValue(objInstance, EditorGUILayout.IntField(MakeLabelFromField(field), (int) field.GetValue(objInstance)));
		}   
		else if(field.FieldType == typeof(float))
		{
			field.SetValue(objInstance, EditorGUILayout.FloatField(MakeLabelFromField(field), (float) field.GetValue(objInstance)));
		}
		else if(field.FieldType == typeof(string))
		{
			field.SetValue(objInstance, EditorGUILayout.TextField(MakeLabelFromField(field), (string) field.GetValue(objInstance)));
		}
		else if(IsFieldAList(field))
		{
			IList collection = (IList)field.GetValue(objInstance);
			DrawList(field.Name,collection);
			//DrawList
		}
		/*
		else if(field.FieldType == typeof(List<MotinScriptData>))
		{
			foreach(MotinScriptEditor editor in scriptEditors)
			{
				editor.Draw();
			}
		}
		*/
		else if(field.FieldType.IsClass)
		{
			return;
			/*
	        System.Type[] parmTypes = new System.Type[]{ field.FieldType};
	
	        string methodName = "DrawDefaultInspectors";
			
	        System.Reflection.MethodInfo drawMethod = 
	           typeof(EditorGUILayout).GetMethod(methodName);
	
	        if(drawMethod == null)
	        {
	           Debug.LogError("No method found: " + methodName);
	        }
	
	        bool foldOut = true;
	
	        drawMethod.MakeGenericMethod(parmTypes).Invoke(null, 
	           new object[]
	           {
	              MakeLabel(field),
	              field.GetValue(scriptData)
	           });
		               
	         }      
	         else
	         {
	            Debug.LogError(
	               "DrawDefaultInspectors does not support fields of type " +
	               field.FieldType);
	               */
		}
	}
	
	public static  GUIContent MakeLabelFromField(System.Reflection.FieldInfo field)
	{
	   GUIContent guiContent = new GUIContent();      
	   guiContent.text = field.Name /*.SplitCamelCase()*/;      
	   object[] descriptions = 
	      field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
	 
	   if(descriptions.Length > 0)
	   {
	      //just use the first one.
	      guiContent.tooltip = 
	         (descriptions[0] as System.ComponentModel.DescriptionAttribute).Description;
	   }
	 
	   return guiContent;
	}
	
	public static void DrawMotinValues(string labelText,List<MotinValue> motinValues)
	{
		GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.BeginHorizontal();
				GUILayout.Label(labelText);
				GUILayout.FlexibleSpace();
				/*
				if(GUILayout.Button("ADD",GUILayout.Width(70)))
				{
					motinData.motinValues.Add(new BiomaScriptData());
					entitiesPopupSelections.Add(BiomaEntityData.EntityType.PLANT);
				}
				*/
			GUILayout.EndHorizontal();
			foreach(MotinValue value in motinValues)
			{
			
				//GUILayout.BeginVertical(GUI.skin.box);
					//GUILayout.BeginHorizontal();
					//GUILayout.Label(labelText + motinValues.IndexOf(value));
					//GUILayout.FlexibleSpace();
					/*
					if(GUILayout.Button("DELETE",GUILayout.Width(70)))
					{
						motinData.motinValues.Remove(value);
						break;
					}
					*/
					//GUILayout.EndHorizontal();
					MotinEditorUtils.DrawMotinValue(value);
				//GUILayout.EndVertical();
			}
		
		GUILayout.EndVertical();
	}
	public static void DrawMotinValue(MotinValue value,bool allowTypeEdit=false)
	{
		MotinEditorUtils.DrawMotinValue(value,Color.white,allowTypeEdit);
	}
	public static void DrawMotinValue(MotinValue value,Color color,bool allowTypeEdit=false)
	{
		Color color_original = GUI.backgroundColor;
		GUI.backgroundColor = color;
		
		GUILayout.BeginVertical(GUI.skin.box);
			GUILayout.BeginHorizontal();
			if(allowTypeEdit)
			{
				value.name = EditorGUILayout.TextField("name:",value.name);
			
				int newVal = (int)((MotinValue.ValueType)EditorGUILayout.EnumPopup("type:",(MotinValue.ValueType)value.valueType));
			
				if(value.valueType!= newVal)
				{
					switch((MotinValue.ValueType)value.valueType)
					{
						case MotinValue.ValueType.Object:
							value.val_obj = null;
						break;
					}
					value.valueType = newVal;
				}
			}
			else
			{
				GUILayout.Label(value.name);
				GUILayout.Label("type:"+ ((MotinValue.ValueType)value.valueType).ToString());
			}
			
			switch((MotinValue.ValueType)value.valueType)
			{
				case MotinValue.ValueType.Integer:
					value.val_int = EditorGUILayout.IntField(value.val_int);
					Debug.Log("MOTIN_VALUE " + value.name + "[ " + value.val_int + " ]" );
				break;
				case MotinValue.ValueType.Long:
				//	value.val_int = EditorGUILayout.IntField(value.val_int);
				break;
				case MotinValue.ValueType.Float:
				value.val_float = EditorGUILayout.FloatField(value.val_float);
				break;
				case MotinValue.ValueType.Double:
					//value.val_double =(double) EditorGUILayout.FloatField(value.val_double);
				break;
				case MotinValue.ValueType.Vector2:
					value.val_vect2 = EditorGUILayout.Vector2Field("",value.val_vect2);
				break;
				case MotinValue.ValueType.Vector3:
					value.val_vect3 =EditorGUILayout.Vector3Field("",value.val_vect3);
				break;
				case MotinValue.ValueType.Vector4:
					value.val_vect4 = EditorGUILayout.Vector4Field("",value.val_vect4);
				break;
				case MotinValue.ValueType.Color:
					value.val_color =EditorGUILayout.ColorField(value.val_color);
				break;
				case MotinValue.ValueType.Rect:
					value.val_rect = EditorGUILayout.RectField(value.val_rect);
				break;
				case MotinValue.ValueType.String:
					Debug.Log("MOTIN_VALUE " + value.name + "[ " + value.val_string + " ]" );
					value.val_string = EditorGUILayout.TextField(value.val_string);
				break;
				case MotinValue.ValueType.Char:
				break;
				case MotinValue.ValueType.Object:
					value.val_obj = EditorGUILayout.ObjectField("",value.val_obj,typeof(Object));
				break;
				case MotinValue.ValueType.Array:
					Debug.Log("MOTIN_VALUE ERROR DRAW ARRAY " + value.name + "[ " + value.lsArray.Count + " ]" );
				break;
				case MotinValue.ValueType.Boolean:
					value.val_bool = EditorGUILayout.Toggle (value.val_bool);
				break;
				
			}
			GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		
		GUI.backgroundColor = color_original;
	}
	
/*	public static int ClampClipId (tk2dSpriteAnimation animationLib,int animClipId)
	{
		int clipId = animClipId;
				// Sanity check clip id
		clipId = Mathf.Clamp(clipId, 0, animationLib.clips.Length - 1);
		return clipId;
	}
	*/
	public static string TypeStringBrief(System.Type t) {
		if(t.IsArray) return TypeStringBrief(t.GetElementType())+"[]";
		if(t == typeof(int)) return "int";
		if(t == typeof(long)) return "long";
		if(t == typeof(float)) return "float";
		if(t == typeof(double)) return "double";
		if(t == typeof(Vector2)) return "Vector2";
		if(t == typeof(Vector3)) return "Vector3";
		if(t == typeof(Vector4)) return "Vector4";
		if(t == typeof(Color)) return "Color";
		if(t == typeof(Rect)) return "Rect";
		if(t == typeof(string)) return "string";
		if(t == typeof(char)) return "char";
		return t.Name;
	}
	
	public static string GetMethodInfoSignature(System.Reflection.MethodInfo methodInfo) {
		System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
		// loop through parameters, add them to signature
		string methodString = methodInfo.Name + " (";
		for(int i=0;i<parameters.Length;i++) {
			methodString += TypeStringBrief(parameters[i].ParameterType);
			if(i<parameters.Length-1) methodString += ", ";
		}
		methodString += ")";
		return methodString;
	}
	
	public static void UpdateCachedMethodInfo(GameObject go,System.Reflection.BindingFlags methodFlags,List<System.Reflection.MethodInfo> cachedMethodInfo,List<string> cachedMethodNames,List<Component> cachedMethodInfoComponents) {
		if(!go) return;
		
		if(cachedMethodInfo==null)
		{
			cachedMethodInfo = new List<System.Reflection.MethodInfo>();
		}
		if(cachedMethodNames==null)
		{
			cachedMethodNames = new List<string>();
		}
		if(cachedMethodInfoComponents==null)
		{
			cachedMethodInfoComponents = new List<Component>();
		}
		
		cachedMethodInfo.Clear();
		cachedMethodNames.Clear();
		cachedMethodInfoComponents.Clear();
		
		Component[] arrComponents = go.GetComponents(typeof(Component));
			foreach(Component c in arrComponents) {
				if(c.GetType().BaseType == typeof(Component) || c.GetType().BaseType == typeof(Behaviour)) continue;
					System.Reflection.MethodInfo[] methodInfos = c.GetType().GetMethods(methodFlags);
					foreach(System.Reflection.MethodInfo methodInfo in methodInfos) {
						if((methodInfo.Name == "Start") || (methodInfo.Name == "Update") || (methodInfo.Name == "Main")) continue;
						cachedMethodNames.Add(MotinEditorUtils.GetMethodInfoSignature(methodInfo));
						cachedMethodInfo.Add(methodInfo);
						cachedMethodInfoComponents.Add(c);
					}
			}	
	}
	
	
	public static string SaveFileInProject(string title, string directory, string filename, string ext)
	{
		string path = EditorUtility.SaveFilePanel(title, directory, filename, ext);
		if (path.Length == 0) // cancelled
			return "";
		string cwd = System.IO.Directory.GetCurrentDirectory().Replace("\\","/") + "/assets/";
		if (path.ToLower().IndexOf(cwd.ToLower()) != 0)
		{
			path = "";
			EditorUtility.DisplayDialog(title, "Assets must be saved inside the Assets folder", "Ok");
		}
		else 
		{
			path = path.Substring(cwd.Length - "/assets".Length);
		}
		return path;
	}
	
	public static string CreateNewPrefab(string name) // name is the filename of the prefab EXCLUDING .prefab
	{
		Object obj = Selection.activeObject;
		string assetPath = AssetDatabase.GetAssetPath(obj);
		if (assetPath.Length == 0)
		{
			assetPath = MotinEditorUtils.SaveFileInProject("Create...", "Assets/", name, "prefab");
		}
		else
		{
			// is a directory
			string path = System.IO.Directory.Exists(assetPath) ? assetPath : System.IO.Path.GetDirectoryName(assetPath);
			assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".prefab");
		}
		
		return assetPath;
	}
	
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	/// 
	/*
	public static T CreateAsset<T> (string path) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
 
		//string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
 
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
 
		AssetDatabase.CreateAsset (asset, assetPathAndName);
 
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
		
		return T;
	}*/
	public static void CreateAsset (string assetPath,ScriptableObject objInstance) 
	{
		//Debug.Log("AssetPath  " + path);
		//string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + objInstance.name + ".asset");
		
		//string assetPathAndName = folderPath + "/" + objInstance.name + ".asset";
		//Debug.Log("AssetPath name " + assetPathAndName);
		AssetDatabase.CreateAsset (objInstance, assetPath);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		//EditorUtility.FocusProjectWindow ();
		Selection.activeObject = objInstance;
		
		//return T;
	}
	
	/*
	public static void UnloadUnusedAssets()
	{
		Object[] previousSelectedObjects = Selection.objects;
		Selection.objects = new Object[0];
		
		System.GC.Collect();
		EditorUtility.UnloadUnusedAssets();
		
		index = null;
		
		Selection.objects = previousSelectedObjects;
	}	
*/
	public static void CollectAndUnloadUnusedAssets()
	{
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		EditorUtility.UnloadUnusedAssets();
	}

	public static void DeleteAsset(UnityEngine.Object obj)
	{
		if (obj == null) return;
		UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(obj));
	}

	public static bool IsPrefab(Object obj)
	{
#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4)
		return AssetDatabase.GetAssetPath(obj).Length != 0;
#else
		return (PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab);
#endif
	}

	public static void SetGameObjectActive(GameObject go, bool active)
	{
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9
		go.SetActiveRecursively(active);
#else
		go.SetActive(active);
#endif		
	}

	public static bool IsGameObjectActive(GameObject go)
	{
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9
		return go.active;
#else
		return go.activeSelf;
#endif		
	}

	/*
	public static tk2dSpriteCollectionData SpriteCollectionList(string label, tk2dSpriteCollectionData currentValue)
	{
		GUILayout.BeginHorizontal();
		if (label.Length > 0)
			EditorGUILayout.PrefixLabel(label);

		currentValue = tk2dSpriteGuiUtility.SpriteCollectionList(currentValue);
		GUILayout.EndHorizontal();

		return currentValue;
	}
	*/


	public static void WriteDefinesFile(string enumName,string Namespace,string filePath,List<string> defineNames)
	{
		List<string> textLines = new List<string>();

		WriteHeader(textLines,Namespace);
		WriteDefines(textLines,enumName,defineNames);
		WriteFooter(textLines,Namespace);

		System.IO.File.WriteAllLines(Application.dataPath + filePath /* "/MotinGames/SoundManager/SoundDefinitions.cs"*/, textLines.ToArray());
		
		AssetDatabase.Refresh();
	}

	protected static void WriteHeader(List<string> textLines,string Namespace)
	{
		textLines.Add ("using UnityEngine;");
		if(Namespace!=null && Namespace!="")
			textLines.Add ("namespace " + Namespace + " {");

		
	}
	protected static void WriteDefines(List<string> textLines,string enumName,List<string> defineNames)
	{
		textLines.Add ("	public enum " + enumName);
		textLines.Add ("	{");
		

		for(int i =0 ;i < defineNames.Count;i++)
		{
			textLines.Add("		" + defineNames[i] + "=" + i.ToString() + ",");
		}
		textLines.Add("		COUNT=" + defineNames.Count.ToString());
		textLines.Add ("	}");
	}

	protected static void WriteFooter(List<string> textLines,string Namespace)
	{

		if(Namespace!=null && Namespace!="")
			textLines.Add ("}");
		
	}
	
}
