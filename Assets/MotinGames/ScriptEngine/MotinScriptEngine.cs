using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using HutongGames.PlayMaker;
using System.Reflection;

namespace MotinGames
{


	public enum MotinScriptNames
	{
		SequenceScript,
		PlayAnimTk2d,
		PlayAnimAnimator,
		RunFsm,
		ScriptAnimatedObjectPlayAnimator
	}


	public class MotinScriptEngine : MonoBehaviour {

		static int 					runningScriptsCreated =0;
		static int					runningScriptPoolDefaultCount = 20;
		static List<RunningScript>  runningScriptsPool 	= new List<RunningScript>();
		static bool 				isPoolInitialized = false;

		List<RunningScript> runningScripts 		= new List<RunningScript>();
	//	Dictionary<string,Fsm>	fsmDict	=	new Dictionary<string, Fsm>();
		public  ScriptSequenceData			scriptSequences=null;


		public static void InitRunningScriptPool()
		{
			if(isPoolInitialized)
				return;
			runningScriptsCreated =0;
			isPoolInitialized = true;
			for(int i = 0; i < runningScriptPoolDefaultCount;i++)
			{
				AddRunningScriptToPool();
			}

		}

		static RunningScript GetRunningScriptFromPool()
		{
			if(runningScriptsPool.Count==0)
			{
				Debug.Log ("ADDING RUNNING SCRIPTS TO POOL " + runningScriptsPool.Count);
				AddRunningScriptToPool();
			}

			RunningScript tmpRunningScript = runningScriptsPool[0];
			runningScriptsPool.RemoveAt(0);
			return tmpRunningScript;

		}
		static void ReturnRunningScriptToPool(RunningScript tmpRunningScript)
		{
			tmpRunningScript.Reset();
			runningScriptsPool.Add( tmpRunningScript);
		}
		static RunningScript AddRunningScriptToPool()
		{
			runningScriptsCreated++;
			//Debug.Log ("CREATED RUNNING SCRIPTS " + runningScriptsCreated);
			RunningScript tmpRunningScript = new RunningScript();
			runningScriptsPool.Add( tmpRunningScript);
			return tmpRunningScript;
		}


		public virtual MotinScript CreateMotinScriptFromData(MotinData scriptData)
		{
			System.Type dataType = scriptData.GetType();
			
			if(dataType == typeof(ScriptSequenceData))
			{
				return new ScriptSequence(this,scriptData);
			}
			/*
			else if(dataType == typeof(ScriptPlayAnimTk2dData)){
				return new ScriptPlayAnimTk2d(this,scriptData);
			}
			else if(dataType == typeof(ScriptPlayAnimAnimatorData)){
				return new ScriptPlayAnimAnimator(this,scriptData);
			}
			else if(dataType == typeof(ScriptRunScriptFsmData)){
				return new MotinRunFsm(this,scriptData);
			}
			else if(dataType == typeof(ScriptPlayAnimAnimatedObjectData)){
				return new ScriptAnimatedObjectPlayAnimator(this,scriptData);
			}
			else if(dataType == typeof(ScriptShowWindowData)){
				return new ScriptShowWindow(this,scriptData);
			}
			else if(dataType == typeof(ScriptShowPopupData)){
				return new ScriptShowPopupWindow(this,scriptData);
			}
			else if(dataType == typeof(ScriptShowMaskData)){
				return new ScriptShowMask(this,scriptData);
			}
			else if(dataType == typeof(ScriptDismissWindowData)){
				return new ScriptDismissWindow(this,scriptData);
			}
			else if(dataType == typeof(ScriptSetMaskTargetData)){
				return new ScriptSetTargetMask(this,scriptData);
			}
			*/
			else
			{
				MotinScript script =  (MotinScript)System.Activator.CreateInstance(Types.GetType(dataType.ToString().Replace("Data",""),"Assembly-CSharp"),this,scriptData);
				if(script!=null)
				{
					//script.scriptEngine = this;
					//script.scriptData = scriptData;
					//script.parseScriptData(scriptData);
					return script;
				}
					
			}
			
			Debug.Log("MOTINSCRIPT NOT FOUND  " + dataType.ToString());
			return null;
		}

		void Awake()
		{
			InitRunningScriptPool();

			scriptSequences = null;

			/*
			PlayMakerFSM[] fsmScripts =  this.GetComponents<PlayMakerFSM>();
			foreach(PlayMakerFSM fsm in fsmScripts)
			{
				fsmDict.Add(fsm.FsmName,fsm.Fsm);
			}
			*/
		}
		
		void Start()
		{
			//Estoy pasando scriptSequence a MotinScriptEngine
		}
		
		
		public int getRunningScriptIndex(MotinScript script)
		{
			int index = 0;
			foreach(RunningScript runningScript in runningScripts)
			{
				if(runningScript.script == script)
				{
					return index;
				}
				index++;
			}
			return -1;
		}
		/*
		public int getRunningScriptIndex(Fsm script)
		{
			int index = 0;
			foreach(RunningScript runningScript in runningScripts)
			{
				if(runningScript.fsmScript == script)
				{
					return index;
				}
				index++;
			}
			return -1;
		}
		*/
		public ScriptSequenceData getScriptSequence(string name)
		{
			if(scriptSequences==null)
			{
				Debug.LogWarning("SCRIPT SECUENCE NOT FOUND" + name);
				return null;
			}
			
			return scriptSequences.GetSecuenceByName(name);
		}
		public MotinData getScript(string name)
		{
			if(scriptSequences==null)
			{
				Debug.LogWarning("SCRIPT SECUENCE NOT FOUND" + name);
				return null;
			}
			
			return scriptSequences.GetDataByName(name);
		}
		/*
		public Fsm GetFsm(string name)
		{
			Fsm script = null;
			fsmDict.TryGetValue(name,out script);
			if(script==null)
			{
				Debug.Log("FSM SCRIPT NOT FOUND " + name);
				return null;
			}
			
			return script;
		}
		*/
		/*
		public void SetFsmVar(string fsmName,string fsmVarName,GameObject value)
		{
			Fsm script = GetFsm(fsmName);
			if(script==null)
			{
				return;
			}
			
			script.Variables.GetFsmGameObject(fsmVarName).Value = value;
		}
		*/
		public void runMotinScript(string name,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed=null)
		{
	//		Debug.Log("runMotinScript " + name);
			runScript(getScript(name),onComplete,onFailed);
		}

		/*
		public void runScript(string name , MotinScript.MotinScriptDelegate onComplete = null, MotinScript.MotinScriptDelegate onFailed = null )
		{
			Fsm script = GetFsm(name);
			if(script==null)
			{
				Debug.Log("FSM SCRIPT NOT FOUND " + name);
				if(onComplete!=null)
					onComplete(null);
				
				return;
			}
			if(script.ActiveStateName.StartsWith("IDLE") || script.ActiveStateName.StartsWith("END"))
			{
				//Debug.Log("RUN SCRIPT FSM " + name);
				RunningScript runningScript = GetRunningScriptFromPool();
				runningScript.fsmScript = script;
				runningScript.OnComplete = onComplete;
				runningScript.OnFailed = onFailed;
				runningScript.scriptType = (int)RunningScript.ScriptTypes.Playmaker;


				runningScripts.Add(runningScript);
				
				FsmObject scriptEngineVar =  script.GetFsmObject("ScriptEngine");
				scriptEngineVar.Value = this;
				
				script.Event("RUN");
				return;
			}
			else
			{
				//Debug.Log("RUN SCRIPT FSM " + name + " NOT IDLE ");

			}
			
			if(onComplete!=null)
				onComplete(null);
				
		}
	*/
		//MotinScript tmpMotinScript = null;

		public void runScript(MotinData[] scriptData,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed = null)
		{
			ScriptSequenceData secuenceData = new ScriptSequenceData();
			secuenceData.childDatas = new List<MotinData>(scriptData);
			runScript(secuenceData,onComplete,onFailed);
		}


		public void runScript(MotinData scriptData,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed = null )
		{
			//tmpMotinScript =null;
			if(scriptData==null)
			{
				//Debug.Log("SCRIPT ENGINE MotinScriptData is NULL" );
				if(onComplete!=null)
					onComplete(null);
				
				return;
			}
			//Debug.Log("SCRIPT ENGINE RUN SCRIPT " + scriptData.name);
			MotinScript tmpMotinScript = CreateMotinScriptFromData(scriptData);
			runScript(tmpMotinScript,onComplete,onFailed);
			tmpMotinScript = null;
		}

		public void runScript(MotinScript motinScript,MotinScript.MotinScriptDelegate onComplete = null,MotinScript.MotinScriptDelegate onFailed = null)
		{
			
			if(motinScript==null)
			{
				Debug.LogError("Run Script MotinScript is null");
				return ;
			}

			motinScript.onScriptCompleted += OnScriptComplete;
			motinScript.onScriptFailed += OnScriptFailed;
			motinScript.scriptEngine = this;
	//		Debug.Log("RUN MotinScript  " + motinScript.scriptData.name);
			RunningScript runningScript = GetRunningScriptFromPool();
			runningScript.script = motinScript;
			runningScript.scriptType = (int)RunningScript.ScriptTypes.MotinScript;
			runningScript.OnComplete = onComplete;
			runningScript.OnFailed = onFailed;

			//tmpRunningScript = new RunningScript(motinScript,onComplete,onFailed);
			runningScripts.Add(runningScript);
			runningScript.script.run();
			runningScript = null;
		}
		
		void Update()
		{
			foreach(RunningScript script in runningScripts)
			{
				if(script.scriptType== (int)RunningScript.ScriptTypes.MotinScript)
					script.script.update(Time.deltaTime);
			}
		}
		
		void OnScriptComplete(MotinScript script)
		{
		
	//		Debug.Log("SCRIPTENGINE COMPLETE " + script.scriptData.scriptName + " " + script.scriptData.name);
			int index = getRunningScriptIndex(script);
			script.onScriptCompleted -= OnScriptComplete;
			script.onScriptFailed -= OnScriptFailed;
			RunningScript runningScript = 	runningScripts[index];
			runningScripts.RemoveAt(index);
			if(runningScript.OnComplete!=null)
			{
			//	Debug.Log("RAISE COMPLETE " + script.scriptData.scriptName + " " + script.scriptData.name);
				runningScript.OnComplete(script);

			}

			ReturnRunningScriptToPool(runningScript);
		}
		
		void OnScriptFailed(MotinScript script)
		{
		//	Debug.Log("SCRIPTENGINE OnScriptFailed " + script.scriptData.name);
			script.onScriptCompleted -= OnScriptComplete;
			script.onScriptFailed -= OnScriptFailed;
			int index = getRunningScriptIndex(script);
			RunningScript runningScript = 	runningScripts[index];
			runningScripts.RemoveAt(index);
			if(runningScript.OnFailed!=null)
			{
				runningScript.OnFailed(script);
			}

			ReturnRunningScriptToPool(runningScript);
		}
		
		/*
		public void OnFsmScriptComplete(Fsm script)
		{
			int index = getRunningScriptIndex(script);
			RunningScript runningScript = 	runningScripts[index];
			runningScripts.RemoveAt(index);
			if(runningScript.OnComplete!=null)
			{
				//Debug.Log("RAISE COMPLETE " + script.scriptData.scriptName + " " + script.scriptData.name);
				runningScript.OnComplete(null);
			}
			ReturnRunningScriptToPool(runningScript);
		}
		*/
	}
}
