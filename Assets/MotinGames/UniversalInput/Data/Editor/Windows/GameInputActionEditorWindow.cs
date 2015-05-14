using UnityEngine;
using UnityEditor;
using System.Collections;
using MotinGames;

public class GameInputActionEditorWindow : MotinDataEditorWindow {
	
	public static GameInputActionEditorWindow sharedInstance = null;
	
	[MenuItem ("MotinGames/Input/1.Game Actions",false,1)]
	static void Init () 
	{
		
		if(GameInputActionEditorWindow.sharedInstance==null)
		{
			GameInputActionEditorWindow v = EditorWindow.GetWindow( typeof(GameInputActionEditorWindow), false, "Game Actions editor Window" ) as GameInputActionEditorWindow;	
			GameInputActionEditorWindow.sharedInstance = v;
		} 
		
		//La data se guarda en la carpeta
		GameInputActionEditorWindow.sharedInstance.Load(UniversalInputManager.GameActionsFilePath);
	}
	
	
	//Override esta funcion para crear un editor especifico para la data que se quiere manejar
	protected override void CreateDataEditor()
	{
		dataEditor = new MotinArrayListEditor(this,typeof(MotinGames.GameInputActionData));
		((MotinArrayListEditor)dataEditor).orderDatas  = true;
		((MotinArrayListEditor)dataEditor).overwriteFileOrder  = true;
		//dataEditor.AddType(typeof(CardPowerData));
	}

	protected override void OnCommited ()
	{
		base.OnCommited ();
		MotinEditorUtils.WriteDefinesFile("INPUT_ACTIONS","MotinGames",AppSettings.GeneratedSourcePath +"InputActionsDefines.cs",GetDataNames());
	}

}