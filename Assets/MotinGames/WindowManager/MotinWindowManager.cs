using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using HutongGames.PlayMaker;

namespace MotinGames
{

	[ExecuteInEditMode]
	public class MotinWindowManager : MonoBehaviour {
		
		public MotinWindow[] childWindows;

		[System.NonSerialized]
		public string definesNamesPath = "WindowsEnum.cs";

		[SerializeField]
			private string startWindowName ="";
		

		private bool isQuitting = false;
		private static MotinWindowManager sharedInstance = null;
		public static MotinWindowManager sharedManager()
		{
			return sharedInstance;
		}
		
		void Awake()
		{
			if(Application.isPlaying)
			{
				isQuitting = false;
				sharedInstance = this;
			}
		}
		void OnApplicationQuit() {
			isQuitting = true;
			//		Debug.Log("QUITTING");
		}

		void OnEnable()
		{
		//	Debug.Log("WINDOW MANAGER ON ENABLE");
			//#if UNITY_EDITOR
			//if(!Application.isPlaying)
			//{
				FindWindows();
				//return;
					
			//}
			//#endif
			EnableWindows();
		}
		void OnDisable ()
		{
			if(isQuitting)
			{
				sharedInstance = null;
				return;
			}
			/*
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return;
					
			}
			
			#endif
			*/
				
		}
		void Start()
		{
			if(Application.isPlaying && !string.IsNullOrEmpty(startWindowName))
				ShowWindow(startWindowName,true);
				//PrepareScene(startSceneIndex);
		}
		
		public T GetWindow<T>(string name) where T : MotinWindow
		{
			return (T)GetWindow(name);
		}

		public MotinWindow	GetWindow(string name)
		{
			if(childWindows==null || childWindows.Length ==0)
				return null;
			
			for(int i = 0 ; i < childWindows.Length;i++)
			{
				if(childWindows[i].name == name)
				{
					return childWindows[i];
				}
			}
			
			return null;
		}

		public void DisableWindows()
		{
			if(childWindows==null || childWindows.Length ==0)
				return;
				
			for(int i = 0 ; i < childWindows.Length;i++)
			{	
				childWindows[i].gameObject.SetActive(false);
			}
			
		}
		public void EnableWindows()
		{
			if(childWindows==null || childWindows.Length ==0)
				return;
				
			for(int i = 0 ; i < childWindows.Length;i++)
			{
				if(childWindows[i]!=null)
					childWindows[i].gameObject.SetActive(true);
				else
					Debug.LogWarning("WINDOW IS NULL");
					
			}
			
		}

		//protected bool runningAction = false;
		//System.Action currentActionDelegate = null;
		public void FindWindows()
		{
			childWindows = null;
				
			List<MotinWindow> foundWindows = new List<MotinWindow>();
			MotinWindow auxWindow;
			for(int i = 0 ; i < transform.childCount; i ++)
			{
				auxWindow = transform.GetChild(i).gameObject.GetComponent<MotinWindow>();
				if(auxWindow!=null)
				{
					foundWindows.Add(auxWindow);
				}
					
			}
				
			childWindows = foundWindows.ToArray();
				
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				UnityEditor.EditorUtility.SetDirty(this);
			}
			#endif
		}
		
			/*
		public void PrepareScene(int sceneIndex)
		{
			if(sceneIndex<0 || scenes == null || sceneIndex >= scenes.Length)
				return;
			
			if(runningAction)
				return;
			
			DisableScenes();
			
	//		Debug.Log ("RUNNIG ACTION FALSE 1");
			runningAction = false;
			
			currentActionDelegate = null;
			previusScene = null;
			
			currentScene = scenes[sceneIndex];
			currentScene.gameObject.SetActive(true);
			currentScene.PrepareScene();
			//currentScene.ShowInmediate();
		}
	*/


		public void DismissAllWindows()
		{
			foreach(MotinWindow window in childWindows)
			{
				window.DismissInmediate();
			}
		}
		public void ShowAllWindows()
		{
			foreach(MotinWindow window in childWindows)
			{
				window.ShowInmediate();
			}
		}

		/*
		public void ShowDialog(string name,MotinStrings title_string,MotinStrings message_string,System.Action OnComplete = null)
		{
			ShowDialog(name,false,false,title_string,message_string,MotinStrings.STRING_COUNT,MotinStrings.STRING_COUNT,OnComplete);
		}
		public void ShowDialog(string name,bool showOk,bool showCancel,MotinStrings title_string,MotinStrings message_string,MotinStrings okButton_string,MotinStrings cancelButton_string,System.Action OnComplete = null)
		{
			ShowDialog( name, showOk, showCancel, title_string, message_string.ToString(), okButton_string, cancelButton_string, OnComplete );
			
		}
		public void ShowDialog(string name,bool showOk,bool showCancel,MotinStrings title_string,string message_string,MotinStrings okButton_string,MotinStrings cancelButton_string,System.Action OnComplete = null)
		{
			MotinWindow window =  GetWindow(name);
			if(window==null)
			{
				if(OnComplete!=null)
					OnComplete();
				return;
			}

			MotinDialogWindow dialog =(MotinDialogWindow) window;

			dialog.Show(showOk,showCancel,title_string,message_string,okButton_string,cancelButton_string,OnComplete);
		}
*/
		public void ShowWindow(string name,bool showInmediate,System.Action OnComplete = null)
		{
			MotinWindow sceneWindow =  GetWindow(name);
			if(sceneWindow==null)
			{
				if(OnComplete!=null)
					OnComplete();
				return;
			}
			if(showInmediate)
				sceneWindow.ShowInmediate(OnComplete);
			else
				sceneWindow.Show(OnComplete);
		}

		public void PauseWindow(string name)
		{
			MotinWindow sceneWindow =  GetWindow(name);
			if(sceneWindow==null)
			{

				return;
			}

			sceneWindow.Pause();
		}
		public void ResumeWindow(string name)
		{
			MotinWindow sceneWindow =  GetWindow(name);
			if(sceneWindow==null)
			{
				return;
			}
			sceneWindow.Resume();
		}
		public void DismissWindow(string name,System.Action OnComplete = null)
		{
			MotinWindow sceneWindow =  GetWindow(name);
			if(sceneWindow==null)
			{
				if(OnComplete!=null)
					OnComplete();
				return;
			}
			sceneWindow.Dismiss(OnComplete);
		}
		public void DismissWindowInmediate(string name)
		{
			MotinWindow sceneWindow =  GetWindow(name);
			if(sceneWindow==null)
			{
				return;
			}
			sceneWindow.DismissInmediate();
		}
		/*
		public void ShowScene(string name,string defaultWindow,System.Action OnComplete = null)
		{
			//if(currentScene!=null && currentScene.name!=name)
				ShowScene(GetSceneIndex(name),defaultWindow,OnComplete);
		}
		
		public void ShowScene(string name,System.Action OnComplete = null)
		{
			//if(currentScene!=null && currentScene.name!=name)
				ShowScene(GetSceneIndex(name),null,OnComplete);
		}
		*/
		//string defaultWindow_ = null;
			/*
		public void ShowScene(int index,string defaultWindow,System.Action OnComplete = null)
		{

			if(index<0 || scenes == null || index >= scenes.Length)
				return;
			
			if(runningAction)
				return;
			
			if(currentActionDelegate!=null)
			{
				Debug.LogError("Transitions NOT FINISHED " + this.name);
				currentActionDelegate();
				return;
			}
			
			defaultWindow_ = defaultWindow;
	//		Debug.Log ("RUNNIG ACTION TRUE 1");
			runningAction = true;
			currentActionDelegate = OnComplete;
			
			
			previusScene = currentScene;
			currentScene = scenes[index];
			
			if(previusScene!=null)
			{
				//previusScene.gameObject.SetActive(false);
				if(Application.isPlaying)
					previusScene.Dismiss(previusSceneDismissed);
				else
					previusScene.DismissInmediate(previusSceneDismissed);

			}
			else
			{

				if(currentScene!=null)
				{
					
					currentScene.gameObject.SetActive(true);
					if(defaultWindow_!=null && defaultWindow_.Length>0)
					{
						currentScene.ShowWindow(defaultWindow_,true);
					}
					
					//if(uiManager==null)
					//	uiManager = tk2dUIManager.Instance
					//tk2dUIManager.Instance.UICamera = currentScene.cameras2d[0].GetComponent<Camera>();
					//if(Application.isPlaying)
					//	currentScene.Show (showAnimComplete);
					//else
						currentScene.ShowInmediate(showAnimComplete);
				}
			}
		}
		public void previusSceneDismissed()
		{
			if(currentScene!=null)
			{
				//previusScene.DismissInmediateAll();
				previusScene.gameObject.SetActive(false);
				currentScene.gameObject.SetActive(true);
				
				
				
				if(defaultWindow_!=null && defaultWindow_.Length>0)
						currentScene.ShowWindow(defaultWindow_,true);
				
				//tk2dUIManager.Instance.UICamera = currentScene.cameras2d[0].GetComponent<Camera>();
				
				if(Application.isPlaying)
					currentScene.Show (showAnimComplete);
				else
					currentScene.ShowInmediate(showAnimComplete);
			}
		}
		*/
			/*
		public void showAnimComplete()
		{
			runningAction = false;
			PlayMakerFSM.BroadcastEvent("SCENE_CHANGED");
			RaiseCurrentActionDelegate();
		}
		protected void RaiseCurrentActionDelegate()
		{
			if(currentActionDelegate!=null)
			{
				System.Action auxDelegate = currentActionDelegate;
	//			Debug.Log ("RUNNIG ACTION FALSE 2");
				runningAction= false;
				currentActionDelegate = null;
				auxDelegate();
				
			}
		}
	*/
		
		/*
		public void ShowWindow(string name,MotinWindow.MotinWindowStateDelegate delegate)
		{
			MotinWindow window = GetWindow(name);
			
			
			if(window==null)
				return;
			
			
			window.OnWindowStateChanged
			
		}
		 */
	}

}