using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootUI : MonoBehaviour {

	ItemLoader.ItemType currentCategory = ItemLoader.ItemType.Weapon;

	[Header("Item Buttons")]
	[SerializeField]
	private GameObject itemButtonGroup;
	private ItemButton[] itemButtons;

	private int currentPage 	= 0;
	private int maxPage 		= 0;

	[Header("Action Button")]
	[SerializeField] private GameObject actionButtonObj;
	[SerializeField] private string[] actionButtonTexts;
	private int selectionIndex = 0;

	[Header("Category")]
	[SerializeField] private Button[] categoryButtons;

	[Header("Pages")]
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;



	void Start () {

		itemButtons = itemButtonGroup.GetComponentsInChildren<ItemButton>();
		int a = 0;
		foreach ( ItemButton itemButton in itemButtons ) {
			itemButton.Index = a;
			++a;
		}
		SwitchCategory (0);
	}

	void Update () {

		if ( Input.GetKeyDown (KeyCode.L))  {
			LootManager.playerLoot.RemoveItem (currentCategory, 1);
			UpdateLootUI ();
		}
	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		foreach ( ItemButton itemButton in itemButtons ) {

			itemButton.gameObject.SetActive ( a < LootManager.playerLoot.getCategory (currentCategory).Length );

			if ( a < LootManager.playerLoot.getCategory (currentCategory).Length ) {

				Item item = ItemLoader.Instance.getItem (currentCategory,LootManager.playerLoot.getCategory (currentCategory)[a].ID);

				itemButton.Name 		= item.name;
				itemButton.Description 	= item.description;

				itemButton.gameObject.SetActive (true);

				itemButton.Param = currentCategory != ItemLoader.ItemType.Mics ? item.value : 0;

				itemButton.Price 		= item.price;
			}

			a++;
		}
	}
	#endregion



	#region category navigation
	public void SwitchCategory ( int cat ) {

		if ( cat == (int)currentCategory)
			return;

		categoryButtons [(int)currentCategory].interactable = true;
		categoryButtons [cat].interactable = false;

		currentCategory = (ItemLoader.ItemType)cat;
		currentPage = 0;

		UpdateLootUI ();
	}

	public void UpdateLootUI () {
		UpdateActionButton (0);
		UpdatePages ();
		UpdateItemButtons ();
	}
	#endregion

	#region page navigation
	public void NextPage () {
		++currentPage;
		UpdateLootUI ();
	}
	public void PreviousPage () {
		--currentPage;
		UpdateLootUI ();
	}

	private void UpdatePages () {
		maxPage = Mathf.CeilToInt ( LootManager.playerLoot.getCategory (currentCategory).Length / ItemPerPage);

		previousPageButton.SetActive( currentPage > 0 );
		nextPageButton.SetActive( currentPage < maxPage );
	}

	public int ItemPerPage {
		get {
			return itemButtons.Length;
		}
	}
	#endregion

	#region action button
	public void UpdateActionButton (int index) {
		bool actionButtonActive = LootManager.playerLoot.getCategory (currentCategory).Length > 0 && currentCategory != ItemLoader.ItemType.Mics;
		actionButtonObj.SetActive (actionButtonActive);

		Vector3 targetPos = actionButtonObj.transform.position;

		targetPos.y = itemButtons [index].transform.position.y;

		actionButtonObj.transform.position = targetPos;

		actionButtonObj.GetComponentInChildren<Text> ().text = actionButtonTexts[(int)currentCategory];

		selectionIndex = index;

	}
	#endregion

	public Item SelectedItem {
		get {
			return LootManager.playerLoot.getCategory (currentCategory)[currentPage+selectionIndex];
		}
	}

	public ItemLoader.ItemType CurrentCategory {
		get {
			return currentCategory;
		}
	}


	public int CurrentPage {
		get {
			return currentPage;
		}
	}

	public int SelectionIndex {
		get {
			return selectionIndex;
		}
	}
}
