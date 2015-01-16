using UnityEngine;
using UnityEditor;
using System.Collections;
namespace MotinGames

{
public class MotinDataEditor : MotinEditor  {
	
	
	protected MotinData motinData_ = null;
	protected MotinData motinData
	{
		get {return motinData_;}
		set{
			motinData_ = value;
		}
	}
	
	protected override void targetUpdated()
	{
		base.targetUpdated();
		
		motinData = (MotinData)target;
		editorName = "["+motinData.GetType().ToString() + "] "+ motinData.name;
		
	}
	public MotinDataEditor():base()
	{
		
	}
	public MotinDataEditor(EditorWindow host): base(host)
	{

	}
	
	// Events
	public System.Action<MotinEditor> 	OnDataNameChanged;
		
	void RaiseNameChanged() { if (OnDataNameChanged != null) OnDataNameChanged(this); }
	
	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		if(field.Name == "name")
		{
			if(DrawDefaultField(value,field))
			{
				RaiseNameChanged();
				//return true;
			}
			return true;
		}
		return base.DrawField (value, field);
	}
}
}
