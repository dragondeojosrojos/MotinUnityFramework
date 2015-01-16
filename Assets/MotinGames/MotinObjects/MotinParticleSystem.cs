using UnityEngine;
using System.Collections;


namespace MotinGames
{

public class MotinParticleSystem : MotinBaseObject {


	ParticleSystem particleSys = null;

	// Use this for initialization
	protected override void Awake () {
		particleSys = GetComponent<ParticleSystem>();
	}

	public void Play( )
	{
		StartCoroutine(PlayCoroutine());
	}
	public void Play(System.Action callback )
	{
		StartCoroutine(PlayCoroutine(callback));
	}
	public void Play(System.Action<GameObject> callback  )
	{
		StartCoroutine(PlayCoroutine(callback));
	}
	public IEnumerator PlayCoroutine(System.Action callback )
	{
		particleSys.Play();
		while(particleSys.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if(callback!=null)
			callback();
	}

	public IEnumerator PlayCoroutine(System.Action<GameObject> callback )
	{
		particleSys.Play();
		while(particleSys.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if(callback!=null)
			callback(gameObject);
	}

	public IEnumerator PlayCoroutine()
	{
		particleSys.Play();
		while(particleSys.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		this.Unspawn();
	}

}
}
