using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private GameObject quitFeedback;

	[SerializeField]
	private GameObject openButton;
	[SerializeField]
	private GameObject menuGroup;

	[SerializeField]
	private GameObject saveButton;

	bool quit_Confirmed = false;

	void Start () {
		CrewInventory.Instance.closeInventory += HandleCloseInventory;

		Close ();
	}

	void HandleCloseInventory ()
	{
		Close ();
	}

	public void Open () {

		menuGroup.SetActive (true);

		CrewInventory.Instance.HideMenuButtons ();

		Tween.Bounce (menuGroup.transform , 0.2f , 1.1f);
	}

	public void Close () {

		CrewInventory.Instance.ShowMenuButtons ();
		
		menuGroup.SetActive (false);

		quit_Confirmed = false;

	}

	#region buttons
	public void SaveButton () {

		Tween.Bounce (saveButton.transform);

		MessageDisplay.Instance.Show ("Sauvegarder partie ?");
		MessageDisplay.onValidate += HandleOnValidate;
	}

	void HandleOnValidate ()
	{
		SaveManager.Instance.SaveOverallGame ();
	}
	public void QuitButton () {

		Tween.Bounce (transform);
		Invoke ("Quit",Tween.defaultDuration);
//
//		if (quit_Confirmed) {
//
//		} else {
//			quit_Confirmed = true;
//			quitFeedback.SetActive (true);
//			Tween.Bounce (quitFeedback.transform, 0.2f, 1.2f);
//		}
	}
	void Quit () {
		Application.Quit ();
	}
	#endregion
}