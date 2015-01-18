using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A console that displays the contents of Unity's debug log.
/// </summary>
/// <remarks>
/// Developed by Matthew Miner (www.matthewminer.com)
/// Permission is given to use this script however you please with absolutely no restrictions.
/// </remarks>
public class MotinGuiLog : MonoBehaviour
{
	
	static MotinGuiLog	sharedInstance = null;
	public static readonly Version version = new Version(1, 0);
	
	public int maxMessages = 200;
	
	struct ConsoleMessage
	{
		public readonly string message;
		public readonly string stackTrace;
		public readonly LogType	type;
		
		public ConsoleMessage (string message, string stackTrace, LogType type)
		{
			this.message = message;
			this.stackTrace	= stackTrace;
			this.type = type;
		}
	}
	
	public KeyCode toggleKey = KeyCode.BackQuote;
	
	List<ConsoleMessage> entries = new List<ConsoleMessage>();
	Vector2 scrollPos;
	bool show;
	bool collapse;
//	bool smsSent = false;
	
	// Visual elements:
	
	const int margin = 20;
	Rect windowRect = new Rect(margin, margin, Screen.width - (2 * margin), Screen.height - (2 * margin));
	
	GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
	GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");
	
	
	void Awake()
	{
		if(sharedInstance == null)
		{
			sharedInstance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			if(sharedInstance !=this)
			{
				DestroyImmediate(this.gameObject);
				return;
			}
		}
		
		Application.RegisterLogCallback(HandleLog); 
	}
	/*
	void OnEnable () { 
		
		Application.RegisterLogCallback(HandleLog); 
		Debug.Log("GUI ON ENABLE");
	}
	void OnDisable () { 
		Debug.Log("GUI ON DISABLE");
		Application.RegisterLogCallback(null); 
	}
	*/
	
	void Update ()
	{
		if (Input.GetKeyDown(toggleKey)) {
			//Debug.Log("keypressed");
			show = !show;
			//if(show)
			//	DeviceManager.sharedManager().DismissLoading();
		}
	}
	
	void OnGUI ()
	{
		if (!show) {
			return;
		}
		
		windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console",GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
	}
	
//	string loadSceneName = "";
	/// <summary>
	/// A window displaying the logged messages.
	/// </summary>
	/// <param name="windowID">The window's ID.</param>

	void ConsoleWindow (int windowID)
	{
	
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));

		GUILayout.BeginHorizontal(GUI.skin.box,  GUILayout.ExpandWidth(true) );
		
			//UILevelSelector.showAll = GUILayout.Toggle(UILevelSelector.showAll ,"SHOW ALL LEVELS");

		//GUILayout.Label("PLayerName " + PhotonNetwork.playerName);
			if(GUILayout.Button("DELETE PLAYER PREFS"))
			{
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();
			}
			if(GUILayout.Button("OPEN DEV CONSOLE"))
			{
				Debug.developerConsoleVisible = true;
			}

		GUILayout.EndHorizontal();

		/*
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
		GUILayout.BeginHorizontal(GUI.skin.box,  GUILayout.ExpandWidth(true) );
			if(GUILayout.Button("LOAD NEXT LEVEL"))
			{
				LevelManager.GetInstance().NextLevel();
				return;
			}
			GUILayout.Label("Scene to load");
			loadSceneName = GUILayout.TextField(loadSceneName);
			if(GUILayout.Button("LOAD"))
			{
				Application.LoadLevelAsync(loadSceneName);
				return;
			}
			
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		*/
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		// Go through each logged entry
		for (int i = 0; i < entries.Count; i++) {
			ConsoleMessage entry = entries[i];
			
			// If this message is the same as the last one and the collapse feature is chosen, skip it
			if (collapse && i > 0 && entry.message == entries[i - 1].message) {
				continue;
			}
			
			// Change the text colour according to the log type
			switch (entry.type) {
			case LogType.Error:
			case LogType.Exception:
				GUI.contentColor = Color.red;
				break;
				
			case LogType.Warning:
				GUI.contentColor = Color.yellow;
				break;
				
			default:
				GUI.contentColor = Color.white;
				break;
			}
			
			GUILayout.Label(entry.message);
		}
		
		GUI.contentColor = Color.white;
		
		GUILayout.EndScrollView();
		
		GUILayout.BeginHorizontal();
		
		// Clear button
		if (GUILayout.Button(clearLabel)) {
			entries.Clear();
		}

		/*
		if (GUILayout.Button("Send SMS")) {
			//if(smsSent==false)
			//{
				string send="";
				for (int i = 0; i < entries.Count; i++) {
					send += entries[i].message + "\n";
				
				}
				SMSManager.sharedManager().SendSms("1555172035",send);
			//}
		}
		*/
		/*
		if (GUILayout.Button("Set Ahora")) {
			SelectionScene.sharedManager().SetDaysSinceInstall(0);
		}
		if (GUILayout.Button("Set 1 dia")) {
			SelectionScene.sharedManager().SetDaysSinceInstall(1);
		}
		if (GUILayout.Button("Set 9 dias")) {
			SelectionScene.sharedManager().SetDaysSinceInstall(9);
			SelectionScene.sharedManager().SetDaysSinceRegisteredPhone(8);
		}
		if (GUILayout.Button("Set 15 dias")) {
			SelectionScene.sharedManager().SetDaysSinceInstall(15);
			SelectionScene.sharedManager().SetDaysSinceRegisteredPhone(14);
		}
		*/
		// Collapse toggle
		collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));
		
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		// Set the window to be draggable by the top title bar
		//GUI.DragWindow(new Rect(0, 0, 10000, 20));
	}
	
	/// <summary>
	/// Logged messages are sent through this callback function.
	/// </summary>
	/// <param name="message">The message itself.</param>
	/// <param name="stackTrace">A trace of where the message came from.</param>
	/// <param name="type">The type of message: error/exception, warning, or assert.</param>
	void HandleLog (string message, string stackTrace, LogType type)
	{
		ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
		entries.Add(entry);
		
		
		while(entries.Count > maxMessages)
		{
			entries.RemoveAt(0);
		}
		
	}
}