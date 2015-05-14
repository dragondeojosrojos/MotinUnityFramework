using UnityEngine;
using System.Collections;


namespace MotinGames
{
	public class MotinWeapon : MotinBaseObject {

		public bool  autoFire = true;
		public float fireRate= 10;//per second
		public GameObject modelObject;
		public Transform 	ammoSpawnNode; 
		public string		ammoPoolName = "";

		public bool updateDirectionFromAmmoSpawnNode = true;

		MotinGameObjectPool	ammoPool;

		protected override void Start ()
		{
			base.Start ();

			SetAmmoPool(ammoPoolName);

			if(updateDirectionFromAmmoSpawnNode)
				direction = ammoSpawnNode.forward;

		}


		public void SetAmmoPool(string poolName)
		{
			ammoPool = MotinPoolManager.sharedManager().getPoolByName(poolName);
		}

		Vector3	_direction = Vector3.zero;
		public Vector3 direction 
		{
			get
			{
				return _direction;
			}
			set{
				_direction = value;
				updateModelDirection();
			}
		}

		void updateModelDirection()
		{
			if(modelObject!=null)
				modelObject.transform.forward = direction;

		}

		bool _isTriggered = false;
		public bool isTriggered
		{
			get{return _isTriggered;}
		}
		public void PullTrigger(System.Action<MotinAmmo> OnBulletFired = null)
		{
			if(!autoFire)
			{
				DoFire(OnBulletFired);
				return;
			}

			if(isTriggered)
				return;

			_isTriggered = true;
			StartCoroutine(FireCoroutine(OnBulletFired));

		}
		public void ReleaseTrigger()
		{
			if(!isTriggered)
				return;

			_isTriggered = false;
			StopAllCoroutines();

		}
		GameObject 	ammoObject = null;
		MotinAmmo 	firedAmmo = null;
		void DoFire(System.Action<MotinAmmo> OnBulletFired = null)
		{
			if(updateDirectionFromAmmoSpawnNode)
				direction = ammoSpawnNode.forward;

			ammoObject = ammoPool.Spawn(ammoSpawnNode.position);
			firedAmmo = ammoObject.GetComponent<MotinAmmo>();
			firedAmmo.pool = ammoPool;
			firedAmmo.Fire(direction);

			if(OnBulletFired!=null)
				OnBulletFired(firedAmmo);

			firedAmmo = null;
		}

		IEnumerator FireCoroutine(System.Action<MotinAmmo> OnBulletFired = null)
		{
			//GameObject ammoObject = null;
			while(isTriggered)
			{
				DoFire(OnBulletFired);
				yield return new WaitForSeconds(1/fireRate);
			}
		}

	}
}
