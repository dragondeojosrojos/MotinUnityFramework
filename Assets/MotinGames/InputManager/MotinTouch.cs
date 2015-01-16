using UnityEngine;
using System.Collections;

public enum MotinTouchPhase
{
	Began,
	Moved,
	Stationary,
	Ended,
	Canceled
}
public class MotinTouch {
	public int fingerId =-1;
	public int iphoneFingerId = -1;
	public MotinTouchPhase phase = MotinTouchPhase.Began;
 	
	public float deltaTime;
	public Vector2 deltaPos = Vector2.zero;
 	public int tapCount =1;
 	
	Vector2 m_position = Vector2.zero;
	public Vector3 position3
	{
		get
		{
			return new Vector3(m_position.x,m_position.y,0);
		}
		set
		{
			position = value;
		}
	}
	public Vector2 position
	{
		get
		{
			return m_position;
		}
		set
		{
			deltaPos = value-m_position;
			m_position = value;
			
		}
	}
	
	//Vector2 m_positionFlipped = Vector2.zero;
	public Vector2 positionFlipped
	{
		get
		{
			return new Vector2(position.x,Screen.height-position.y);
		}
	}
 	public MotinTouch()
 	{
 		
 	}
 	public MotinTouch(Touch touch)
 	{
 		Set(touch);
 	}

 	public void Set(Touch touch)
 	{
		fingerId =touch.fingerId;
 		iphoneFingerId = touch.fingerId;
 		position = touch.position;
 		tapCount = touch.tapCount;
		deltaTime	= touch.deltaTime;
		if(phase!=(MotinTouchPhase)((int)touch.phase))
		{
			phase = (MotinTouchPhase)((int)touch.phase);
			if(phase == MotinTouchPhase.Began)
			{
				MotinInputManager.sharedManager().raiseInputEvent(MotinInputManager.sharedManager().OnTouchBegan,this);
			}
			if(phase == MotinTouchPhase.Moved)
			{
				MotinInputManager.sharedManager().raiseInputEvent(MotinInputManager.sharedManager().OnTouchMoved,this);
			}
			if(phase == MotinTouchPhase.Stationary)
			{
				MotinInputManager.sharedManager().raiseInputEvent(MotinInputManager.sharedManager().OnTouchStationary,this);
			}
			if(phase == MotinTouchPhase.Ended)
			{
				MotinInputManager.sharedManager().raiseInputEvent(MotinInputManager.sharedManager().OnTouchEnded,this);
			}
			if(phase == MotinTouchPhase.Canceled)
			{
				MotinInputManager.sharedManager().raiseInputEvent(MotinInputManager.sharedManager().OnTouchCanceled,this);
			}
		}
		
 	}
 	
}
