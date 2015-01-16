using UnityEngine;
using System.Collections.Generic;

namespace MotinGames
{
public class MotinPoolManager : MonoBehaviour
{
	public MotinGameObjectPoolData[] poolDatas;
	public bool createPoolsOnStart = true;


	protected static MotinPoolManager sharedInstance_ = null;
	
	List<MotinGameObjectPool> poolList_ = null;


	
	bool isQuitting = false;
	public static MotinPoolManager sharedManager()
	{
		//if(sharedInstance_ == null)
		//{
		//	sharedInstance_ = new MotinPoolManager();
		//}
		
		return sharedInstance_;
	}
	
	void Awake()
	{
		if(sharedInstance_!=null)
			Debug.LogError("Only 1 instance of MotinPoolManager allowed");
		//Debug.Log("Pool Manager Constructor");
		isQuitting = false;
		sharedInstance_ = this;
		poolList_ = new List<MotinGameObjectPool>();
		
	}

	void Start()
	{
		if(!createPoolsOnStart)
			return;

		createPools(poolDatas);
	}

	void OnEnable()
	{

	}
	void OnDisable()
	{
		if(isQuitting)
			sharedInstance_ = null;
	}

	/*
	~MotinPoolManager()
	{
		Debug.Log("Pool Manager Destructor");
		destroyPools();
	}
	*/
	
	public void createPools(MotinGameObjectPoolData[] poolDatas)
	{
		foreach(MotinGameObjectPoolData data in poolDatas)
		{
			createPool(data);
		}
	}
	
	public void createPool(MotinGameObjectPoolData poolData)
	{
		if(getPoolByName(poolData.name)==null)
		{
//			Debug.Log ("Create Pool " + poolData.name);
			MotinGameObjectPool pool = new MotinGameObjectPool(poolData);
			pool.InitializePool();
			poolList_.Add(pool);
		}
		else
		{
			Debug.LogError("Pool with name " + poolData.name + "Already exists");
		}
	}
	
	public MotinGameObjectPool getPoolByName(string name)
	{
		foreach(MotinGameObjectPool pool in poolList_)
		{
			if(pool.name==name)
				return pool;
		}
		//Debug.LogError("Pool " + name + " not found");
		return null;
	}

	public List<MotinGameObjectPool> Pools
	{
		get{
			return poolList_;
		}
	}
	public void UnspawnAllPools()
	{
		foreach(MotinGameObjectPool pool in poolList_)
		{
			pool.UnspawnAll();
		}

	}
	public void destroyPool(string name)
	{
		MotinGameObjectPool pool = getPoolByName(name);
		if(pool != null)
		{
			int index = poolList_.IndexOf(pool);
			pool.DestroyPool();
			poolList_.RemoveAt(index);
			pool = null;
		}
	}
	
	public void destroyPools()
	{
		foreach(MotinGameObjectPool pool in poolList_)
		{
			pool.DestroyPool();
		}
		poolList_.Clear();
	}

	void OnApplicationQuit() {
		isQuitting = true;
//		Debug.Log("QUITTING");
	}
}

}