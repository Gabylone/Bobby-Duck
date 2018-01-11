using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum InventoryActionType {

	Eat,
	Equip,
	Throw,
	Sell,
	Buy,
	PickUp

}

public class LootUI : MonoBehaviour {

	public static LootUI Instance;

	Loot handledLoot;

	private Item selectedItem = null;

	public Item SelectedItem {
		
		get {
			
//			int index = (ItemPerPage * currentPage) + SelectionIndex;
//
//			if ( index >= handledLoot.allItems [(int)currentCat].Count ) {
//				print ("apparmeent pas d'objet dans la catégorie :  " + currentCat + " INDEX : " + index);
//				return null;
//			}
//
//			return handledLoot.allItems [(int)currentCat] [index];

			return selectedItem;
		}

		set {
			
			selectedItem = value;
			selectedItemDisplay.HandledItem = value;

			UpdateActionButton (value);

		}
	}

	private Item[] selectedItems {
		get {
			return handledLoot.allItems [(int)currentCat].ToArray();
		}
	}

	private ItemCategory currentCat;

	public delegate void UseInventory ( InventoryActionType actionType );
	public static UseInventory useInventory;

	[SerializeField]
	private GameObject lootObj;

	public GameObject closeButton;
	public GameObject switchToPlayer;
	public GameObject switchToOther;

	bool visible = false;

	[Header("Item Buttons")]
	[SerializeField]
	private GameObject itemButtonGroup;
	private DisplayItem_Loot[] displayItems = new DisplayItem_Loot[0];
	public DisplayItem_Loot selectedItemDisplay;

	[Header("Categories")]
	[SerializeField] private Button[] categoryButtons;
	private CategoryContent categoryContent;

	[Header("Pages")]
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;
	private int currentPage 	= 0;
	private int maxPage 		= 0;

	[Header("Actions")]
	[SerializeField]
	private ActionGroup actionGroup;

	void Awake () {
		Instance = this;

		Init ();
	}

	void Start () {
		DisplayItem_Crew.onRemoveItemFromMember += HandleOnRemoveItemFromMember;
	}

	private void Init () {

		if ( displayItems.Length == 0 )
			displayItems = itemButtonGroup.GetComponentsInChildren<DisplayItem_Loot>();

		int a = 0;
		foreach ( DisplayItem_Loot itemButton in displayItems ) {
			itemButton.index = a;
			++a;
		}

		Hide ();
	}

	#region show / hide
	public void Show (CategoryContentType catContentType,Crews.Side side) {

		SelectedItem = null;

		handledLoot = LootManager.Instance.getLoot(side);

		categoryContent = LootManager.Instance.GetCategoryContent(catContentType);

		InitButtons (catContentType);
		InitCategory();

		visible = true;
		lootObj.SetActive (true);

		UpdateLootUI ();

		Tween.Bounce ( lootObj.transform , 0.2f , 1.05f);

		CrewInventory.Instance.HideMenuButtons ();


	}

	void InitButtons (CategoryContentType catContentType)
	{
		switch (catContentType) {
		case CategoryContentType.PlayerTrade:
		case CategoryContentType.PlayerLoot:
//			closeButton.SetActive (true);
			switchToOther.SetActive (true);
			switchToPlayer.SetActive (false);
			break;

		case CategoryContentType.OtherTrade:
		case CategoryContentType.OtherLoot:
//			closeButton.SetActive (true);
			switchToOther.SetActive (false);
			switchToPlayer.SetActive (true);
			break;

		case CategoryContentType.Inventory:
		case CategoryContentType.Combat:
//			closeButton.SetActive (false);
			switchToOther.SetActive (false);
			switchToPlayer.SetActive (false);
			break;
		default:
			break;
		}
	}

	public void Hide () {
		visible = false;
		lootObj.SetActive (false);

		CrewInventory.Instance.ShowMenuButtons ();
	}
	#endregion

	void HandleOnRemoveItemFromMember (Item item)
	{
		UpdateLootUI ();
	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		for (int i = 0; i < ItemPerPage; ++i ) {

			DisplayItem_Loot displayItem = displayItems [i];

			displayItem.gameObject.SetActive ( a < selectedItems.Length );

			if ( a < selectedItems.Length ) {

				Item item = selectedItems[a];
				displayItem.HandledItem = item;

			}


			a++;
		}

	}
	#endregion



	#region category navigation

	void InitCategory ()
	{
		for (int catIndex = 0; catIndex < categoryButtons.Length; catIndex++) {

			if ( handledLoot.allItems[catIndex].Count > 0 ) {
				currentCat = (ItemCategory)catIndex;
				return;
			}

		}
	}
	public void SwitchCategory ( int cat ) {
		SwitchCategory ((ItemCategory)cat);
	}
	public void SwitchCategory ( ItemCategory cat ) {

		if ( DisplayItem_Loot.selectedDisplayItem != null )
			DisplayItem_Loot.selectedDisplayItem.Deselect ();

		currentCat = cat;

		Tween.Bounce (categoryButtons[(int)cat].transform, 0.2f , 1.1f);

		currentPage = 0;

		SelectedItem = null;

		UpdateLootUI ();

	}

	public void UpdateLootUI () {

		if (!visible)
			return;

		UpdateNavigationButtons ();
		UpdateItemButtons ();
		UpdateCategoryButtons ();
	}

	public CategoryContent CategoryContent {
		get {

			if (categoryContent == null) {
				Debug.LogError ("pas de category content");
				return null;
			}

			return categoryContent;
		}
			
	}

	private void UpdateCategoryButtons () {

		for (int buttonIndex = 0; buttonIndex < categoryButtons.Length; ++buttonIndex) {

			categoryButtons [buttonIndex].image.color = Color.white;

			// no items in category
			if ( handledLoot.allItems[buttonIndex].Count == 0 ) {
				categoryButtons [buttonIndex].interactable = false;
			}
			else {
				categoryButtons [buttonIndex].interactable = true;
			}
		}

		categoryButtons [(int)currentCat].image.color = Color.yellow;
	}
	#endregion

	#region page navigation
	public void NextPage () {
		++currentPage;
		UpdateLootUI ();
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}
	public void PreviousPage () {
		--currentPage;
		UpdateLootUI ();
		SoundManager.Instance.PlaySound (SoundManager.Sound.Select_Big);
	}

	private void UpdateNavigationButtons () {
		maxPage = Mathf.CeilToInt ( selectedItems.Length / ItemPerPage);

		previousPageButton.SetActive( currentPage > 0 );
		nextPageButton.SetActive( selectedItems.Length > ItemPerPage * (currentPage+1));
	}

	public int ItemPerPage {
		get {
			return displayItems.Length;
		}
	}
	#endregion

	#region action button
	public void InventoryAction ( int i ) {
		

		if (useInventory != null)
			useInventory ((InventoryActionType)i);
		else
			print ("no function liked to the event : use inventory");

		Tween.Bounce (actionGroup.ButtonObjects[i].transform );

		UpdateLootUI ();

		if (DisplayItem_Loot.selectedDisplayItem != null)
			DisplayItem_Loot.selectedDisplayItem.Deselect ();

		SelectedItem = null;

	}
	public void UpdateActionButton (Item item) {

		if ( item == null ) {
			actionGroup.Visible =  false;
			return;
			//
		}

//		bool enoughItemsOnPage = selectedItems.Length > CurrentPage * ItemPerPage;

		actionGroup.Visible =  true;
		actionGroup.UpdateButtons (CategoryContent.catButtonType[(int)currentCat].buttonTypes);

	}
	#endregion



	public int CurrentPage {
		get {
			return currentPage;
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
}

[System.Serializable]
public class CategoryContent {
	public CategoryButtonType[] catButtonType = new CategoryButtonType[4];
}

[System.Serializable]
public class CategoryButtonType {
	
	public ActionGroup.ButtonType[] buttonTypes = new ActionGroup.ButtonType[2];

	public ActionGroup.ButtonType this [int i] {
		get {
			return buttonTypes [i];
		}
		set {
			buttonTypes [i] = value;
		}
	}
}
