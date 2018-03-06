using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {


	[SerializeField]
	private GameObject quitButton;

	[SerializeField]
	private GameObject loadButton;

	[SerializeField]
	private GameObject playButton;

	[SerializeField]
	private GameObject loadMenu;

	public bool loadMenuOpened = false;

	void Start () {
		Transitions.Instance.ScreenTransition.Fade = false;

		Screen.orientation = ScreenOrientation.Landscape;

		if (SaveTool.Instance.FileExists ("game data")) {
			loadButton.SetActive (true);
			KeepOnLoad.displayTuto = false;
		} else {
			loadButton.SetActive (false);
			KeepOnLoad.displayTuto = true;
		}
	}

	public void NewGameButton () {

		Tween.Bounce (playButton.transform);

		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("NewGameDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void NewGameDelay () {
		KeepOnLoad.dataToLoad = -1;
		SceneManager.LoadScene (1);
	}
	public void QuitButton () {

		Tween.Bounce (quitButton.transform);

		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("QuitDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void QuitDelay () {
		Application.Quit ();
		//
	}

	public void Load () {
		Tween.Bounce (loadButton.transform);

		KeepOnLoad.dataToLoad = 0;
		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("LoadDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void LoadDelay () {
		SceneManager.LoadScene (1);
	}

	public void OpenLoadMenu () {
		loadMenu.SetActive (true);
		loadButton.SetActive (false);

		Tween.Bounce (loadMenu.transform, 0.1f , 1.05f );
	}

	public void CloseLoadMenu () {
		loadMenu.SetActive (false);
		loadButton.SetActive (true);
	}
}
