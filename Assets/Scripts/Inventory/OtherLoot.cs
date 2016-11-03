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
	[SerializeField] private AudioClip buySound;
	[SerializeField] private AudioClip sellSound;
	[SerializeField] private AudioClip equipSound;
	[SerializeField] private AudioClip lootSound;

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

		if ( IslandManager.Instance.OnIsland ) {
			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
		}

	}

	public bool Trading {
		get {
			return trading;
		}
	}

	public void SellItem ( ItemCategory category , int index ) {

		SoundManager.Instance.PlaySound (sellSound);

		GoldManager.Instance.AddGold (playerLootUI.SelectedItem.price);

		Debug.Log (otherLootUI.SelectedItem.weight.ToString( ));
		WeightManager.Instance.RemoveWeight (playerLootUI.SelectedItem.weight);

		LootManager.Instance.OtherLoot.AddItem (playerLootUI.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (playerLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();


	}
	public void BuyItem () {

		SoundManager.Instance.PlaySound (buySound);

		if (!WeightManager.Instance.CheckWeight (otherLootUI.SelectedItem.weight))
			return;
		WeightManager.Instance.AddWeight (otherLootUI.SelectedItem.weight);

		GoldManager.Instance.RemoveGold (otherLootUI.SelectedItem.price);

		LootManager.Instance.PlayerLoot.AddItem (otherLootUI.SelectedItem);
		LootManager.Instance.OtherLoot.RemoveItem (otherLootUI.SelectedItem);

		playerLootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();

	}
	#endregion

	#region looting
	public void StartLooting () {

		SoundManager.Instance.PlaySound (lootSound);

		buttonObj.SetActive (false);

		// enemy loot ui
		LootManager.Instance.OtherLoot.Randomize (ItemLoader.allCategories);

		otherLootUI.CategoryContent = category_OtherLootContent;
		otherLootUI.Visible = true;
		otherLootUI.UpdateActionButton(0);

	}
	public void PickUpItem () {

		SoundManager.Instance.PlaySound (equipSound);

		if (!WeightManager.Instance.CheckWeight (otherLootUI.SelectedItem.weight))
			return;
		WeightManager.Instance.AddWeight (otherLootUI.SelectedItem.weight);

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

			EndTrade ();
		} else {
			trading = false;
		}
		

	}

}
