using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MotinGames;

namespace MotinGames
{
	public class UniversalInputManager : MonoBehaviour {

		public const string GameActionsFilePath = "Data/GameImputActionsDefinitions.xml";
		public const string InputFilePath = "Data/InputDefinitions.xml";

		//public string defaultControlName = "GAMEPAD_OSX";
		public InputPlatforms	inputPlatform{get;protected set;}
		public	bool			enableDebugLog = false;
		ControlSetupData[]	controlSetups = null;
		ControlSetupData currentSetup = null;

		static UniversalInputManager	sharedInstance = null;
		public static UniversalInputManager	sharedManager()
		{
			return sharedInstance;
		}


		void Log(string text)
		{
			if(!enableDebugLog)
				return;


			Debug.Log(text);
		}

		void Awake()
		{
			if(sharedInstance==null)
			{
				sharedInstance = this;
			}
			else
			{
				Debug.LogError("Only 1 PowersDataManager can exist, destroying");
				Destroy(this);
				return;
			}


			#if UNITY_STANDALONE_WIN
				inputPlatform = InputPlatforms.WINDOWS;
			#elif UNITY_STANDALONE_OSX
				inputPlatform = InputPlatforms.OSX;
			#elif UNITY_ANDROID
				inputPlatform = InputPlatforms.ANDROID;
			#elif UNITY_IOS
				inputPlatform = InputPlatforms.IOS;
			#elif UNITY_XBOXONE || UNITY_XBOX360
				inputPlatform = InputPlatforms.XBOX;
			#elif UNITY_PS3 || UNITY_PS4
				inputPlatform = InputPlatforms.PLAYSTATION;
			#endif

			controlSetups =  MotinDataManager.Load<ControlSetupData>(InputFilePath);

			DetectControlSetup();

		}

		protected void DetectControlSetup()
		{
			List<ControlSetupData> posibleSetups = new List<ControlSetupData>();
			foreach(ControlSetupData setup in controlSetups)
			{
				if(setup.platform == inputPlatform || setup.platform == InputPlatforms.ALL)
					posibleSetups.Add(setup);
			}

			string[] joyNames = Input.GetJoystickNames();
			ControlSetupData resultSetup = null;
			if(joyNames!=null && joyNames.Length>0)
			{
				Log("FOUND " + joyNames.Length + " joysticks");
				foreach(string joyName in joyNames)
				{
					Log("JOY " + joyName );
				}
				foreach(ControlSetupData setup in posibleSetups)
				{
					if(!string.IsNullOrEmpty(setup.controlPrefix) && joyNames[0].ToUpper().Contains( setup.controlPrefix.ToUpper()))
					{
						resultSetup = setup;
						break;
					}
				}

				if(resultSetup == null)
				{
					foreach(ControlSetupData setup in posibleSetups)
					{
						if(setup.controlPrefix.Contains( "GENERIC"))
						{
							resultSetup = setup;
							break;
						}
					}
				}


			}

			if(resultSetup == null)
			{
				foreach(ControlSetupData setup in posibleSetups)
				{
					if(setup.isDefault)
					{
						resultSetup = setup;
						break;
					}
				}
			}
			currentSetup = resultSetup;
			Log("SELECTED SETUP " + currentSetup.name);
		}

		public bool GetButton(INPUT_ACTIONS gameInput)
		{
			Log("test GetButton "+ gameInput.ToString());
			return Input.GetButton(currentSetup.inputBindings[(int)gameInput].inputName);
		}
		public bool GetButtonDown(INPUT_ACTIONS gameInput)
		{
			Log("test GetButtonDown "+ gameInput.ToString());
			return Input.GetButtonDown(currentSetup.inputBindings[(int)gameInput].inputName);
		}
		public bool GetButtonUp(INPUT_ACTIONS gameInput)
		{
			return Input.GetButtonUp(currentSetup.inputBindings[(int)gameInput].inputName);
		}
		public float GetAxis(INPUT_ACTIONS gameInput)
		{
			//Debug.Log("GET AXIS " + gameInput.ToString() + " " + currentSetup.inputBindings[(int)gameInput].inputName);

			return Input.GetAxis(currentSetup.inputBindings[(int)gameInput].inputName);
		}
		public float GetAxisRaw(INPUT_ACTIONS gameInput)
		{
			return Input.GetAxisRaw(currentSetup.inputBindings[(int)gameInput].inputName);
		}
	}
}
