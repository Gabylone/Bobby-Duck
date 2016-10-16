using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	private Item[][] currentLoot = new Item[4][];

	ItemLoader.ItemType currentCategory;

	[SerializeField]
	private GameObject itemButtonGroup;
	private ItemButton[] itemButtons;

	private int itemsPerPage 	= 0;

	private int currentPage 	= 0;
	private int maxPage 		= 0;

	void Start () {

		itemButtons = itemButtonGroup.GetComponentsInChildren<ItemButton>();

		currentLoot = ItemLoader.Instance.Items;

		itemsPerPage = itemButtons.Length;

		UpdateButtons ();
		UpdatePageButtons ();
	}

	#region item buttons
	public void UpdateButtons () {

		maxPage = Mathf.CeilToInt ( currentLoot[(int)currentCategory].Length / itemsPerPage );

		int a = currentPage * itemsPerPage;

		foreach ( ItemButton itemButton in itemButtons ) {

			itemButton.gameObject.SetActive ( a < ItemLoader.Instance.getItems(currentCategory).Length-1 );

			if ( a < ItemLoader.Instance.getItems(currentCategory).Length -1) {

				Item item = ItemLoader.Instance.getItem (currentCategory,currentLoot[(int)currentCategory][a].ID);

				itemButton.Name 		= item.name;
				itemButton.Description 	= item.description;
				itemButton.Param 		= item.value;
				itemButton.Price 		= item.price;
			}

			a++;
		}
	}
	#endregion

	#region category navigation

	[SerializeField] 
	private Button[] categoryButtons;

	public void SwitchCategory ( int cat ) {

		if ( cat == (int)currentCategory)
			return;

		categoryButtons[(int)currentCategory].enabled = true;

		currentCategory = (ItemLoader.ItemType)cat;
		currentPage = 0;

		categoryButtons[cat].enabled = false;

		UpdatePageButtons ();
		UpdateButtons ();
		
		
	}
	#endregion

	#region page navigation
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;

	public void NextPage () {
		++currentPage;
		UpdatePageButtons ();
		UpdateButtons ();
	}
	public void PreviousPage () {
		--currentPage;
		UpdatePageButtons ();
		UpdateButtons ();
	}

	private void UpdatePageButtons () {
		previousPageButton.SetActive( currentPage > 0 );
		nextPageButton.SetActive( currentPage < maxPage-1 );
	}
	#endregion

	public Item[][] CurrentLoot (ItemLoader.ItemType itemType ){
		return CurrentLoot (itemType);
	}
}
