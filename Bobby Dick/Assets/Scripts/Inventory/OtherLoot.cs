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

		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Loot:
			StartLooting ();
			break;
		case FunctionType.Trade:
			StartTrade ();
			break;

		}
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

		ItemLoader.Instance.Mult = 3;
		Loot loot = LootManager.Instance.GetIslandLoot ();
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);
		lootUi.Show (CategoryContentType.OtherTrade);

		PlayerLoot.Instance.CanOpen = true;
		PlayerLoot.Instance.ShowInventory (CategoryContentType.PlayerTrade);

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

		Loot loot = LootManager.Instance.GetIslandLoot ();
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);
		lootUi.Show (CategoryContentType.OtherLoot);

		PlayerLoot.Instance.CanOpen = true;
		PlayerLoot.Instance.ShowInventory (CategoryContentType.PlayerLoot);

	}
	#endregion

	public void Buy () {


		if (!GoldManager.Instance.CheckGold (lootUi.SelectedItem.price)) {
			print ("pas assez d'or");
			return;
		}

		if (!WeightManager.Instance.CheckWeight (lootUi.SelectedItem.weight)) {
			print ("pas asse zde place");
			return;
		}

		GoldManager.Instance.GoldAmount -= lootUi.SelectedItem.price;

		LootManager.Instance.PlayerLoot.AddItem (lootUi.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (lootUi.SelectedItem);

		WeightManager.Instance.UpdateDisplay ();

		playerLootUI.UpdateLootUI ();
		lootUi.UpdateLootUI ();
	}

	public void PickUp () {
		
		if (!WeightManager.Instance.CheckWeight (lootUi.SelectedItem.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (lootUi.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (lootUi.SelectedItem);

		WeightManager.Instance.UpdateDisplay ();

		playerLootUI.UpdateLootUI ();
		lootUi.UpdateLootUI ();
	}

	#region open / close
	public void Close () {

		lootUi.Hide ();
		playerLootUI.Hide ();

		trading = false;

		PlayerLoot.Instance.CanOpen = false;

		PlayerLoot.Instance.HideInventory ();

		Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		if ( StoryLauncher.Instance.PlayingStory ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			if ( CombatManager.Instance.Fighting ) {
				CombatManager.Instance.EndFight ();
			}
		}

	}

	public LootUI LootUi {
		get {
			return lootUi;
		}
	}
	#endregion
}
