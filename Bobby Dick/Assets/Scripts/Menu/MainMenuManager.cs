using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	[SerializeField]
	private Button[] loadButtons;

	public bool loadMenuOpened = false;

	void Start () {
		Transitions.Instance.ScreenTransition.Fade = false;
	}

	public void NewGameButton () {
		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("NewGameDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void NewGameDelay () {
		KeepOnLoad.dataToLoad = -1;
		SceneManager.LoadScene ("Main");
	}
	public void QuitButton () {
		Invoke ("QuitDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void QuitDelay () {
		Transitions.Instance.ScreenTransition.Fade = true;
		Application.Quit ();
		//
	}

	public void Load (int index) {
		KeepOnLoad.dataToLoad = index;
		Transitions.Instance.ScreenTransition.Fade = true;
		Invoke ("LoadDelay" , Transitions.Instance.ScreenTransition.Duration);
	}
	private void LoadDelay () {
		SceneManager.LoadScene ("Main");
	}

	public bool LoadMenuOpened {
		get {
			return loadMenuOpened;
		}
		set {
			loadMenuOpened = value;

			if (value) {
				UpdateButtons ();
			}
		}
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
