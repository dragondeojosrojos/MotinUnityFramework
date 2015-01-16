using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

	public class BaseDataManager<K> : MonoBehaviour where K : MotinData  {

		
		protected List<K> dataList = new List<K>();
		protected List<string> namesListlookup = new List<string>(); 
		protected virtual void Awake()
		{

		}
		protected void Initialize(string filePath)
		{
			dataList = new List<K>( MotinDataManager.Load<K>(filePath));

			namesListlookup = new List<string>();
			
			for(int i = 0; i < dataList.Count;i++)
			{
				namesListlookup.Add( dataList[i].name);
			}
		}
		
		public K GetByName(string dataName)
		{
				foreach(K data in dataList)
			{
				if(data.name == dataName)
					return data;
			}
			
			return null;
		}

		public K[] GetDataArray()
		{
			return dataList.ToArray();
		}

		public string[] GetNamesArray()
		{
			return namesListlookup.ToArray();
		}
		public List<string> GetNamesList()
		{

			return new List<string>(namesListlookup) ;
		}
	}

}
