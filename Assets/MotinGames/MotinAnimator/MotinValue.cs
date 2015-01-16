using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




[System.Serializable]
public class MotinValue /*: MotinSerializableData */{
	
	/*
	new	public static string xmlName = "MotinValue";
	public override string typeName()
	{
		return MotinValue.xmlName;
	}
	*/
	/*
	public MotinValue( ) : base()
	{
		//name = "default";
	}
	public MotinValue( System.Xml.XmlElement element) : base(element)
	{
		
	}
	*/
	public enum ValueType {
		Integer = 0,
		Long = 1,
		Float = 2,
		Double = 3,
		Vector2 = 4,
		Vector3 = 5,
		Vector4 = 6,
		Color = 7,
		Rect = 8,
		String = 9,
		Char = 10,
		Object = 11,
		Array = 12,
		Boolean = 13,
		MotinPropertyData = 14
	}
	public string name = "default";
	
	public bool val_bool;
	public int valueType;
	
	public int val_int;
	public float val_float;
	public double val_double;
	public Vector2 val_vect2;
	public Vector3 val_vect3;
	public Vector4 val_vect4;
	public Color val_color;
	public Rect val_rect;
	public string val_string;
	//public MotinPropertyData val_motinData = null;
	public UnityEngine.Object val_obj = null;
	public List<MotinValue> lsArray = null;
	
	/*
	public MotinValue( ) : base()
	{
		val_motinData = null;
		val_obj = null;
	}
	
	public MotinValue( System.Xml.XmlElement element) : base(element)
	{

	}
	 */
	public bool setBool(bool val_bool) {
		if(this.val_bool!=val_bool) {
			this.val_bool = val_bool;
			return true;
		}
		return false;
	}
	public bool setInt(int val_int) {
		if(this.val_int!=val_int) {
			this.val_int = val_int;
			return true;
		}
		return false;
	}
	public bool setDouble(double val_double) {
		if(this.val_double!=val_double) {
			this.val_double = val_double;
			return true;
		}
		return false;
	}
	public bool setFloat(float val_float) {
		if(this.val_float!=val_float) {
			this.val_float = val_float;
			return true;
		}
		return false;
	}
	public bool setVector2(Vector2 val_vect2) {
		if(this.val_vect2!=val_vect2) {
			this.val_vect2 = val_vect2;
			return true;
		}
		return false;
	}
	public bool setVector3(Vector3 val_vect3) {
		if(this.val_vect3!=val_vect3) {
			this.val_vect3 = val_vect3;
			return true;
		}
		return false;
	}
	public bool setVector4(Vector4 val_vect4) {
		if(this.val_vect4!=val_vect4) {
			this.val_vect4 = val_vect4;
			return true;
		}
		return false;
	}
	public bool setColor(Color val_color) {
		if(this.val_color!=val_color) {
			this.val_color = val_color;
			return true;
		}
		return false;
	}
	public bool setRect(Rect val_rect) {
		if(this.val_rect!=val_rect) {
			this.val_rect = val_rect;
			return true;
		}
		return false;
	}
	public bool setString(string val_string) {
		if(this.val_string!=val_string) {
			this.val_string = val_string;
			return true;
		}
		return false;
	}
	public bool setObject(UnityEngine.Object val_obj) {
		if(this.val_obj!=val_obj) {
			this.val_obj = val_obj;
			return true;
		}
		return false;
	}
	/*
	public bool setMotinData (MotinPropertyData motinData)
	{
		if(this.val_motinData!=motinData) {
			this.val_motinData = motinData;
			return true;
		}
		return false;
	}
	*/
	public Type getValueType() {
		if(valueType == (int) ValueType.Boolean) return typeof(bool);
		if(valueType == (int) ValueType.Integer || valueType == (int) ValueType.Long) return typeof(int);
		if(valueType == (int) ValueType.Float || valueType == (int) ValueType.Double) return typeof(float);
		if(valueType == (int) ValueType.Vector2) return typeof(Vector2);
		if(valueType == (int) ValueType.Vector3) return typeof(Vector3);
		if(valueType == (int) ValueType.Vector4) return typeof(Vector4);
		if(valueType == (int) ValueType.Color) return typeof(Color);
		if(valueType == (int) ValueType.Rect) return typeof(Rect);
		if(valueType == (int) ValueType.Object) return typeof(UnityEngine.Object);
		
		if(valueType == (int) ValueType.String) return typeof(string);
		if(valueType == (int) ValueType.Char) return typeof(char);
		if(valueType == (int) ValueType.MotinPropertyData) return typeof(MotinPropertyData);
		if(valueType == (int) ValueType.Array) {
		if(lsArray==null || lsArray.Count<=0) return typeof(object[]);
			else return lsArray[0].getValueType();
		}
		Debug.LogError("Animator: Type not found for Event Parameter.");
		return typeof(object);
	}
	
	
	public string getStringValue() {
		if(valueType == (int) ValueType.Boolean) return val_bool.ToString().ToLower();
		if(valueType == (int) ValueType.String) return "\""+val_string+"\"";
		if(valueType == (int) ValueType.Char) {
			if(val_string == null || val_string.Length <= 0) return "''";
			else return "'"+val_string[0]+"'";
		}
		if(valueType == (int) ValueType.Integer || valueType == (int) ValueType.Long) return val_int.ToString();
		if(valueType == (int) ValueType.Float || valueType == (int) ValueType.Double) return val_float.ToString();
		if(valueType == (int) ValueType.Vector2) return val_vect2.ToString();
		if(valueType == (int) ValueType.Vector3) return val_vect3.ToString();
		if(valueType == (int) ValueType.Vector4) return val_vect4.ToString();
		if(valueType == (int) ValueType.Color) return val_color.ToString();
		if(valueType == (int) ValueType.Rect) return val_rect.ToString();
		if(valueType == (int) ValueType.Object) 
			if (!val_obj) return "None";
			else return val_obj.name;

		if(valueType == (int) ValueType.Array) return "Array";
		Debug.LogError("Animator: Type not found for Event Parameter.");
		return "object";	
	}
	
	public void setValueType(Type t) {
		if(t == typeof(bool)) valueType = (int)ValueType.Boolean;
		else if(t == typeof(string)) valueType = (int)ValueType.String;
		else if(t == typeof(char)) valueType = (int)ValueType.Char;
		else if(t == typeof(int)) valueType = (int) ValueType.Integer;
		else if(t == typeof(long)) valueType = (int) ValueType.Long;
		else if(t == typeof(float)) valueType = (int)ValueType.Float;
		else if(t == typeof(double)) valueType = (int)ValueType.Double;
		else if(t == typeof(Vector2)) valueType = (int)ValueType.Vector2;
		else if(t == typeof(Vector3)) valueType = (int)ValueType.Vector3;
		else if(t == typeof(Vector4)) valueType = (int)ValueType.Vector4;
		else if(t == typeof(Color)) valueType = (int)ValueType.Color;
		else if(t == typeof(Rect)) valueType = (int)ValueType.Rect;
		else if(t == typeof(MotinPropertyData)) valueType = (int)ValueType.MotinPropertyData;
		else if(t.IsArray) valueType = (int)ValueType.Array;
		else if(t.BaseType.BaseType == typeof(UnityEngine.Object)) valueType = (int)ValueType.Object;
		
		else { 
			valueType = (int)ValueType.Object;
		}
	}
	public void setValueType(ValueType t) {

		valueType= (int)t;
	}
	public object toObject() {
		if(valueType == (int) ValueType.Boolean) return (val_bool/* as object*/);
		if(valueType == (int) ValueType.String) return (val_string/* as object*/);
		if(valueType == (int) ValueType.Char) {
			if(val_string == null || val_string.Length<=0) return '\0';
			return (val_string[0]/* as object*/);
		}
		if(valueType == (int) ValueType.Integer || valueType == (int) ValueType.Long) return (val_int/* as object*/);
		if(valueType == (int) ValueType.Float || valueType == (int) ValueType.Double) return (val_float/* as object*/);
		if(valueType == (int) ValueType.Vector2) return (val_vect2/* as object*/);
		if(valueType == (int) ValueType.Vector3) return (val_vect3/* as object*/);
		if(valueType == (int) ValueType.Vector4) return (val_vect4/* as object*/);
		if(valueType == (int) ValueType.Color) return (val_color/* as object*/);
		if(valueType == (int) ValueType.Rect) return (val_rect/* as object*/);
		if(valueType == (int) ValueType.Object) return (val_obj/* as object*/);
		if(valueType == (int) ValueType.Array && lsArray.Count > 0) {
			if(lsArray[0].valueType == (int) ValueType.Boolean) {
				bool[] arrObj = new bool[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_bool;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.String) {
				string[] arrObj = new string[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_string;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Char) {
				char[] arrObj = new char[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_string[0];
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Integer || lsArray[0].valueType == (int) ValueType.Long) {
				int[] arrObj = new int[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_int;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Float || lsArray[0].valueType == (int) ValueType.Double) {
				float[] arrObj = new float[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_float;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Vector2) {
				Vector2[] arrObj = new Vector2[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_vect2;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Vector3) {
				Vector3[] arrObj = new Vector3[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_vect3;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Vector4) {
				Vector4[] arrObj = new Vector4[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_vect4;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Color) {
				Color[] arrObj = new Color[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_color;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Rect) {
				Rect[] arrObj = new Rect[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_rect;
				}
				return arrObj;
			}
			if(lsArray[0].valueType == (int) ValueType.Object) {
				UnityEngine.Object[] arrObj = new UnityEngine.Object[lsArray.Count];
				for(int i=0;i<lsArray.Count;i++){
					arrObj[i] = lsArray[i].val_obj;
				}
				return arrObj;
			}
			
		}
		Debug.LogError("Animator: Type not found for Event Parameter.");
		return null;
	}
	
	
	public object[] toArray() {
		object[] arr = new object[lsArray.Count];
		for(int i=0;i<lsArray.Count;i++)
			arr[i] = lsArray[i].toObject();
		
		return arr;
	}
	public bool isArray() {
		if(lsArray.Count > 0) return true;
		return false;
	}
	
	public MotinValue CreateClone() {
		MotinValue a = new MotinValue();
		a.val_bool = val_bool;
		a.valueType = valueType;
		a.val_int = val_int;
		a.val_double = val_double;
		a.val_float = val_float;
		a.val_vect2 = val_vect2;
		a.val_vect3 = val_vect3;
		a.val_vect4 = val_vect4;
		a.val_color = val_color;
		a.val_rect = val_rect;
		a.val_string = val_string;
		a.val_obj = val_obj;
		//a.val_motinData = val_motinData;
		foreach(MotinValue e in lsArray) {
			a.lsArray.Add(e.CreateClone());
		}
		return a;
	}
	
	
	
	//SERIALIZABLE
	/*
	public override void OnSerialize (System.Xml.XmlElement element)
	{ 
		base.OnSerialize(element);
		
		SetInt(element,"valueType",valueType);
		
		if(valueType == (int) ValueType.Boolean) SetBool(element,"val_bool",val_bool);
		if(valueType == (int) ValueType.Integer || valueType == (int) ValueType.Long) SetInt(element,"val_int",val_int);
		if(valueType == (int) ValueType.Float || valueType == (int) ValueType.Double) SetFloat(element,"val_float",val_float);
		if(valueType == (int) ValueType.Vector2) SetVector2(element,"val_vect2",val_vect2);
		if(valueType == (int) ValueType.Vector3) SetVector3(element,"val_vect3",val_vect3);
		if(valueType == (int) ValueType.Vector4) SetVector4(element,"val_vect4",val_vect4);
		if(valueType == (int) ValueType.Color) 	SetColor(element,"val_color",val_color);
		if(valueType == (int) ValueType.Rect) 	SetRect(element,"val_rect",val_rect);
		if(valueType == (int) ValueType.Object) SetObject(element,"val_obj",val_obj);
		
		if(valueType == (int) ValueType.String) SetString(element,"val_string",val_string);
		if(valueType == (int) ValueType.Char) SetString(element,"val_char",val_string);
		if(valueType == (int) ValueType.MotinPropertyData) serializeMotinSerializableData(element,"val_motinData",val_motinData);
		
		if(valueType == (int) ValueType.Array) 
		{
			
			seria
			foreach(MotinValue motValue in lsArray)
			{
				element.AppendChild( motValue.Serialize(element.OwnerDocument));
			}
		}

	}
	
	public override void OnDeserialize (System.Xml.XmlElement element)
	{
		base.OnDeserialize(element);
		
		valueType = GetInt(element,"valueType");
		
		if(valueType == (int) ValueType.Boolean) val_bool= GetBool(element,"val_bool");
		if(valueType == (int) ValueType.Integer || valueType == (int) ValueType.Long) val_int=GetInt(element,"val_int");
		if(valueType == (int) ValueType.Float || valueType == (int) ValueType.Double)val_float= GetFloat(element,"val_float");
		if(valueType == (int) ValueType.Vector2) val_vect2=GetVector2(element,"val_vect2");
		if(valueType == (int) ValueType.Vector3) val_vect3=GetVector3(element,"val_vect3");
		if(valueType == (int) ValueType.Vector4) val_vect4=GetVector4(element,"val_vect4");
		if(valueType == (int) ValueType.Color) 	val_color=GetColor(element,"val_color");
		if(valueType == (int) ValueType.Rect) 	val_rect=GetRect(element,"val_rect");
		if(valueType == (int) ValueType.Object)val_obj= GetObject(element,"val_obj");
		
		if(valueType == (int) ValueType.String) val_string=GetString(element,"val_string");
		if(valueType == (int) ValueType.Char) val_string=GetString(element,"val_char");
		//if(valueType == (int) ValueType.MotinPropertyData) get
		
		//if(valueType == (int) ValueType.Array) 
		//{
			//childs_ is array , serialized on base class
		//}
	}
	
	public override void OnDeserializeChildElement(MotinSerializableData data)
	{
		if(data.name == "val_motinData")
		{
			val_motinData = (MotinPropertyData)data;
		}
		else if(data.name == "placeOnEntity")
		{
			placeOnEntity = ((MotinArrayData)data).int_items;
		}
		else
		{
			base.OnDeserializeChildElement(data);
		}
	}

	*/
	
}
