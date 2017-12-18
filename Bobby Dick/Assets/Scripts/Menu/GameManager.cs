using UnityEngine;
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

	public float fadeDuration = 1f;

	void Start () {

		Instance = this;

		LoadGame ();
	}

	public void LoadGame () {

		Transitions.Instance.ScreenTransition.Fade = false;

		ItemLoader.Instance.Init ();
		FormulaManager.Instance.Init ();
		Crews.Instance.Init ();



		if (KeepOnLoad.dataToLoad < 0) {

			MapGenerator.Instance.GenerateIslands ();

			LootManager.Instance.CreateNewLoot ();

			Crews.Instance.RandomizePlayerCrew ();

			FormulaManager.Instance.CreateNewClues ();

			Boats.Instance.RandomizeBoats ();

			GoldManager.Instance.InitGold ();

		} else {
			SaveManager.Instance.LoadGame ();
		}

		CrewInventory.Instance.Init ();

		WeightManager.Instance.Init ();

		TimeManager.Instance.Init ();

		QuestMenu.Instance.Init ();

		NavigationManager.Instance.ChangeChunk (Directions.None);

		if (KeepOnLoad.dataToLoad < 0) {
			MemberCreator.Instance.Show ();
			Transitions.Instance.ActionTransition.Fade = true;

//			SaveManager.Instance.SaveGame (1);

		}


	}

	public void Restart () {
		SceneManager.LoadScene ("Menu");
	}

	public void GameOver (float delay) {
		Invoke ("GameOver", delay);
	}

	public void GameOver () {

		overallObj.SetActive (true);

		image.color = Color.clear;
		HOTween.To ( image , fadeDuration , "color" , Color.black  );

		textObj.SetActive (false);

		Invoke ("GameOverDelay",fadeDuration);
	}

	void GameOverDelay () {
		textObj.SetActive (true);
		Tween.Bounce ( textObj.transform );
	}
}
