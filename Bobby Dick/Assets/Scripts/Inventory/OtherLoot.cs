using UnityEngine;
using System.Collections;

public class OtherLoot : MonoBehaviour {

	public static OtherLoot Instance;

	[SerializeField]
	private LootUI playerLootUI;

	[SerializeField]
	private LootUI lootUi;

	[SerializeField]
	private CategoryContent category_TradeContent;
	[SerializeField]
	private CategoryContent category_OtherLootContent;

	private string[] playerPreviousLootActions;

	[SerializeField]
	private ActionGroup actionGroup;

	bool trading = false;

	void Awake () {
		Instance = this;
	}

	void Start () {
		lootUi.useInventory += HandleUseInventory;
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Buy:
			Buy ();
			break;
		case InventoryActionType.PickUp:
			PickUp ();
			break;
		default:
			break;
		}
	}

	#region trade
	public void StartTrade () {
			// player loot ui

		PlayerLoot.Instance.Open(PlayerLoot.Instance.TradeCategoryContent);
		playerLootUI.Visible = true;
		playerLootUI.UpdateActionButton (0);

		lootUi.Show (category_TradeContent);
		lootUi.UpdateActionButton(0);

		trading = true;
	}

	public bool Trading {
		get {
			return trading;
		}
	}
	#endregion

	#region looting
	public void StartLooting () {

		lootUi.Show (category_OtherLootContent);
		lootUi.UpdateActionButton(0);

	}
	#endregion

	public void Buy () {
		if (!GoldManager.Instance.CheckGold (lootUi.SelectedItem.price))
			return;

		if (!WeightManager.Instance.CheckWeight (lootUi.SelectedItem.weight))
			return;

		GoldManager.Instance.GoldAmount -= lootUi.SelectedItem.price;

		LootManager.Instance.PlayerLoot.AddItem (lootUi.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (lootUi.SelectedItem);

		playerLootUI.UpdateLootUI ();
		lootUi.UpdateLootUI ();
	}

	public void PickUp () {
		if (!WeightManager.Instance.CheckWeight (lootUi.SelectedItem.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (lootUi.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (lootUi.SelectedItem);

		playerLootUI.UpdateLootUI ();
		lootUi.UpdateLootUI ();
	}

	#region open / close
	public void Close () {

		if ( CombatManager.Instance.Fighting ) {
			CombatManager.Instance.Fighting = false;
		}

		lootUi.Visible = false;
		playerLootUI.Visible = false;

		trading = false;

		PlayerLoot.Instance.CanOpen = true;
		PlayerLoot.Instance.Close ();

		if ( StoryLauncher.Instance.PlayingStory ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
		}

	}

	public LootUI LootUi {
		get {
			return lootUi;
		}
	}
	#endregion
}
