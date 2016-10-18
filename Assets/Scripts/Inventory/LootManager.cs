using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static Loot playerLoot;
	public static Loot enemyLoot;

	void Awake () {
		playerLoot = GetComponentsInChildren<Loot> () [0];
		enemyLoot = GetComponentsInChildren<Loot> () [1];
	}

	public Loot getLoot (Crews.Side side) {
		return side == Crews.Side.Player ? playerLoot : enemyLoot;
	}
}
