using UnityEngine;
using System.Collections;

public class OtherInventory : MonoBehaviour {

	public static OtherInventory Instance;

	public enum Type {
		None,

		Loot,
		Trade,
	}

	public Type type = Type.None;

	void Awake () {
		Instance = this;
	}

	void Start () {

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		LootUI.useInventory += HandleUseInventory;
	}

	public void SwitchLoot () {
		//
	}

	public void SwitchToPlayer () {
		LootUI.useInventory -= HandleUseInventory;
		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerTrade);
	}

	public void SwitchToOther () {
		LootUI.useInventory += HandleUseInventory;

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

			// get loot
		ItemLoader.Instance.Mult = 2;
		Loot loot = LootManager.Instance.GetIslandLoot ();
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		LootUI.Instance.Show (CategoryContentType.OtherTrade);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Trade;
	}
	#endregion

	#region looting
	public void StartLooting () {

		Loot loot = LootManager.Instance.GetIslandLoot ();
		LootManager.Instance.setLoot ( Crews.Side.Enemy, loot);

		LootUI.Instance.Show (CategoryContentType.OtherLoot);

		CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot);

		type = Type.Loot;

	}
	#endregion

	public void Buy () {

		if (!GoldManager.Instance.CheckGold (LootUI.Instance.SelectedItem.price)) {
			return;
		}

		if (!WeightManager.Instance.CheckWeight (LootUI.Instance.SelectedItem.weight)) {
			return;
		}

		GoldManager.Instance.GoldAmount -= LootUI.Instance.SelectedItem.price;

		LootManager.Instance.PlayerLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	public void PickUp () {
		
		if (!WeightManager.Instance.CheckWeight (LootUI.Instance.SelectedItem.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);
	}

	#region open / close
	public void Close () {

		LootUI.Instance.Hide ();

		type = Type.None;

		CrewInventory.Instance.HideInventory ();

		Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		if ( StoryLauncher.Instance.PlayingStory ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			if ( CombatManager.Instance.Fighting ) {
				CombatManager.Instance.EndFight ();
			}
		}

	}
	#endregion
}
