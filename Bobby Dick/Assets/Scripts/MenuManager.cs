using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private SaveMenu saveMenu;
	[SerializeField]
	private UIButton uiButton;

	[SerializeField]
	private GameObject quitFeedback;

	bool quit_Confirmed = false;

	public void Open () {
		uiButton.Opened = true;
	}

	public void Close () {
		
		uiButton.Opened = false;

		saveMenu.Opened = false;

		quitFeedback.SetActive (false);

		quit_Confirmed = false;

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

		if (quit_Confirmed) {

			Application.Quit ();
		} else {
			quit_Confirmed = true;
			quitFeedback.SetActive (true);
		}
	}
	#endregion
}