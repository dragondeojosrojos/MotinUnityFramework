using UnityEngine;
using UnityEditor;
using System.Collections;
using MotinGames;

public class DemoSerializableDataEditorWindow : MotinDataEditorWindow {

	public static DemoSerializableDataEditorWindow sharedInstance = null;
	
	[MenuItem ("MotinGames/Serializable Demo Editor Window")]
	static void Init () 
	{
		
		if(DemoSerializableDataEditorWindow.sharedInstance==null)
		{
			DemoSerializableDataEditorWindow v = EditorWindow.GetWindow( typeof(DemoSerializableDataEditorWindow), false, "Serializable Demo Window" ) as DemoSerializableDataEditorWindow;	
			DemoSerializableDataEditorWindow.sharedInstance = v;
		} 

		//La data se guarda en la carpeta
		DemoSerializableDataEditorWindow.sharedInstance.Load("Data/SerializableDemoDataFile.xml");
	}


	//Override esta funcion para crear un editor especifico para la data que se quiere manejar
	protected override void CreateDataEditor()
	{
		dataEditor =new MotinArrayListEditor(this,"MotinGames.DemoApp.Data");
	}
}
