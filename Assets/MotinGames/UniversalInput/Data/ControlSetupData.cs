using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{
	public class ControlSetupData : MotinData 
	{
		public List<InputActionBindingData> inputBindings = new List<InputActionBindingData>();
		public string						controlPrefix = "";
		public bool							isDefault	 = false;
		public InputPlatforms				platform = InputPlatforms.ALL;



		public override void SetDefaultValues ()
		{
			base.SetDefaultValues ();

			CreateDefaultActions();
		}

		public override void OnFinishedDeserializing ()
		{
			base.OnFinishedDeserializing ();
			if(inputBindings.Count==0)
			{
				CreateDefaultActions();
				return;
			}

			List<InputActionBindingData> newBindings = new List<InputActionBindingData>();
			for(int enumIndex = 0 ; enumIndex < (int)INPUT_ACTIONS.COUNT;enumIndex++)
			{
				newBindings.Add(new InputActionBindingData((INPUT_ACTIONS)enumIndex));
				foreach(InputActionBindingData binding in inputBindings)
				{
					if(newBindings[enumIndex].gameActionId == binding.gameActionId)
					{
						newBindings[enumIndex].inputName = binding.inputName;
					}
				}
			}

			inputBindings = newBindings;

		}

		void CreateDefaultActions()
		{
			for(int enumIndex = 0 ; enumIndex < (int)INPUT_ACTIONS.COUNT;enumIndex++)
			{
				inputBindings.Add(new InputActionBindingData((INPUT_ACTIONS)enumIndex));
			}
		}
	}
}
