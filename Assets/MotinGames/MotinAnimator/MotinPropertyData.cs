using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MotinPropertyData  {
	
	//public MotinValue[]		motinValuesSerialized;

	public List<MotinValue> motinValues  = new List<MotinValue>();

	public void ClearData()
	{
		motinValues.Clear();
	}
	public bool containsKey(string name)
	{
		foreach(MotinValue motinValue in motinValues)
		{
			if(motinValue.name==name)
				return true;
		}
		return false;
	}
	public void deleteMotinValue(string name)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			return;
		}
		else
		{
			motinValues.RemoveAt(index);
		}
	}
	public int getMotinValueIndex(string name)
	{
		//Debug.Log("getMotinValueIndex " + name);
		int index = 0;
		foreach(MotinValue motinValue in motinValues)
		{
		//	Debug.Log("motinValue index " + index + " " + motinValue.name);
			if(motinValue.name==name)
			{
				//Debug.Log("found " + index);
				return index;
			}
			
			index++;
		}
		return -1;
	}
	
	public bool addMotinValue(MotinValue motinValue)
	{
		int index= getMotinValueIndex(motinValue.name);
		if(index==-1) 
		{
			motinValues.Add(motinValue);
			return true;
		}
		else
		{
			motinValues[index]=motinValue;
			return true;
		}

	}
	
	public bool add(string name,int value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(int));
			motinValue.setInt(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(int));
			motinValues[index].setInt(value);
			return true;
		}
		
	}
	
	public bool add(string name,bool value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(bool));
			motinValue.setBool(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(bool));
			motinValues[index].setBool(value);
			return true;
		}
		
	}
	public bool add(string name,double value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(double));
			motinValue.setDouble(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(double));
			motinValues[index].setDouble(value);
			return true;
		}
		
	}
	public bool add(string name,float value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(float));
			motinValue.setFloat(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(float));
			motinValues[index].setFloat(value);
			return true;
		}
		
	}
	public bool add(string name,string value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(string));
			motinValue.setString(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(string));
			motinValues[index].setString(value);
			return true;
		}
		
	}
	public bool add(string name,Vector3 value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(Vector3));
			motinValue.setVector3(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(Vector3));
			motinValues[index].setVector3(value);
			return true;
		}
		
	}
	public bool add(string name,Vector2 value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(Vector2));
			motinValue.setVector2(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(Vector2));
			motinValues[index].setVector2(value);
			return true;
		}
		
	}
	public bool add(string name,Rect value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(Rect));
			motinValue.setRect(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(Rect));
			motinValues[index].setRect(value);
			return true;
		}
		
	}
	public bool add(string name,Color value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(Color));
			motinValue.setColor(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(Color));
			motinValues[index].setColor(value);
			return true;
		}
		
	}
	public bool add(string name,Object value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(Object));
			motinValue.setObject(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(Object));
			motinValues[index].setObject(value);
			return true;
		}
		
	}
	public bool add(string name,List<MotinValue> value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(MotinValue.ValueType.Array);
			motinValue.lsArray = new List<MotinValue>(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(MotinValue.ValueType.Array);
			motinValues[index].lsArray = value;
			return true;
		}
		
	}
	
	
	public int getArrayCount(string name)
	{
		int index= getMotinValueIndex(name);
//		Debug.Log("array index " + index + " " +name);
		if(index==-1)
		{
			//Debug.Log("getArrayCount " + name + "not found");
			return 0;
		}
		else
		{
			//Debug.Log("getArrayCount " +name + " " + motinValues[index].lsArray.Count);
			return motinValues[index].lsArray.Count;
		}
	}
	public void addToArray(string name , int value)
	{
		
		MotinValue motinValue = new MotinValue();
		motinValue.name = name;
		motinValue.setValueType(typeof(int));
		motinValue.setInt(value);
		addToArray(name,motinValue);
	}
	public void addToArray(string name , string value)
	{
		
		MotinValue motinValue = new MotinValue();
		motinValue.name = name;
		motinValue.setValueType(typeof(string));
		motinValue.setString(value);
		addToArray(name,motinValue);
	}
	/*
	public void addToArray(string name , MotinPropertyData value)
	{
		Debug.Log("ADD MOTIN DATA " + name);
		MotinValue motinValue = new MotinValue();
		motinValue.name = name;
		motinValue.setValueType(typeof(MotinPropertyData));
		motinValue.setMotinData(value);
		addToArray(name,motinValue);
	}
	*/
	public void addToArray(string name , MotinValue value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			Debug.Log("Array not found create " + name);
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(MotinValue.ValueType.Array);
			motinValue.lsArray = new List<MotinValue>();
			
			value.name = name + "_" + motinValue.lsArray.Count;
			motinValue.lsArray.Add(value);
			addMotinValue(motinValue);	
		}
		else
		{
			value.name = name + "_" + motinValues[index].lsArray.Count;
			motinValues[index].lsArray.Add(value);
		}
	}
	public void arrayRemoveAt(string name , int value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			return;
		}
		else
		{
			if(motinValues[index].isArray()==false) return ;
			
			if(value >= motinValues[index].lsArray.Count)
			{
				return;
				
			}
			motinValues[index].lsArray.RemoveAt(value);
			return;
		}
	}
	public bool arrayContains(string name , int value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			return false;
		}
		else
		{
			if(motinValues[index].isArray()==false) return false;
			
			foreach(MotinValue motinValue in motinValues[index].lsArray)
			{
				if(motinValue.getValueType()==typeof(int))
				{
					if(motinValue.val_int == value) return true;
				}
			}
			return false;
		}
	}
	public bool arrayContains(string name , string value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			return false;
		}
		else
		{
			if(motinValues[index].isArray()==false) return false;
			
			foreach(MotinValue motinValue in motinValues[index].lsArray)
			{
				if(motinValue.getValueType()==typeof(string))
				{
					if(motinValue.val_string == value) return true;
				}
			}
			return false;
		}
	}
	public void removeFromArray(string name , MotinValue value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			Debug.LogError("Array not exist");
		}
		else
		{
			motinValues[index].lsArray.Remove(value);
		}
	}
	/*
	public bool add(string name,MotinPropertyData value)
	{
		int index= getMotinValueIndex(name);
		if(index==-1)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.name = name;
			motinValue.setValueType(typeof(MotinPropertyData));
			motinValue.setMotinData(value);
			addMotinValue(motinValue);
			return true;
		}
		else
		{
			motinValues[index].setValueType(typeof(MotinPropertyData));
			motinValues[index].setMotinData(value);
			return true;
		}
		
	}
	*/
	public MotinValue getMotinValue(string name)
	{
		foreach(MotinValue motinValue in motinValues)
		{
			if(motinValue.name==name)
				return motinValue;
		}
		return null;
	}
	
	public int getInt(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist " + name );
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Integer)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_int;
	}
	public float getFloat(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Float)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_float;
	}
	public double getDouble(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Double)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_double;
	}
	
	public bool getBool(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Boolean)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_bool;
	}
	public string getString(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
		{
			Debug.Log("Motin value does not exist" + name);
			return "";
		}
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.String)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_string;
	}
	public Vector2 getVector2(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Vector2)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_vect2;
	}
	public Vector3 getVector3(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Vector3)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_vect3;
	}
	public Rect getRect(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Rect)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_rect;
	}
	public Color getColor(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Color)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_color;
	}
	public Object getObject(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Object)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_obj;
	}
	public List<MotinValue> getArray(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.Array)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.lsArray;
	}
	public int getIntFromArray(string name,int index)
	{
		List<MotinValue> array = getArray(name);
		if(array == null || index >= array.Count)
			Debug.LogError("Motin array is null or out of range");

		return array[index].val_int;
	}
	public string getStringFromArray(string name,int index)
	{
		List<MotinValue> array = getArray(name);
		if(array == null || index >= array.Count)
			Debug.LogError("Motin array is null or out of range");

		return array[index].val_string;
	}
	/*
	public MotinPropertyData getMotinDataFromArray(string name,int index)
	{
		List<MotinValue> array = getArray(name);
		if(array == null || index >= array.Count)
			Debug.LogError("Motin array is null or out of range");

		return array[index].val_motinData;
	}
	
	public MotinPropertyData getMotinData(string name)
	{
		MotinValue motinValue = getMotinValue(name);
		if(motinValue==null)
			Debug.LogError("Motin value does not exist" + name);
		
		if(motinValue.valueType!=(int)MotinValue.ValueType.MotinPropertyData)
			Debug.LogError("Motin value invalid type");
		
		return motinValue.val_motinData;
	}
	*/
	
}
