using UnityEngine;
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

	public void Show (CategoryContent _categoryContent) {
		categoryContent = _categoryContent;
		Visible = true;
		itemButtons [0].Select ();
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

//				Debug.Log (a.ToString ());
				Item item = SelectedItems[a];

				itemButton.gameObject.SetActive (true);

				itemButton.Name 		= item.name;

				itemButton.Param 		= CategoryContent.itemCategories[currentCat].categories[0] != ItemCategory.Misc ? item.value : 0;

				itemButton.Price 		= item.price;

				itemButton.Weight 		= item.weight;

				itemButton.Level 		= item.level;

			}

			a++;
		}

	}
	#endregion

	[SerializeField]
	private ActionGroup actionGroup;

	#region category navigation
	public void SwitchCategory ( int cat ) {

		categoryButtons [currentCat].interactable = true;

		currentCat = cat;

		categoryButtons [cat].interactable = false;

		currentPage = 0;

		UpdateLootUI ();

		SetActionButtons ();
	}

	private void SetActionButtons (){ 



	}

	public void UpdateLootUI () {

		if (!visible)
			return;

		ItemCategory[] categories = CategoryContent.itemCategories [currentCat].categories;
		Item[] items = LootManager.Instance.getLoot (side).getCategory (categories);
		SelectedItems = items;

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

	[SerializeField]
	private Sprite[] categorySprites;

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

				if (i < CategoryContent.amount) {
//				categoryButtons [i].GetComponentInChildren<Text> ().text = CategoryContent.names[i];
					categoryButtons [i].image.color = CategoryContent.colors [i];
				}
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
	public void UpdateActionButton (int itemIndex) {

		if ( currentCat >= CategoryContent.interactable.Length ) {

			Debug.LogError ("Y AUN TRUC QUI VA PAS DANS L INVENTAIRE ET LES CATEGORIES");
			Debug.LogError ("CURRENT CAT : " + currentCat);
			Debug.LogError ("LONGUEUR DE LA LISTE : " + CategoryContent.interactable.Length);

		}
		bool enoughItemsOnPage = SelectedItems.Length > CurrentPage * ItemPerPage;

		actionGroup.Visible = CategoryContent.interactable[currentCat] && enoughItemsOnPage;
		actionGroup.UpdateButtons (CategoryContent.catButtonType[currentCat].buttonTypes [0], CategoryContent.catButtonType[currentCat].buttonTypes [1]);


		foreach (ItemButton itemButton in itemButtons)
			itemButton.Enabled = true;


		// set group index
		SelectionIndex = itemIndex;



	}
	#endregion

	public Item SelectedItem {
		get {
			ItemCategory[] cats= CategoryContent.itemCategories [currentCat].categories;
			Item[] items = LootManager.Instance.getLoot (side).getCategory (cats);

			int index = currentPage + SelectionIndex;

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
			itemButtons [selectionIndex].Enabled = false;
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

	public CategoryButtonType[] catButtonType;


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
