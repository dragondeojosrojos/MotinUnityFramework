
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

	[System.Serializable]
	public class MotinEditor  {
		
		//public delegate void MotinEditorDelegate();
		//public delegate void MotinEditorEvent(MotinEditor editor );
		//public delegate void DataEventDelegate(object target, int data);

		public System.Action<MotinEditor>	OnDeletePressed;
		public System.Action<MotinEditor> 	OnUpPressed;
		public System.Action<MotinEditor> 	OnDownPressed;
		public System.Action				OnDataChanged;
		public System.Action<MotinEditor>	OnDataChangedEditor;
		

		protected virtual void RaiseOnDeletePressed() { if (OnDeletePressed != null) OnDeletePressed(this); }
		protected virtual void RaiseOnUpPressed() { if (OnUpPressed != null) OnUpPressed(this); }
		protected virtual void RaiseOnDownPressed() { if (OnDownPressed != null) OnDownPressed(this); }
		protected virtual void RaiseOnDataChanged() { if (OnDataChanged != null) OnDataChanged(); if (OnDataChangedEditor!= null) OnDataChangedEditor(this); }

		public bool drawDefaultEditor = false;

		public bool foldOpen = true;
		public Color editorColor = Color.white;
		public bool showListToolbar = false;
		protected Rect editorRect = new Rect(0,0,0,0);
		// Locals
		//int editorWidth_ = 150;
		//int editorHeight_ = 150;
		
		//int minInspectorWidth = 200;
		//int minInspectorHeight = 200;
		public string  	editorName = "Default Editor";
		public float 	editorWidth { get { return editorRect.width; } set { editorRect.width =value;/* Mathf.Max(value, minInspectorWidth); */} }
		public float 	editorHeight { get { return editorRect.height; } set { editorRect.height =value;/* Mathf.Max(value, minInspectorHeight);*/ } }
		
		public EditorWindow hostEditorWindow = null;
		
		bool	targetSetFirstTime =true;

		[SerializeField]
		protected object target_ = null;
		public object target
		{
			get {return target_;}
			set{
				if(value!=target_ || targetSetFirstTime)
				{
					targetSetFirstTime = false;
					target_ = value;
					targetUpdated();
				}
				
			}
		}
		public void ForceTarget(object value)
		{
			target_ = value;
			targetUpdated();
		}

		protected virtual void targetUpdated()
		{
	//		Debug.Log ("TARGET UPDATED " + target.GetType().ToString());
			//editorName ="["+ target.GetType().ToString() + "] " + target_.ToString();
		}
		public MotinEditor( )	
		{
			hostEditorWindow = null;
			Initialize();
		}
		public MotinEditor(EditorWindow hostWindow)
		{
			hostEditorWindow =hostWindow;
			Initialize();
		}


		
		protected virtual void Initialize()
		{
			
		}
		public virtual void Destroy()
		{

		}
		public void Repaint() { 
				if (hostEditorWindow != null) {
					hostEditorWindow.Repaint();
				}
				else {
					HandleUtility.Repaint();
				}
			}
		
		/*
		public virtual void Destroy()
		{
			hostEditorWindow =null;
		}
		*/
		public void Draw(Rect position,bool expand = true)
		{
			//if (target==null)
			//	return;
			
			editorRect = position;
			//Debug.Log("MOTIN EDITOR  DRAW");
			//editorWidth = windowWidth;
			//editorHeight= windowHeight;
			Color startColor = GUI.color ;
			GUI.color = editorColor;
			if(foldOpen)
			{
				
				//GUI.BeginGroup(editorRect);
				if(expand)
				{
					GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.Height(position.height),GUILayout.MinHeight(200));
					//GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
				}
				else
					GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.Height(position.height));

				DrawToolbar();
				//GUI.color = Color.white;
				GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
				DoDraw();



				GUILayout.EndVertical();
				GUILayout.EndVertical();
				
				
				//GUI.EndGroup();
				
			}
			else
			{
				//GUI.color = editorColor;
				GUILayout.BeginVertical(EditorStyles.toolbar ,  GUILayout.ExpandWidth(true),GUILayout.Height(70));
				foldOpen = EditorGUILayout.Foldout(foldOpen,editorName);
			//GUILayout.Label("DEFAULT EDITOR");
				GUILayout.EndVertical();
				//DrawClosed();

				//GUI.color = Color.white;
			}
				
			GUI.color = startColor;
			
			if(GUI.changed)
				RaiseOnDataChanged();
			
		}
		protected virtual void DrawClosed()
		{
			//GUI.color = editorColor;
			//GUI.BeginGroup(editorRect);
			//GUI.BeginGroup(new Rect(editorRect.x,editorRect.y,editorRect.width,150f));
			GUILayout.BeginHorizontal(EditorStyles.toolbar ,  GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
			foldOpen = EditorGUILayout.Foldout(foldOpen,editorName);
			//GUILayout.Label("DEFAULT EDITOR");
			GUILayout.EndHorizontal();
			//GUI.EndGroup();
			//GUI.color = Color.white;
			
		}
		protected virtual void DoDraw()
		{
			//Debug.Log("DATA EDITOR DO DRAW");
			if(target==null)
				return;
			
			GUILayout.BeginVertical();
			DrawFields();
			GUILayout.EndVertical();
		}
		
		protected virtual void DrawToolbarButtons()
		{
			GUILayout.Label("Editor");
		}
		protected void  DrawToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar,GUILayout.ExpandWidth(true) );
			foldOpen = EditorGUILayout.Foldout(foldOpen,editorName);
			GUILayout.FlexibleSpace();
			DrawToolbarButtons();
			if(showListToolbar)
			{
				
				if(GUILayout.Button("Up",EditorStyles.toolbarButton))
				{
					RaiseOnUpPressed();
				}
				if(GUILayout.Button("Down",EditorStyles.toolbarButton))
				{
					RaiseOnDownPressed();
				}
				if(GUILayout.Button("Del",EditorStyles.toolbarButton))
				{
					RaiseOnDeletePressed();
				}
			}
			
			GUILayout.EndHorizontal();
		}

		protected  GUIContent MakeLabel(System.Reflection.FieldInfo field)
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
		
		
		public virtual void DrawFields()
		{
			DrawFields(target);
		}
		
		protected void DrawFields(object value)
		{
			DrawFields(value,editorRect);
		}
		
		protected void DrawField(object value,string fieldName)
		{
			GUILayout.BeginVertical();
			  // Debug.Log("MOTINSCRIPT EDITOR DRAWFIELDS "  + name);
			  
			   System.Type type = value.GetType();      
			   System.Reflection.FieldInfo field = type.GetField(fieldName);
			   EditorGUI.indentLevel++;
			  
				
			      if(field!=null && field.IsPublic && !field.IsStatic)
			      {
			        if(!DrawField(value,field))
					{
						DrawDefaultField(value,field);
					}
			      }         
			
			 
			   EditorGUI.indentLevel--;
			GUILayout.EndVertical();
		}
		protected void DrawFields(object value,Rect viewRect)
		{
			//GUI.BeginGroup(viewRect);
			bool changed = false;
			GUILayout.BeginVertical();
			  // Debug.Log("MOTINSCRIPT EDITOR DRAWFIELDS "  + name);
			  
			   System.Type type = value.GetType();      
			   System.Reflection.FieldInfo[] fields = type.GetFields();
			   //EditorGUI.indentLevel++;
			   System.Reflection.FieldInfo field  = null;
			
			   for(int i =fields.Length-1;  i>=0 ;i--)
			   {
				  field = fields[i];
	//				Debug.Log("Draw Field "  + field.Name + " " + field.FieldType.ToString());
			      if(field.IsPublic && !field.IsStatic && field.GetCustomAttributes(typeof(HideInInspector),false).Length==0)
			      {
			        if(!DrawField(value,field))
					{
						if(DrawDefaultField(value,field))
						{
							changed = true;
						}
					}
			      }         
			   }
			 
			   //EditorGUI.indentLevel--;
			GUILayout.EndVertical();
			
			if(changed)
				RaiseOnDataChanged();
			//GUI.EndGroup();
		}
		
		protected bool DrawDefaultField(object value,System.Reflection.FieldInfo field)
		{
			object oldValue =  field.GetValue(value);
			object newValue = oldValue;
			if(field.GetCustomAttributes(typeof(MotinEditorReadonlyField),false).Length==0)
			{
				if(field.FieldType == typeof(int))
				{
					newValue =(object)  EditorGUILayout.IntField(MakeLabel(field), (int) oldValue);
				} 
				if(field.FieldType == typeof(bool))
				{
					newValue =(object)  EditorGUILayout.Toggle(MakeLabel(field), (bool) oldValue);
				}   
				else if(field.FieldType == typeof(float))
				{
					newValue =(object) EditorGUILayout.FloatField(MakeLabel(field), (float) oldValue);
				}
				else if(field.FieldType == typeof(string))
				{
					if(field.GetCustomAttributes(typeof(MotinEditorSoundEnumField),false).Length!=0)
					{
						SoundDefinitions.Sounds tmpSound = (SoundDefinitions.Sounds)EditorGUILayout.EnumPopup(field.Name,MotinUtils.StringToEnum<SoundDefinitions.Sounds>((string)oldValue));
						if(tmpSound.ToString() != (string)oldValue)
						{
							newValue = tmpSound.ToString(); 
							//EditorUtility.SetDirty(stringComponent);
							//AssetDatabase.SaveAssets();
						}
					}
					else if(field.GetCustomAttributes(typeof(MotinEditorStringEnumField),false).Length!=0)
					{
						MotinStrings tmpString = (MotinStrings)EditorGUILayout.EnumPopup(field.Name,MotinUtils.StringToEnum<MotinStrings>((string)oldValue));
						if(tmpString.ToString() != (string)oldValue)
						{
							newValue = tmpString.ToString(); 
							//EditorUtility.SetDirty(stringComponent);
							//AssetDatabase.SaveAssets();
						}
					}
					else if(field.GetCustomAttributes(typeof(MotinEditorEnumField),false).Length!=0)
					{
						MotinEditorEnumField motinEnumAttr = (MotinEditorEnumField)field.GetCustomAttributes(typeof(MotinEditorEnumField),false)[0];

						string tmpString = DrawEnumField(field.Name,motinEnumAttr.enumeration,(string)oldValue);
						if(tmpString != (string)oldValue)
						{
							newValue = tmpString; 
							//EditorUtility.SetDirty(stringComponent);
							//AssetDatabase.SaveAssets();
						}
					}
					else if(field.GetCustomAttributes(typeof(MotinEditorMotinDataEnumField),false).Length!=0)
					{

						/*
						string[] dataNames = MotinDataManager.GetDataNames(motinDataAttr.filePath);
						int currentIndex = MotinUtils.StringArrayIndex(dataNames,(string)oldValue);
						if(currentIndex==-1)
								currentIndex = dataNames.Length-1;

						string tmpString =dataNames[ EditorGUILayout.Popup(field.Name,currentIndex,dataNames)];
						*/
						string tmpString = DrawMotinDataEnumField(field,(string)oldValue);
						if(tmpString != (string)oldValue)
						{
							newValue = tmpString; 
							//EditorUtility.SetDirty(stringComponent);
							//AssetDatabase.SaveAssets();
						}
					}
					else
					{
						newValue =(object) EditorGUILayout.TextField(MakeLabel(field), (string) oldValue);
					}

				}

				else if(field.FieldType == typeof(Vector2))
				{
					newValue =(object)  EditorGUILayout.Vector2Field(field.Name, (Vector2) oldValue);
				}
				else if(field.FieldType == typeof(Vector3))
				{
					newValue =(object)  EditorGUILayout.Vector3Field(field.Name, (Vector3) oldValue);
				}
				else if(field.FieldType == typeof(Vector4))
				{
					newValue =(object)  EditorGUILayout.Vector4Field(field.Name, (Vector4) oldValue);
				}
				else if(field.FieldType == typeof(Rect))
				{
					newValue =(object)  EditorGUILayout.RectField(field.Name, (Rect) oldValue);
				}
				else if(field.FieldType == typeof(Color))
				{
					newValue =(object)  EditorGUILayout.ColorField(field.Name, (Color) oldValue);
				}
				else if(field.FieldType.BaseType == typeof(Object))
				{
					newValue =(object)  EditorGUILayout.ObjectField(field.Name,(Object)oldValue,field.FieldType,true);
				}
				else if(field.FieldType.IsEnum )
				{
					newValue =  EditorGUILayout.EnumPopup(field.Name,(System.Enum)System.Enum.ToObject(field.FieldType,oldValue));
				}
				else if(field.FieldType.IsClass)
				{
					newValue=oldValue;
				}
				
				if(newValue==null && oldValue!=null)
				{
					//Debug.Log("DEFAULT CHANGED  "+ field.Name + "  " + newValue.ToString()+ "  " + oldValue.ToString());
					field.SetValue(value, newValue);
					FieldValueChanged(value,field);
					return true;
				}
				else if ( newValue !=null && !newValue.Equals(oldValue))
				{
					field.SetValue(value, newValue);
					FieldValueChanged(value,field);
					return true;
				}
				else
				{
					return false;
				}
			}
			else	
			{
				
				GUILayout.BeginHorizontal();
				GUILayout.Space(10);
				GUILayout.Label(field.Name + ":	" );
				GUILayout.FlexibleSpace();
				GUILayout.Label(field.GetValue(value).ToString());
				GUILayout.EndHorizontal();
				return false;
					
			}
		}
		
		protected string DrawMotinDataEnumField(System.Reflection.FieldInfo field,string currentValue)
		{
			MotinEditorMotinDataEnumField motinDataAttr = (MotinEditorMotinDataEnumField)field.GetCustomAttributes(typeof(MotinEditorMotinDataEnumField),false)[0];
			string[] dataNames = MotinDataManager.GetDataNames(motinDataAttr.filePath);
			int currentIndex = MotinUtils.StringArrayIndex(dataNames,currentValue);
			if(currentIndex==-1)
				currentIndex = dataNames.Length-1;

			return dataNames[ EditorGUILayout.Popup(field.Name,currentIndex,dataNames)];
		}

		protected string DrawEnumField(string fieldName,System.Type enumerationType,string currentValue)
		{
			string[] dataNames = System.Enum.GetNames(enumerationType);
			int currentIndex = MotinUtils.StringArrayIndex(dataNames,currentValue);
			if(currentIndex==-1)
				currentIndex = dataNames.Length-1;
			
			return dataNames[ EditorGUILayout.Popup(fieldName,currentIndex,dataNames)];
		}

		protected virtual bool DrawField(object value,System.Reflection.FieldInfo field) 
		{
			/*
			if(field.Name == "childScripts")
			{
				foreach(MotinScriptEditor editor in scriptEditors)
				{
					if(cancelUpdate)
					{
						cancelUpdate=false;
						return true;
					}
					editor.Draw();
					if(cancelUpdate)
					{
						cancelUpdate=false;
						return true;
					}
				}
				return true;
			}
			*/
			return false;
		}

		protected virtual void FieldValueChanged(object value,System.Reflection.FieldInfo field) 
		{

		}
	}
}
