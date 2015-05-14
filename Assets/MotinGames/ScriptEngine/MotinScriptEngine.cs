using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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


		public virtual MotinScript CreateMotinScriptFromData(MotinData scriptData, string namespaceName = "" )
		{
			System.Type dataType = scriptData.GetType();
			
			if(dataType == typeof(ScriptSequenceData))
			{
				return new ScriptSequence(this,scriptData);
			}
			else
			{
				MotinScript script =  (MotinScript)System.Activator.CreateInstance(Types.GetType( (string.IsNullOrEmpty(namespaceName)? "" : (namespaceName + ".")) +  dataType.ToString().Replace("Data",""),"Assembly-CSharp"),this,scriptData);
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

		public void runMotinScript(string name,Hashtable contextData = null,System.Action<MotinScript> OnFinished = null)
		{
	//		Debug.Log("runMotinScript " + name);
			runScript(getScript(name),contextData,OnFinished);
		}


		public void runScript(MotinData[] scriptData,Hashtable contextData = null,System.Action<MotinScript> OnFinished = null)
		{
			ScriptSequenceData secuenceData = new ScriptSequenceData();
			secuenceData.childDatas = new List<MotinData>(scriptData);
			runScript(secuenceData,contextData,OnFinished);
		}


		public void runScript(MotinData scriptData,Hashtable contextData = null,System.Action<MotinScript> OnFinished = null )
		{
			//tmpMotinScript =null;
			if(scriptData==null)
			{
				//Debug.Log("SCRIPT ENGINE MotinScriptData is NULL" );
				if(OnFinished!=null)
					OnFinished(null);
				
				return;
			}
			//Debug.Log("SCRIPT ENGINE RUN SCRIPT " + scriptData.name);
			MotinScript tmpMotinScript = CreateMotinScriptFromData(scriptData);
			tmpMotinScript.context = contextData;
			runScript(tmpMotinScript,OnFinished);
			tmpMotinScript = null;
		}

		public IEnumerator RunScriptCoroutine(MotinData data,Hashtable context = null)
		{
			MotinScript scriptBase =(MotinScript)CreateMotinScriptFromData(data);
			runScript(scriptBase);
			
			while(scriptBase.scriptState == SCRIPT_STATE.RUNNING)
				yield return null;

		}

		public IEnumerator runScriptCoroutine(MotinScript motinScript,System.Action<MotinScript> OnFinished = null)
		{
			if(motinScript==null)
			{
				Debug.LogError("Run Script MotinScript is null");
				yield break ;
			}
			motinScript.scriptEngine = this;
			//		Debug.Log("RUN MotinScript  " + motinScript.scriptData.name);
			RunningScript runningScript = GetRunningScriptFromPool();
			runningScript.script = motinScript;
			runningScript.scriptType = (int)RunningScript.ScriptTypes.MotinScript;
			runningScript.OnFinished = OnFinished;
			//runningScript.OnFailed = onFailed;
			
			//tmpRunningScript = new RunningScript(motinScript,onComplete,onFailed);
			runningScripts.Add(runningScript);
			runningScript.script.run();

			while(runningScript.script.scriptState == SCRIPT_STATE.RUNNING)
				yield return null;


			//Debug.Log("runScriptCoroutine COMPLETE " + motinScript.scriptData.GetType().Name );
			runningScripts.Remove(runningScript);
			if(runningScript.OnFinished!=null)
			{
				//	Debug.Log("RAISE COMPLETE " + script.scriptData.scriptName + " " + script.scriptData.name);
				runningScript.OnFinished(runningScript.script);
				
			}
			
			ReturnRunningScriptToPool(runningScript);

			runningScript = null;
		}


		public void runScript(MotinScript motinScript,System.Action<MotinScript> OnFinished = null)
		{
			
			if(motinScript==null)
			{
				Debug.LogError("Run Script MotinScript is null");
				return ;
			}

			motinScript.OnFinishedWithScript += OnScriptFinishedCallback;
			//motinScript.onScriptFailed += OnScriptFailed;
			motinScript.scriptEngine = this;
	//		Debug.Log("RUN MotinScript  " + motinScript.scriptData.name);
			RunningScript runningScript = GetRunningScriptFromPool();
			runningScript.script = motinScript;
			runningScript.scriptType = (int)RunningScript.ScriptTypes.MotinScript;
			runningScript.OnFinished = OnFinished;
			//runningScript.OnFailed = onFailed;

			//tmpRunningScript = new RunningScript(motinScript,onComplete,onFailed);
			runningScripts.Add(runningScript);
			runningScript.script.run();
			runningScript = null;
		}

		public void StopScript(MotinScript script,bool raiseCompleted = true)
		{
			if(script==null)
				return;

			script.ForceStop();
			//OnScriptFinished(script,raiseCompleted);
		}
		
		void Update()
		{
			foreach(RunningScript script in runningScripts)
			{
				if(script.scriptType== (int)RunningScript.ScriptTypes.MotinScript)
					script.script.OnUpdate();
			}
		}

		void FixedUpdate()
		{
			foreach(RunningScript script in runningScripts)
			{
				if(script.scriptType== (int)RunningScript.ScriptTypes.MotinScript)
					script.script.OnLateUpdate();
			}
		}

		void LateUpdate()
		{
			foreach(RunningScript script in runningScripts)
			{
				if(script.scriptType== (int)RunningScript.ScriptTypes.MotinScript)
					script.script.OnFixedUpdate();
			}
		}

		void OnScriptFinishedCallback(MotinScript script,bool raiseCompleted = true)
		{
			OnScriptFinished(script);
		}

		void OnScriptFinished(MotinScript script,bool raiseCompleted = true)
		{
			//Debug.Log("SCRIPTENGINE COMPLETE " + script.scriptData.GetType().Name );
			int index = getRunningScriptIndex(script);
			script.OnFinishedWithScript -= OnScriptFinishedCallback;

			RunningScript runningScript = 	runningScripts[index];
			runningScripts.RemoveAt(index);
			if(runningScript.OnFinished!=null & raiseCompleted)
			{
				//	Debug.Log("RAISE COMPLETE " + script.scriptData.scriptName + " " + script.scriptData.name);
				runningScript.OnFinished(script);
				
			}
			
			ReturnRunningScriptToPool(runningScript);
		}

	}
}
