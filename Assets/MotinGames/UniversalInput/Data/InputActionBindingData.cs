using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{
	public class InputActionBindingData : MotinData 
	{
		public string inputName = "";

		[MotinEditorReadonlyField]
		public INPUT_ACTIONS gameActionId =INPUT_ACTIONS.COUNT;

		public InputActionBindingData( ) : base()
		{

		}
		public InputActionBindingData(INPUT_ACTIONS action) : base()
		{
			gameActionId = action;
			name = action.ToString();
		}
	}
}
