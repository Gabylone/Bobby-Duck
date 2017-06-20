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

		MapGenerator.Instance.GenerateIslands ();

		ItemLoader.Instance.Init ();
		LootManager.Instance.Init ();
		ClueManager.Instance.Init ();

		Crews.Instance.Init ();

		Boats.Instance.Init ();

		MapImage.Instance.InitImage ();
		MapImage.Instance.Init ();

		StoryLauncher.Instance.PlayStory (MapData.Instance.currentChunk.IslandData.storyManager,StoryLauncher.StorySource.island);

		if ( PlayerBoatInfo.Instance.PosX == MapData.Instance.homeIslandXPos &&
			PlayerBoatInfo.Instance.PosY == MapData.Instance.homeIslandYPos ) {
		}

		NavigationManager.Instance.ChangeChunk (Directions.None);

		Island.Instance.Init ();

	}

	public void Restart () {
		SceneManager.LoadScene ("Main");
	}

	public void GameOver (float delay) {
		Invoke ("GameOver", delay);
	}

	public void GameOver () {
		gameOver_Object.SetActive (true);
	}
}
