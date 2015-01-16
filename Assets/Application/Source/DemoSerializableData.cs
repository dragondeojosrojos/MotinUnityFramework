using UnityEngine;
using System.Collections;

namespace MotinGames.DemoApp.Data {

public class DemoSerializableData : MotinData {

	public int 			testInt;
	public float 		testFloat;
	public string		testString;
	public Vector3		testVector;
	public MotinStrings	testEnum;

	[MotinEditorStringEnumField]
	public string		testMotinStringField;

	[MotinEditorSoundEnumField]
	public string		testSoundEnum;

	public int[]		testIntArray;
}
}
