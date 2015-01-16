using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MotinGames;
public class MotinDemoController : MonoBehaviour {

	// Use this for initialization

	MotinGameObjectPool cubePool = null;

	List<GameObject> spawnedCubes = new List<GameObject>();

	void Start () {
		cubePool = MotinPoolManager.sharedManager().getPoolByName("Cube");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Spawn Cube",GUILayout.Width(100),GUILayout.Height(50)))
		{
			SpawnCube();
		}
		if(GUILayout.Button("Unspawn Cube",GUILayout.Width(100),GUILayout.Height(50)))
		{
			UnspawnCube();
		}
		if(GUILayout.Button("Unspawn ALL",GUILayout.Width(100),GUILayout.Height(50)))
		{
			cubePool.UnspawnAll();
			spawnedCubes.Clear();
		}
		if(GUILayout.Button("Change Lang",GUILayout.Width(100),GUILayout.Height(50)))
		{
			MotinStringManager.sharedManager().SetNextLanguage();
		}
		if(GUILayout.Button("Play Sound",GUILayout.Width(100),GUILayout.Height(50)))
		{
			MotinSoundManager.sharedManager().PlayFx(SoundDefinitions.Sounds.FX_COMBO_GREAT);
		}
		GUILayout.EndHorizontal();
		foreach(MotinTouch touch in MotinInputManager.sharedManager().touchList)
		{
			GUILayout.Label("TOUCH index " + touch.fingerId + " phase " + touch.phase.ToString() + " pos " + touch.position.ToString() );
		}
	

	}


	void SpawnCube()
	{
		Vector3 position = Vector3.zero;
		position.x = Random.Range(-8,8);
		position.y = Random.Range(-8,8);
		position.z =  Random.Range(-5,5);;
		GameObject spawnedCube =  cubePool.Spawn(position);
		spawnedCube.GetComponent<MotinAnimator>().Play("ScaleBounce",true);
	}
	void UnspawnCube()
	{
		cubePool.Unspawn(0);
	}
}
