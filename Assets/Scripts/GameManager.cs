using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	[SerializeField]
	private ItemLoader itemLoader;

	[SerializeField]
	private LootManager lootManager;

	[SerializeField]
	private GameObject gameOver_Object;

	void Awake () {

		Instance = this;

		LoadGame ();
	}

	public void LoadGame () {

		itemLoader.Init ();
		lootManager.Init ();
		MapManager.Instance.Init ();
		ClueManager.Instance.Init ();

	}

	public void GameOver () {
		gameOver_Object.SetActive (true);
	}
}
