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

	public float transitionDuration = 1.2f;

	public bool loadMenuOpened = false;

	void Start () {
		Transitions.Instance.ScreenTransition.FadeOut (0.5f);

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

		if (SaveTool.Instance.FileExists ("game data")) {

			MessageDisplay.onValidate += HandleOnValidate;
			MessageDisplay.Instance.Show ("Ecraser sauvegarde ?");

		} else {
			
			HandleOnValidate ();

		}
	}

	void HandleOnValidate ()
	{
		Transitions.Instance.ScreenTransition.FadeIn (transitionDuration);
		Invoke ("NewGameDelay" , transitionDuration);
	}
	private void NewGameDelay () {
		KeepOnLoad.dataToLoad = -1;
		SceneManager.LoadScene (1);
	}
	public void QuitButton () {

		Tween.Bounce (quitButton.transform);

		Transitions.Instance.ScreenTransition.FadeIn (transitionDuration);
		Invoke ("QuitDelay" , transitionDuration);
	}
	private void QuitDelay () {
		Application.Quit ();
		//
	}

	public void Load () {
		Tween.Bounce (loadButton.transform);

		KeepOnLoad.dataToLoad = 0;
		Transitions.Instance.ScreenTransition.FadeIn (transitionDuration);
		Invoke ("LoadDelay" , transitionDuration);
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
