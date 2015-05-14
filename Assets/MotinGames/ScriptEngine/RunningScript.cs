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



		public System.Action<MotinScript> OnFinished=null;
		//public System.Action<MotinScript> OnFailed=null;

		public int scriptType = 0;
		public MotinScript script= null;
		//	public Fsm	fsmScript = null;
		
		public void Reset()
		{
			script = null;
			scriptType = 0;
			OnFinished = null;
			//OnFailed = null;
			//fsmScript = null;
		}
		public RunningScript( )
		{
			Reset();
		}
		
		public RunningScript(MotinScript runScript,System.Action<MotinScript> OnFinished = null )
		{
			script = runScript;
			scriptType = (int)ScriptTypes.MotinScript;
			this.OnFinished = OnFinished;
			//OnFailed = onFailed;
		}
		/*
		public RunningScript(Fsm runScript,System.Action<MotinScript> onComplete = null,System.Action<MotinScript> onFailed = null )
		{
			scriptType = (int)ScriptTypes.Playmaker;
			fsmScript = runScript;
			OnComplete = onComplete;
			OnFailed = onFailed;
		}
		*/
	}
}