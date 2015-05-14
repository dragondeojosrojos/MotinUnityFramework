using UnityEngine;
using UnityEditor;
using System.Collections;
using MotinGames;

public class ControlSetupsEditorWindow : MotinDataEditorWindow {
	
	public static ControlSetupsEditorWindow sharedInstance = null;
	
	[MenuItem ("MotinGames/Input/2.Control setups",false,2)]
	static void Init () 
	{
		
		if(ControlSetupsEditorWindow.sharedInstance==null)
		{
			ControlSetupsEditorWindow v = EditorWindow.GetWindow( typeof(ControlSetupsEditorWindow), false, "Control Setups editor Window" ) as ControlSetupsEditorWindow;	
			ControlSetupsEditorWindow.sharedInstance = v;
		} 
		
		//La data se guarda en la carpeta
		ControlSetupsEditorWindow.sharedInstance.Load(UniversalInputManager.InputFilePath);
	}
	
	
	//Override esta funcion para crear un editor especifico para la data que se quiere manejar
	protected override void CreateDataEditor()
	{
		dataEditor = new MotinArrayListEditor(this,typeof(MotinGames.ControlSetupData));
		((MotinArrayListEditor)dataEditor).orderDatas  = true;
		((MotinArrayListEditor)dataEditor).overwriteFileOrder  = true;

		//dataEditor.AddType(typeof(CardPowerData));
	}

	protected override void OnDataLoaded ()
	{
		base.OnDataLoaded ();
	}

	/*
	protected override void OnCommited ()
	{
		base.OnCommited ();
		MotinEditorUtils.WriteDefinesFile("INPUT_ACTIONS","MotinGames",AppSettings.GeneratedSourcePath +"InputActionsDefines.cs",GetDataNames());
	}
	*/
	
}