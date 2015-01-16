using UnityEngine;
using System.Collections;

[System.Serializable]
public class MotinGroupData  {

	public string group_name;
	public int group_id;
	
	public void Save(AMGroup grp)
	{
		group_name = grp.group_name;
		group_id =  grp.group_id;
	}
}
