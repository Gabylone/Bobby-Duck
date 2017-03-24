using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private SaveMenu saveMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region buttons
	public void SaveButton () {
		saveMenu.Saving = true;
		saveMenu.Opened = !saveMenu.Opened;
	}
	public void LoadButton () {
		saveMenu.Saving = false;
		saveMenu.Opened = !saveMenu.Opened;
	}
	public void QuitButton () {



		Application.Quit ();	
	}

	#endregion
}
