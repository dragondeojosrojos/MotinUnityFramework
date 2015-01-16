using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MotinTakeData /*: MotinSerializableData */{
	
	/*
	new	public static string xmlName = "MotinTakeData";
	public override string typeName()
	{
		return MotinTakeData.xmlName;
	}
	
	*/
	
	public string 			name;					// take name
	public int 				frameRate = 24;				// frames per second
	public int 				numFrames = 1440;			// number of frames
	public float 			startFrame = 1f;				// first frame to render
	public float 			endFrame = 100f;
	public int 				playbackSpeedIndex = 2;		// default playback speed x1
	
	//public int[] trackKeys = null;
	public MotinTrackData[] trackValues = null;
		
	//public MotinGroupData rootGroup = null;
	
	//public int[] groupKeys = null;
	public MotinGroupData[] groupValues = null;
	
	
	/*
	public MotinTakeData( ) : base()
	{
		name = "clip";			
		frameRate = 24;	
		numFrames = 1440;
		startFrame = 1f;
		endFrame = 100f;
		playbackSpeedIndex = 2;
		trackValues = null;
		
		groupValues = null;

	}
	public MotinTakeData( System.Xml.XmlElement element) : base(element)
	{

	}
	//SERIALIZABLE
	public override void OnSerialize (System.Xml.XmlElement element)
	{ 
		base.OnSerialize(element);
		SetString(element,"name",name);
		SetInt(element,"frameRate",frameRate);
		SetInt(element,"numFrames",numFrames);
		SetFloat(element,"startFrame",startFrame);
		SetFloat(element,"endFrame",endFrame);
		SetInt(element,"playbackSpeedIndex",playbackSpeedIndex);
		
		//trackValues = null;

		//groupValues = null;
		
		
	}
	
	public override void OnDeserialize (System.Xml.XmlElement element)
	{
		base.OnDeserialize(element);
		name				= GetString(element,"name");
		frameRate 			= GetInt(element,"frameRate");
		numFrames 			= GetInt(element,"numFrames");
		startFrame 			= GetFloat(element,"startFrame");
		endFrame 			= GetFloat(element,"endFrame");
		playbackSpeedIndex 	= GetInt(element,"playbackSpeedIndex");
		
	
	}
	
	public override void OnDeserializeChildElement(MotinSerializableData data)
	{
	//	Debug.Log("ON DESERIALIZE CHILD "+  data.name + " IN " + this.name );
		//MotinScriptData childData = (MotinScriptData)data;
		//if(data==null)
		//	Debug.LogError("Data is null");
		
		//childScripts.Add(childData);
	
	}
	
	*/
	
	
	
	public void Save(AMTake take)
	{
		name = take.name;
		frameRate = take.frameRate;
		numFrames = take.numFrames;
		startFrame = take.startFrame;
		endFrame = take.endFrame;
		playbackSpeedIndex = take.playbackSpeedIndex;
		//trackKeys = take.trackKeys.ToArray();
		
		trackValues = null;
		List<MotinTrackData> trackList = new List<MotinTrackData>();
		foreach(AMTrack track in take.trackValues)
		{
			MotinTrackData trackData = new MotinTrackData();
			trackData.Save(track);
			trackList.Add(trackData);
		}
		trackValues = trackList.ToArray();
		
		
		//MotinGroupData groupData = new MotinGroupData();
		//groupData.Save(take.rootGroup);
		//rootGroup = groupData;
		
		//groupKeys = take.groupKeys.ToArray();
		
		
		groupValues = null;
		List<MotinGroupData> groupList = new List<MotinGroupData>();
		foreach(AMGroup grp in take.groupValues)
		{
			MotinGroupData grpData = new MotinGroupData();
			grpData.Save(grp);
			groupList.Add(grpData);
		}
		groupValues = groupList.ToArray();
		
		
	}
	
}
