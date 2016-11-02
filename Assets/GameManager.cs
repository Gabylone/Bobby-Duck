using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private ItemLoader itemLoader;

	[SerializeField]
	private LootManager lootManager;

	void Awake () {
		itemLoader.Init ();
		lootManager.Init ();
	}
}
