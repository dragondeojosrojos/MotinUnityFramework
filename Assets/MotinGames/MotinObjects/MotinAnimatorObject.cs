using UnityEngine;
using System.Collections;


namespace MotinGames
{
	
	public class MotinAnimatorObject : MotinBaseObject {
		
		public bool unspawnOnComplete = true;
		MotinAnimator motinAnimator = null;
		
		// Use this for initialization
		protected override void Awake () {
			motinAnimator = GetComponent<MotinAnimator>();
		}
		
		public void Play(string clipName)
		{
			motinAnimator.Play(clipName,AnimationCompleteDelegate);
		}
	
		void AnimationCompleteDelegate(MotinAnimator motinAnimator,string animationName)
		{
			if(unspawnOnComplete)
				this.Unspawn();
		}
	}
}
