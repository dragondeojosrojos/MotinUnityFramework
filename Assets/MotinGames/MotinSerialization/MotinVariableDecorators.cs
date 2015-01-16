using UnityEngine;
using System.Collections;

using System;


public class MotinEditorTk2dSpriteField: Attribute {
	
	public MotinEditorTk2dSpriteField() {
		
	}
}

public class MotinEditorSoundEnumField: Attribute {
	
	public MotinEditorSoundEnumField() {
		
	}
}

public class MotinEditorStringEnumField: Attribute {
	
	public MotinEditorStringEnumField() {
		
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
	public MotinEditorMotinDataEnumField(string file) {
		filePath = file;
	}
}

public class MotinEditorReadonlyField: Attribute {
	
	public MotinEditorReadonlyField() {
		
	}
}


