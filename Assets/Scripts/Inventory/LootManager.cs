using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private Loot playerLoot;
	[SerializeField]
	private Loot enemyLoot;

	void Awake () {
		Instance = this;
		playerLoot.Randomize ();
	}

	public Loot PlayerLoot {
		get {
			return playerLoot;
		}
	}

	public Loot EnemyLoot {
		get {
			return enemyLoot;
		}
	}

	public Loot getLoot (Crews.Side side) {
		return side == Crews.Side.Player ? playerLoot : enemyLoot;
	}
}
