using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

public class ScriptSequenceData : MotinData
{
	/*
	new	public static string xmlName = "ScriptSequenceData";
	public override string typeName()
	{
		return ScriptSequenceData.xmlName;
	}
	


	public ScriptSequenceData( ) : base()
	{
	}
	public ScriptSequenceData( System.Xml.XmlElement element) : base(element)
	{

	}*/
	public List<MotinData> childDatas = new List<MotinData>();


	/*
	public override void OnSerialize (System.Xml.XmlElement element)
	{
		base.OnSerialize (element);
	}
	*/
	public virtual MotinData GetDataByName(string name)
	{
		
		foreach(MotinData data in childDatas)
		{
			if(data.name==name)
			{
				return  data;
			}
		}
		return null;
	}
	public virtual ScriptSequenceData GetSecuenceByName(string name)
	{
		
		foreach(MotinData data in childDatas)
		{
			if(data.name==name)
			{
				//Debug.Log ("RETURN SECUENCE " + data.name);
				return  (ScriptSequenceData)data;
			}
		}
		return null;
	}
	
}
}
