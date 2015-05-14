
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
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
		public System.Action				OnEditorChanged;
		public System.Action<MotinEditor>	OnEditorChangedWithEditor;
		//public System.Action	OnFieldValueChanged = null;
		public System.Action<object,System.Reflection.FieldInfo>	OnFieldValueChanged = null;
		

		protected virtual void RaiseOnDeletePressed() { if (OnDeletePressed != null) OnDeletePressed(this); }
		protected virtual void RaiseOnUpPressed() { if (OnUpPressed != null) OnUpPressed(this); }
		protected virtual void RaiseOnDownPressed() { if (OnDownPressed != null) OnDownPressed(this); }
		protected virtual void RaiseOnFieldValueChanged(object instance,System.Reflection.FieldInfo field) { SetDirty();FieldValueChanged(instance,field); if (OnFieldValueChanged != null) OnFieldValueChanged(instance,field); }
		protected virtual void RaiseOnEditorChanged() { if (OnEditorChanged != null) OnEditorChanged(); if (OnEditorChangedWithEditor!= null) OnEditorChangedWithEditor(this); }
		//protected virtual void RaiseOnDataChanged() { if (OnDataChanged != null) OnDataChanged(); if (OnDataChangedEditor!= null) OnDataChangedEditor(this); }

		public bool drawDefaultEditor = false;

		public bool foldOpen = true;
		public Color editorColor = Color.white;
		public Color GetColor()
		{
			if(editorColor== Color.white && parentEditor!=null)
				return parentEditor.GetColor();

			return editorColor;
		}
		public bool showListToolbar = false;

		public Rect		editorContentRect = new Rect(0,0,0,0);

		protected Rect editorRect = new Rect(0,0,0,0);
		// Locals
		//int editorWidth_ = 150;
		//int editorHeight_ = 150;
		
		//int minInspectorWidth = 200;
		//int minInspectorHeight = 200;
		public string 	editorFieldName = "";
		protected  List<MotinEditor> 	motinEditors_ = new List<MotinEditor>();

		public string  	editorName = "Default Editor";
		public float 	editorWidth { get { return editorRect.width; } set { editorRect.width =value;/* Mathf.Max(value, minInspectorWidth); */} }
		public float 	editorHeight { get { return editorRect.height; } set { editorRect.height =value;/* Mathf.Max(value, minInspectorHeight);*/ } }
		
		public EditorWindow hostEditorWindow = null;
		public MotinEditor 	parentEditor = null;

		protected	List<string>		readOnlyFieldsList = new List<string>();
		protected	List<string>		hideFieldsList = new List<string>();

		protected Dictionary<string,string[]> popupNamesLookup = new Dictionary<string, string[]>();
		protected Dictionary<string,int[]> 	  dataIntIdsLookup = new Dictionary<string, int[]>();
		bool	targetSetFirstTime =true;

		[SerializeField]
		protected object target_ = null;
		public object target
		{
			get {return target_;}
			set{
				if(value!=target_ || targetSetFirstTime)
				{
					popupNamesLookup.Clear();
					dataIntIdsLookup.Clear();
					targetSetFirstTime = false;
					target_ = value;
					UpdateEditorClassSettings();
					targetUpdated();
				}
				
			}
		}
		public void ForceTarget(object value)
		{
			target_ = value;
			targetUpdated();
		}


		protected bool	isDirty = false;
		public void SetDirty()
		{
			isDirty = true;
		}

		protected virtual void targetUpdated()
		{
	//		Debug.Log ("TARGET UPDATED " + target.GetType().ToString());
			editorName =editorFieldName ;
			if(target!=null)
					editorName += "["+ target.GetType().ToString() + "] "+ target_.ToString();
		}



		void UpdateEditorClassSettings()
		{
			readOnlyFieldsList.Clear();
			hideFieldsList.Clear();

			if(target_ == null)
				return;

			object[] decorators =  target_.GetType().GetCustomAttributes(typeof(MotinEditorClassReadonlyFields),true);

			MotinEditorClassReadonlyFields readOnlyDecorator = null;

			foreach(object objDecorator in decorators)
			{
				readOnlyDecorator = (MotinEditorClassReadonlyFields)objDecorator;
				foreach(string fieldName in readOnlyDecorator.readonlyFields)
				{
					if(readOnlyFieldsList.Contains(fieldName))
						continue;

					readOnlyFieldsList.Add(fieldName);
				}
			}

			decorators =  target_.GetType().GetCustomAttributes(typeof(MotinEditorClassHideFields),true);
			MotinEditorClassHideFields hideDecorator = null;
			
			foreach(object objDecorator in decorators)
			{
				hideDecorator = (MotinEditorClassHideFields)objDecorator;
				foreach(string fieldName in hideDecorator.hideFields)
				{
					if(hideFieldsList.Contains(fieldName))
						continue;
					
					hideFieldsList.Add(fieldName);
				}
			}

		

		}

		// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // 
		//SUB EDITORS HANDLING
		protected int GetEditorIndex ( object targetObject)
		{
			for(int i = 0 ; i < motinEditors_.Count; i ++)
			{
				if(motinEditors_[i].target == targetObject)
					return i;
			}
			return -1;
		}

		private MotinEditor CreateInitializedEditor(System.Reflection.FieldInfo newEditorField ,object fieldObject,int index = -1)
		{
			MotinEditor editor = CreateInitializedEditor(newEditorField.FieldType,fieldObject,index);
			if(editor == null)
				return null;

			editor.editorFieldName = newEditorField.Name;
			object[] foundAttrs = newEditorField.GetCustomAttributes(typeof(MotinEditorFieldSettings),false);
			MotinEditorFieldSettings editorFieldSettings = null;
			if(foundAttrs!=null && foundAttrs.Length>0)
			{
				editorFieldSettings = (MotinEditorFieldSettings)newEditorField.GetCustomAttributes(typeof(MotinEditorFieldSettings),false)[0];
			}

			if(editorFieldSettings!=null)
			{
				editor.editorColor = editorFieldSettings.editorColor;
				editor.editorName = editorFieldSettings.editorName ;
			}

			if(newEditorField.FieldType.IsGenericType && newEditorField.FieldType.GetGenericTypeDefinition() == typeof(List<>) )
				SetArrayEditorTypesAndNamespaces(newEditorField,(MotinArrayBaseEditor)editor);

			return editor;
		}


		protected virtual MotinEditor CreateInitializedEditor(Type dataType,object target,int index = -1)
		{
			MotinEditor editor = CreateEditor(dataType);
			return CreateInitializedEditor(editor, target, index );
		}
		protected virtual MotinEditor CreateInitializedEditor(MotinEditor editor,object editorTarget,int index = -1)
		{
			editor.OnEditorChangedWithEditor+=OnSubEditorChanged;
			editor.target = editorTarget;
			editor.editorName = editorTarget.GetType().Name;
			editor.parentEditor = this;
			if(index<0)
				motinEditors_.Add(editor);
			else
				motinEditors_.Insert(index,editor);
			
			return editor;
		}

		protected virtual MotinEditor CreateEditor(Type dataType)
		{
//			Debug.Log ("CREATE EDITOR DATA TYPE " + dataType.Name);
			string editorClassSuffix = "";
			string dataTypeName = "";
			bool isList = false;
			if(dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>))
			{
				isList = true;
				editorClassSuffix = "ArrayEditor";
				dataTypeName = dataType.GetGenericArguments()[0].Name ;
			}
			else
			{
				dataTypeName = dataType.Name ;
				editorClassSuffix = "Editor";
			}


			Type editorType = Types.GetType(dataTypeName + editorClassSuffix,"Assembly-CSharp-Editor");
			MotinEditor editor = null;

			if(editorType!=null)
			{
//				Debug.Log("Creating instance for " + editorType.Name);
				editor = (MotinEditor)System.Activator.CreateInstance(editorType);

				//if(isList)
				//{
				//	((MotinArrayBaseEditor)editor).AddType(dataType.GetGenericArguments()[0]);
				//}

				return editor;
			}
		

			if(isList)
			{
				return new MotinArrayBaseEditor();
			}
			else
			{
				if(MotinUtils.IsTypeDerivedFrom(dataType,typeof(MotinData)))
				{
					return new MotinDataEditor(hostEditorWindow,null);
				}
				return new MotinEditor(hostEditorWindow,null);
			}


			
		}

		void SetArrayEditorTypesAndNamespaces(System.Reflection.FieldInfo fieldInfo, MotinArrayBaseEditor arrayEditor)
		{
			MotinEditorFieldListNamespaces[] 	namespacesAttr 	= (MotinEditorFieldListNamespaces[])fieldInfo.GetCustomAttributes(typeof(MotinEditorFieldListNamespaces),true);
			MotinEditorFieldListTypes[] 		typesAttr	 	=(MotinEditorFieldListTypes[]) fieldInfo.GetCustomAttributes(typeof(MotinEditorFieldListTypes),true);

			foreach(MotinEditorFieldListNamespaces namespaceAttr in namespacesAttr)
			{
				foreach(string namespaceName in namespaceAttr.namespaces)
				{
					arrayEditor.AddNamespace(namespaceName);
				}
			}

			foreach(MotinEditorFieldListTypes typeAttr in typesAttr)
			{
				foreach(Type type in typeAttr.types)
				{
					arrayEditor.AddType(type);
				}
			}

			if( (namespacesAttr == null || namespacesAttr.Length ==0) && (typesAttr == null || typesAttr.Length ==0) )
				arrayEditor.AddType(fieldInfo.FieldType.GetGenericArguments()[0]);

			arrayEditor.SetDirty();
		}

		void OnSubEditorChanged(MotinEditor subEditor)
		{
			if(!string.IsNullOrEmpty(subEditor.editorFieldName))
			{
				System.Reflection.FieldInfo editorField =  target.GetType().GetField(subEditor.editorFieldName);
				editorField.SetValue(target,subEditor.target);
			}
			SetDirty();
			//int index = GetEditorIndex(editor.target);

		}

		// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // 
		// // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // // 

		public MotinEditor(  )	
		{
			parentEditor = null;
			hostEditorWindow = null;
			Initialize();
		}
		public MotinEditor( MotinEditor parent )	
		{
			parentEditor = parent;
			hostEditorWindow = null;
			Initialize();
		}
		public MotinEditor(EditorWindow hostWindow,MotinEditor parent)
		{
			parentEditor = parent;
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


		public void Draw(Rect position,bool expand = true)
		{
			//if (target==null)
			//	return;
			isDirty = false;
			editorRect = position;
			//Debug.Log("MOTIN EDITOR  DRAW");
			//editorWidth = windowWidth;
			//editorHeight= windowHeight;
			Color startColor = GUI.color ;
		
			GUI.color = GetColor();

			GUILayout.BeginHorizontal();
			if(parentEditor!=null)
				GUILayout.Space(10);

			if(foldOpen)
			{
				
				//GUI.BeginGroup(editorRect);
				if(expand)
				{
					GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
					//GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
				}
				else
					GUILayout.BeginVertical(GUI.skin.scrollView, GUILayout.ExpandWidth(true),GUILayout.Height(position.height));

				DrawToolbar();
				//GUI.color = Color.white;
				GUILayout.BeginVertical(MotinEditorSkin.SC_BodyBackground, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
				DoDraw();
				GUILayout.EndVertical();
				GUILayout.Space(10);
				GUILayout.EndVertical();
				
				
				//GUI.EndGroup();
				
			}
			else
			{
				//GUI.color = editorColor;
				GUILayout.BeginVertical(EditorStyles.toolbar ,  GUILayout.ExpandWidth(true),GUILayout.Height(70));
				foldOpen = EditorGUILayout.Foldout(foldOpen,editorName);
			//GUILayout.Label("DEFAULT EDITOR");
				GUILayout.Space(10);
				GUILayout.EndVertical();
				//DrawClosed();

				//GUI.color = Color.white;
			}

			GUILayout.EndHorizontal();
			GUI.color = startColor;


			
			if(isDirty)
				RaiseOnEditorChanged();

			if (Event.current.type == EventType.Repaint )
				editorContentRect = GUILayoutUtility.GetLastRect();
			
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
			
			GUILayout.BeginVertical( GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
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
			bool tmpFold =  EditorGUILayout.Foldout(foldOpen,editorName);
			if(foldOpen !=tmpFold)
			{
				foldOpen = tmpFold;
				Repaint();
			}
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
			      if(field.IsPublic && !field.IsStatic && field.GetCustomAttributes(typeof(HideInInspector),false).Length==0 && !hideFieldsList.Contains(field.Name))
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
			
			//if(changed)
			//	RaiseOnDataChanged();
			//GUI.EndGroup();
		}
		
		protected bool DrawDefaultField(object value,System.Reflection.FieldInfo field)
		{
			object oldValue =  field.GetValue(value);
			object newValue = oldValue;
			if(field.GetCustomAttributes(typeof(MotinEditorReadonlyField),false).Length==0 && !readOnlyFieldsList.Contains(field.Name))
			{
				if(field.FieldType == typeof(int))
				{
					if(field.GetCustomAttributes(typeof(MotinEditorMotinDataEnumField),false).Length!=0)
					{

						int tmpInt = DrawMotinDataEnumIntField(field,(int)oldValue);
						if(tmpInt != (int)oldValue)
						{
							newValue = tmpInt; 

						}
					}
					else
					{
						newValue =(object)  EditorGUILayout.IntField(MakeLabel(field), (int) oldValue);
					}
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
					else if(field.GetCustomAttributes(typeof(MotinEditorLocalizationEnumField),false).Length!=0)
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

						string tmpString = DrawMotinDataEnumField(field,(string)oldValue);
						if(tmpString != (string)oldValue)
						{
							newValue = tmpString; 
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
				else if(field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>) )
				{
					//Debug.Log("ES UNA LISTA " +field.FieldType.ToString() + " " + field.FieldType.GetGenericArguments()[0].ToString()  );
					if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetGenericArguments()[0] ,typeof(MotinData)))
					{
						int editorIndex = GetEditorIndex(oldValue);
						MotinEditor tmpEditor = null;
						if(editorIndex==-1)
						{

							tmpEditor = CreateInitializedEditor(field,oldValue);

						}
						else
						{
							tmpEditor = motinEditors_[editorIndex];
						}
						tmpEditor.editorFieldName = field.Name;
						tmpEditor.editorName = field.Name;
						tmpEditor.target = oldValue;
						tmpEditor.Draw(tmpEditor.editorContentRect,false);
						newValue=oldValue;
					}

				}
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType ,typeof(UnityEngine.Object)))
				{
					newValue =(object)  EditorGUILayout.ObjectField(field.Name,(UnityEngine.Object)oldValue,field.FieldType,true);
				}
				else if(field.FieldType.IsEnum )
				{
					newValue =  EditorGUILayout.EnumPopup(field.Name,(System.Enum)System.Enum.ToObject(field.FieldType,oldValue));
				}
				else if(field.FieldType.IsClass)
				{
					if(MotinUtils.IsTypeDerivedFrom(field.FieldType ,typeof(MotinData)))
					{
						int editorIndex = GetEditorIndex(oldValue);
						MotinEditor tmpEditor = null;
						if(editorIndex==-1)
						{
							tmpEditor = CreateInitializedEditor(field,oldValue);

						}
						else
						{
							tmpEditor = motinEditors_[editorIndex];
						}
						tmpEditor.editorFieldName = field.Name;
						tmpEditor.target = oldValue;
						tmpEditor.editorName = field.Name;
						tmpEditor.Draw(tmpEditor.editorContentRect,false);
						newValue=oldValue;
					}
				}
				
				if(newValue==null && oldValue!=null)
				{
					//Debug.Log("DEFAULT CHANGED  "+ field.Name + "  " + newValue.ToString()+ "  " + oldValue.ToString());
					field.SetValue(value, newValue);
					//FieldValueChanged(value,field);

					RaiseOnFieldValueChanged(value,field);

					return true;
				}
				else if ( newValue !=null && !newValue.Equals(oldValue))
				{
					field.SetValue(value, newValue);
					//FieldValueChanged(value,field);

					RaiseOnFieldValueChanged(value,field);
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
			string[] dataNames  = GetPopupNames(field.Name);
			if(dataNames==null)
			{
				MotinEditorMotinDataEnumField motinDataAttr = (MotinEditorMotinDataEnumField)field.GetCustomAttributes(typeof(MotinEditorMotinDataEnumField),false)[0];
				dataNames = MotinDataManager.GetDataNames(motinDataAttr.filePath);
				popupNamesLookup.Add(field.Name,dataNames);
				dataIntIdsLookup.Add(field.Name, MotinDataManager.GetDataIntUniqueIds(motinDataAttr.filePath));
			}

			int currentIndex = MotinUtils.StringArrayIndex(dataNames,currentValue);
			if(currentIndex==-1)
				currentIndex = dataNames.Length-1;

			return dataNames[ EditorGUILayout.Popup(field.Name,currentIndex,dataNames)];
		}

		protected int DrawMotinDataEnumIntField(System.Reflection.FieldInfo field,int intUniqueId)
		{
			string[] dataNames  = GetPopupNames(field.Name);

			if(dataNames==null)
			{
				MotinEditorMotinDataEnumField motinDataAttr = (MotinEditorMotinDataEnumField)field.GetCustomAttributes(typeof(MotinEditorMotinDataEnumField),false)[0];
				dataNames = MotinDataManager.GetDataNames(motinDataAttr.filePath);
				popupNamesLookup.Add(field.Name,dataNames);
				dataIntIdsLookup.Add(field.Name, MotinDataManager.GetDataIntUniqueIds(motinDataAttr.filePath));
			}
			int[] dataIds = GetUniqueIds(field.Name);

			int currentIndex = MotinUtils.IntArrayIndex(dataIds,intUniqueId);
			if(currentIndex==-1)
				currentIndex = dataNames.Length-1;
			
			return dataIds[ EditorGUILayout.Popup(field.Name,currentIndex,dataNames)];
		}

		protected string DrawEnumField(string fieldName,System.Type enumerationType,string currentValue)
		{

			string[] dataNames  = GetPopupNames(fieldName);
			if(dataNames==null)
			{
				dataNames = System.Enum.GetNames(enumerationType);
				popupNamesLookup.Add(fieldName,dataNames);

			}

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

		int[] GetUniqueIds(string key)
		{
			if(dataIntIdsLookup.ContainsKey(key))
				return dataIntIdsLookup[key];
			
			return null;
		}
		string[] GetPopupNames(string key)
		{
			if(popupNamesLookup.ContainsKey(key))
				return popupNamesLookup[key];

			return null;
		}
	}
}
