using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{
	public class ScriptSequence : MotinScript
	{
		ScriptSequenceData arrayData = null;
		int currentScript = 0;

			/*
		public static new  MotinData CreateData()
		{
			MotinData scriptData =new  MotinData();
			//scriptData.scriptName  = System.Enum.GetName(typeof(MotinScriptNames) ,MotinScriptNames.SequenceScript );
			scriptData.name = "SequenceScript";
			return scriptData;
		}	
		*/

		public ScriptSequence(MotinScriptEngine scriptEngine ,MotinData data) : base (scriptEngine,data)
		{
			
		}
		public override void parseScriptData(MotinData data)
		{
			arrayData =(ScriptSequenceData)data ;

		}

		protected override void DoAction ()
		{
			Debug.Log("RUN SECUENCES " + scriptData.name);
			currentScript = 0;
			if(currentScript == arrayData.childDatas.Count)
			{
				Debug.Log("Secuence completed " + arrayData.childDatas.Count);
				raiseOnScriptCompleted();
				return;
			}
			scriptEngine.runScript(arrayData.childDatas[currentScript],this.OnScriptCompleted,this.OnScriptFailed);
		}
		
		public void OnScriptCompleted(MotinScript script)
		{
		//	Debug.Log("Secuence script completed SECUENCES " + script.scriptData.name);
			currentScript++;
			if(currentScript == arrayData.childDatas.Count)
			{
			//	Debug.Log("Secuence completed " + arrayData.childDatas.Count);
				raiseOnScriptCompleted();
				return;
			}
			scriptEngine.runScript(arrayData.childDatas[ currentScript],this.OnScriptCompleted,this.OnScriptFailed);
		}
		public void OnScriptFailed(MotinScript script)
		{
			raiseOnScriptFailed();
		}
	}
}