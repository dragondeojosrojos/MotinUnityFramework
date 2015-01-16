using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class MotinAnimator : MonoBehaviour {
	// show
	
	public TextAsset			dataFile;
	public MotinAnimatorData	animatorData;
	
	public List<AMTake> takes = new List<AMTake>();
	public AMTake playOnStart = null;
	
	protected string animatorId ="";
	
	private float tmpStartFrame =0;
	private float tmpStartTime =0;
	// hide
	
	[HideInInspector] public bool isPlaying {
		get {
			if(nowPlayingTake != null && !isPaused) return true;
			return false;
		}
	}
	[HideInInspector] public string takeName {
		get {
			if(nowPlayingTake != null) return nowPlayingTake.name;
			return null;
		}
	}
	
	public delegate void AnimationCompleteDelegate(MotinAnimator motinAnimator,string animationName);
	
	public AnimationCompleteDelegate OnAnimationComplete = null;
	void RaiseOnAnimationComplete(string animationName)
	{
		if(OnAnimationComplete!=null)
		{
			//Debug.Log("ANIMATOR PLAY TAKE ON COMPLETE " +this.gameObject.name + " "+ animationName);
			OnAnimationComplete(this,animationName);
			//OnAnimationComplete=null;
		}
	}
	
	public delegate void AnimationLoopCompleteDelegate(MotinAnimator motinAnimator,string animationName);
	public AnimationLoopCompleteDelegate OnAnimationLoopComplete = null;
	void RaiseOnAnimationLoopComplete(string animationName)
	{
		if(OnAnimationLoopComplete!=null)
		{
			OnAnimationLoopComplete(this,animationName);
		}
	}
	//[HideInInspector] public bool isAnimatorOpen = false;
	//[HideInInspector] public bool isInspectorOpen = false;
	//[HideInInspector] public bool inPlayMode = false;
	//[HideInInspector] public float zoom = 0.4f;
	
	[HideInInspector] public int currentTake;
	//[HideInInspector] public int codeLanguage = 0; 	// 0 = C#, 1 = Javascript
	//[HideInInspector] public float gizmo_size = 0.05f;
	//[HideInInspector] public float width_track = 150f;
	// temporary variables for selecting a property
	//[HideInInspector] public bool didSelectProperty = false;
	//[HideInInspector] public AMPropertyTrack propertySelectTrack;
	//[HideInInspector] public Component propertyComponent;
	//[HideInInspector] public PropertyInfo propertyInfo;
	//[HideInInspector] public FieldInfo fieldInfo;
	//[HideInInspector] public bool autoKey = false;
	
	[HideInInspector] public float elapsedTime = 0f;
	
	
	
	// private
	private AMTake nowPlayingTake_ = null;
	private AMTake nowPlayingTake
	{
		get{ return nowPlayingTake_;}
		set
		{
			nowPlayingTake_ = value;
			//if(nowPlayingTake_==null)
			//{
				//Debug.Log(gameObject.name + " take setted null");
			//}
		}
	}
	
	private bool isPaused_ = false;
	private bool isPaused
	{
		get{ return isPaused_;}
		set
		{
			isPaused_ = value;
			//if(isPaused_==true)
			//{
			//	Debug.Log(gameObject.name + " isPaused_ setted true");
			//}
		}
	}
	
	private bool isLooping = false;
	private float takeTime = 0f;
	
	private string tmpString = "";
	
	public object Invoker(object[] args) {
		switch((int)args[0]) {
			// check if is playing
			case 0:
				return isPlaying;
			// get take name
			case 1:
				return takeName;
			// play
			case 2:
				Play((string)args[1],true,0f,(bool)args[2]);
				break;
			// stop
			case 3:
				StopLoop();
				break;
			// pause
			case 4:
				PauseLoop();
				break;
			// resume
			case 5:
				ResumeLoop();
				break;
			// play from time
			case 6:
				Play((string)args[1],false,(float)args[2],(bool)args[3]);
				break;
			// play from frame
			case 7:
				Play((string)args[1],true,(float)((int)args[2]),(bool)args[3]);
				break;
			// preview frame
			case 8:
				PreviewValue((string)args[1],true,(float)args[2]);
				break;
			// preview time
			case 9:
				PreviewValue((string)args[1],false,(float)args[2]);
				break;
			// running time
			case 10:
				if(takeName == null) return 0f;
				else return elapsedTime;
			// total time
			case 11:
				if(takeName == null) return 0f;
				else return (float) nowPlayingTake.numFrames / (float) nowPlayingTake.frameRate;
			case 12:
				if(takeName == null) return false;
				return isPaused;
			default:
				break;
		}
		return null;
	}
	
	void Awake()
	{
		//Debug.Log("Animator awake");
		animatorId = AMTween.GenerateID();
		Load(animatorData);
	}
	void OnDestroy()
	{
		//Debug.Log("Animator destroy");
		destroy();
	}
	
	void Start() {
		/*
		if(playOnStart) {
			Play (playOnStart.name,true,0f,false);
			playOnStart = null;
		}
		*/
	}
	/*
	void OnDrawGizmos() {
		//if(!isAnimatorOpen) return;
		takes[currentTake].drawGizmos(gizmo_size, inPlayMode);		
	}
	*/
	void Update() {
	/*	if(this.gameObject.name=="ui_mainMenu")
		{
			Debug.Log("MAIN MENU UPDATE");
		}
		*/
		if(isPaused || nowPlayingTake == null)
		{
			/*
			if(this.gameObject.name=="ui_mainMenu")
			{
				Debug.Log(gameObject.name +  " Play take is paused or null" );
			}
			*/
			//if(isPaused)
			//	Debug.Log(gameObject.name +  " Play take is paused or null" );

			return;
		}
		elapsedTime += Time.deltaTime;
		if(elapsedTime >= takeTime) 
		{
			/*
			if(this.gameObject.name=="ui_mainMenu")
			{
				Debug.Log(gameObject.name + "take complete " + nowPlayingTake.name + " loop " + isLooping.ToString() );
			}
			*/
			//Debug.Log(gameObject.name + "take complete " + nowPlayingTake.name + " loop " + isLooping.ToString() );
			nowPlayingTake.stopAudio();
			if(isLooping)
			{
				RaiseOnAnimationLoopComplete(nowPlayingTake.name);
				Execute(nowPlayingTake);
				
			}
			else 
			{
				tmpString = nowPlayingTake.name;
				AMTween.StopById(/*this.gameObject,*/animatorId/*,true*/);
				PreviewValue(nowPlayingTake.name,true,nowPlayingTake_.numFrames);
				nowPlayingTake = null;
//				 Debug.Log(gameObject.name +  " RAISE ON COMPLETE" );
				RaiseOnAnimationComplete(tmpString);
			}
		}
	}
	public void Play(string takeName)
	{
		Play(takeName,true,0,false,null);
	}
	public void Play(string takeName,MotinAnimator.AnimationCompleteDelegate animDelegate )
	{
		Play(takeName,true,0,false,animDelegate);
	}
	public void Play(string takeName,bool loop,MotinAnimator.AnimationCompleteDelegate animDelegate =null)
	{
		Play(takeName,true,0,loop,animDelegate);
	}
	public void Play(string take_name, bool isFrame, float value, bool loop,MotinAnimator.AnimationCompleteDelegate animDelegate = null) {
		//Debug.Log("Play take " + take_name + " loop " + loop.ToString() );
		/*
		if(nowPlayingTake != null)
			AMTween.Stop(this.gameObject,true);
		*/
		int index = getTakeIndex(take_name);
		if(index>=0)
		{
			//AMTake newTake = getTake(take_name);
			//if(newTake) {
//				Debug.Log("Execute take " + take_name + " " + this.gameObject.name);
				isLooping = loop;
				Execute (takes[index], isFrame, value,animDelegate);
			//}
		}
		else
		{
			OnAnimationComplete =animDelegate;
			RaiseOnAnimationComplete(take_name);
			//Debug.LogWarning("Take name not found " + take_name + " " + this.gameObject.name);
		}
	}
	
	public void PreviewValue(string take_name, bool isFrame, float value) {
		AMTake take = null;
		if(nowPlayingTake && nowPlayingTake.name == takeName) take = nowPlayingTake;
		else take = getTake(take_name);
		if(take==null) return;

		tmpStartFrame = value;
		if(!isFrame) tmpStartFrame *= take.frameRate;	// convert time to frame
		take.previewFrameInvoker(tmpStartFrame);
	}
	
	public void Execute(AMTake take, bool isFrame = true, float value = 0f /* frame or time */,MotinAnimator.AnimationCompleteDelegate animDelegate = null) {
		if(nowPlayingTake != null)
		{
			AMTween.StopById(/*this.gameObject,*/animatorId/*,true*/);
			tmpString = nowPlayingTake.name;
			nowPlayingTake = null;
			RaiseOnAnimationComplete(tmpString);
		}
		OnAnimationComplete =animDelegate;
		// delete AMCameraFade
//		Debug.Log("EXECUTE TAKE " + take.name);
		tmpStartFrame = value;
		tmpStartTime = value;
		if(!isFrame) tmpStartFrame *= take.frameRate;	// convert time to frame
		if(isFrame) tmpStartTime /= take.frameRate;	// convert frame to time
		
		take.executeActions(tmpStartFrame,animatorId);
		elapsedTime = tmpStartTime;
		takeTime = (float)take.numFrames/(float)take.frameRate;
//		Debug.Log("Play " + take.name + " takeTime " + takeTime.ToString()  + " " + this.gameObject.name);
		nowPlayingTake = take;
		
	}


	public void Pause()
	{
		PauseGameobject(gameObject);
	}
	public void PauseLoop() {
		/*
		Debug.Log("Pause Animator " + gameObject.name);
		GameObject child;
		MotinAnimator childAnimator = null;
		for(int i = 0 ; i < transform.childCount; i++)
		{
			childAnimator = null;
			child = transform.GetChild(i).gameObject;
			childAnimator = child.GetComponent<MotinAnimator>();
			if(childAnimator!=null)
				childAnimator.PauseLoop();

		}
*/
		if(nowPlayingTake == null) return;

		isPaused = true;
		nowPlayingTake.stopAudio();
		AMTween.Pause(this.gameObject,true);
		
	}

	void PauseGameobject(GameObject go)
	{
		GameObject child;
		MotinAnimator childAnimator = null;
		for(int i = 0 ; i < go.transform.childCount; i++)
		{
			child = go.transform.GetChild(i).gameObject;
			PauseGameobject(child);

		}

		childAnimator = go.GetComponent<MotinAnimator>();
		if(childAnimator!=null)
			childAnimator.PauseLoop();

	}
	public void Resume()
	{
		ResumeGameobject(gameObject);
	}
	public void ResumeLoop() {

		/*
		GameObject child;
		MotinAnimator childAnimator = null;
		for(int i = 0 ; i < transform.childCount; i++)
		{
			childAnimator = null;
			child = transform.GetChild(i).gameObject;
			childAnimator = child.GetComponent<MotinAnimator>();
			if(childAnimator!=null)
				childAnimator.ResumeLoop();
			
		}
*/

		if(nowPlayingTake == null) return;
		AMTween.Resume(this.gameObject,true);	
		isPaused = false;
	}

	void ResumeGameobject(GameObject go)
	{
		GameObject child;
		MotinAnimator childAnimator = null;
		for(int i = 0 ; i < go.transform.childCount; i++)
		{
			child = go.transform.GetChild(i).gameObject;
			ResumeGameobject(child);
			
		}
		
		childAnimator = go.GetComponent<MotinAnimator>();
		if(childAnimator!=null)
			childAnimator.ResumeLoop();
		
	}
	public void StopLoop() {
		if(nowPlayingTake == null) return;
		//Debug.Log(gameObject.name + " StopLoop");
		nowPlayingTake.stopAudio();
		nowPlayingTake.stopAnimations();
		nowPlayingTake = null;
		isLooping = false;
		isPaused = false;
		AMTween.StopById(/*this.gameObject,*/animatorId/*,true*/);
	}
	
	public int getCurrentTakeValue() {
		return currentTake;	
	}
	
	public int getTakeCount() {
		return takes.Count;	
	}

	public bool setCurrentTakeValue(int _take) {
		if(_take != currentTake) {
			// reset preview to frame 1
			getCurrentTake().previewFrame(1f);
			// change take
			currentTake = _take;
			return true;
		}
		return false;
	}
	
	public AMTake getCurrentTake() {
		return takes[currentTake];	
	}
	
	public AMTake getTake(string takeName) {
		foreach(AMTake take in takes) {
			if(take.name == takeName) return take;	
		}
		//Debug.LogError ("Animator: Take '"+takeName+"' not found." + this.gameObject.name);
		//return new AMTake(null);
		return null;
	}
	public int getTakeIndex(string takeName) {
		for(int i=0;i<takes.Count;i++) {
			if(takes[i].name == takeName) return i;	
		}
		return -1;
	}
	
	/*
	public void addTake() {
		string name = "Take"+(takes.Count+1);
		AMTake a = ScriptableObject.CreateInstance<AMTake>();
		// set defaults
		a.name = name;
		makeTakeNameUnique(a);
		a.frameRate = 24;
		a.numFrames = 1440;
		a.startFrame = 1;
		a.selectedFrame = 1;
		a.selectedTrack = -1;
		a.playbackSpeedIndex = 2;
		//a.lsTracks = new List<AMTrack>();
		//a.dictTracks = new Dictionary<int,AMTrack>();
		a.trackKeys = new List<int>();
		a.trackValues = new List<AMTrack>();
		takes.Add (a);
		selectTake (takes.Count-1);
		
	}
	
	public void deleteTake(int index) {
		//if(shouldCheckDependencies) shouldCheckDependencies = false;
		if(playOnStart == takes[index]) playOnStart = null;
		takes[index].destroy();
		takes.RemoveAt(index);
		if((currentTake>=index)&&(currentTake>0)) currentTake--;
	}
	
	public void deleteCurrentTake() {
		deleteTake (currentTake);	
	}

	public void selectTake(int index) {
		currentTake = index;
	}
	
	public void selectTake(string name) {
		for(int i=0;i<takes.Count;i++)
			if(takes[i].name == name) {
				selectTake (i);
				break;
			}
	}
	public void makeTakeNameUnique(AMTake take) {
		bool loop = false;
		int count = 0;
		do {
			if(loop) loop = false;
			foreach(AMTake _take in takes) {
				if(_take != take && _take.name == take.name) {
					if(count>0) take.name = take.name.Substring(0,take.name.Length-3);
					count++;
					take.name += "("+count+")";
					loop = true;
					break;
				}
			}
		} while (loop);
	}
	 */
	public string[] getTakeNames() {
		string[] names = new string[takes.Count+1];
		for(int i=0;i<takes.Count;i++) {
			names[i] = takes[i].name;
		}
		names[names.Length-1] = "Create new...";
		return names;
	}
	
	public int getTakeIndex(AMTake take) {
		for(int i=0;i<takes.Count;i++) {
			if(takes[i] == take) return i;	
		}
		return -1;
	}
	/*
	public bool setCodeLanguage(int codeLanguage) {
		if(this.codeLanguage != codeLanguage) {
			this.codeLanguage = codeLanguage;
			return true;
		}
		return false;
	}
	public bool setGizmoSize(float gizmo_size) {
		if(this.gizmo_size != gizmo_size) {
			this.gizmo_size = gizmo_size;
			// update target gizmo size
			foreach(Object target in GameObject.FindObjectsOfType(typeof(AMTarget))) {
				if((target as AMTarget).gizmo_size != gizmo_size) (target as AMTarget).gizmo_size = gizmo_size;
			}
			return true;
		}
		return false;
	}
	*/
	/*public bool setShowWarningForLostReferences(bool showWarningForLostReferences) {
		if(this.showWarningForLostReferences != showWarningForLostReferences) {
			this.showWarningForLostReferences = showWarningForLostReferences;
			return true;
		}
		return false;
	}*/
	/*
	public void deleteAllTakesExcept(AMTake take) {
		for(int index=0;index<takes.Count;index++) {
			if(takes[index] == take) continue;
			deleteTake(index);
			index--;
		}
	}
	*/
	
	/*
	public void mergeWith(AnimatorData _aData) {
		foreach(AMTake take in _aData.takes) {
			takes.Add(take);
			makeTakeNameUnique(take);
		}
	}
	*/
	/*
	public List<GameObject> getDependencies(AMTake _take = null)
	{
		// if only one take
		if(_take != null) return _take.getDependencies().ToList();

		// if all takes
		List<GameObject> ls = new List<GameObject>();
		foreach(AMTake take in takes) {
			ls = ls.Union(take.getDependencies()).ToList();
		}
		return ls;
	}
	
	public List<GameObject> updateDependencies(List<GameObject> newReferences, List<GameObject> oldReferences) {
		List<GameObject> lsFlagToKeep = new List<GameObject>();
		foreach(AMTake take in takes) {
			lsFlagToKeep = lsFlagToKeep.Union(take.updateDependencies(newReferences,oldReferences)).ToList();
		}
		return lsFlagToKeep;
	}
	
	*/
	public void Load(MotinAnimatorData motinAnimatorData)
	{
		if(motinAnimatorData.takes !=null)
		{
			if(takes!=null)
			{
				foreach(AMTake take in takes)
				{
					if(take!=null)
						take.destroy();
				}
				takes.Clear();
			}
			
			
			foreach(MotinTakeData takeData in motinAnimatorData.takes )
			{
//				Debug.Log("Load MotinTakeData ");
				AMTake newTake = ScriptableObject.CreateInstance<AMTake>();
				newTake.Load(takeData);
				takes.Add(newTake);
				if(motinAnimatorData.playOnStart == takeData.name)
				{
				//	Debug.Log("playonstar ");
					playOnStart = newTake;
				}
				
				//selectTake(takes.Count-1);
			}
		}
	}
	
	public void destroy()
	{
		foreach(AMTake take in takes)
		{
			take.destroy();
		}
		takes.Clear();
		//DestroyImmediate(this);
	}
	
}
