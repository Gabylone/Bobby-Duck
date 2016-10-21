﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootUI : MonoBehaviour {

	private int currentCat = 0;

	[SerializeField]
	private GameObject lootObj;

	[SerializeField]
	private Crews.Side side;

	bool visible = false;

	[Header("Item Buttons")]
	[SerializeField]
	private GameObject itemButtonGroup;
	private ItemButton[] itemButtons = new ItemButton[0];

	private int currentPage 	= 0;
	private int maxPage 		= 0;

	[Header("Action Button")]
	[SerializeField] private GameObject actionButtonObj;
	[SerializeField] private string[] actionButtonTexts;
	private int selectionIndex = 0;

	[Header("Categories")]
	[SerializeField] private Button[] categoryButtons;
	private CategoryContent categoryContent;

	[Header("Pages")]
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;

	private Item[] selectedItems;

	void Awake () {
		Init ();
	}

	private void Init () {

		if ( itemButtons.Length == 0 )
			itemButtons = itemButtonGroup.GetComponentsInChildren<ItemButton>();

		int a = 0;
		foreach ( ItemButton itemButton in itemButtons ) {
			itemButton.Index = a;
			++a;
		}

		Visible = false;
	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;
			lootObj.SetActive (value);
			if ( value == true )
				SwitchCategory (0);
		}
	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		for (int i = 0; i < ItemPerPage; ++i ) {

			ItemButton itemButton = itemButtons [i];

			itemButton.gameObject.SetActive ( a < SelectedItems.Length );

			if ( a < SelectedItems.Length ) {
				Debug.Log (LootManager.Instance.PlayerLoot.getCategory (ItemCategory.Provisions).Length);

//				Debug.Log (a.ToString ());
				Item item = SelectedItems[a];

				itemButton.gameObject.SetActive (true);

				itemButton.Name 		= item.name;
				itemButton.Description 	= item.description;

				itemButton.Param 		= CategoryContent.itemCategories[currentCat].categories[0] != ItemCategory.Mics ? item.value : 0;

				itemButton.Price 		= item.price;
			}

			a++;
		}
	}
	#endregion



	#region category navigation
	public void SwitchCategory ( int cat ) {

		categoryButtons [currentCat].interactable = true;

		currentCat = cat;

		categoryButtons [cat].interactable = false;

		currentPage = 0;

		UpdateLootUI ();
	}

	public void UpdateLootUI () {

		if (!visible) {
			return;
		}

		ItemCategory[] categories = CategoryContent.itemCategories [currentCat].categories;
		Item[] items = LootManager.Instance.getLoot (side).getCategory (categories);
		SelectedItems = items;

		UpdatePages ();
		UpdateItemButtons ();
		UpdateActionButton (0);
	}

	public CategoryContent CategoryContent {
		get {
			if (categoryContent == null)
				categoryContent = LootManager.Instance.DefaultCategoryContent;
			return categoryContent;
		}
		set {
			categoryContent = value;
			UpdateCategoryButtons ();
		}
	}

	private void UpdateCategoryButtons () {
		
		for (int i = 0; i < categoryButtons.Length; ++i ) {

			categoryButtons [i].gameObject.SetActive ( i < CategoryContent.amount );

			if ( i < categoryContent.amount ) {
				categoryButtons [i].GetComponentInChildren<Text> ().text = CategoryContent.names[i];
				categoryButtons [i].image.color = CategoryContent.colors[i];
			}

		}
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
		maxPage = Mathf.CeilToInt ( SelectedItems.Length / ItemPerPage);

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

		actionButtonObj.SetActive (CategoryContent.interactable[currentCat] && SelectedItems.Length > 0);

		Vector3 targetPos = actionButtonObj.transform.position;

		targetPos.y = itemButtons [index].transform.position.y;

		actionButtonObj.transform.position = targetPos;

		actionButtonObj.GetComponentInChildren<Text> ().text = CategoryContent.actionNames [currentCat];

		selectionIndex = index;

	}
	#endregion

	public Item SelectedItem {
		get {
			ItemCategory[] cats= CategoryContent.itemCategories [currentCat].categories;
			Item[] items = LootManager.Instance.getLoot (side).getCategory (cats);
			return items[currentPage+selectionIndex];
		}
	}

	public int CurrentPage {
		get {
			return currentPage;
		}
	}

	public int ItemIndex {
		get {

			int l = 0;
			 
			if ( CategoryContent.itemCategories[currentCat].categories.Length > 1 ) {
				for (int i = 0; i < CategoryContent.itemCategories[currentCat].categories.Length; ++i ) {

					ItemCategory category = CategoryContent.itemCategories [currentCat].categories [i];
					Item[] items = LootManager.Instance.getLoot (side).getCategory (category);
					l += items.Length;
				}
			}

			return l + currentPage + selectionIndex;
		}
	}

	public int SelectionIndex {
		get {
			return selectionIndex;
		}
	}

	public GameObject LootObj {
		get {
			return lootObj;
		}
		set {
			lootObj = value;
		}
	}

	public string[] ActionButtonTexts {
		get {
			return actionButtonTexts;
		}
		set {
			actionButtonTexts = value;
		}
	}

	public Item[] SelectedItems {
		get {
			return selectedItems;
		}
		set {
			selectedItems = value;
		}
	}
}

[System.Serializable]
public class CategoryContent {

	public int amount = 0;

	public Categories[] itemCategories;

	public string[] names;
	public Color[] colors;
	public bool[] interactable;

	public string[] actionNames;

}

[System.Serializable]
public class Categories {
	public ItemCategory[] categories;

	public ItemCategory this [int i] {
		get {
			return categories [i];
		}
		set {
			categories [i] = value;
		}
	}
}
