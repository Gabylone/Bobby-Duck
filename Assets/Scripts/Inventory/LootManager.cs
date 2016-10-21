using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private CategoryContent defaultCategoryContent;

	private Loot playerLoot;
	private Loot otherLoot;

	void Awake () {
		
		Instance = this;

		playerLoot = new Loot ();
		playerLoot.Randomize ();

		otherLoot = new Loot ();
	}

	public Loot PlayerLoot {
		get {
			return playerLoot;
		}
	}

	public Loot OtherLoot {
		get {
			return otherLoot;
		}
	}

	public Loot getLoot (Crews.Side side) {
		return side == Crews.Side.Player ? playerLoot : otherLoot;
	}

	public CategoryContent DefaultCategoryContent {
		get {
			return defaultCategoryContent;
		}
	}
}
