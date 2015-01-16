using UnityEngine;
using System.Collections;

namespace MotinGames
{

[System.Serializable]
public class ScriptRunScriptData : MotinData
{
	/*
	new	public static string xmlName = "ScriptRunScriptData";
	public override string typeName()
	{
		return ScriptRunScriptData.xmlName;
	}
	*/
	public string runScript = "default";

	/*
	public ScriptRunScriptData( ) : base()
	{
		runScript ="default";
	}
	public ScriptRunScriptData( System.Xml.XmlElement element) : base(element)
	{

	}
	*/
	//SERIALIZABLE
	/*
	public override void OnSerialize (System.Xml.XmlElement element)
	{ 
		base.OnSerialize(element);
		SetString(element,"runScript",runScript);
	}
	*/
	/*
	public override void OnDeserialize (System.Xml.XmlElement element)
	{
		base.OnDeserialize(element);
		runScript = GetString(element,"runScript");
	}
	*/
}
}


