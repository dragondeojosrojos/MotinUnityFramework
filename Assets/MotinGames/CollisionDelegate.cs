using UnityEngine;
using System.Collections;

public class CollisionDelegate : MonoBehaviour {
	

	public System.Action<Collision> OnCollisionEnterEvent;
	public System.Action<Collision> OnCollisionStayEvent;
	public System.Action<Collision> OnCollisionExitEvent;
	
	public System.Action<Collider> OnTriggerEnterEvent;
	public System.Action<Collider> OnTriggerStayEvent;
	public System.Action<Collider> OnTriggerExitEvent;

	public bool followParent = true;

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("Collision ENTER "+ collisionInfo.gameObject.name);
		if(OnCollisionEnterEvent!=null)
			OnCollisionEnterEvent(collisionInfo);
	}
	void OnCollisionStay(Collision collisionInfo)
	{
		//Debug.Log ("Collision STAY"+ collisionInfo.gameObject.tag);
		if(OnCollisionStayEvent!=null)
			OnCollisionStayEvent(collisionInfo);
	}
	void OnCollisionExit(Collision collisionInfo)
	{
		//Debug.Log ("Collision EXIT"+ collisionInfo.gameObject.tag);
		if(OnCollisionExitEvent!=null)
			OnCollisionExitEvent(collisionInfo);
	}
	
	void OnTriggerEnter(Collider hitCollider)
	{
		//Debug.Log ("TRIGGER ENTER"+ hitCollider.gameObject.tag);
		if(OnTriggerEnterEvent!=null)
			OnTriggerEnterEvent(hitCollider);
	}
	void OnTriggerStay(Collider hitCollider)
	{
		//Debug.Log ("TRIGGER STAY"+ hitCollider.gameObject.tag);
		if(OnTriggerStayEvent!=null)
			OnTriggerStayEvent(hitCollider);
	}
	void OnTriggerExit(Collider hitCollider)
	{
		//Debug.Log ("TRIGGER EXIT"+ hitCollider.gameObject.tag);
		if(OnTriggerExitEvent!=null)
			OnTriggerExitEvent(hitCollider);
	}

	void FixedUpdate()
	{
		if(followParent)
			transform.position = transform.parent.position;
	}
	
}
