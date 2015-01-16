using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MotinInputManager : MonoBehaviour  {
	
	
	public float dragTolerance = 1;
	public List<MotinTouch> touchList = new List<MotinTouch>();
	
	public delegate void InputDelegate(MotinTouch motinTouch);
	
	public InputDelegate	OnTouchBegan=null;
	public InputDelegate	OnTouchMoved=null;
	public InputDelegate	OnTouchStationary=null;
	public InputDelegate	OnTouchEnded=null;
	public InputDelegate	OnTouchCanceled=null;

	bool isQuitting = false;

	public void raiseInputEvent(InputDelegate callback,MotinTouch motinTouch)
	{
		if(callback!=null)
			callback(motinTouch);
	}
	
	//Singleton
	private Vector3 tmpVector3;


	private static MotinInputManager m_instance;
	void Awake()
	{

		if(m_instance==null)
		{
//			Debug.Log("INPUT MANAGER INIT");
			m_instance = this;
			if (Application.isPlaying) {
				DontDestroyOnLoad( gameObject );
			}
		}
		else
		{
			if(m_instance!=this)
			{
				Debug.Log("INPUT Manager already exists Destroy");
				Destroy(this.gameObject);
				return;
			}
		}
		isQuitting = false;
		#if UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN  || UNITY_EDITOR 
			InitPc();
		#endif
		

	} 
	
	public static MotinInputManager sharedManager ()
	{ 
		return m_instance;   
	}
	/////////////////////////////////////////////////
	// Update is called once per frame
	public int touchCount
	{
		get{
			return touchList.Count;
		}
	}
	public Vector3 mousePosition3
	{
		get
		{
			Vector2 tmpVector = mousePosition; 
			return new Vector3(tmpVector.x,tmpVector.y,0);
		}
	}
	public Vector2 mousePosition
	{
		get{
		
			if(touchCount>0)
			{
				return GetTouch(0).position;
			}
			else
			{
				return Vector2.zero;
			}
		}
	}
	public Vector2 mousePositionFlipped
	{
		get{
		
			if(touchCount>0)
			{
				return GetTouch(0).positionFlipped;
			}
			else
			{
				return Vector2.zero;
			}
		}
	}

	void OnEnable()
	{}
	void OnDisable()
	{
		if(isQuitting)
			m_instance = null;
	}

	public void Update () {
		#if (UNITY_IPHONE || UNITY_ANDROID ) && ! UNITY_EDITOR 
			UpdateIphone();
		#else
			UpdatePc();
		#endif

	}
	#if  UNITY_WEBPLAYER || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR  
	public void InitPc()
	{
		MotinTouch touch = new MotinTouch();
		touch.fingerId =0;
			touchList.Add(touch);
	}
	public void UpdatePc()
	{
		//Debug.Log("MPUSE pos " + Input.mousePosition.ToString());
		//touchList[0].position =new Vector3 (Input.mousePosition.x,Screen.height-Input.mousePosition.y,0);
		touchList[0].position3 =Input.mousePosition;
		
		if(Input.GetMouseButtonDown(0))
		{
			touchList[0].phase = MotinTouchPhase.Began;
			touchList[0].deltaPos = Vector2.zero;
			touchList[0].deltaTime = Time.deltaTime;
			raiseInputEvent(OnTouchBegan,touchList[0]);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			touchList[0].phase = MotinTouchPhase.Ended;
			touchList[0].deltaTime = Time.deltaTime;
			raiseInputEvent(OnTouchEnded,touchList[0]);
		}
		else
		{
			if(touchList[0].phase == MotinTouchPhase.Began)
			{
				touchList[0].phase =MotinTouchPhase.Stationary;
				touchList[0].deltaTime = Time.deltaTime;
				raiseInputEvent(OnTouchStationary,touchList[0]);
			}
			else if(touchList[0].phase == MotinTouchPhase.Stationary)
			{
				if(Mathf.Abs (touchList[0].deltaPos.x)>dragTolerance ||Mathf.Abs (touchList[0].deltaPos.y)>dragTolerance )
				{
					touchList[0].phase = MotinTouchPhase.Moved;
					touchList[0].deltaTime = Time.deltaTime;
					raiseInputEvent(OnTouchMoved,touchList[0]);
				}
				
			}
			else if(touchList[0].phase == MotinTouchPhase.Moved)
			{
				if(Mathf.Abs (touchList[0].deltaPos.x)<dragTolerance && Mathf.Abs (touchList[0].deltaPos.y)<dragTolerance )
				{
					touchList[0].phase = MotinTouchPhase.Stationary;
					touchList[0].deltaTime = Time.deltaTime;
					raiseInputEvent(OnTouchStationary,touchList[0]);
				}
			}
			else if(touchList[0].phase == MotinTouchPhase.Ended)
			{
				touchList[0].phase = MotinTouchPhase.Canceled;
				touchList[0].deltaTime = Time.deltaTime;
				raiseInputEvent(OnTouchCanceled,touchList[0]);
			}
		}
	}
	#elif UNITY_IPHONE || UNITY_ANDROID
	public void UpdateIphone()
	{
		int fingerCount = 0;
		bool hasTouches = false;
        foreach (Touch touch in Input.touches) 
		{
			hasTouches = true;
			if(fingerCount < touchList.Count)
			{
				touchList[fingerCount].Set(touch);
			}
			else if(fingerCount >= touchList.Count)
			{
				MotinTouch motinTouch = new MotinTouch(touch);
				touchList.Add(motinTouch);
				raiseInputEvent(OnTouchBegan,motinTouch);
			}
          
			fingerCount++;
        }
		if(!hasTouches)
		{
			touchList.Clear();
			return;
		}
		if(fingerCount < touchList.Count)
		{
			//int i = touchList.Count-1;
			for(int i= touchList.Count-1; i >= fingerCount;i--)
			{
				touchList.RemoveAt(i);
			}
		}
	}
	#endif
	public MotinTouch GetTouch(int touchIndex)
	{
		if(touchIndex >= touchList.Count)
			return null;
		
		return touchList[touchIndex];
	}
	public bool GetTouchDown(int touchIndex)
	{
		if(touchIndex >= touchList.Count)
			return false;
		
		if(touchList[touchIndex].phase == MotinTouchPhase.Began)
		{
			return true;
		}
		return false;
	}	
	public bool GetTouchUp(int touchIndex)
	{
		if(touchIndex >= touchList.Count)
			return false;
		
		if(touchList[touchIndex].phase == MotinTouchPhase.Ended)
		{
			return true;
		}
		return false;
	}	
	public bool GetTouchMoved(int touchIndex)
	{
		if(touchIndex >= touchList.Count)
			return false;
		
		if(touchList[touchIndex].phase == MotinTouchPhase.Moved)
		{
			return true;
		}
		return false;
	}	

	void OnApplicationQuit() {
		isQuitting = true;
		//Debug.Log("QUIT INPUT MANAGER");
	}
}
