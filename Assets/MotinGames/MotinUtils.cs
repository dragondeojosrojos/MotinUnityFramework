using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using HutongGames.PlayMaker;

using System.Reflection;
using System.Linq;
public class MotinUtils  {
	
	public struct Tuple<T1, T2> {
   	 	public readonly T1 Item1;
   	 	public readonly T2 Item2;
    	public Tuple(T1 item1, T2 item2) { Item1 = item1; Item2 = item2;} 
	}

	    public static Tuple<T1,T2> CreateTuple<T1,T2>(T1 item1, T2 item2) { 
	        return new Tuple<T1,T2>(item1, item2); 
	    }

	public static System.Random randomGenerator = null;

	public static string GetUniqueString()
	{
		return GetUniqueInteger().ToString() + GetUniqueInteger().ToString() + GetUniqueInteger().ToString();
	}
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

	public static T StringToEnum<T>(string name)
	{
		try {
			return (T)Enum.Parse(typeof(T), name);
		}
		catch (ArgumentException) {
			Debug.LogWarning("ENUM NOT FOUND" + name);
			return (T)indexToEnum<T>(0);
		}
	}


	public static int StringArrayIndex(string[] stringArray,string value)
	{
		if(stringArray==null || value==null)
			return -1;

		int count  =  stringArray.Length;
		for(int i = 0 ; i < count;i++)
		{
			if(stringArray[i] == value)
			{
				return i;
			}
		}
		return -1;
	}

	public static int StringToIndex<T>(string name)
	{
		string[] enumNames = Enum.GetNames(typeof(T));
		int count =  enumNames.Length;
		
		for(int i = 0 ; i < count;i++)
		{
			if(enumNames[i] == name)
			{
				return i;
			}
		}
		return -1;
	}
	public static T indexToEnum<T>(int index)
	{
		string[] enumNames = Enum.GetNames(typeof(T));
		return MotinUtils.StringToEnum<T>(enumNames[index]);

	}
	/*
	public static T[] FieldToArray(System.Reflection.FieldInfo field)
	{
		Array reflectArray = (Array)field.GetValue(target);					
		T[] value = new T[reflectArray.Length];	
		for(int i = 0; i< reflectArray.Length; i++)
		{
			value[i] =(MotinSerializableData) reflectArray.GetValue(i);
		}
		return value;
	}*/

	/* NEEDS PLAYMAKER
	public static PlayMakerFSM GetFsm(string name,GameObject go)
	{
		if(go==null || name.Length==0)
			return null;
		
		PlayMakerFSM[] fsmScripts =  go.GetComponents<PlayMakerFSM>();
		foreach(PlayMakerFSM fsm in fsmScripts)
		{
			if(name==fsm.FsmName)
				return fsm;
			
		}
		return null;
	}
	*/
		
	
	public static string[] EnumNames<T>()
	{
		return Enum.GetNames(typeof(T));
	}
	
	public static bool IsTypeDerivedFrom(Type type,Type fromType)
	{
		if(type==fromType)
			return true;
		
		if(type.BaseType==null)
			return false;
		
		return MotinUtils.IsTypeDerivedFrom(type.BaseType,fromType);
		
	}
	public static  Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
	{
	    return (from type in assembly.GetTypes() where type.Namespace==nameSpace select type).ToArray();
	}

	/// <summary>
	/// Perform a deep Copy of the object.
	/// </summary>
	/// <typeparam name="T">The type of object being copied.</typeparam>
	/// <param name="source">The object instance to copy.</param>
	/// <returns>The copied object.</returns>
	public static T Clone<T>(T source)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", "source");
		}

		// Don't serialize a null object, simply return the default for that object
		if (object.ReferenceEquals(source, null))
		{
			return default(T);
		}
		
		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using (stream)
		{
			formatter.Serialize(stream, source);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	public static bool IsOddNumber(int value)
	{
		return value % 2 != 0;
	}

	//NEEDS TK2D
	/*
	static tk2dBaseSprite tmpSprite;
	static Bounds resultBounds;
	static Bounds tmpBounds;
	static Vector3	tmpMax = Vector3.zero;
	static Vector3	tmpMin = Vector3.zero;

	//Returns recursive bounds
	public static Bounds GetTk2dSpriteBounds(GameObject target)
	{
		resultBounds.SetMinMax(Vector3.zero,Vector3.zero);
		resultBounds.size = Vector3.zero;
		resultBounds.center = Vector3.zero;
		tmpMax.Set(0,0,0);
		tmpMin.Set(0,0,0);


		TestBounds(target);
		return resultBounds;
	}

	private static void TestBounds(GameObject target)
	{
		tmpSprite  = target.GetComponent<tk2dBaseSprite>();
		if(tmpSprite !=null)
		{
			tmpBounds = tmpSprite.GetBounds();

			tmpMin = tmpBounds.min;
 
			if(tmpBounds.min.x < resultBounds.min.x)
				tmpMin.x = tmpBounds.min.x;
			if(tmpBounds.min.y < resultBounds.min.y)
				tmpMin.y = tmpBounds.min.y;
			if(tmpBounds.min.z < resultBounds.min.z)
				tmpMin.z = tmpBounds.min.z;

			resultBounds.min = tmpMin;


			tmpMax = tmpBounds.max;
			if(tmpBounds.max.x > resultBounds.max.x)
				tmpMax.x = tmpBounds.max.x;
			if(tmpBounds.max.y > resultBounds.max.y)
				tmpMax.y = tmpBounds.max.y;
			if(tmpBounds.max.z > resultBounds.max.z)
				tmpMax.z = tmpBounds.max.z;

			resultBounds.max = tmpMax;
		}
		tmpSprite = null;
		for(int i = 0 ; i< target.transform.childCount;i++)
		{
			TestBounds(target.transform.GetChild(i).gameObject);
		}


	}
	*/
}
