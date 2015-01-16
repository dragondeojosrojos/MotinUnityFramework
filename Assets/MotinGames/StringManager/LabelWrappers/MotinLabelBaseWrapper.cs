using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MotinLabelBaseWrapper : MonoBehaviour {

	public int maxChars = 10;

	public virtual string text
	{
		get;
		set;
	}

	protected virtual void Awake()
	{
		
	}
	protected virtual void OnEnable()
	{

	}
	protected virtual void OnDisable()
	{
		
	}
	protected virtual void Start()
	{
		
	}
}
