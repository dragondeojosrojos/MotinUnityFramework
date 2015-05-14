using UnityEngine;
using System.Collections;


namespace MotinGames
{

	[RequireComponent(typeof(MotinAnimator))]
	[ExecuteInEditMode]
	public class MotinWindow : MonoBehaviour {

		public enum WindowStates
		{
			Opened = 0,
			Closed = 1,
			Disabled = 2,
			Count = 3
		}
		public enum WindowAnchors
		{
			lower_left,
			lower_center,
			lower_right,
			middle_center,
			top_left,
			top_center,
			top_right
		}

		public System.Action<int> 	OnWindowStateChanged=null;

		public void raiseOnStateChanged(int state)
		{
			windowState_ = state;
			if(OnWindowStateChanged!=null)
				OnWindowStateChanged(state);
		}
		//public PlayMakerFSM		windowFsm=null;
		public bool 			defaultShow = false;
		//public bool 			updateAnchors = true;
		//public WindowAnchors	windowAnchor_ = WindowAnchors.middle_center;
		
		
		/*[HideInInspector]*/ public string anim_show_ = "show";
		public string anim_show
		{
			get{return anim_show_;}
			set{ anim_show_=value;}
		}
		/*[HideInInspector]*/public string anim_idle_ = "idle";
		public string anim_idle
		{
			get{return anim_idle_;}
			set{ anim_idle_=value;}
		}
		
		/*[HideInInspector]*/public string anim_dismiss_ = "dismiss";
		public string anim_dismiss
		{
			get{return anim_dismiss_;}
			set{ anim_dismiss_=value;}
		}
		
		int windowState_ =(int) WindowStates.Closed;
		public int WindowState
		{
			get
			{
				return windowState_;
			}
		}
		
	
		protected MotinAnimator motinAnimator = null;
		 
		protected virtual void Awake()
			{

			}
		
		public void Start()
		{
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return;
				
			}
			#endif

			motinAnimator.PreviewValue("show",true,0);
			WindowStart();
		
		}

		protected virtual void WindowStart()
		{

		}
		
		public virtual void OnEnable()
		{
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return;
				
			}
			#endif
			motinAnimator = GetComponent<MotinAnimator>();

			ActivateChilds(false);

		}
		public virtual void OnDisable()
		{

			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return;
				
			}
			#endif
			motinAnimator.PreviewValue("show",true,0);
		}


		protected virtual void SendEvent(string eventName)
		{
			/*
			//I use this function to sync the playmaker fsm
			if(windowFsm!=null && Application.isPlaying)
			{
				windowFsm.SendEvent(eventName);
			}
			*/
		}

		protected void ActivateChilds(bool value)
		{
			for(int i = 0 ; i < transform.childCount; i ++)
			{
				transform.GetChild(i).gameObject.SetActive(value);
			}
		}
		/*
		public WindowAnchors windowAnchor
		{
			get
			{
				return windowAnchor_;
			}
			set
			{
				windowAnchor_ = value;
	
			}
		}
*/
		bool runningTransition = false;
		[HideInInspector]public System.Action currentActionDelegate = null;
		protected void RaiseActionDelegate(System.Action actionDelegate)
		{
			if(actionDelegate!=null)
			{
				actionDelegate();
			}
		}
		protected void RaiseCurrentActionDelegate()
		{
			if(currentActionDelegate!=null)
			{
				System.Action auxDelegate = currentActionDelegate;

				runningTransition= false;
				currentActionDelegate = null;
				auxDelegate();
			}
		}
		
		public void Show(System.Action actionDelegate =null)
		{

			Show(this.transform.localPosition,actionDelegate);
		}
		public virtual void ShowInmediate(System.Action actionDelegate =null)
		{

			if(anim_idle.Length==0 || runningTransition)
			{
				RaiseActionDelegate(actionDelegate);
				return;
			}
			
			if(currentActionDelegate != null)
			{
				Debug.LogError("TRANSITION DIDNT FINISH " + this.name);
				RaiseActionDelegate(actionDelegate);
				return;
			}
		
			runningTransition = true;
			currentActionDelegate = actionDelegate;

			if(Application.isPlaying)
			{
				this.OnWillShow();
			}

			ActivateChilds(true);
		
			if(Application.isPlaying)
			{

				SendEvent("WINDOW_OPENED");

				motinAnimator.Play(anim_idle,true,null);
				
			}
		
			runningTransition = false;

			if(Application.isPlaying)
			{
				this.OnShowed();
			}
			RaiseCurrentActionDelegate();
			raiseOnStateChanged((int)WindowStates.Opened);

		}
	
		public virtual void Show(Vector3 position,System.Action actionDelegate =null)
		{

			transform.localPosition = position;
			if(anim_show.Length==0 || runningTransition)
			{
				RaiseActionDelegate(actionDelegate);
				return;
			}
			
			if(currentActionDelegate != null)
			{
		
				RaiseActionDelegate(actionDelegate);
				return;
			}

			runningTransition = true;
			currentActionDelegate = actionDelegate;
			

		
			ActivateChilds(true);

			if(Application.isPlaying)
			{
				this.OnWillShow();
			}

			if(Application.isPlaying)
			{
				SendEvent("WINDOW_SHOW");
				motinAnimator.Play(anim_show,ShowAnimComplete);
			}
			else
			{

				runningTransition = false;
				RaiseCurrentActionDelegate();
			}
		}
		
		public virtual void ShowAnimComplete(MotinAnimator animator,string takeName)
		{

			runningTransition = false;

			SendEvent("WINDOW_OPENED");
			motinAnimator.Play(anim_idle,true);

			if(Application.isPlaying)
			{
				this.OnShowed();
			}

			RaiseCurrentActionDelegate();
			raiseOnStateChanged((int)WindowStates.Opened);
			
		}

		public void Pause()
		{
			motinAnimator.Pause();
		}
		public void Resume()
		{
			motinAnimator.Resume();
		}
		public void Dismiss(System.Action actionDelegate =null)
		{
			if(anim_dismiss.Length==0 || runningTransition)
			{
				RaiseActionDelegate(actionDelegate);
				return;
			}
			
			if(currentActionDelegate != null)
			{
				Debug.LogError("TRANSITION DIDNT FINISH " + this.name);
				RaiseActionDelegate(currentActionDelegate);
				return;
			}
			
		
			runningTransition = true;
			currentActionDelegate = actionDelegate;

			if(Application.isPlaying)
			{
				this.OnWillDismiss();

			}
			SendEvent("WINDOW_DISMISS");
			motinAnimator.Play(anim_dismiss,DismissAnimComplete);
		}
		public virtual void DismissAnimComplete(MotinAnimator animator,string takeName)
		{
		
			runningTransition = false;
			SendEvent("WINDOW_DISMISS");
			ActivateChilds(false);

			if(Application.isPlaying)
			{
				this.OnDismissed();
			}

			RaiseCurrentActionDelegate();
			raiseOnStateChanged((int)WindowStates.Closed);

			
		}
		
		public virtual void DismissInmediate(System.Action actionDelegate =null)
		{
			if(Application.isPlaying)
			{
				OnWillDismiss();
			}
			SendEvent("START");

			if(motinAnimator!=null)
				motinAnimator.PreviewValue("show",true,0);

			ActivateChilds(false);

			if(Application.isPlaying)
			{
				this.OnDismissed();
			}
			raiseOnStateChanged((int)WindowStates.Closed);
			if(actionDelegate!=null)
				actionDelegate();
			
		}
		
		protected virtual void OnDrawGizmos()
		{
			
		}
		
		protected void Update()
		{
			if(windowState_ == (int)WindowStates.Opened)
				WindowOpenUpdate();
		}
		protected void FixedUpdate()
		{
			if(windowState_ == (int)WindowStates.Opened)
					WindowOpenFixedUpdate();
		}
		protected void LateUpdate()
		{
			if(windowState_ == (int)WindowStates.Opened)
				WindowOpenLateUpdate();
		}

		protected virtual void WindowOpenUpdate()
		{

		}
		protected virtual void WindowOpenFixedUpdate()
		{
				
		}
		protected virtual void WindowOpenLateUpdate()
		{
				
		}
	
		protected virtual void OnWillDismiss(){}
		protected virtual void OnDismissed(){}
		protected virtual void OnWillShow(){}
		protected virtual void OnShowed(){}
	}
}
