using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Holoville.HOTween;

public class MainMenu : DisplayGroup {

	public static MainMenu Instance;

    public Button newGameButton;
    public Button quitGameButton;

    public Transform mapButton;

    public Transform sound_OnTransform;
    public Transform sound_OffTransform;

	public GameObject openButton;

	void Awake () {
		Instance = this;
	}

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        UpdateSoundUI();
    }

	public override void Open ()
	{
		base.Open ();

		if ( openButton != null)
			openButton.SetActive (false);
	}

	public override void Close ()
	{
		base.Close ();
	}

	public override void Hide ()
	{
		base.Hide ();

		if ( openButton != null)
			openButton.SetActive (true);
	}

	public override void Update ()
	{
		base.Update ();

        if (!opened)
        {
            if (Input.GetKeyDown(KeyCode.Escape)){
                Open();
            }
        }
	}

    public override void Return()
    {
        //base.Return();

        if ( SceneManager.GetActiveScene().name == "map" )
        {
			QuitGame();
        }
        else
        {
			RetourCarte ();
        }
    }

  

    public void NewGame()
    {
        Close();

        Tween.Bounce( newGameButton.transform );

        PlayerPrefs.DeleteAll();

		CancelInvoke ("NewGameDelay");
        Invoke("NewGameDelay", 1f);

        Transition.Instance.Fade(1f);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Open);
    }

    void NewGameDelay()
    {
        SceneManager.LoadScene("map");
    }

	#region screen orientation
	public void SwitchScreenOrientation(){

		Transition.Instance.Fade(0.3f);

		CancelInvoke ("SwitchScreenOrientationDelay");
		Invoke ("SwitchScreenOrientationDelay", 0.3f);
	}

	void SwitchScreenOrientationDelay() {

		Inventory.Instance.portrait = !Inventory.Instance.portrait;

		Inventory.Instance.UpdateScreenOrientation ();

		Inventory.Instance.Save ();

		CancelInvoke ("SwitchScreenOrientationDelay2");
		Invoke ("SwitchScreenOrientationDelay2", 0.5f);


	}
	void SwitchScreenOrientationDelay2(){
		Transition.Instance.Clear (0.3f);

	}
	#endregion

    public void LoadGame()
    {
		CancelInvoke ("LoadGameDelay");
        Invoke("LoadGameDelay", 1f);

        SoundManager.Instance.Play(SoundManager.SoundType.Door_Open);
        Transition.Instance.Fade(1f);
    }

    void LoadGameDelay()
    {

        SceneManager.LoadScene("map");
    }

	void HandleOnConfirm ()
	{
		QuitGameConfirm ();
	}

    public void QuitGame()
    {
		DisplayConfirm.Instance.Open ();
		DisplayConfirm.Instance.onConfirm += HandleOnConfirm;

		Time.timeScale = 1f;

    }

	void QuitGameConfirm(){
		
		CancelInvoke ("QuitGameDelay");
		Invoke("QuitGameDelay", 1f);

		Tween.Bounce(quitGameButton.transform);

		SoundManager.Instance.Play(SoundManager.SoundType.Door_Close);
		Time.timeScale = 1f;
		Transition.Instance.Fade(1f);
	}

    void QuitGameDelay()
    {
        Application.Quit();
    }

    public void RetourCarte()
    {
        Close();

        Tween.Bounce(mapButton.transform);

		CancelInvoke ("LoadGameDelay");
        Invoke("LoadGameDelay", 1f);

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Bip);
        Transition.Instance.Fade(1f);
    }

    public void Sounds_On()
    {
        SoundManager.Instance.playSounds = true;
        Music.Instance.source.enabled = true;

        Inventory.Instance.Save();

        UpdateSoundUI();
    }

    public void Sounds_Off()
    {
        SoundManager.Instance.playSounds = false;
        Music.Instance.source.enabled = false;

        Inventory.Instance.Save();

        UpdateSoundUI();
    }

    void UpdateSoundUI()
    {
        if (SoundManager.Instance.playSounds)
        {
            sound_OffTransform.localScale = Vector3.one;
            sound_OnTransform.localScale = Vector3.one * 1.2f;
        }
        else
        {
            sound_OnTransform.localScale = Vector3.one;
            sound_OffTransform.localScale = Vector3.one * 1.2f;
        }
        
    }



}
