using UnityEngine;
using System.Collections;

public class MotinGizmoCube : MonoBehaviour {

	public float 	scaleFactor = 1f;
	public Vector3 size = Vector3.one;
	public Color	color = Color.white;
	void OnDrawGizmos()
	{
		Gizmos.color =color;
		Gizmos.DrawCube(transform.position,size*scaleFactor);
		Gizmos.color = Color.white;
	}
}
