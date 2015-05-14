using UnityEngine;
using System.Collections;
using MotinGames;


public class Window_MainMenu : MotinWindow {


	public void OnButtonClicked()
	{
		MotinWindowManager.sharedManager().ShowWindow("VentanaPrueba",false);
	}

}
