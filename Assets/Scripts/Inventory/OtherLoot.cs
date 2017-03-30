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

	[Header("Sounds")]
	[SerializeField] private AudioClip equipSound;
	[SerializeField] private AudioClip lootSound;

	[SerializeField]
	private ActionGroup actionGroup;

	bool trading = false;

	void Awake () {
		Instance = this;
	}

	#region trade
	public void StartTrade () {

		PlayerLoot.Instance.Open ();
		PlayerLoot.Instance.InventoryButton.Locked = true;
		PlayerLoot.Instance.CloseButton.SetActive (false);

			// player loot ui
		playerLootUI.CategoryContent = PlayerLoot.Instance.TradeCategoryContent;
		playerLootUI.Visible = true;
		playerLootUI.UpdateActionButton (0);

		otherLootUI.CategoryContent = category_TradeContent;
		otherLootUI.Visible = true;
		otherLootUI.UpdateActionButton(0);

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

		SoundManager.Instance.PlaySound (lootSound);

		buttonObj.SetActive (false);

		otherLootUI.CategoryContent = category_OtherLootContent;
		otherLootUI.Visible = true;
		otherLootUI.UpdateActionButton(0);
//
//		PlayerLoot.Instance.ActionGroup.UpdateButtons (ActionGroup.ButtonType.Throw);
//		actionGroup.UpdateButtons (ActionGroup.ButtonType.PickUp);

	}
	#endregion

	public void Buy () {
		if (!GoldManager.Instance.CheckGold (otherLootUI.SelectedItem.price))
			return;

		if (!WeightManager.Instance.CheckWeight (otherLootUI.SelectedItem.weight))
			return;

		GoldManager.Instance.GoldAmount -= otherLootUI.SelectedItem.price;

		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();
	}

	public void PickUp () {
		SoundManager.Instance.PlaySound (equipSound);

		if (!WeightManager.Instance.CheckWeight (otherLootUI.SelectedItem.weight))
			return;

		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();
	}

	public void Close () {
		
		buttonObj.SetActive (true);

		otherLootUI.Visible = false;
		playerLootUI.Visible = false;

		trading = false;

		PlayerLoot.Instance.InventoryButton.Locked = false;
		PlayerLoot.Instance.CloseButton.SetActive (true);
		PlayerLoot.Instance.InventoryButton.Opened = false;
		PlayerLoot.Instance.Close ();

		if ( IslandManager.Instance.OnIsland ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
		}

	}

}
