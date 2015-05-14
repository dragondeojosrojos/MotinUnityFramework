using UnityEngine;
using System.Collections;
using MotinGames;
public class ShowWindowComponent : MonoBehaviour {

	public MOTIN_WINDOWS	windowName =MOTIN_WINDOWS.COUNT;
	public bool				showInmediate = false;

	public void Show()
	{
		MotinWindowManager.sharedManager().ShowWindow(windowName.ToString(),showInmediate);
	}
}
