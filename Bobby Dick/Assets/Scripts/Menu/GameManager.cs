﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Holoville.HOTween;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	[SerializeField]
	private GameObject overallObj;

	public GameObject textObj;

	public Image image;

	public bool loadOnStart = false;
	public bool saveOnStart = false;
	public bool hideMemberCreation = false;

	public float fadeDuration = 1f;

	void Start () {

		Instance = this;

		InitializeGame ();
//		StartCoroutine( InitializeGame () );

	}

//	IEnumerator InitializeGame () {
	public void InitializeGame () {

		ItemLoader.Instance.Init ();

		FormulaManager.Instance.Init ();

		Crews.Instance.Init ();

		if (loadOnStart) {
			
			KeepOnLoad.dataToLoad = 0;
			SaveManager.Instance.LoadGame ();

		} else if (KeepOnLoad.dataToLoad >= 0) {

			SaveManager.Instance.LoadGame ();

		} else {

			MapGenerator.Instance.CreateNewMap ();

			LootManager.Instance.CreateNewLoot ();

			Crews.Instance.RandomizePlayerCrew ();

			Boats.Instance.RandomizeBoats ();

			GoldManager.Instance.InitGold ();

			TimeManager.Instance.Reset ();

            KeepOnLoad.displayTuto = true;

            /// TEST
            /// 


        }

		InGameMenu.Instance.Init ();

		WeightManager.Instance.Init ();

		QuestMenu.Instance.Init ();

		if (KeepOnLoad.dataToLoad < 0) {

			if (hideMemberCreation ) {
				StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
			} else {
				MemberCreator.Instance.Show ();
			}


		}

	}

	public void Restart () {
		SceneManager.LoadScene ("Menu");
	}

	public void GameOver (float delay) {
		Invoke ("GameOver", delay);
	}

	void GameOver () {

		overallObj.SetActive (true);

		image.color = Color.clear;
		HOTween.To ( image , fadeDuration , "color" , Color.black  );

		textObj.SetActive (false);

		SaveTool.Instance.DeleteGameData ();

		Invoke ("GameOverDelay",fadeDuration);
	}

	void GameOverDelay () {
		textObj.SetActive (true);
		Tween.Bounce ( textObj.transform );
	}

    public void QuitGame()
    {
        Transitions.Instance.ScreenTransition.FadeIn(1);

        Invoke("QuitGameDelay" , 1f);
    }

    void QuitGameDelay()
    {
        Application.Quit();
    }
}
