using UnityEngine;
using System.Collections;
using Pathfinding.Serialization.JsonFx;
namespace MotinGames
{

	[System.Serializable]
	public class MotinData  {

		public string name = "";

		[MotinEditorReadonlyField]
		public string uniqueId ="";

		public MotinData()
		{
			SetDefaultValues();
		}

		public MotinData(string json)
		{
			SetJson(json);
		}


		public virtual void SetDefaultValues()
		{
			uniqueId = MotinUtils.GetUniqueString();
		}

		public virtual void OnFinishedDeserializing()
		{
			if(string.IsNullOrEmpty(uniqueId))
				uniqueId = MotinUtils.GetUniqueString();
		}
		public virtual void OnWillSerialize()
		{

		}

		public void CopyFromData(MotinData sourceData)
		{
			if(sourceData == null)
				return;
			
			System.Type type = this.GetType();      
			System.Reflection.FieldInfo[] fields = type.GetFields();
			System.Reflection.FieldInfo field  = null;
			
			System.Type sourceType = sourceData.GetType();      
			System.Reflection.FieldInfo sourceField = null; 
			
			
			for(int i =fields.Length-1;  i>=0 ;i--)
			{
				field = fields[i];
				if(field.IsPublic && !field.IsStatic )
				{
					sourceField = sourceType.GetField(field.Name);
					if(sourceField!=null)
					{

						field.SetValue(this,sourceField.GetValue(sourceData));
					}
					
				}         
			}
		}

		public string GetJson()
		{
			return JsonWriter.Serialize(this);;
		}
		public void SetJson(string json)
		{
			object result = JsonReader.Deserialize(json,this.GetType());

			//MotinData data = JsonReader.Deserialize<(json);
			CopyFromData((MotinData)result);
		}
	}
}
