using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
/*


public class MotinEditorTk2dSpriteField: Attribute {
	
	public MotinEditorTk2dSpriteField() {
		
	}
}

public class MotinEditorSoundEnumField: Attribute {
	
	public MotinEditorSoundEnumField() {
		
	}
}

public class MotinEditorLocalizationEnumField: Attribute {
	
	public MotinEditorLocalizationEnumField() {
		
	}
}
public class MotinEditorReadonlyField: Attribute {

	public MotinEditorReadonlyField() {
	
	}
}

*/


public interface IMotinSerializable 
{
    string typeName();
	//string GetElementName();
	//void Serialize(string filePath);
	
	XmlElement Serialize(XmlDocument xmlDom);
	//void Deserialize(string filePath);

	void Deserialize(XmlElement element);
	
	//void DeserializeFromString(string text);

	
	void OnSerialize(XmlElement element);
	
	void OnDeserialize(XmlElement element);
	
	//void OnDeserializeChildElement(MotinSerializableData data);
	
	
	void SetString(XmlElement element,string name, string value);

	string GetString(XmlElement element,string name);
	
	void SetFloat(XmlElement element,string name, float value);
	
	float GetFloat(XmlElement element,string name);
	
	void SetInt(XmlElement element,string name, int value);
	
	int GetInt(XmlElement element,string name);
	
	void SetBool(XmlElement element,string name, bool value);
	
	bool GetBool(XmlElement element,string name);
	
	void SetLong(XmlElement element,string name, long value);
	
	long GetLong(XmlElement element,string name);
	
	void SetVector3(XmlElement element,string name, Vector3 value);
	Vector3 GetVector3(XmlElement element,string name);
	
	
	void SetVector2(XmlElement element,string name, Vector2 value);
	Vector2 GetVector2(XmlElement element,string name);
	
	void SetVector4(XmlElement element,string name, Vector4 value);
	Vector4 GetVector4(XmlElement element,string name);
	
	void SetColor(XmlElement element,string name, Color value);
	Color GetColor(XmlElement element,string name);
	
	void SetRect(XmlElement element,string name, Rect value);
	Rect GetRect(XmlElement element,string name);
	
	void SetObject(XmlElement element,string name, UnityEngine.Object value);
	UnityEngine.Object GetObject(XmlElement element,string name);
	
	void SetDateTime(XmlElement element,string name,DateTime dateTime);
	
	
	void  serializeIntArray(XmlElement element,string name, int[] value);
	void  serializeStringArray(XmlElement element,string name, string[] value);
	void  serializeFloatArray(XmlElement element,string name, float[] value);
	
	DateTime GetDateTime(XmlElement element,string name);
	/*
	void 	SetList<T>(XmlElement element,string name,List<T> value)  where T : IMotinSerializable;
	List<T> GetList<T>(XmlElement element,string name)  where T : IMotinSerializable;
	*/
}
 

/*
public interface IMotinDataDelegate 
{
	void OnUpdateMotinData(MotinSerializableData motinData);
	void OnMotinDataDeserialized(XmlElement element);
}
 */

/*
public class MotinSerializableData : IMotinSerializable  {
	//protected IMotinDataLibrary	dataLibrary = null;
	//IMotinDataDelegate motinDataDelegate_ = null;
	
	public static  string xmlName = "MotinSerializableData";
	public virtual string typeName()
	{
		return MotinSerializableData.xmlName;
	}
	
	//protected string m_elementName = "MotinXmlData";
	
	//protected XmlDocument	xmlDom = null;
	//protected XmlElement rootElement_ = null; 
	
	public string name = "MotinSerializableData";
	
	//List<MotinSerializableData> childs_ = null;

	public MotinSerializableData()
	{
		//dataLibrary = lib;
	}
	
	public MotinSerializableData(System.Xml.XmlElement element)
	{
		//dataLibrary = lib;
		Deserialize(element);
	}

	


	public virtual void setTypeName(string name)
	{
		MotinSerializableData.xmlName = name;
	}
	

	public XmlElement Serialize(XmlDocument xmlDom)
	{
		if(xmlDom==null)
			throw new Exception("Serialize: XmlDocument is Null");
		
//		Debug.Log("SERIALIZE ELEMENT " + name + " "+ typeName());
		XmlElement rootElement_ = xmlDom.CreateElement(typeName());
		//SerializeFields(rootElement_);
		OnSerialize(rootElement_);
		return rootElement_;
	}
	

	public void Deserialize(XmlElement element)
	{
//		Debug.Log("ELEMENT " + element.Name);
		OnDeserialize(element);
		
	}

	
	public virtual void OnSerialize(XmlElement element)
	{
		
		SerializeFields(element);
		//SetString(element,"name",name);
	}
	
	public virtual void OnDeserialize(XmlElement element)
	{
		//name= GetString(element,"name");
		DeserializeFields(element);
	}

	public void SerializeFields(XmlElement element)
	{
	// Debug.Log("MOTINSCRIPT EDITOR DRAWFIELDS "  + name);
	
	   System.Type type = this.GetType();      
	   System.Reflection.FieldInfo[] fields = type.GetFields();
	   System.Reflection.FieldInfo field  = null;
		
	   for(int i =fields.Length-1;  i>=0 ;i--)
	   {
		  	field = fields[i];
	    	if(field.IsPublic && !field.IsStatic )
			{
	        	SerializeField(element,this,field);
			}         
		}
		 
	}

	public void CopyFromData(MotinSerializableData sourceData)
	{
		if(sourceData == null)
			return;

		System.Type type = this.GetType();      
		System.Reflection.FieldInfo[] fields = type.GetFields();
		System.Reflection.FieldInfo field  = null;

		System.Type sourceType = sourceData.GetType();      
		System.Reflection.FieldInfo sourceField = null; 


		for(int i =fields.Length-1;  i>=0 ;i--)
		{
			field = fields[i];
			if(field.IsPublic && !field.IsStatic )
			{
				sourceField = sourceType.GetField(field.Name);
				if(sourceField!=null)
				{
					field.SetValue(this,sourceField.GetValue(sourceData));
				}

			}         
		}
	}

	protected void SerializeField(XmlElement element,object value,System.Reflection.FieldInfo field)
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
			else if(!field.FieldType.IsArray && MotinUtils.IsTypeDerivedFrom(field.FieldType,typeof(MotinSerializableData)))
			{
				serializeMotinSerializableData(element,field.Name,(MotinSerializableData)newValue);
			}
			else if(field.FieldType.IsArray )
			{
//				Debug.Log ("IS ARRAY! " + field.Name);
				if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(MotinSerializableData)))
				{
					serializeSerializableDataArrayField(element,value,field);
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
				if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(MotinSerializableData)))
				{
					serializeSerializableDataList(element,value,field);
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

	public void DeserializeFields(XmlElement element)
	{
		System.Type type = this.GetType();      
	   	System.Reflection.FieldInfo[] fields = type.GetFields();
	   	System.Reflection.FieldInfo field  = null;
		
	   	for(int i =fields.Length-1;  i>=0 ;i--)
	   	{
			field = fields[i];
		// && field.GetCustomAttributes(typeof(HideInInspector),false).Length==0
//			Debug.Log("Draw Field "  + field.Name + " " + field.FieldType.ToString());
	    	if(field.IsPublic && !field.IsStatic )
			{
	        	DeserializeField(element,this,field);
			}         
		}
		
	
	}
	protected void DeserializeField(XmlElement element,object value,System.Reflection.FieldInfo field)
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
			else if(!field.FieldType.IsArray && MotinUtils.IsTypeDerivedFrom(field.FieldType,typeof(MotinSerializableData)))
			{
				//GetMotinSerializableData(element,field.Name,(MotinSerializableData)newValue);
				field.SetValue(value,GetMotinSerializableData(element,field.Name));
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
				if(MotinUtils.IsTypeDerivedFrom(field.FieldType.GetElementType(),typeof(MotinSerializableData)))
				{
					deserializeSerializableDataArrayField(element,value,field);
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

			}
			else if(field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				//Debug.Log ("IS A LIST! " + field.Name);
				Type itemType =   field.FieldType.GetGenericArguments()[0];
				if(MotinUtils.IsTypeDerivedFrom(itemType,typeof(MotinSerializableData)))
				{
					deserializeSerializableDataListField(element,value,field);
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

				
			}
			else if(field.FieldType.IsClass)
			{
				//newValue=oldValue;
			}
		
	}
	
	#region HELPERS
	
	protected XmlElement GetChildElement(XmlElement parentElement,string name)
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
	
	public void SetString(XmlElement element, string name, string value)
	{
		element.SetAttribute(name,value);
	}
	public string GetString(XmlElement element,string name)
	{
		return element.GetAttribute(name);
	}
	public void SetFloat(XmlElement element, string name, float value)
	{
		SetString(element,name,value.ToString());
	}
	public float GetFloat( XmlElement element,string name)
	{
		string result = GetString(element,name);
		if(result.Length==0)
			return 0;
		
		return float.Parse( result);
	}
	public void SetInt(XmlElement element, string name, int value)
	{
		
		SetString(element,name,value.ToString());
	}
	public int GetInt(XmlElement element, string name)
	{
		string result = GetString(element,name);
		if(result.Length==0)
			return 0;
		
		
		return int.Parse( result);
	}
	public void SetBool( XmlElement element,string name, bool value)
	{
		SetString(element,name,value.ToString());
	}
	public bool GetBool(XmlElement element, string name)
	{
		string result = GetString(element,name);
		if(result.Length==0)
			return false;
		
	
		return bool.Parse(result);
	}
	public void SetLong( XmlElement element,string name, long value)
	{
		SetString(element,name,value.ToString());
	}
	public long GetLong(XmlElement element, string name)
	{
		string result = GetString(element,name);
		if(result.Length==0)
			return 0;
		
		
		return long.Parse( result);
	}
	public void SetVector3(XmlElement element,string name, Vector3 value)
	{
		SetFloat(element,"Vector3x_" + name,value.x);
		SetFloat(element,"Vector3y_" + name,value.y);
		SetFloat(element,"Vector3z_" + name,value.z);
	}
	public Vector3 GetVector3(XmlElement element, string name)
	{

		Vector3 aux = Vector3.zero;
		aux.x= GetFloat(element,"Vector3x_" + name);
		aux.y= GetFloat(element,"Vector3y_" + name);
		aux.z= GetFloat(element,"Vector3z_" + name);
		return aux	;
	}
	
	public void SetVector2(XmlElement element, string name, Vector2 value)
	{
		SetFloat(element,"Vector2x_" + name,value.x);
		SetFloat(element,"Vector2y_" + name,value.y);
	}
	public Vector2 GetVector2(XmlElement element, string name)
	{
		Vector2 aux = Vector2.zero;
		aux.x= GetFloat(element,"Vector2x_" + name);
		aux.y= GetFloat(element,"Vector2y_" + name);
		return aux	;
	}
	
	public void SetVector4(XmlElement element,string name, Vector4 value)
	{
		SetFloat(element,"Vector4x_" + name,value.x);
		SetFloat(element,"Vector4y_" + name,value.y);
		SetFloat(element,"Vector4z_" + name,value.z);
		SetFloat(element,"Vector4w_" + name,value.w);
	}
	public Vector4 GetVector4(XmlElement element,string name)
	{
		
		Vector4 aux = Vector4.zero;
		aux.x= GetFloat(element,"Vector4x_" + name);
		aux.y= GetFloat(element,"Vector4y_" + name);
		aux.z= GetFloat(element,"Vector4z_" + name);
		aux.w= GetFloat(element,"Vector4w_" + name);
		return aux;
	}
	
	public void SetQuaternion(XmlElement element, string name, Quaternion value)
	{
	
		SetFloat(element,"QuaternionW_" + name,value.w);
		SetFloat(element,"QuaternionX_" + name,value.x);
		SetFloat(element,"QuaternionY_" + name,value.y);
		SetFloat(element,"QuaternionZ_" + name,value.z);
	}
	public Quaternion GetQuaternion( XmlElement element,string name)
	{
		Quaternion aux = Quaternion.identity;
		aux.w= GetFloat(element,"QuaternionW_" + name);
		aux.x= GetFloat(element,"QuaternionX_" + name);
		aux.y= GetFloat(element,"QuaternionY_" + name);
		aux.z= GetFloat(element,"QuaternionZ_" + name);
		return aux	;
	}
	public void SetColor(XmlElement element,string name, Color value)
	{
		SetFloat(element,"Color_r_" + name,value.r);
		SetFloat(element,"Color_g_" + name,value.g);
		SetFloat(element,"Color_b_" + name,value.b);
		SetFloat(element,"Color_a_" + name,value.a);
	}
	public Color GetColor(XmlElement element,string name)
	{
		Color aux = Color.white;
		aux.r= GetFloat(element,"Color_r_" + name);
		aux.g= GetFloat(element,"Color_g_" + name);
		aux.b= GetFloat(element,"Color_b_" + name);
		aux.a= GetFloat(element,"Color_a_" + name);
		return aux	;
	}
	
	public void SetRect(XmlElement element,string name, Rect value)
	{
		SetFloat(element,"Rect_x_" + name,value.x);
		SetFloat(element,"Rect_y_" + name,value.y);
		SetFloat(element,"Rect_width_" + name,value.width);
		SetFloat(element,"Rect_height_" + name,value.height);
	}
	public Rect GetRect(XmlElement element,string name)
	{
		Rect aux = new Rect(0,0,1,1);
		aux.x= GetFloat(element,"Rect_x_" + name);
		aux.y= GetFloat(element,"Rect_y_" + name);
		aux.width= GetFloat(element,"Rect_width_" + name);
		aux.height= GetFloat(element,"Rect_height_" + name);
		return aux	;
	}
	
	public void SetObject(XmlElement element,string name, UnityEngine.Object value)
	{
		
	}
	public UnityEngine.Object GetObject(XmlElement element,string name)
	{
		return null;
	}
	
	
	public void SetDateTime(XmlElement element,string name,DateTime dateTime)
	{
		SetLong(element,name,dateTime.Ticks);
	}
	public DateTime GetDateTime(XmlElement element, string name)
	{
		return (new DateTime( GetLong(element,name)));
	}

	public MotinSerializableData GetMotinSerializableData(XmlElement element,string name)
	{
		XmlElement targetElement =  GetChildElement(element,name);
		if(targetElement==null)
			return null;
		
		MotinSerializableData data = 	MotinDataManager.InstanceSerializableDataFromName(targetElement.Name);
		data.Deserialize(targetElement);
		return data;
	}
	public void serializeMotinSerializableData(XmlElement element,string name, MotinSerializableData value)
	{
		if(value==null)
		{
			Debug.Log ("SERIALIZE DATA NULL " + name);
			return;
		}
		
		value.name = name;
		element.AppendChild( value.Serialize(element.OwnerDocument));
	}

	public void  serializeIntArray(XmlElement element,string name, int[] value)
	{
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.setData(value);
		serializeMotinSerializableData(element,name,arrayData);
	}
	
	public int[]  GetIntArray(XmlElement element,string name)
	{	
		XmlElement arrayElement = GetChildElement(element,name);
		if(arrayElement == null)
			return null;
		
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.Deserialize(arrayElement);
		return arrayData.int_items;
		
	}
	public float[]  GetFloatArray(XmlElement element,string name)
	{	
		XmlElement arrayElement = GetChildElement(element,name);
		if(arrayElement == null)
			return null;
		
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.Deserialize(arrayElement);
		return arrayData.float_items;
		
	}
	
	public string[]  GetStringArray(XmlElement element,string name)
	{	
		XmlElement arrayElement = GetChildElement(element,name);
		if(arrayElement == null)
			return null;
		
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.Deserialize(arrayElement);
		return arrayData.string_items;
		
	}
	
	public void  serializeStringArray(XmlElement element,string name, string[] value)
	{
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.setData(value);
		serializeMotinSerializableData(element,name,arrayData);
	}

	public void  serializeFloatArray(XmlElement element,string name, float[] value)
	{
		MotinArrayData arrayData = new MotinArrayData();
		arrayData.setData(value);
		serializeMotinSerializableData(element,name,arrayData);
	}
				
	public void  serializeFloatArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{				
		Array reflectArray = (Array)field.GetValue(target);					
		float[] value = new float[reflectArray.Length];	
		for(int i = 0; i< reflectArray.Length; i++)
		{
			value[i] =(float) reflectArray.GetValue(i);
		}
					
		serializeFloatArray(element,field.Name,value);
	}
	public void  serializeEnumArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{				
		Array reflectArray = (Array)field.GetValue(target);					
		string[] value = new string[reflectArray.Length];	
		for(int i = 0; i< reflectArray.Length; i++)
		{
			value[i] =reflectArray.GetValue(i).ToString();
		}
					
		serializeStringArray(element,field.Name,value);
	}
	public void  serializeStringArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{				
		Array reflectArray = (Array)field.GetValue(target);					
		string[] value = new string[reflectArray.Length];	
		for(int i = 0; i< reflectArray.Length; i++)
		{
			value[i] =(string) reflectArray.GetValue(i);
		}
					
		serializeStringArray(element,field.Name,value);
	}
	
	public void  serializeIntArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
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
				
	public void  serializeSerializableDataList(XmlElement element,object target, System.Reflection.FieldInfo field)
	{	
		IList list =(IList)field.GetValue(target);
		
		MotinSerializableData[] value = new MotinSerializableData[list.Count];	
		for(int i = 0; i< list.Count; i++)
		{
			value[i] =(MotinSerializableData) list[i];
		}
		serializeSerializableDataArray(element,field.Name,value);
	}
	public void  serializeIntListField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{	
		IList list =(IList)field.GetValue(target);
		
		int[] value = new int[list.Count];	
		for(int i = 0; i< list.Count; i++)
		{
			value[i] =(int) list[i];
		}
		serializeIntArray(element,field.Name,value);
	}			
	public void  serializeStringListField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{	
		IList list =(IList)field.GetValue(target);
		
		string[] value = new string[list.Count];	
		for(int i = 0; i< list.Count; i++)
		{
			value[i] =(string) list[i];
		}
		serializeStringArray(element,field.Name,value);
	}
	public void  serializeEnumListField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{	
		IList list =(IList)field.GetValue(target);
		string[] value = new string[list.Count];	
		for(int i = 0; i< list.Count; i++)
		{
			value[i] =list[i].ToString();
		}
		serializeStringArray(element,field.Name,value);
	}
	public void  serializeFloatListField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{	
		IList list =(IList)field.GetValue(target);
		
		float[] value = new float[list.Count];	
		for(int i = 0; i< list.Count; i++)
		{
			value[i] =(float) list[i];
		}
		serializeFloatArray(element,field.Name,value);
	}	
	public void  serializeSerializableDataArrayField(XmlElement element,object target, System.Reflection.FieldInfo field)
	{				
		Array reflectArray = (Array)field.GetValue(target);		
		if(reflectArray==null)
			return ;
		
		MotinSerializableData[] value = new MotinSerializableData[reflectArray.Length];	
		for(int i = 0; i< reflectArray.Length; i++)
		{
			value[i] =(MotinSerializableData) reflectArray.GetValue(i);
		}
					
		serializeSerializableDataArray(element,field.Name,value);
	}
		
	public void  serializeSerializableDataArray(XmlElement element,string name, MotinSerializableData[] array)
	{			
		
		MotinSerializableArrayData	arrayData = new MotinSerializableArrayData();
		arrayData.childDatas = new List<MotinSerializableData>(array);
		serializeMotinSerializableData(element,name,arrayData);
	}
	
	public void  deserializeSerializableDataListField(XmlElement element,object target,System.Reflection.FieldInfo field)
	{
		MotinSerializableData[] deserializedArray = deserializeSerializableDataArray(element,field.Name);
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
	public void  deserializeIntListField(XmlElement element,object target,System.Reflection.FieldInfo field)
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
	public void  deserializeStringListField(XmlElement element,object target,System.Reflection.FieldInfo field)
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
	public void  deserializeEnumListField(XmlElement element,object target,System.Reflection.FieldInfo field)
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
	public void  deserializeFloatListField(XmlElement element,object target,System.Reflection.FieldInfo field)
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
	public void  deserializeIntArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
	{
		
		int[] deserializedArray = GetIntArray(element,field.Name);

		field.SetValue(target,deserializedArray);
	}
	public void  deserializeFloatArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
	{
		
		float[] deserializedArray = GetFloatArray(element,field.Name);

		field.SetValue(target,deserializedArray);
	}
	public void  deserializeEnumArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
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
	public void  deserializeStringArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
	{
		
		string[] deserializedArray = GetStringArray(element,field.Name);

		field.SetValue(target,deserializedArray);
	}
	public void  deserializeSerializableDataArrayField(XmlElement element,object target,System.Reflection.FieldInfo field)
	{
		MotinSerializableData[] deserializedArray = deserializeSerializableDataArray(element,field.Name);
		if(deserializedArray==null)
				return;
		
		Array a =  Array.CreateInstance(field.FieldType.GetElementType(),deserializedArray.Length);
		for(int i = 0; i< deserializedArray.Length; i++)
		{
			 a.SetValue(deserializedArray[i],i);
		}
		field.SetValue(target,a);
	}
	public MotinSerializableData[]  deserializeSerializableDataArray(XmlElement element,string name)
	{
		MotinSerializableArrayData	arrayData = new MotinSerializableArrayData();
		//MotinSerializableData data = null;
		foreach(XmlElement childElement in  element.ChildNodes)
	 	{
			//data = null;
			if(childElement.NodeType == XmlNodeType.Element)	
			{
				if(GetString(childElement,"name") == name)
				{
					arrayData.Deserialize(childElement);
					break;
				}
			}
		}
		
		return arrayData.childDatas.ToArray();
	
		
	}
	#endregion
	
}
*/
/*
[System.Serializable]
public class MotinArrayData : MotinData
{

	new	public static string xmlName = "MotinArrayData";
	public override string typeName()
	{
		return MotinArrayData.xmlName;
	}

	//public int 		array_count = 0;
	public int[]	int_items = null;
	public string[]	string_items = null;
	public float[]	float_items = null;


	public MotinArrayData( ) : base()
	{
	}
	public MotinArrayData( System.Xml.XmlElement element) : base(element)
	{

	}


	public void setData(int[] data)
	{
		int_items = data;
	}
	public void setData(string[] data)
	{
		string_items = data;
	}
	public void setData(float[] data)
	{
		float_items = data;
	}
	//SERIALIZABLE
	
	public override void OnSerialize (System.Xml.XmlElement element)
	{ 
		//base.OnSerialize(element);
		SetString(element,"name" ,name);
		
		if(int_items!=null){
			SetInt(element,"array_int_count",int_items.Length);
			for(int i = 0 ; i < int_items.Length; i ++)
			{
				SetInt(element,"int_item_" +i,int_items[i]);
			}
		}
		else{
			SetInt(element,"array_int_count",0);
		}
		if(string_items!=null){
			SetInt(element,"array_string_count",string_items.Length);
			for(int i = 0 ; i < string_items.Length; i ++)
			{
				SetString(element,"string_item_" +i,string_items[i]);
			}
		}
		else{
			SetInt(element,"array_string_count",0);
		}
		
		if(float_items!=null){
			SetInt(element,"array_float_count",float_items.Length);
			for(int i = 0 ; i < float_items.Length; i ++)
			{
				SetFloat(element,"float_item_" +i,float_items[i]);
			}
		}
		else{
			SetInt(element,"array_float_count",0);
		}
		
	}
	
	public override void OnDeserialize (System.Xml.XmlElement element)
	{
		//base.OnDeserialize(element);
		name = GetString(element,"name");
		int arrayCount = GetInt(element,"array_int_count");
		if(arrayCount>0)
		{
			int_items = new int[arrayCount];
			for(int i = 0 ; i < arrayCount; i ++)
			{
				int_items[i] = GetInt(element,"int_item_"+i );
			}
		}
		
		arrayCount = GetInt(element,"array_string_count");
		if(arrayCount>0)
		{
			string_items = new string[arrayCount];
			for(int i = 0 ; i < arrayCount; i ++)
			{
				string_items[i] = GetString(element,"string_item_"+i );
			}
		}
		
		arrayCount = GetInt(element,"array_float_count");
		if(arrayCount>0)
		{
			float_items = new float[arrayCount];
			for(int i = 0 ; i < arrayCount; i ++)
			{
				float_items[i] = GetFloat(element,"float_item_"+i );
			}
		}
	}
	
}
*/

/*
[System.Serializable]
public class MotinSerializableArrayData : MotinSerializableData
{
	new	public static string xmlName = "MotinSerializableArrayData";
	public override string typeName()
	{
		return MotinSerializableArrayData.xmlName;
	}
	
	public List<MotinSerializableData> childDatas = new List<MotinSerializableData>();
	
	
	
	public MotinSerializableArrayData( ) : base()
	{
		
	}
	
	public MotinSerializableArrayData(System.Xml.XmlElement element) :base(element)
	{

	}
	
	protected MotinSerializableData GetItemByName(string name,MotinSerializableData[] datas)
	{
		foreach(MotinSerializableData data in datas)
		{
			if(data.name == name)
				return data;
		}
		return null;
	}
	
	public virtual MotinSerializableData GetItemByName(string name)
	{
		return GetItemByName(name,childDatas.ToArray());
	}
	
	public void SetArray(MotinSerializableData[] array)
	{
		childDatas.Clear();
		foreach(MotinSerializableData data in array)
		{
			childDatas.Add(data);
		}
	}
	
	//SERIALIZABLE
	
	public override void OnSerialize (System.Xml.XmlElement element)
	{ 
		SetString(element,"name",name);
		//SetString(element,"count",childDatas.Count);
		
		foreach(MotinSerializableData data in childDatas)
		{
			serializeMotinSerializableData(element,data.name,data);
		}
		

	}
	
	public override void OnDeserialize (System.Xml.XmlElement element)
	{
		//DeserializeFields(element);
		name = GetString(element,"name");
		childDatas.Clear();
		
		MotinSerializableData data;
		foreach(XmlElement childElement in element.ChildNodes)
		{
			data = MotinDataManager.InstanceSerializableDataFromName(childElement.Name);
			data.Deserialize(childElement);
			childDatas.Add(data);
			
		}
		
		///base.OnDeserialize(element);
		
		
	}

}
*/
