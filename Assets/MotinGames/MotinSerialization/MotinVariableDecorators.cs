using UnityEngine;
using System.Collections;
using System;


namespace MotinGames
{

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

	public class MotinEditorEnumField: Attribute {
		
		public Type enumeration;
		public MotinEditorEnumField(Type enumtype) {
			enumeration = enumtype;
		}
	}

	public class MotinEditorMotinDataEnumField: Attribute {

		public string filePath = "";
		public MotinEditorMotinDataEnumField(string file) 
		{
			filePath = file;
		}
	}



	public class MotinEditorReadonlyField: Attribute {
		
		public MotinEditorReadonlyField() {
			
		}
	}


	public class MotinEditorFieldListNamespaces: Attribute 
	{
		public string[] namespaces = null;
		public MotinEditorFieldListNamespaces(params string[] args ) {
			namespaces = args;
		}
	}
	public class MotinEditorFieldListTypes: Attribute 
	{
		public Type[] types = null;
		public MotinEditorFieldListTypes(params Type[] args ) {
			types = args;
		}
	}

	public class MotinEditorFieldSettings: Attribute 
	{

		public MotinEditorTypes editorType = MotinEditorTypes.DEFAULT;
		public Color editorColor = Color.white;
		public string editorName = "MotinEditor";
		
		public MotinEditorFieldSettings(MotinEditorTypes type,string color ="FFFFFFFF",string name = "MotinEditor") {
			editorType = type;
			editorColor = MotinUtils.HexToColor(color);
			editorName  = name;
		}
		/*
		public MotinEditorFieldSettings(MotinEditorTypes type,string color ) {
			editorType = type;
			editorColor =  MotinUtils.HexToColor(color);;
		
		}
		*/
		/*
		public MotinEditorFieldSettings(string color ) {
			editorColor =  MotinUtils.HexToColor(color);;
			
		}
*/
		/*
		public MotinEditorFieldSettings(MotinEditorTypes type,string name) {
			editorType = type;
			editorName  = name;
		}
		
		*/
		public MotinEditorFieldSettings(string name ) {
			editorName  = name;
		}

		public MotinEditorFieldSettings(string name ="MotinEditor" ,string color ="FFFFFFFF") {
			editorColor =  MotinUtils.HexToColor(color);;
			editorName  = name;
			
		}
	}

	public class MotinEditorClassCreateMenuName: Attribute 
	{
		public string name = null;
		
		public MotinEditorClassCreateMenuName( string showname) {
			name = showname;
		}
	}

	public class MotinEditorClassReadonlyFields: Attribute 
	{
		public string[] readonlyFields = null;

		public MotinEditorClassReadonlyFields(params string[] args) {
			readonlyFields = args;
		}
	}
	public class MotinEditorClassHideFields: Attribute 
	{
		public string[] hideFields = null;
		
		public MotinEditorClassHideFields(params string[] args) {
			hideFields = args;
		}
	}
}


