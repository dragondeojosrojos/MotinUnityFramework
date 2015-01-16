using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class MotinTrackData : MotinPropertyData  {
	public enum TrackTypes
	{
		PROPERTY_TRACK = 0,
		EVENT_TRACK = 1,
		AUDIO_TRACK = 2
	}
	
	public MotinKeyData[] keys= null;
	//public MotinActionData[] cache = null;	// action cache
	public int groupId = 0;

	
	public void Save(AMTrack track)
	{
		//Debug.Log("Save MotinTrack");
		add("id",track.id);
		add("name",track.name);
		
		if(track.GetType()==typeof(AMPropertyTrack))
		{
			Save((AMPropertyTrack)track);
		}
		else if(track.GetType()==typeof(AMEventTrack))
		{
			Save((AMEventTrack)track);
		}
		
		
//		Serialize();
	}
	
	public void Save(AMPropertyTrack track)
	{
		//Debug.Log("Save AMPropertyTrack");
		add("trackType",(int)TrackTypes.PROPERTY_TRACK);
		add("valueType",track.valueType);
		add("obj",track.obj);
		add("component",track.component);
		add("propertyName",track.propertyName);
		add("fieldName",track.fieldName);
		add("methodName",track.methodName);
		
		List<MotinKeyData> listKeys = new List<MotinKeyData>();
		foreach(AMPropertyKey key in track.keys)
		{
			MotinKeyData newKey = new MotinKeyData();
			newKey.Save((AMPropertyTrack.ValueType)track.valueType,key);
			listKeys.Add(newKey);
		}
		keys = listKeys.ToArray();
		
	}
	public void Save(AMEventTrack track)
	{
//		Debug.Log("Save envent TRACK");
		add("trackType",(int)TrackTypes.EVENT_TRACK);
		add("obj",track.obj);
		
		List<MotinKeyData> listKeys = new List<MotinKeyData>();
		foreach(AMEventKey key in track.keys)
		{
			MotinKeyData newKey = new MotinKeyData();
			newKey.Save(key);
			listKeys.Add(newKey);
		}
		keys = listKeys.ToArray();
	}
	public void Save(AMAudioTrack track)
	{
		add("trackType",(int)TrackTypes.AUDIO_TRACK);
	}
}
