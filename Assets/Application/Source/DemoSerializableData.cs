using UnityEngine;
using System.Collections;
using MotinGames;

namespace MotinGames.DemoApp.Data {

	[MotinEditorClassCreateMenuName("DemoSerializableData")] //set the name that will be showed on the editor create popup
	[MotinEditorClassReadonlyFields("testInt")] //names of the fields that will be readonly on the editor, MotinEditorClassHideFields works in the same way
	public class DemoSerializableData : MotinData {

		public int 			testInt;
		public float 		testFloat;
		public string		testString;
		public Vector3		testVector;
		public MotinStrings	testEnum;

		[MotinEditorLocalizationEnumField]//Shows popups with the strings from the localization sistem
		public string		testMotinStringField;

		[MotinEditorSoundEnumField]
		public string		testSoundEnum;

		public int[]		testIntArray;
	}
}
