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

	private int currentCat = 0;

	public delegate void UseInventory ( InventoryActionType actionType );
	public UseInventory useInventory;

	[SerializeField]
	private GameObject lootObj;

	[SerializeField]
	private Crews.Side side;

	bool visible = false;

	[Header("Item Buttons")]
	[SerializeField]
	private GameObject itemButtonGroup;
	private DisplayItem_Loot[] displayItems = new DisplayItem_Loot[0];
	private Item[] selectedItems {
		get {
			ItemCategory[] categories = CategoryContent.itemCategories [currentCat].categories;
			return LootManager.Instance.getLoot (side).getCategory (categories);
		}
	}
	private int selectionIndex = 0;


	[Header("Categories")]
	[SerializeField] private Button[] categoryButtons;
	private CategoryContent categoryContent;
	[SerializeField]
	private Sprite[] categorySprites;

	[Header("Pages")]
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;
	private int currentPage 	= 0;
	private int maxPage 		= 0;

	[Header("Actions")]
	[SerializeField]
	private ActionGroup actionGroup;

	void Awake () {
		Init ();
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

	public void Show (CategoryContentType catContentType) {

		categoryContent = LootManager.Instance.GetCategoryContent(catContentType);

		displayItems [0].Select ();
		SwitchCategory (0);

		visible = true;
		lootObj.SetActive (true);

		UpdateLootUI ();

	}

	public void Hide () {

		visible = false;
		lootObj.SetActive (false);
	}

	public bool Visible {
		get {
			return visible;
		}
	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		for (int i = 0; i < ItemPerPage; ++i ) {

			DisplayItem_Loot displayItem = displayItems [i];

			displayItem.gameObject.SetActive ( a < selectedItems.Length );

			if ( a < selectedItems.Length ) {

//				Debug.Log (a.ToString ());
				Item item = selectedItems[a];
				displayItem.HandledItem = item;

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

		Tween.Bounce (categoryButtons[cat].transform, 0.2f , 1.1f);

		currentPage = 0;

		UpdateLootUI ();

	}

	public void UpdateLootUI () {

		if (!visible)
			return;

		UpdatePages ();
		UpdateItemButtons ();
		UpdateActionButton (0);
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

		for (int i = 0; i < categoryButtons.Length; ++i ) {

			categoryButtons [i].gameObject.SetActive ( false );

			if (i < CategoryContent.itemCategories.Length) {

				categoryButtons [i].gameObject.SetActive ( true );

				Sprite sprite;

				if (CategoryContent.itemCategories [0].categories.Length > 1)
					sprite = categorySprites [(int)ItemCategory.Misc];
				else
					sprite = categorySprites [(int)CategoryContent.itemCategories [i].categories [0]];

				categoryButtons [i].GetComponentsInChildren<Image> () [1].sprite = sprite;


			}

		}
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

	private void UpdatePages () {
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
		Tween.Bounce (actionGroup.ButtonObjects[i].transform );

		if (useInventory != null)
			useInventory ((InventoryActionType)i);
		else
			print ("no function liked to the event : use inventory");

	}
	public void UpdateActionButton (int itemIndex) {

		if ( currentCat >= CategoryContent.itemCategories.Length ) {

			Debug.LogError ("Y AUN TRUC QUI VA PAS DANS L INVENTAIRE ET LES CATEGORIES");
			Debug.LogError ("CURRENT CAT : " + currentCat);
			Debug.LogError ("LONGUEUR DE LA LISTE : " + CategoryContent.itemCategories.Length);
		}
		bool enoughItemsOnPage = selectedItems.Length > CurrentPage * ItemPerPage;

		actionGroup.Visible =  enoughItemsOnPage;
		actionGroup.UpdateButtons (CategoryContent.catButtonType[currentCat].buttonTypes);


		foreach (DisplayItem_Loot displayItem in displayItems)
			displayItem.Enabled = true;


		// set group index
		SelectionIndex = itemIndex;

	}
	#endregion

	public Item SelectedItem {
		get {
			ItemCategory[] cats= CategoryContent.itemCategories [currentCat].categories;
			Item[] items = LootManager.Instance.getLoot (side).getCategory (cats);

			int index = (ItemPerPage * currentPage) + SelectionIndex;


			return items[index];
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

			return l + currentPage + SelectionIndex;
		}
	}

	public int SelectionIndex {
		get {
			return selectionIndex;
		}
		set {

			selectionIndex = value;
			displayItems [selectionIndex].Enabled = false;

			Tween.Bounce( displayItems[selectionIndex].transform , 0.2f , 1.1f);
//			if ( bounce )
//				Tween.Bounce( itemButtons[selectionIndex].transform , 0.2f , 1.1f);

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

	public Categories[] itemCategories;

//	public string[] names;
//	public Color[] colors;
//	public bool[] interactable;

	public CategoryButtonType[] catButtonType = new CategoryButtonType[4];


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
