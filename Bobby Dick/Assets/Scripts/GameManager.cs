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
		LootManager.Instance.Init ();
		ClueManager.Instance.Init ();
		Crews.Instance.Init ();

		if (KeepOnLoad.dataToLoad < 0) {

			MapGenerator.Instance.GenerateIslands ();

			Crews.Instance.RandomizePlayerCrew ();
			LootManager.Instance.CreateNewLoot ();
			ClueManager.Instance.CreateNewClues ();

			Boats.Instance.RandomizeBoats ();

		} else {
			SaveManager.Instance.LoadGame (KeepOnLoad.dataToLoad);
		}

		Boats.Instance.Init ();

		MapImage.Instance.InitImage ();
		MapImage.Instance.Init ();

		Island.Instance.Init ();

		NavigationManager.Instance.ChangeChunk (Directions.None);

		if (KeepOnLoad.dataToLoad < 0) {
			MemberCreator.Instance.Show ();
			Transitions.Instance.ActionTransition.Fade = true;
		} else {
			
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
