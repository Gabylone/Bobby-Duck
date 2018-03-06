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

	public bool loadOnStart = false;

	public float fadeDuration = 1f;

	void Start () {

		Instance = this;

		InitializeGame ();

	}

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

			MapGenerator.Instance.GenerateIslands ();

			LootManager.Instance.CreateNewLoot ();

			Crews.Instance.RandomizePlayerCrew ();

			Boats.Instance.RandomizeBoats ();

			GoldManager.Instance.InitGold ();

			TimeManager.Instance.Reset ();

		}

		CrewInventory.Instance.Init ();

		WeightManager.Instance.Init ();

		QuestMenu.Instance.Init ();

		NavigationManager.Instance.ChangeChunk (Directions.None);

		if (KeepOnLoad.dataToLoad < 0) {
			
			MemberCreator.Instance.Show ();

			Transitions.Instance.ActionTransition.Fade = true;

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
