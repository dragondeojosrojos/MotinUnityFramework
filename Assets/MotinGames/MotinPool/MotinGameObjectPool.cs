using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MotinGames
{

[System.Serializable]
public class MotinGameObjectPoolData : MotinData
{
	public GameObject prefab;
	
	public int 		defaultPoolCount=10;
	
	//public bool combineMeshes = false;
	
	public Vector3 	defaultStoragePosition = new Vector3(10000,10000,0);
	
	public bool 	keepActive = false;
	
	public enum SpawnBehaviour
	{
		AddNew,
		Reutilize,
		Nothing
	}
	
	public MotinGameObjectPoolData.SpawnBehaviour spawnBehaviour = MotinGameObjectPoolData.SpawnBehaviour.AddNew;
	
	/*
	public void CopyFrom(MotinGameObjectPoolData source)
	{
		name = source.name;
		prefab = source.prefab;
		//spriteCollection = source.spriteCollection;
		defaultPoolCount = source.defaultPoolCount;
		defaultStoragePosition = source.defaultStoragePosition;
		spawnBehaviour = source.spawnBehaviour;
		keepActive	 = source.keepActive;
	}
*/

	public void Clear()
	{
		name = "";
		prefab = null;
		defaultPoolCount = 10;
		defaultStoragePosition =new  Vector3(10000,10000,0);
		spawnBehaviour = MotinGameObjectPoolData.SpawnBehaviour.AddNew;
		keepActive = false;
	}

	public bool Empty
	{
		get { return name.Length == 0 ||  prefab == null; }
	}
	
	
}


public class MotinGameObjectPool {
	

	MotinGameObjectPoolData poolData = null;
	
	//vars
	
	
	int i =0;//aux variable
	GameObject tmpGo = null;//aux variable

	
	Transform parentTransform=null;

	public List<GameObject> poolObjects = null;
	List<GameObject> spawnedObjects = null;
	List<GameObject> storedObjects = null;
	
	bool hasMotinObject = false;
	
	public MotinGameObjectPool(MotinGameObjectPoolData data)
	{
		poolData = data;
	}
	//PROPERTIES
	public string name
	{
		get
		{
			return poolData.name;
		}
	}

	public GameObject prefab
	{
		get{return poolData.prefab;}
	}

	bool isInitialized_=false;
	public bool isInitialized
	{
		get
		{
			return isInitialized_;
		}
	}
		
		
	public int spawnedCount
	{
		get
		{
			return spawnedObjects.Count;
		}
	}
	public int storedObjectCount
	{
		get
		{
			return storedObjects.Count;
		}
	}
	public GameObject[] spawnedObjectsArray
	{
		get{
			return spawnedObjects.ToArray();
		}
	}
	//Events
	public delegate void OnGameobjectSpawnedEventHandler(GameObject spawnedObject);
	public OnGameobjectSpawnedEventHandler OnGameobjectSpawned;
	void RaiseOnGameobjectSpawned(GameObject spawnedObject)
	{
		if(OnGameobjectSpawned!=null)
		{
			OnGameobjectSpawned(spawnedObject);
		}
	}
	
	public delegate void OnGameobjectUnspawnedEventHandler(GameObject unspawnedObject);
	public OnGameobjectUnspawnedEventHandler OnGameobjectUnspawned;
	void RaiseOnGameobjectUnspawned(GameObject unspawnedObject)
	{
		if(OnGameobjectUnspawned!=null)
		{
			OnGameobjectUnspawned(unspawnedObject);
		}
	}
	
	public void InitializePool()
	{
		if(isInitialized_)
			return;
		
		
//		Debug.Log("Initialize pool " + poolData.name);
		poolObjects = new List<GameObject>();
		spawnedObjects = new List<GameObject>();
		storedObjects = new List<GameObject>();
		
		tmpGo = new GameObject("Pool_"+ name );
		tmpGo.transform.position = Vector3.zero;
		parentTransform = tmpGo.transform;

		if(poolData.prefab!=null)
		{
			hasMotinObject =  poolData.prefab.GetComponent<MotinBaseObject>() !=null?true:false;
		}

		//gameObjectPrefab =  (GameObject)Resources.Load(poolData.prefabPath);
	
		//keepActive = true;
			
		CreatePool();
		
		/*
		if(poolData.combineMeshes)
		{
			SetPositionAll(new Vector3(0,0,0));
			CombineSkinnedMeshes combinedMeshes  = (CombineSkinnedMeshes)tmpGo.AddComponent(typeof(CombineSkinnedMeshes));
			combinedMeshes.castShadows = false;
			combinedMeshes.receiveShadows = false;
			combinedMeshes.Combine();
			GameObject.Destroy(combinedMeshes);
			SetPositionAll(poolData.defaultStoragePosition);
		}
		*/
		
		isInitialized_ = true;
		tmpGo = null;
	}
	
	void CreatePool()
	{
		//if(!isInitialized)
		//	return;
		//Debug.Log("Creating pool " + poolData.name);
		for(i = 0 ; i < poolData.defaultPoolCount; i ++)
		{
			AddObjectToPool();
		}
	}
	public void DestroyPool()
	{
		if(!isInitialized_)
			return;
		
		spawnedObjects.Clear();
		storedObjects.Clear();
		
		for(int i = 0 ; i < poolObjects.Count; i ++)
		{
			//if(Application.isEditor)
			//{
			//	Debug.Log("MotinPool Is Editor");
			//	GameObject.DestroyImmediate(poolObjects[i]);
			//}
			//else
			{
				Debug.Log("MotinPool Is Normal");
				GameObject.Destroy(poolObjects[i]);
			}
			poolObjects[i] = null;
		}
		poolObjects.Clear();
		
		poolObjects = null;
		spawnedObjects = null;
		storedObjects = null;
		
		if(parentTransform !=null /*&& parentTransform.gameObject.renderer!=null*/)
			{
			
				//SkinnedMeshRenderer skinnedMesh =(SkinnedMeshRenderer) parentTransform.gameObject.renderer;
				
				//GameObject.DestroyImmediate( skinnedMesh.sharedMaterial);
				//.sharedMaterial=null;
				//GameObject.DestroyImmediate(skinnedMesh.sharedMesh);
				//skinnedMesh.sharedMesh = null;
				//skinnedMesh = null;
				GameObject.Destroy(parentTransform.gameObject);
				parentTransform = null;
			}
		
		isInitialized_ = false;
	}
	
	void AddObjectToPool( )
	{
		if(poolData.prefab==null)
		{
			Debug.Log("Prefab is Null " + poolData.name);
			return;
		}
		
		
		//Debug.Log("Add object to pool");
		tmpGo = (GameObject)GameObject.Instantiate(poolData.prefab);
		
		if(!poolData.keepActive)
			tmpGo.SetActive(false);
		
		tmpGo.name = poolData.prefab.name;
		//tmpGo.name = tmpGo.name + " "+ poolObjects.Count;
		
		if(parentTransform !=null)
			tmpGo.transform.parent = parentTransform;
		
		tmpGo.transform.position = poolData.defaultStoragePosition;
		
		//tmpGo.transform.localPosition = new Vector3(1000,0,0);
		poolObjects.Add(tmpGo);
		storedObjects.Add(tmpGo);

		if(hasMotinObject)
		{
			tmpGo.GetComponent<MotinBaseObject>().pool = this;
			tmpGo.GetComponent<MotinBaseObject>().Initialize();
		}
	}
	
	//public GameObject Spawn()
	//{
		
//	}
	public GameObject Spawn(Vector3 spawnPos)
	{
		tmpGo=null;
		tmpGo = Spawn();
		if(tmpGo !=null)
		{
			tmpGo.transform.position = spawnPos;
			return tmpGo;
		}
		return null;
	}
	
	public GameObject Spawn( )
	{
		if(storedObjects.Count==0)
		{
			if(poolData.spawnBehaviour == MotinGameObjectPoolData.SpawnBehaviour.AddNew)
			{
				Debug.LogWarning("Not Enought objects in pool for spawn , adding new "+ poolData.name + " "  + poolObjects.Count);
				AddObjectToPool();
			}
			else if(poolData.spawnBehaviour == MotinGameObjectPoolData.SpawnBehaviour.Reutilize)
			{
				Unspawn(0);
			}
			else if(poolData.spawnBehaviour == MotinGameObjectPoolData.SpawnBehaviour.Nothing)
			{
				return null;
			}
		}
		tmpGo = storedObjects[0];
		spawnedObjects.Add(tmpGo);
		storedObjects.Remove(tmpGo);
		
		

		if(!poolData.keepActive)
			tmpGo.SetActive(true);
		
		if(hasMotinObject)
				tmpGo.GetComponent<MotinBaseObject>().OnSpawned();

		RaiseOnGameobjectSpawned(tmpGo);
		return tmpGo;
	}
	
	public void UnspawnAll()
	{
		while(spawnedObjects.Count>0)
		{
			Unspawn(0);
		}
	}
	public void SetLocalPositionAll(Vector3 position)
	{
		for(int i = 0;i < poolObjects.Count;i++)
		{
			SetLocalPosition(i,position);
		}
	}
	public void SetLocalPosition(int index,Vector3 position)
	{
		if(index<0)
		{
			Debug.Log("Invaled index for unspawn");
			return;
		}
		
		tmpGo = poolObjects[index];
		tmpGo.transform.localPosition = position;
	}
	public void SetPositionAll(Vector3 position)
	{
		for(int i = 0;i < poolObjects.Count;i++)
		{
			SetPosition(i,position);
		}
	}
	public void SetPosition(int index,Vector3 position)
	{
		if(index<0)
		{
			Debug.Log("Invaled index for unspawn");
			return;
		}
		
		tmpGo = poolObjects[index];
		tmpGo.transform.position = position;
	}
	public void Unspawn(GameObject go)
	{
		Unspawn(spawnedObjects.IndexOf(go));
	}
	
	
	public void Unspawn(int index)
	{
		if(index<0 || index >=spawnedObjects.Count)
		{
			Debug.Log("Invalid index for unspawn");
			return;
		}
		
		tmpGo = spawnedObjects[index];
		if(!poolData.keepActive)
		{
			tmpGo.SetActive(false);
		}

		if(hasMotinObject)
		{
			tmpGo.GetComponent<MotinBaseObject>().OnWillUnspawn();
		}

		tmpGo.transform.parent = this.parentTransform;
		tmpGo.transform.position=  poolData.defaultStoragePosition;
		
		storedObjects.Add(tmpGo);
		spawnedObjects.Remove(tmpGo);
		
		if(hasMotinObject)
		{
			tmpGo.GetComponent<MotinBaseObject>().OnUnspawned();
		}
		RaiseOnGameobjectUnspawned(tmpGo);
	}
	
}

}


