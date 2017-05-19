using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private CategoryContent defaultCategoryContent;

	private Loot playerLoot;
	private Loot otherLoot;

	void Awake (){
		Instance = this;
	}

	public void Init () {

		Loot playerLoot = new Loot (0, 0);
		playerLoot.Randomize (ItemCategory.Provisions);

		setLoot (Crews.Side.Player, playerLoot);

		WeightManager.Instance.UpdateDisplay ();

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

	public Loot GetIslandLoot ( ItemCategory[] categories ) {

		int row = StoryReader.Instance.Decal;
		int col = StoryReader.Instance.Index;

		var tmpLoot = MapData.Instance.currentChunk.IslandData.Loots.Find (x => x.col == col && x.row == row);

		if (tmpLoot == null) {

			Loot newLoot = new Loot (row , col);

			newLoot.Randomize (categories);

			MapData.Instance.currentChunk.IslandData.Loots.Add (newLoot);

			return newLoot;

		}

		return tmpLoot;
	}

	public void setLoot ( Crews.Side side , Loot targetLoot) {
		if (side == Crews.Side.Player) {
			
			playerLoot = targetLoot;

		} else {
			
			otherLoot = targetLoot;

		}
	}
}
