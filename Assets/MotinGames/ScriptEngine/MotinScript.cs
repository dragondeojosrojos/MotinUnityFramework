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
		CANCELLED,
		
	}

	public class MotinScript
	{
		public SCRIPT_STATE scriptState	= SCRIPT_STATE.IDLE;

		public System.Action				OnFinished = null;
		public System.Action<MotinScript>	OnFinishedWithScript = null;

		public void RaiseFinished(bool succeded = true)
		{
			if(succeded)
				scriptState = SCRIPT_STATE.COMPLETED;
			else
				scriptState = SCRIPT_STATE.FAILED;

			OnEnded();

			if(OnFinished!=null)
			{
				OnFinished();
			}

			if(OnFinishedWithScript!=null)
			{
				OnFinishedWithScript(this);
			}
		}
		//public System.Action<MotinScript>	onScriptFailed = null;



		public MotinScriptEngine 		scriptEngine = null;

		private Hashtable				_context = null;
		public Hashtable				context
		{
			get{return _context;}
			set{
				_context = value;
				parseContextData(_context);
			}
		}



		private MotinData				_scriptData = null;
		public MotinData				scriptData
		{
			get{return _scriptData;}
			set{
				_scriptData = value;
				parseScriptData(scriptData);
			}
		}

		public int uniqueId
		{
			get;
			protected set;
		}
		
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
			uniqueId = MotinUtils.GetUniqueInteger();
		}
		public MotinScript(MotinScriptEngine scriptEngine ,MotinData data)
		{
			uniqueId = MotinUtils.GetUniqueInteger();
			this.scriptEngine = scriptEngine;
			scriptData = data;
		}
		/*
		public void SetData(MotinData data)
		{
			scriptData = data;
			parseScriptData(scriptData);
		}
		*/
		protected virtual void parseScriptData(MotinData data)
		{
			
		}
		protected virtual void parseContextData(Hashtable data)
		{
			
		}
		public bool isCancelled()
		{
			return scriptState == SCRIPT_STATE.CANCELLED;
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
			OnStart();
		}
		public void ForceStop()
		{
			scriptState = SCRIPT_STATE.CANCELLED;
			OnWillForceStop();
		}

		protected virtual void OnStart()
		{
			Debug.LogError("MOTIN SCRIPT OnEnter NOT IMPLEMENTED");
			//raiseOnScriptCompleted();
		}
		public virtual void OnUpdate()
		{
			
		}
		public virtual void OnFixedUpdate()
		{
			
		}

		public virtual void OnLateUpdate()
		{
			
		}

		protected virtual void OnEnded()
		{
			
		}

		protected virtual void OnWillForceStop()
		{
			RaiseFinished(false);
		}
		/*
		public virtual void step()
		{
			
		}
		*/

	/*
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
		*/
		
	}
}
