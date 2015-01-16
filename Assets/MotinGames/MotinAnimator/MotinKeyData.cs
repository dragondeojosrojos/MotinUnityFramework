using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class MotinKeyData : MotinPropertyData  {

	
	public float[] customEase = null;
	public float[] morph = null;
	
	public void Save(AMPropertyTrack.ValueType valueType, AMPropertyKey key)
	{
		add("frame",key.frame);
		add("easeType",key.easeType);
		
		if(key.customEase!=null)
		{
			customEase = key.customEase.ToArray();
		}
		else
		{
			customEase = null;
		}
		
		switch(valueType)
		{
			case AMPropertyTrack.ValueType.Integer:
			case AMPropertyTrack.ValueType.Float:
			case AMPropertyTrack.ValueType.Double:
			case AMPropertyTrack.ValueType.Long:
				add("value",key.val);
			break;
			case AMPropertyTrack.ValueType.Color:
				add("value",key.color);
			break;
			case AMPropertyTrack.ValueType.Rect:
				add("value",key.rect);
			break;
			case AMPropertyTrack.ValueType.Vector2:
				add("value",key.vect2);
			break;
			case AMPropertyTrack.ValueType.Vector3:
				add("value",key.vect3);
			break;
		}
		
		if(key.morph!=null)
		{
			morph = key.morph.ToArray();
		}
		else
		{
			morph = null;
		}

	}
	
	public void Save(AMEventKey key)
	{
		add("frame",key.frame);
		add("easeType",key.easeType);
		add("component",key.component);
		add("methodName",key.methodName);
		add("useSendMessage",key.useSendMessage);
		
		
		List<MotinValue> parameterList = new List<MotinValue>();
		foreach(AMEventParameter parameter in key.parameters)
		{
			MotinValue motinValue = new MotinValue();
			motinValue.setValueType(parameter.getParamType());
			motinValue.setInt(parameter.val_int);
			motinValue.setFloat(parameter.val_float);
			motinValue.setBool(parameter.val_bool);
			motinValue.setRect(parameter.val_rect);
			motinValue.setColor(parameter.val_color);
			motinValue.setObject(parameter.val_obj);
			motinValue.setString(parameter.val_string);
			motinValue.setVector2(parameter.val_vect2);
			motinValue.setVector3(parameter.val_vect3);
			motinValue.setVector4(parameter.val_vect4);
			
			parameterList.Add (motinValue);
		}
		
		add("parameters",parameterList);
	}
}
