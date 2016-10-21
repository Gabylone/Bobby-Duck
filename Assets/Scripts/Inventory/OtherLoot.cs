using UnityEngine;
using System.Collections;

public class OtherLoot : MonoBehaviour {

	public static OtherLoot Instance;

	[SerializeField]
	private LootUI playerLootUI;

	[SerializeField]
	private LootUI otherLootUI;

	[SerializeField]
	private GameObject buttonObj;

	[SerializeField]
	private CategoryContent category_TradeContent;
	[SerializeField]
	private CategoryContent category_OtherLootContent;

	private string[] playerPreviousLootActions;

	bool trading = false;

	void Awake () {
		Instance = this;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.L)) {
			if (trading)
				EndTrade ();
			else
				StartTrade ();
		}
	}

	#region trade
	public void StartTrade () {

		buttonObj.SetActive (false);

			// player loot ui
		playerLootUI.CategoryContent = PlayerLoot.Instance.TradeCategoryContent;
		playerLootUI.Visible = true;
		playerLootUI.UpdateActionButton (0);

			// enemy loot ui
		LootManager.Instance.OtherLoot.Randomize ();

		otherLootUI.CategoryContent = category_TradeContent;
		otherLootUI.Visible = true;
		otherLootUI.UpdateActionButton(0);

		trading = true;
	}

	public void EndTrade () {

		buttonObj.SetActive (true);

		otherLootUI.Visible = false;
		playerLootUI.Visible = false;

		trading = false;

	}

	public bool Trading {
		get {
			return trading;
		}
	}

	public void SellItem ( ItemCategory category , int index ) {

		GoldManager.Instance.AddGold (playerLootUI.SelectedItem.price);

		LootManager.Instance.OtherLoot.AddItem (playerLootUI.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (playerLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();


	}
	public void BuyItem () {

		GoldManager.Instance.RemoveGold (otherLootUI.SelectedItem.price);

		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (playerLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();

	}
	#endregion

	#region looting
	public void StartLooting () {

		buttonObj.SetActive (false);

		// enemy loot ui
		LootManager.Instance.OtherLoot.Randomize ();
		otherLootUI.CategoryContent = category_OtherLootContent;
		otherLootUI.Visible = true;
		otherLootUI.UpdateActionButton(0);

	}
	public void PickUpItem () {
		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();
	}
	#endregion

	public void Action () {
		if ( trading ) {
			BuyItem ();
		} else {
			PickUpItem ();
		}
	}

	public void Close () {
		
		buttonObj.SetActive (true);

		otherLootUI.Visible = false;

		if (trading) {
			trading = false;
			playerLootUI.ActionButtonTexts = playerPreviousLootActions;
			playerLootUI.Visible = false;
		} else {
			trading = false;
		}
		

	}

}
