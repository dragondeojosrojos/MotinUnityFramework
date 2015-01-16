using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MotinGames
{

public class MotinAssemblyFoldListEditor : MotinDataFoldListEditor  {
	
	
	public List<string> namespaceList = new List<string>();
	public MotinAssemblyFoldListEditor( ):base()
	{
		UpdateCreateMenu();
	}
	public MotinAssemblyFoldListEditor(EditorWindow host):base(host)
	{
		
		//UpdateCreateMenu();
		//List<GUIContent> Items = new List<GUIContent>();
	
		/*
		
		Items.Add(new GUIContent("ScriptSequence"));
		Items.Add(new GUIContent("ScriptPlayAnimTk2d"));
		Items.Add(new GUIContent("ScriptPlayAnimAnimator"));
		Items.Add(new GUIContent("MotinRunFsm"));
		
		//LoadEntityFile();
		this.menuItems = Items.ToArray();
		*/
		/*
		if(scriptName == "SequenceScript")
		{
//			Debug.Log("CreateScriptDataFromName MotinScriptNames.SequenceScript editor");
			return (MotinSerializableData)ScriptSequence.CreateData();
		}
		else if(scriptName == "PlayAnimTk2d"){
			return (MotinSerializableData)ScriptPlayAnimTk2d.CreateData();
		}
		else if(scriptName == "PlayAnimAnimator"){
			return (MotinSerializableData)ScriptPlayAnimAnimator.CreateData();
		}
		else if(scriptName == "RunFsm"){
			return (MotinSerializableData)MotinRunFsm.CreateData();
		}
		*/
	}
	public void AddNamespace(string value)
	{
		namespaceList.Add(value);
		UpdateCreateMenu();
	}
	public override void UpdateCreateMenu()
	{
		System.Type[] types = null;
		List<GUIContent> Items = new List<GUIContent>();
		foreach(string namespaceName in namespaceList)
		{
			
			types = MotinUtils.GetTypesInNamespace(System.Reflection.Assembly.Load("Assembly-CSharp"),namespaceName);
			foreach(Type type in types)
			{
				Items.Add(new GUIContent(type.Name));
			}
		}
		
		
		/*
		Items.Add(new GUIContent("ScriptSequence"));
		Items.Add(new GUIContent("ScriptPlayAnimTk2d"));
		Items.Add(new GUIContent("ScriptPlayAnimAnimator"));
		Items.Add(new GUIContent("MotinRunFsm"));
		*/
		//LoadEntityFile();
		this.menuItems = Items.ToArray();
	}
	protected override bool DoCreateMenu (int selected)
	{ 
		Debug.Log("DoCreateMenu  " + selected);
		MotinData newData = null;
		MotinEditor newEditor =null;
		
		newData = MotinDataManager.InstanceDataFromName(menuItems[selected].text);
	
		
		if(newData!=null)
		{
			newData.name = UniqueDataName(objectList_,newData.name);
			objectList_.Add(newData);
			
			CreateInitializedEditor(newData.GetType(),newData);
			//newEditor = CreateEditor(newData.GetType());
			//newEditor.target = newData;
			//newEditor.showListToolbar = true;
			//motinEditors_.Add(newEditor);
		}
		FilterList();
		RaiseOnDataChanged();
		
		return true;
	}
/*
	protected override MotinEditor CreateEditor(Type dataType)
	{
		return base.CreateEditor(dataType);
	}
	*/

}
}
	