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

		if (Application.isMobilePlatform)
			Screen.fullScreen = true;
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

		StoryReader.Instance.CurrentStoryHandler = MapData.Instance.currentChunk.IslandData.StoryHandler;
		StoryLauncher.Instance.CurrentStorySource = StoryLauncher.StorySource.island;
		StoryLauncher.Instance.PlayingStory = true;

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
