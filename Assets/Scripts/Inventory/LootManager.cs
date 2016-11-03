using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private CategoryContent defaultCategoryContent;

	private Loot playerLoot;
	private Loot otherLoot;

	public void Init () {
		
		Instance = this;

		playerLoot = new Loot ();
		playerLoot.Randomize ( ItemLoader.allCategories );

		Item[] items = playerLoot.getCategory (ItemLoader.allCategories);

		foreach ( Item item in items ) {
			WeightManager.Instance.AddWeight ( item.weight );
		}


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

	public void setLoot ( Crews.Side side , Loot targetLoot) {
		otherLoot = targetLoot;
	}

	public CategoryContent DefaultCategoryContent {
		get {
			return defaultCategoryContent;
		}
	}
}
