using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MotinAnimatorData  {

	public MotinTakeData[]		takes = null;
	public string 				playOnStart = null;

	public void Save(AnimatorData animatorData)
	{
		//Debug.Log("MotinAnimatorData Save");
		if(takes!=null)
		{
			takes = null;
		}
		List<MotinTakeData> takesList = new List<MotinTakeData>();
		//if(animatorData.takes!=null)
			foreach(AMTake take in animatorData.takes)
			{
				//Debug.Log("save take "+ take.name);
				MotinTakeData takeData = new MotinTakeData();
				takeData.Save(take);
				takesList.Add(takeData);
				
				if(animatorData.playOnStart==take)
				{
					playOnStart = takeData.name;
				}
				
			}
		takes = takesList.ToArray();
	}
}
