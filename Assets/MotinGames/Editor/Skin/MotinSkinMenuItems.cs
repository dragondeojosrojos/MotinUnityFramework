using UnityEditor;
using UnityEngine;
using System.Collections;



namespace MotinGames
{
	public class MotinSkinMenuItems   {

		[MenuItem ("MotinGames/Refresh Skin",false,1)]
		static void Init () 
		{
			
			MotinEditorSkin.RefreshSkin();
		}
	}
}
