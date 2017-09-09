using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	[SerializeField]
	private Button[] loadButtons;

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
	}

	public void NewGameButton () {

		Tween.Bounce (playButton.transform);

		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("NewGameDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void NewGameDelay () {
		KeepOnLoad.dataToLoad = -1;
		SceneManager.LoadScene ("Main");
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

	public void Load (int index) {
		
		Tween.Bounce (loadButtons[index-1].transform);

		KeepOnLoad.dataToLoad = index;
		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("LoadDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void LoadDelay () {
		SceneManager.LoadScene ("Main");
	}

	public void OpenLoadMenu () {
		loadMenu.SetActive (true);
		loadButton.SetActive (false);

		Tween.Bounce (loadMenu.transform, 0.1f , 1.05f );

		UpdateButtons ();
	}

	public void CloseLoadMenu () {
		loadMenu.SetActive (false);
		loadButton.SetActive (true);
	}

	public void UpdateButtons ()
	{
		int loadIndex = 1;

		for (int buttonIndex = 0; buttonIndex < loadButtons.Length; buttonIndex++) {

			if (SaveTool.Instance.FileExists (loadIndex)) {

				loadButtons[buttonIndex].GetComponentInChildren<Text> ().text = "SAVE " + loadIndex;
				loadButtons[buttonIndex].GetComponentInChildren<Text> ().color = Color.black;
				loadButtons[buttonIndex].image.color = Color.white;
				loadButtons[buttonIndex].interactable = true;


			} else {

				loadButtons[buttonIndex].GetComponentInChildren<Text> ().text = "aucun";
				loadButtons[buttonIndex].GetComponentInChildren<Text> ().color = Color.white;
				loadButtons[buttonIndex].image.color = Color.black;
				loadButtons[buttonIndex].interactable = false;

			}

			++loadIndex;
		}
	}
}
