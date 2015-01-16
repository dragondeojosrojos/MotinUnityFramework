using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

namespace MotinGames
{

	public class MotinDataManager {
		
		public static string  resourcesPath = "Assets/Resources/";
		//static MotinDataLibrary sharedInstance = null;
		/*
		public static bool IsInitialized()
		{
			if(sharedInstance==null)
				return false;
			else
				return true;
			
		}*/
		/*
		public static MotinDataLibrary sharedManager()
		{
			if(sharedInstance==null)
				sharedInstance = new MotinDataLibrary();
			
			return sharedInstance;
		}
		*/
		/*
		public static void Init(MotinDataLibrary lib)
		{
			sharedInstance = lib;
		}
		*/
		/*
		public static MotinSerializableData CreateData(string name)
		{
			return sharedInstance.CreateDataByName(name);
		}
		*/
		
		public static System.Type GetSerializableDataType(string name)
		{
			return Types.GetType(name,"Assembly-CSharp");
		}
		/*
		public static MotinSerializableData InstanceSerializableDataFromName(string name)
		{
			//		Debug.Log ("CREATE INSTANCE " + name);
			MotinSerializableData instance = (MotinSerializableData)System.Activator.CreateInstance(Types.GetType(name,"Assembly-CSharp"));
			return instance;
		}

	*/
		public static MotinData InstanceDataFromName(string name)
		{
	//		Debug.Log ("CREATE INSTANCE " + name);
			MotinData instance = (MotinData)System.Activator.CreateInstance(Types.GetType(name,"Assembly-CSharp"));
			return instance;
		}
		public static object InstanceFromName(string name)
		{
			//		Debug.Log ("CREATE INSTANCE " + name);
			object instance =System.Activator.CreateInstance(Types.GetType(name,"Assembly-CSharp"));
			return instance;
		}
		

		public static void Save(string filePath,List<MotinData> datas)
		{
			MotinDataManager.Save(filePath,datas.ToArray());
		}
		public static void Save(string filePath,MotinData[] datas)
		{
			XmlDocument xmlDom = new XmlDocument();
			XmlDeclaration dec = xmlDom.CreateXmlDeclaration("1.0", null, null);
			xmlDom.AppendChild(dec);// Create the root element
			
			
			XmlElement elementRoot = xmlDom.CreateElement("DOCUMENT_ROOT");
			xmlDom.AppendChild(elementRoot);
			
			foreach(MotinData data in datas)
			{
				elementRoot.AppendChild(Serialize(xmlDom,data));
			}
			
			xmlDom.Save(resourcesPath + filePath);
			xmlDom= null;
			
		}

		/*
		public static T[] Load<T>(string filePath) where T : MotinSerializableData
		{
			MotinSerializableData[] loadedData = Load(filePath);
			if(loadedData==null)
				return null;
			
			T[] resultArray = new T[loadedData.Length];
			
		
			
			for(int i=0 ;i < loadedData.Length;i++)
			{
				resultArray[i] = (T)loadedData[i];
			}
			return resultArray;
		}
		public static MotinSerializableData[] Load(string filePath)
		{
			TextAsset textAsset = (TextAsset)Resources.Load(filePath.Substring(0,filePath.LastIndexOf(".")),typeof(TextAsset));
			if(textAsset==null)
			{
				Debug.Log("Asset File not Found " + filePath);
				return null;
			} 
			
			StringReader textReader = new StringReader(textAsset.text);
			XmlDocument xmlDom = new XmlDocument();
			xmlDom.Load(textReader);

			//Debug.Log("Type " + typeName());
			XmlElement rootElement = (XmlElement)xmlDom.GetElementsByTagName("DOCUMENT_ROOT")[0];
			if(rootElement==null)
			{
				Debug.LogError("MotinSerializableData root node null DOCUMENT_ROOT");	
			}

			List<MotinSerializableData> dataList = new List<MotinSerializableData>();
			MotinSerializableData loadedData = null;
			foreach(XmlElement childElement in  rootElement.ChildNodes)
		 	{
				loadedData = null;
				if(childElement.NodeType == XmlNodeType.Element)	
				{
					loadedData = MotinDataManager.Deserialize(childElement);
					dataList.Add(loadedData);
				}
			}
			loadedData = null;
			rootElement = null;
			xmlDom = null;
			textReader = null;
			textAsset = null;
			
			return dataList.ToArray();
		}

	*/

		public static string[] GetDataNames(string filePath)
		{
			MotinData[] datas = Load(filePath);
			string[] dataNames = null;
			if(datas==null)
			{
				dataNames = new string[1];
				dataNames[0] = "EMPTY";
				return dataNames;
			}

			dataNames = new string[datas.Length+1];

				for(int i = 0 ;i < datas.Length;i++)
				{
					dataNames[i] = datas[i].name;
				}
				dataNames[datas.Length] = "EMPTY";

				return dataNames;

		}

		public static T GetDataByName<T>(string name, string filePath) where T : MotinData
		{
			T[] datas = Load<T>(filePath);
			foreach(T data in datas)
			{
				if(data.name == name)
					return data;
			}

			return null;
		}

		public static T[] Load<T>(string filePath) where T : MotinData
		{
			MotinData[] loadedData = Load(filePath);
			if(loadedData==null)
				return null;
			
			T[] resultArray = new T[loadedData.Length];

			for(int i=0 ;i < loadedData.Length;i++)
			{
				resultArray[i] = (T)loadedData[i];
			}
			return resultArray;
		}
		public static MotinData[] Load(string filePath)
		{
	//		Debug.Log("LOAD FILE " + filePath);
			TextAsset textAsset = (TextAsset)Resources.Load(filePath.Substring(0,filePath.LastIndexOf(".")),typeof(TextAsset));
			if(textAsset==null)
			{
				Debug.Log("Asset File not Found " + filePath);
				return null;
			} 
			
			StringReader textReader = new StringReader(textAsset.text);
			XmlDocument xmlDom = new XmlDocument();
			xmlDom.Load(textReader);
			
			//Debug.Log("Type " + typeName());
			XmlElement rootElement = (XmlElement)xmlDom.GetElementsByTagName("DOCUMENT_ROOT")[0];
			if(rootElement==null)
			{
				Debug.LogError("MotinSerializableData root node null DOCUMENT_ROOT");	
			}
			
			List<MotinData> dataList = new List<MotinData>();
			MotinData loadedData = null;
			foreach(XmlElement childElement in  rootElement.ChildNodes)
			{
				loadedData = null;
				if(childElement.NodeType == XmlNodeType.Element)	
				{
					loadedData = MotinDataManager.Deserialize(childElement);
					dataList.Add(loadedData);
				}
			}
			loadedData = null;
			rootElement = null;
			xmlDom = null;
			textReader = null;
			textAsset = null;
			
			return dataList.ToArray();
		}


		#region SERIALIZATION METHODS
		public static MotinData Deserialize(XmlElement element)
		{
			//string className = element.Name;
			//Debug.Log("CREATE " + element.Name);
			MotinData instance = (MotinData)System.Activator.CreateInstance(Types.GetType(element.Name,"Assembly-CSharp"));
			
			DeserializeFields(element,instance);
			instance.OnFinishedDeserializing();
			return instance;
		}
		public static void DeserializeFields(XmlElement element,MotinData dataInstance)
		{
			System.Type type = dataInstance.GetType();      
			System.Reflection.FieldInfo[] fields = type.GetFields();
			System.Reflection.FieldInfo field  = null;
			
			for(int i =fields.Length-1;  i>=0 ;i--)
			{
				field = fields[i];
				// && field.GetCustomAttributes(typeof(HideInInspector),false).Length==0
				//			Debug.Log("Draw Field "  + field.Name + " " + field.FieldType.ToString());
				if(field.IsPublic && !field.IsStatic && field.GetCustomAttributes(typeof(System.NonSerializedAttribute),false).Length==0 )
				{
					DeserializeField(element,dataInstance,field);
				}         
			}
			
			
		}
		protected static void DeserializeField(XmlElement element,object value,System.Reflection.FieldInfo field)
		{
			//object newValue =  field.GetValue(value);
			//field.SetValue(
			
			string prefName = field.Name;
			
			if(field.FieldType == typeof(int))
			{
				field.SetValue(value,  GetInt(element,prefName));
			} 
			if(field.FieldType == typeof(bool))
			{
				field.SetValue(value,  GetBool(element,prefName));
			}   
			else if(field.FieldType == typeof(float))
			{
				field.SetValue(value,  GetFloat(element,prefName));
			}
			else if(field.FieldType == typeof(string))
			{
				field.SetValue(value,  GetString(element,prefName));
			}
			else if(field.FieldType == typeof(Vector2))
			{
				field.SetValue(value,  GetVector2(element,prefName));
			}
			else if(field.FieldType == typeof(Vector3))
			{
				field.SetValue(value,  GetVector3(element,prefName));
			}
			else if(field.FieldType == typeof(Vector4))
			{
				field.SetValue(value,  GetVector4(element,prefName));
			}
			else if(field.FieldType == typeof(Rect))
			{
				field.SetValue(value,  GetRect(element,prefName));
			}
			else if(field.FieldType == typeof(Color))
			{
				field.SetValue(value,  GetColor(element,prefName));
			}
			else if(!field.FieldType.IsArray && MotinUtils.IsTypeDerivedFrom(field.FieldType,typeof(MotinData)))
			{
				//GetMotinSerializableData(element,field.Name,(MotinSerializableData)newValue);
				field.SetValue(value,GetMotinData(element,field.Name));
			}	
			else if(field.FieldType.IsEnum )
			{
				string enumVal = GetString(element,prefName);
				if(enumVal!=null && enumVal.Length>0)
				{
					field.SetValue(value,Enum.Parse(field.FieldType,enumVal));
				}
				//newValue =  EditorGUILayout.EnumPopup(field.Name,(System.Enum)System.Enum.ToObject(field.FieldType,oldValue));
				//Debug.Log ("IS ENUM! " + field.Name);
			}
			else if(field.FieldType.IsArray )
			{
				//Debug.Log ("IS ARRAY! " + field.Name);
				if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(MotinData)))
				{
					deserializeMotinDataArrayField(element,value,field);
				}
				else if( field.FieldType.GetElementType().IsEnum)
				{
					deserializeEnumArrayField(element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(int)))
				{
					deserializeIntArrayField (element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(string)))
				{
					deserializeStringArrayField (element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(float)))
				{
					deserializeFloatArrayField (element,value,field);
				}
				
				/*
					Array a = (Array)field.GetValue(value);
					for(int i = 0; i< a.Length; i++)
					{
					  object o = a.GetValue(i);
					}
					*/
			}
			else if(field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				//Debug.Log ("IS A LIST! " + field.Name);
				Type itemType =   field.FieldType.GetGenericArguments()[0];
				if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(MotinData)))
				{
					deserializeMotinDataListField(element,value,field);
				}
				else if( itemType.IsEnum)
				{
					deserializeEnumListField(element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(int)))
				{
					deserializeIntListField (element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(string)))
				{
					deserializeStringListField (element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(float)))
				{
					deserializeFloatListField (element,value,field);
				}
				/*
				
					Debug.Log ("IS A LIST! " + field.Name);
					Type itemType =   field.FieldType.GetGenericArguments()[0];
					if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(MotinSerializableData)))
					{
						deserializeSerializableDataListField(element,value,field);
					}
					MotinSerializableData data = null;
					*/
				/*
			foreach(XmlElement childElement in  element.ChildNodes)
		 	{
				data = null;
				if(childElement.NodeType == XmlNodeType.Element)	
				{
					//Debug.Log("CHILD ELEMENT " + childElement.Name);
					data =  MotinDataManager.Deserialize(childElement);
					//AddChild(data);
					OnDeserializeChildElement(data);
				}
			}*/
				
			}
			else if(field.FieldType.IsClass)
			{
				//newValue=oldValue;
			}
			
		}
		public static XmlElement Serialize(XmlDocument xmlDom, MotinData dataInstance)
		{
			if(xmlDom==null)
				throw new Exception("Serialize: XmlDocument is Null");


			dataInstance.OnWillSerialize();

			//		Debug.Log("SERIALIZE ELEMENT " + name + " "+ typeName());
			XmlElement rootElement_ = xmlDom.CreateElement(dataInstance.GetType().FullName);
			//SerializeFields(rootElement_);
			SerializeFields(rootElement_,dataInstance);
			return rootElement_;
		}
		public static void SerializeFields(XmlElement element,MotinData dataInstance)
		{
			// Debug.Log("MOTINSCRIPT EDITOR DRAWFIELDS "  + name);
			
			System.Type type = dataInstance.GetType();      
			System.Reflection.FieldInfo[] fields = type.GetFields();
			System.Reflection.FieldInfo field  = null;
			
			for(int i =fields.Length-1;  i>=0 ;i--)
			{
				field = fields[i];
				if(field.IsPublic && !field.IsStatic && field.GetCustomAttributes(typeof(System.NonSerializedAttribute),false).Length==0 )
				{
					SerializeField(element,dataInstance,field);
				}         
			}
			
		}
		protected static void SerializeField(XmlElement element,object value,System.Reflection.FieldInfo field)
		{
			object newValue =  field.GetValue(value);
			string fieldName = field.Name;
			//Debug.Log("Serialize field "+ fieldName + " " + newValue.ToString());
			if(field.FieldType == typeof(int))
			{
				SetInt(element,fieldName,(int)newValue);
			} 
			if(field.FieldType == typeof(bool))
			{
				SetBool(element,fieldName,(bool)newValue);
			}   
			else if(field.FieldType == typeof(float))
			{
				SetFloat(element,fieldName,(float)newValue);
			}
			else if(field.FieldType == typeof(string))
			{
				SetString(element,fieldName,(string)newValue);
			}
			else if(field.FieldType == typeof(Vector2))
			{
				SetVector2(element,fieldName,(Vector2)newValue);
			}
			else if(field.FieldType == typeof(Vector3))
			{
				SetVector3(element,fieldName,(Vector3)newValue);
			}
			else if(field.FieldType == typeof(Vector4))
			{
				SetVector4(element,fieldName,(Vector4)newValue);
			}
			else if(field.FieldType == typeof(Rect))
			{
				SetRect(element,fieldName,(Rect)newValue);
			}
			else if(field.FieldType == typeof(Color))
			{
				SetColor(element,fieldName,(Color)newValue);
			}
			else if(field.FieldType.IsEnum )
			{
				SetString(element,fieldName,newValue.ToString());
				//				Debug.Log ("IS ENUM! " + field.Name);
			}
			else if(!field.FieldType.IsArray && MotinUtils.IsTypeDerivedFrom(field.FieldType,typeof(MotinData)))
			{
				serializeMotinData(element,field.Name,(MotinData)newValue);
			}
			else if(field.FieldType.IsArray )
			{
				//				Debug.Log ("IS ARRAY! " + field.Name);
				if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(MotinData)))
				{
					serializeMotinDataArrayField(element,value,field);
				}
				else if( field.FieldType.GetElementType().IsEnum)
				{
					
					serializeEnumArrayField(element,value,field);
					
				}
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(int)))
				{
					serializeIntArrayField(element,value,field);
				}
				
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(string))  )
				{
					serializeStringArrayField(element,value,field);
				}
				
				else if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(float)))
				{
					serializeFloatArrayField(element,value,field);
				}
			}
			else if(field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				//				Debug.Log ("IS A LIST! " + field.Name);
				Type itemType =   field.FieldType.GetGenericArguments()[0];
				if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(MotinData)))
				{
					serializeMotinDataList(element,value,field);
				}
				else if( itemType.IsEnum)
				{
					//SetString(element,fieldName,newValue.ToString());
					serializeEnumListField(element,value,field);
					
					
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(int)))
				{
					serializeIntListField(element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(string)))
				{
					serializeStringListField(element,value,field);
				}
				else if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(float)))
				{
					serializeFloatListField(element,value,field);
				}
				
			}
			else if(field.FieldType.IsClass)
			{
				//newValue=oldValue;
			}
		}


		public static void SetString(XmlElement element, string name, string value)
		{
			element.SetAttribute(name,value);
		}
		public static string GetString(XmlElement element,string name)
		{
			return element.GetAttribute(name);
		}
		public static void SetFloat(XmlElement element, string name, float value)
		{
			SetString(element,name,value.ToString());
		}
		public static float GetFloat( XmlElement element,string name)
		{
			string result = GetString(element,name);
			if(result.Length==0)
				return 0;
			
			return float.Parse( result);
		}
		public static void SetInt(XmlElement element, string name, int value)
		{
			
			SetString(element,name,value.ToString());
		}
		public static int GetInt(XmlElement element, string name)
		{
			string result = GetString(element,name);
			if(result.Length==0)
				return 0;
			
			
			return int.Parse( result);
		}
		public static void SetBool( XmlElement element,string name, bool value)
		{
			SetString(element,name,value.ToString());
		}
		public static bool GetBool(XmlElement element, string name)
		{
			string result = GetString(element,name);
			if(result.Length==0)
				return false;
			
			
			return bool.Parse(result);
		}
		public static void SetLong( XmlElement element,string name, long value)
		{
			SetString(element,name,value.ToString());
		}
		public static long GetLong(XmlElement element, string name)
		{
			string result = GetString(element,name);
			if(result.Length==0)
				return 0;
			
			
			return long.Parse( result);
		}
		public static void SetVector3(XmlElement element,string name, Vector3 value)
		{
			SetFloat(element,"Vector3x_" + name,value.x);
			SetFloat(element,"Vector3y_" + name,value.y);
			SetFloat(element,"Vector3z_" + name,value.z);
		}
		public static Vector3 GetVector3(XmlElement element, string name)
		{
			
			Vector3 aux = Vector3.zero;
			aux.x= GetFloat(element,"Vector3x_" + name);
			aux.y= GetFloat(element,"Vector3y_" + name);
			aux.z= GetFloat(element,"Vector3z_" + name);
			return aux	;
		}
		
		public static void SetVector2(XmlElement element, string name, Vector2 value)
		{
			SetFloat(element,"Vector2x_" + name,value.x);
			SetFloat(element,"Vector2y_" + name,value.y);
		}
		public static Vector2 GetVector2(XmlElement element, string name)
		{
			Vector2 aux = Vector2.zero;
			aux.x= GetFloat(element,"Vector2x_" + name);
			aux.y= GetFloat(element,"Vector2y_" + name);
			return aux	;
		}
		
		public static void SetVector4(XmlElement element,string name, Vector4 value)
		{
			SetFloat(element,"Vector4x_" + name,value.x);
			SetFloat(element,"Vector4y_" + name,value.y);
			SetFloat(element,"Vector4z_" + name,value.z);
			SetFloat(element,"Vector4w_" + name,value.w);
		}
		public static Vector4 GetVector4(XmlElement element,string name)
		{
			
			Vector4 aux = Vector4.zero;
			aux.x= GetFloat(element,"Vector4x_" + name);
			aux.y= GetFloat(element,"Vector4y_" + name);
			aux.z= GetFloat(element,"Vector4z_" + name);
			aux.w= GetFloat(element,"Vector4w_" + name);
			return aux;
		}
		
		public static void SetQuaternion(XmlElement element, string name, Quaternion value)
		{
			
			SetFloat(element,"QuaternionW_" + name,value.w);
			SetFloat(element,"QuaternionX_" + name,value.x);
			SetFloat(element,"QuaternionY_" + name,value.y);
			SetFloat(element,"QuaternionZ_" + name,value.z);
		}
		public static Quaternion GetQuaternion( XmlElement element,string name)
		{
			Quaternion aux = Quaternion.identity;
			aux.w= GetFloat(element,"QuaternionW_" + name);
			aux.x= GetFloat(element,"QuaternionX_" + name);
			aux.y= GetFloat(element,"QuaternionY_" + name);
			aux.z= GetFloat(element,"QuaternionZ_" + name);
			return aux	;
		}
		public static void SetColor(XmlElement element,string name, Color value)
		{
			SetFloat(element,"Color_r_" + name,value.r);
			SetFloat(element,"Color_g_" + name,value.g);
			SetFloat(element,"Color_b_" + name,value.b);
			SetFloat(element,"Color_a_" + name,value.a);
		}
		public static Color GetColor(XmlElement element,string name)
		{
			Color aux = Color.white;
			aux.r= GetFloat(element,"Color_r_" + name);
			aux.g= GetFloat(element,"Color_g_" + name);
			aux.b= GetFloat(element,"Color_b_" + name);
			aux.a= GetFloat(element,"Color_a_" + name);
			return aux	;
		}
		
		public static void SetRect(XmlElement element,string name, Rect value)
		{
			SetFloat(element,"Rect_x_" + name,value.x);
			SetFloat(element,"Rect_y_" + name,value.y);
			SetFloat(element,"Rect_width_" + name,value.width);
			SetFloat(element,"Rect_height_" + name,value.height);
		}
		public static Rect GetRect(XmlElement element,string name)
		{
			Rect aux = new Rect(0,0,1,1);
			aux.x= GetFloat(element,"Rect_x_" + name);
			aux.y= GetFloat(element,"Rect_y_" + name);
			aux.width= GetFloat(element,"Rect_width_" + name);
			aux.height= GetFloat(element,"Rect_height_" + name);
			return aux	;
		}
		
		public static void SetObject(XmlElement element,string name, UnityEngine.Object value)
		{
			
		}
		public static UnityEngine.Object GetObject(XmlElement element,string name)
		{
			return null;
		}

		public static void SetDateTime(XmlElement element,string name,System.DateTime dateTime)
		{
			SetLong(element,name,dateTime.Ticks);
		}
		public static System.DateTime GetDateTime(XmlElement element, string name)
		{
			return (new System.DateTime( GetLong(element,name)));
		}
		
		public static MotinData GetMotinData(XmlElement element,string name)
		{
			XmlElement targetElement =  GetChildElement(element,name);
			if(targetElement==null)
				return null;
			
			//MotinData data = 	MotinDataManager.InstanceDataFromName(targetElement.Name);
			//Deserialize(targetElement);
			return Deserialize(targetElement);;
		}
		public static void serializeMotinData(XmlElement element,string name, MotinData value)
		{
			if(value==null)
			{
				Debug.Log ("SERIALIZE DATA NULL " + name);
				return;
			}
			
			value.name = name;
			element.AppendChild( Serialize(element.OwnerDocument,value));
		}
		
		public static void  serializeIntArray(XmlElement element,string name, int[] value)
		{
			if(value!=null){
				SetInt(element,name + "_count",value.Length);
				for(int i = 0 ; i < value.Length; i ++)
				{
					SetInt(element,name + "_item_" +i,value[i]);
				}
			}
		}
		public static void  serializeStringArray(XmlElement element,string name, string[] value)
		{
			if(value!=null){
				SetInt(element,name + "_count",value.Length);
				for(int i = 0 ; i < value.Length; i ++)
				{
					SetString(element,name + "_item_" +i,value[i]);
				}
			}
		}
		
		public static void  serializeFloatArray(XmlElement element,string name, float[] value)
		{
			if(value!=null){
				SetInt(element,name + "_count",value.Length);
				for(int i = 0 ; i < value.Length; i ++)
				{
					SetFloat(element,name + "_item_" +i,value[i]);
				}
			}
		}
		public static int[]  GetIntArray(XmlElement element,string name)
		{	
			//name = GetString(element,"name");
			int[] int_items = null;

			int arrayCount = GetInt(element,name + "_count");
			if(arrayCount>0)
			{
				int_items = new int[arrayCount];
				for(int i = 0 ; i < arrayCount; i ++)
				{
					int_items[i] = GetInt(element,name + "_item_"+i );
				}
			}
			return int_items;

		}
		public static float[]  GetFloatArray(XmlElement element,string name)
		{	
			float[] items = null;
			
			int arrayCount = GetInt(element,name + "_count");
			if(arrayCount>0)
			{
				items = new float[arrayCount];
				for(int i = 0 ; i < arrayCount; i ++)
				{
					items[i] = GetFloat(element,name + "_item_"+i );
				}
			}
			return items;
			
		}
		
		public static string[]  GetStringArray(XmlElement element,string name)
		{	
			string[] items = null;
			
			int arrayCount = GetInt(element,name + "_count");
			if(arrayCount>0)
			{
				items = new string[arrayCount];
				for(int i = 0 ; i < arrayCount; i ++)
				{
					items[i] = GetString(element,name + "_item_"+i );
				}
			}
			return items;
			
		}
		

		
		public static void  serializeFloatArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{				
			Array reflectArray = (Array)field.GetValue(target);					
			float[] value = new float[reflectArray.Length];	
			for(int i = 0; i< reflectArray.Length; i++)
			{
				value[i] =(float) reflectArray.GetValue(i);
			}
			
			serializeFloatArray(element,field.Name,value);
		}
		public static void  serializeEnumArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{				
			Array reflectArray = (Array)field.GetValue(target);					
			string[] value = new string[reflectArray.Length];	
			for(int i = 0; i< reflectArray.Length; i++)
			{
				value[i] =reflectArray.GetValue(i).ToString();
			}
			
			serializeStringArray(element,field.Name,value);
		}
		public static void  serializeStringArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{				
			Array reflectArray = (Array)field.GetValue(target);	
			string[] value = null;
			if(reflectArray!=null)
			{
				value = new string[reflectArray.Length];	
				for(int i = 0; i< reflectArray.Length; i++)
				{
					value[i] =(string) reflectArray.GetValue(i);
				}
			}
			
			serializeStringArray(element,field.Name,value);
		}
		
		public static void  serializeIntArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{				
			Array reflectArray = (Array)field.GetValue(target);		
			if(reflectArray==null)
				return;
			
			int[] value = new int[reflectArray.Length];	
			for(int i = 0; i< reflectArray.Length; i++)
			{
				value[i] =(int) reflectArray.GetValue(i);
			}
			
			serializeIntArray(element,field.Name,value);
		}
		

		public static void  serializeIntListField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{	
			IList list =(IList)field.GetValue(target);
			
			int[] value = new int[list.Count];	
			for(int i = 0; i< list.Count; i++)
			{
				value[i] =(int) list[i];
			}
			serializeIntArray(element,field.Name,value);
		}			
		public static void  serializeStringListField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{	
			IList list =(IList)field.GetValue(target);
			
			string[] value = new string[list.Count];	
			for(int i = 0; i< list.Count; i++)
			{
				value[i] =(string) list[i];
			}
			serializeStringArray(element,field.Name,value);
		}
		public static void  serializeEnumListField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{	
			IList list =(IList)field.GetValue(target);
			string[] value = new string[list.Count];	
			for(int i = 0; i< list.Count; i++)
			{
				value[i] =list[i].ToString();
			}
			serializeStringArray(element,field.Name,value);
		}
		public static void  serializeFloatListField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{	
			IList list =(IList)field.GetValue(target);
			
			float[] value = new float[list.Count];	
			for(int i = 0; i< list.Count; i++)
			{
				value[i] =(float) list[i];
			}
			serializeFloatArray(element,field.Name,value);
		}	

		public static void  serializeMotinDataArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
		{				
			Array reflectArray = (Array)field.GetValue(target);		
			if(reflectArray==null)
				return ;

			XmlElement arrayRootElement_ =element.OwnerDocument.CreateElement("MotinDataArray");
			SetString(arrayRootElement_,"name",field.Name);
			SetInt(arrayRootElement_,"count", reflectArray.Length);

			MotinData value = null;	
			for(int i = 0; i< reflectArray.Length; i++)
			{
				value =(MotinData) reflectArray.GetValue(i);
				serializeMotinData(arrayRootElement_,value.name,value);
			}
			element.AppendChild(arrayRootElement_);
			//serializeSerializableDataArray(element,field.Name,value);
		}
		public static void  serializeMotinDataList(XmlElement element,object target, System.Reflection.FieldInfo field)
		{	
			IList list =(IList)field.GetValue(target);

			XmlElement arrayRootElement_ = element.OwnerDocument.CreateElement("MotinDataArray");
			SetString(arrayRootElement_,"name",field.Name);
			SetInt(arrayRootElement_,"count", list.Count);

			MotinData value = null;	
			for(int i = 0; i< list.Count; i++)
			{
				value =(MotinData) list[i];
				serializeMotinData(arrayRootElement_,value.name,value);
			}

			element.AppendChild(arrayRootElement_);
		}
		/*
		public static void  serializeSerializableDataArray(XmlElement element,string name, MotinData[] array)
		{			
			
			MotinSerializableArrayData	arrayData = new MotinSerializableArrayData();
			arrayData.childDatas = new List<MotinSerializableData>(array);
			serializeMotinSerializableData(element,name,arrayData);
		}
		*/
		public static void  deserializeMotinDataListField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			MotinData[] deserializedArray = deserializeMotinDataArray(element,field.Name);
			if(deserializedArray==null)
				return;
			
			Type itemType =   field.FieldType.GetGenericArguments()[0];
			
			Type listType = typeof(List<>).MakeGenericType(itemType);
			IList list = (IList)Activator.CreateInstance(listType);
			
			for(int i = 0; i< deserializedArray.Length; i++)
			{
				list.Add(deserializedArray[i]);
			}
			
			field.SetValue(target,list);
		}
		public static void  deserializeIntListField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			int[] deserializedArray = GetIntArray(element,field.Name);
			
			Type itemType =   field.FieldType.GetGenericArguments()[0];
			Type listType = typeof(List<>).MakeGenericType(itemType);
			IList list = (IList)Activator.CreateInstance(listType);
			
			if(deserializedArray!=null)
			{
				for(int i = 0; i< deserializedArray.Length; i++)
				{
					list.Add(deserializedArray[i]);
				}
			}
			
			field.SetValue(target,list);
		}
		public static void  deserializeStringListField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			string[] deserializedArray = GetStringArray(element,field.Name);
			
			Type itemType =   field.FieldType.GetGenericArguments()[0];
			Type listType = typeof(List<>).MakeGenericType(itemType);
			IList list = (IList)Activator.CreateInstance(listType);
			
			if(deserializedArray!=null)
			{
				for(int i = 0; i< deserializedArray.Length; i++)
				{
					list.Add(deserializedArray[i]);
				}
			}
			
			field.SetValue(target,list);
		}
		public static void  deserializeEnumListField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			string[] deserializedArray = GetStringArray(element,field.Name);
			
			Type itemType =   field.FieldType.GetGenericArguments()[0];
			Type listType = typeof(List<>).MakeGenericType(itemType);
			IList list = (IList)Activator.CreateInstance(listType);
			
			if(deserializedArray!=null)
			{
				for(int i = 0; i< deserializedArray.Length; i++)
				{
					list.Add(System.Enum.Parse(itemType,deserializedArray[i]) );
				}
			}
			
			field.SetValue(target,list);
		}
		public static void  deserializeFloatListField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			float[] deserializedArray = GetFloatArray(element,field.Name);
			
			Type itemType =   field.FieldType.GetGenericArguments()[0];
			Type listType = typeof(List<>).MakeGenericType(itemType);
			IList list = (IList)Activator.CreateInstance(listType);
			
			if(deserializedArray!=null)
			{
				for(int i = 0; i< deserializedArray.Length; i++)
				{
					list.Add(deserializedArray[i]);
				}
			}
			
			field.SetValue(target,list);
		}
		public static void  deserializeIntArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			
			int[] deserializedArray = GetIntArray(element,field.Name);
			
			field.SetValue(target,deserializedArray);
		}
		public static void  deserializeFloatArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			
			float[] deserializedArray = GetFloatArray(element,field.Name);
			
			field.SetValue(target,deserializedArray);
		}
		public static void  deserializeEnumArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			
			string[] deserializedArray = GetStringArray(element,field.Name);
			
			Type itemType =  field.GetType().GetElementType();
			//Type listType = typeof(List<>).MakeGenericType(itemType);
			Array array = null;
			if(deserializedArray!=null)
			{
				array = (Array)Activator.CreateInstance(itemType,deserializedArray.Length);
				for(int i = 0; i< deserializedArray.Length; i++)
				{
					array.SetValue(System.Enum.Parse(itemType, deserializedArray[i]),i);
				}
			}
			
			field.SetValue(target,array);
		}
		public static void  deserializeStringArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			
			string[] deserializedArray = GetStringArray(element,field.Name);
			
			field.SetValue(target,deserializedArray);
		}
		public static void  deserializeMotinDataArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
		{
			MotinData[] deserializedArray = deserializeMotinDataArray(element,field.Name);
			if(deserializedArray==null)
				return;
			
			Array a =  Array.CreateInstance(field.FieldType.GetElementType(),deserializedArray.Length);
			for(int i = 0; i< deserializedArray.Length; i++)
			{
				a.SetValue(deserializedArray[i],i);
			}
			field.SetValue(target,a);
		}
		public static MotinData[]  deserializeMotinDataArray(XmlElement element,string name)
		{
			//MotinSerializableArrayData	arrayData = new MotinSerializableArrayData();
			XmlElement arrayElement = GetChildElement(element,name);

			if(arrayElement==null)
				return null;

			List<MotinData> motinDataList = new List<MotinData>();
			MotinData data = null;
			foreach(XmlElement childElement in arrayElement.ChildNodes)
			{
				motinDataList.Add( Deserialize(childElement));
			}
			return motinDataList.ToArray();

		}


		protected static XmlElement GetChildElement(XmlElement parentElement,string name)
		{
			//string debug = "GET NAME " + name + " parent " + parentElement.Name + " " + GetString(parentElement,"name") +"\n";
			string auxName;
			foreach(XmlElement childElement in  parentElement.ChildNodes)
			{
				//data = null;
				if(childElement.NodeType == XmlNodeType.Element)	
				{
					
					auxName = GetString(childElement,"name");
					//			debug += "Child   " + auxName + " element " + childElement.Name + "\n";
					
					if(auxName == name)
					{
						
						return childElement;
					}
				}
			}
			//	debug+= "CHILD NOT FOUND " + name + " parent " + parentElement.Name;
			//	Debug.Log (debug);
			return null;
		}
		#endregion
		
	}
}
