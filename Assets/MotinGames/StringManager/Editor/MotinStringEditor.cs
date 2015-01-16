
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

public class MotinStringEditor : MotinEditor  {
	
	
	//string  selectedString = null;
	
	
	string[] stringNames = null;
	
	int selectedIndex=0;
	protected override void targetUpdated ( )
	{
		base.targetUpdated();
		//selectedString = (string)target;
		
		updateSelected((string)target);
		
		Repaint();
	}
	public MotinStringEditor(EditorWindow host):base(host)
	{
		
		List<string> names = new List<string>(MotinUtils.EnumNames<MotinStrings>() );
		names.Add("EMPTY");
		
		stringNames = names.ToArray();
		
	}
	
	//Rect viewRect;
	protected override void DoDraw ()
	{
//		Debug.Log ("DRAW LEVEL DATA");
		if(target ==null)
			return;
		
		int index =  EditorGUILayout.Popup(selectedIndex,stringNames);
		
			if(index != selectedIndex)
			{
				selectedIndex = index;
				UpdateTarget();
				RaiseOnDataChanged();
			}
	
	
	}
	
	protected void UpdateTarget()
	{
		if(selectedIndex==stringNames.Length-1)
		{
			target= "";
		}
		else
		{
			target = stringNames[selectedIndex];
		}
	}
	protected void updateSelected(string value)
	{
		if(value !=null && value.Length>0)
		{ 
			for(int i = 0; i < stringNames.Length;i++)
			{
				if(value== stringNames[i])
				{
					selectedIndex =i;
					UpdateTarget();
					return;
				}
			}
			
		}
	
		selectedIndex = stringNames.Length-1;
		UpdateTarget();
	} 
	
}
}	
