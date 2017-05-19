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

		PlayerBoatInfo.Instance.UpdatePosition ();

		StoryLauncher.Instance.PlayingStory = true;

	}

	public void Restart () {
		SceneManager.LoadScene ("Main");
	}

	public void GameOver () {
		gameOver_Object.SetActive (true);
	}
}
