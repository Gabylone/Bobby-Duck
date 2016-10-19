using UnityEngine;
using System.Collections;

public class TradeManager : MonoBehaviour {

	public static TradeManager Instance;

	[SerializeField]
	private LootUI playerLootUI;

	[SerializeField]
	private LootUI enemyLootUI;

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
		LootManager.Instance.EnemyLoot.Randomize ();
		enemyLootUI.ActionButtonTexts = new string[4] {
			"Acheter","Acheter","Acheter","Acheter"
		};
		enemyLootUI.Show ();
		enemyLootUI.UpdateActionButton(0);

		trading = true;
	}

	public void EndTrade () {

		buttonObj.SetActive (true);
		playerLootUI.ActionButtonTexts = playerPreviousLootActions;

		enemyLootUI.Hide ();
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

		LootManager.Instance.EnemyLoot.AddItem (category, playerLootUI.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (category, index);

		playerLootUI.UpdateLootUI ();
		enemyLootUI.UpdateLootUI ();


	}

	public void Action () {
		if ( trading ) {
			BuyItem ();
		} else {
			
		}
	}

	public void PickUpItem () {
		//
	}

	public void BuyItem () {

		GoldManager.Instance.RemoveGold (enemyLootUI.SelectedItem.price);

		LootManager.Instance.PlayerLoot.AddItem (enemyLootUI.CurrentCategory, enemyLootUI.SelectedItem);
		LootManager.Instance.EnemyLoot.RemoveItem (enemyLootUI.CurrentCategory, enemyLootUI.ItemIndex);

		playerLootUI.UpdateLootUI ();
		enemyLootUI.UpdateLootUI ();

	}
}
