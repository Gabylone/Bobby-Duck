using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	[SerializeField]
	private GameObject gameOver_Object;

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

			Crews.Instance.RandomizePlayerCrew ();
			LootManager.Instance.CreateNewLoot ();

			FormulaManager.Instance.CreateNewClues ();

			Boats.Instance.RandomizeBoats ();

			GoldManager.Instance.InitGold ();

		} else {
			SaveManager.Instance.LoadGame (KeepOnLoad.dataToLoad);
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
		gameOver_Object.SetActive (true);
	}
}
