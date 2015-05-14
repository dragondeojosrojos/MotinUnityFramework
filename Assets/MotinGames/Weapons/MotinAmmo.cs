using UnityEngine;
using System.Collections;


namespace MotinGames
{
	public class MotinAmmo : MotinBaseObject {


		public System.Action<Collision> OnHit = null;

		public float life_time = 4;
		public float initialVelocity = 1;
		public float damage = 1;
		public string onHitEffectPoolName = "";
		Vector3	_direction = Vector3.zero;
		Rigidbody	rigidbody = null;

		protected override void Awake ()
		{
			base.Awake ();
			rigidbody=  GetComponent<Rigidbody>();
		}

		public Vector3 direction 
		{
			get
			{
				return _direction;
			}
			set{
				_direction = value;
				//updateModelDirection();
			}
		}
		public void Fire(Vector3 newDirection)
		{
			lifeTimeElapsed = 0;
			direction = newDirection;
			transform.forward = direction;
			rigidbody.velocity = (transform.forward*initialVelocity);
			GetComponent<AudioSource>().Play();
			StartCoroutine(LifeTimer());
		}

		void KillAmmo()
		{
			pool.Unspawn(this.gameObject);

			StopAllCoroutines();
		}

		float lifeTimeElapsed = 0;
		IEnumerator LifeTimer()
		{
			while(lifeTimeElapsed*10 < life_time)
			{
				//Debug.Log("elapsedTime " + lifeTimeElapsed);
				lifeTimeElapsed+= Time.deltaTime;
				yield return new WaitForSeconds(0.1f);
			}
			if(OnHit!=null)
				OnHit(null);

			KillAmmo();
		}

		void OnCollisionEnter(Collision collision) {
			if(collision.gameObject.tag == this.tag)
				return;

			MotinPoolManager.sharedManager().SpawnMotinParticleSystem(onHitEffectPoolName,collision.contacts[0].point);
			//Debug.Log("BULLET_HIT");

			if(OnHit!=null)
				OnHit(collision);

			KillAmmo();
		}

	}
}
