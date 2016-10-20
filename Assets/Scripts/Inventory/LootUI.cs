using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootUI : MonoBehaviour {

	ItemLoader.ItemType currentCategory = ItemLoader.ItemType.Weapon;

	[SerializeField]
	private GameObject lootObj;

	[SerializeField]
	private Crews.Side side;

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

	[Header("Category")]
	[SerializeField] private Button[] categoryButtons;

	[Header("Pages")]
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;


	public void Show () {
		
		Init ();

		lootObj.SetActive (true);

	}

	private void Init () {
		
		if ( itemButtons.Length == 0 )
			itemButtons = itemButtonGroup.GetComponentsInChildren<ItemButton>();

		int a = 0;
		foreach ( ItemButton itemButton in itemButtons ) {
			itemButton.Index = a;
			++a;
		}

		SwitchCategory (0);
	}

	public void Hide () {
		lootObj.SetActive (false);
	}


	void Start () {


	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		foreach ( ItemButton itemButton in itemButtons ) {

			itemButton.gameObject.SetActive ( a < LootManager.Instance.getLoot(side).getCategory (currentCategory).Length );

			if ( a < LootManager.Instance.getLoot(side).getCategory (currentCategory).Length ) {

				Item item = ItemLoader.Instance.getItem (currentCategory,LootManager.Instance.getLoot(side).getCategory (currentCategory)[a].ID);

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

		categoryButtons [(int)currentCategory].interactable = true;
		categoryButtons [cat].interactable = false;

		currentCategory = (ItemLoader.ItemType)cat;
		currentPage = 0;

		UpdateLootUI ();
	}

	public void UpdateLootUI () {
		UpdatePages ();
		UpdateItemButtons ();
		UpdateActionButton (0);
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
		maxPage = Mathf.CeilToInt ( LootManager.Instance.getLoot(side).getCategory (currentCategory).Length / ItemPerPage);

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

		if ( LootManager.Instance.getLoot(side).getCategory (currentCategory).Length == 0 
			|| (currentCategory == ItemLoader.ItemType.Mics && !OtherLoot.Instance.Trading) ) {

			actionButtonObj.SetActive (false);
			return;
		}

		actionButtonObj.SetActive (true);

		Vector3 targetPos = actionButtonObj.transform.position;

		targetPos.y = itemButtons [index].transform.position.y;

		actionButtonObj.transform.position = targetPos;

		actionButtonObj.GetComponentInChildren<Text> ().text = actionButtonTexts[(int)currentCategory];

		selectionIndex = index;

	}
	#endregion

	public Item SelectedItem {
		get {
			return LootManager.Instance.getLoot(side).getCategory (currentCategory)[currentPage+selectionIndex];
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

	public int ItemIndex {
		get {
			return currentPage + selectionIndex;
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
}
