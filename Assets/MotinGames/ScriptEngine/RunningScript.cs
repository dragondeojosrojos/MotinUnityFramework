using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using HutongGames.PlayMaker;
using System.Reflection;

namespace MotinGames
{
	
	public class RunningScript
	{
		public enum ScriptTypes
		{
			MotinScript,
			Playmaker
		}



		public MotinScript.MotinScriptDelegate OnComplete=null;
		public MotinScript.MotinScriptDelegate OnFailed=null;

		public int scriptType = 0;
		public MotinScript script= null;
		//	public Fsm	fsmScript = null;
		
		public void Reset()
		{
			script = null;
			scriptType = 0;
			OnComplete = null;
			OnFailed = null;
			//fsmScript = null;
		}
		public RunningScript( )
		{
			Reset();
		}
		
		public RunningScript(MotinScript runScript,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed = null )
		{
			script = runScript;
			scriptType = (int)ScriptTypes.MotinScript;
			OnComplete = onComplete;
			OnFailed = onFailed;
		}
		/*
		public RunningScript(Fsm runScript,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed = null )
		{
			scriptType = (int)ScriptTypes.Playmaker;
			fsmScript = runScript;
			OnComplete = onComplete;
			OnFailed = onFailed;
		}
		*/
	}
}