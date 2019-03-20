using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Holoville.HOTween;

public class MainMenuManager : MonoBehaviour {

    public GameObject mapsGroup;

	[SerializeField]
	private GameObject quitButton;


    public GameObject HandObj;

    [SerializeField]
	private GameObject loadButton;

	[SerializeField]
	private GameObject playButton;

	[SerializeField]
	private GameObject loadMenu;

	public float transitionDuration = 1.2f;

	public bool loadMenuOpened = false;

    public float mapsAppearDuration = 0.5f;

	void Start () {
		Transitions.Instance.ScreenTransition.FadeOut (0.5f);

		Screen.orientation = ScreenOrientation.Landscape;

        mapsGroup.SetActive(false);
        MenuObj.SetActive(false);

		/*if (SaveTool.Instance.FileExists ("game data")) {
			loadButton.SetActive (true);
			KeepOnLoad.displayTuto = false;
		} else {
			loadButton.SetActive (false);
			KeepOnLoad.displayTuto = true;
		}*/
	}

	public void NewGameButton () {

		Tween.Bounce (playButton.transform);

        mapsGroup.SetActive(true);

        CancelInvoke("HideMapsDelay");

        mapsGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f );
        HOTween.To(mapsGroup.GetComponent<RectTransform>(), mapsAppearDuration, "anchoredPosition", Vector2.zero);

        MenuObj.SetActive(false);

        /*if (SaveTool.Instance.FileExists ("game data")) {

			MessageDisplay.onValidate += HandleOnValidate;
			MessageDisplay.Instance.Show ("Ecraser sauvegarde ?");

		} else {
			
			HandleOnValidate ();

		}*/
    }

    public void HideMaps ()
    {
        HOTween.To(mapsGroup.GetComponent<RectTransform>(), 1f, "anchoredPosition", new Vector2(0f, 1000f));

        Invoke("HideMapsDelay", 1f);

        MenuObj.SetActive(true);


    }

    void HideMapsDelay()
    {
        mapsGroup.SetActive(false);
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

    public GameObject MenuObj;
    public void OnTapBackground()
    {
        MenuObj.SetActive(true);
        HandObj.SetActive(false);
    }
}
