using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{
	public enum SCRIPT_STATE
	{
		IDLE,
		RUNNING,
		COMPLETED,
		FAILED,
		
	}

	public class MotinScript
	{
		public SCRIPT_STATE scriptState	= SCRIPT_STATE.IDLE;

		public delegate void MotinScriptDelegate(MotinScript motinScript);
		public MotinScriptDelegate	onScriptCompleted = null;
		public MotinScriptDelegate	onScriptFailed = null;
		
		public MotinScriptEngine 		scriptEngine = null;
		public MotinData				scriptData = null;
		
			/*
		public static   MotinData CreateData()
		{
			MotinData scriptData =new MotinData();
			//scriptData.scriptName  = "MotinScript";
			scriptData.name = "default script";
			return scriptData;
		}
		*/
		protected virtual void OnSourceGameobjectSetted()
		{
			
		}
		public MotinScript()
		{
			
		}
		public MotinScript(MotinScriptEngine scriptEngine ,MotinData data)
		{
			this.scriptEngine = scriptEngine;
			SetData(data);
		}
		public void SetData(MotinData data)
		{
			scriptData = data;
			parseScriptData(scriptData);
		}
		public virtual void parseScriptData(MotinData data)
		{
			
		}
		/*
		public virtual void Clear()
		{
			onScriptCompleted = null;
			onScriptFailed = null;
			scriptData = null;
			scriptEngine= null;
		}
		*/
		public void run()
		{
			scriptState = SCRIPT_STATE.RUNNING;
			DoAction();
		}

		protected virtual void DoAction()
		{
			Debug.LogError("MOTIN SCRIPT DoAction NOT IMPLEMENTED");
			//raiseOnScriptCompleted();
		}
		public virtual void update(float deltaT)
		{
			
		}
		public virtual void step()
		{
			
		}

	
		public void raiseOnScriptCompleted()
		{
			scriptState = SCRIPT_STATE.COMPLETED;
			if(onScriptCompleted!=null)
			{
				onScriptCompleted(this);
			}
		}
		public void raiseOnScriptFailed()
		{
			scriptState = SCRIPT_STATE.FAILED;
			if(onScriptFailed!=null)
			{
				onScriptFailed(this);
			}
		}
		
	}
}
