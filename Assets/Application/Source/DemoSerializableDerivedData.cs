﻿
using UnityEngine;
using System.Collections;

namespace MotinGames.DemoApp.Data {
	
	public class DemoSerializableDerivedData : DemoSerializableData {
		
		public 	Vector2	testVector2;
		public	Rect	testRect;

		public DemoAnotherData[]	anotherDataArray = new DemoAnotherData[0]; 
		public DemoAnotherData	anotherData = new DemoAnotherData(); 
	}
}
