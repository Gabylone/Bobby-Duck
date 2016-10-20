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
		playerPreviousLootActions = playerLootUI.ActionButtonTexts;
		playerLootUI.ActionButtonTexts = new string[4] {
			"Vendre","Vendre","Vendre","Vendre"
		};

		playerLootUI.Show ();
		playerLootUI.UpdateActionButton (0);

			// enemy loot ui
		LootManager.Instance.OtherLoot.Randomize ();
		otherLootUI.ActionButtonTexts = new string[4] {
			"Acheter","Acheter","Acheter","Acheter"
		};
		otherLootUI.Show ();
		otherLootUI.UpdateActionButton(0);

		trading = true;
	}

	public void EndTrade () {

		buttonObj.SetActive (true);
		playerLootUI.ActionButtonTexts = playerPreviousLootActions;

		otherLootUI.Hide ();
		playerLootUI.Hide ();

		trading = false;

	}

	public bool Trading {
		get {
			return trading;
		}
	}

	public void SellItem ( ItemLoader.ItemType category , int index ) {

		GoldManager.Instance.AddGold (playerLootUI.SelectedItem.price);

		LootManager.Instance.OtherLoot.AddItem (category, playerLootUI.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (category, index);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();


	}
	public void BuyItem () {

		GoldManager.Instance.RemoveGold (otherLootUI.SelectedItem.price);

		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.CurrentCategory, otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.CurrentCategory, otherLootUI.ItemIndex);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();

	}
	#endregion

	#region looting
	public void StartLooting () {

		buttonObj.SetActive (false);

		// enemy loot ui
		LootManager.Instance.OtherLoot.Randomize ();
		otherLootUI.ActionButtonTexts = new string[4] {
			"Prendre","Prendre","Prendre","Prendre"
		};

		otherLootUI.Show ();
		otherLootUI.UpdateActionButton(0);

	}
	public void PickUpItem () {
		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.CurrentCategory, otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.CurrentCategory, otherLootUI.ItemIndex);

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

		otherLootUI.Hide ();

		if (trading) {
			trading = false;
			playerLootUI.ActionButtonTexts = playerPreviousLootActions;
			playerLootUI.Hide ();
		} else {
			trading = false;
		}
		

	}

}
