using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotinGames
{

	public class BaseDataManager<K> : MonoBehaviour where K : MotinData  {

		
		protected List<K> dataList = new List<K>();
		protected List<string> namesListlookup = new List<string>(); 
		protected string currentFilePath ="";
		protected virtual void Awake()
		{

		}
		protected void Initialize(string filePath)
		{
			currentFilePath = filePath;
			dataList = new List<K>( MotinDataManager.Load<K>(currentFilePath));

			namesListlookup = new List<string>();
			
			for(int i = 0; i < dataList.Count;i++)
			{
				namesListlookup.Add( dataList[i].name);
			}
		}

		public void Save()
		{
			MotinDataManager.Save(currentFilePath,dataList.ToArray());
		}

		public void AddData(K data,bool replaceById = true)
		{
			int dataIndex = -1;

			if(replaceById)
				dataIndex = GetIndexById(data.intUniqueId);

			if(dataIndex>=0)
				dataList[dataIndex] = data;
			else
				dataList.Add(data);
		}

		public int GetIndexById(int dataId)
		{
			for(int i = 0 ; i < dataList.Count;i++)
			{
				if(dataList[i].intUniqueId == dataId)
					return i;
			}
			
			return -1;
		}
		public K GetById(int dataId)
		{
			foreach(K data in dataList)
			{
				if(data.intUniqueId == dataId)
					return data;
			}
			
			return null;
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
		public int GetIdByName(string dataName)
		{
			foreach(K data in dataList)
			{
				if(data.name == dataName)
					return data.intUniqueId;
			}
			
			return -1;
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
