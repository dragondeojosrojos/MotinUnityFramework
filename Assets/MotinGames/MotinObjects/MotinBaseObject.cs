using UnityEngine;
using System.Collections;


namespace MotinGames
{

	public class MotinBaseObject : MonoBehaviour {

		public string				entityId = "";
		public MotinGameObjectPool 	pool = null;

		public bool isSpawned{get;protected set;}
			
		public virtual Bounds GetBounds()
		{
			return new Bounds();
		}

		protected virtual void Awake()
		{
			
		}

		protected virtual void Start()
		{

		}

		protected virtual void OnEnable()
		{

		}
		protected virtual void OnDisable()
		{
			
		}
		
		public void Unspawn()
		{
			if(pool==null)
				return;

			pool.Unspawn(gameObject);
		}
		
		public virtual void Initialize()
		{

		}
		
		public void OnSpawned()
		{
			isSpawned = true;
			DoOnSpawned();
		}
		public void OnUnspawned()
		{
			isSpawned = false;
			DoOnUnspawned();
		}

		public virtual void DoOnSpawned()
		{

		}
		public virtual void DoOnUnspawned()
		{
				
		}
		public virtual void OnWillUnspawn()
		{

		}

		public virtual void Pause()
		{

		}
		public virtual void Resume()
		{

		}

		public virtual void Reset()
		{

		}
	}
}
