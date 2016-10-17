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
	private int selectionIndex = 0;
	[SerializeField] private GameObject actionButtonObj;
	[SerializeField] private string[] actionButtonTexts;

	[Header("Category")]
	[SerializeField] private Button[] categoryButtons;

	void Start () {

		itemButtons = itemButtonGroup.GetComponentsInChildren<ItemButton>();

		SwitchCategory (0);
	}

	void Update () {

		if ( Input.GetKeyDown (KeyCode.L))  {
			LootManager.Instance.RemoveItem (currentCategory, 1);
			UpdateLootUI ();
		}
	}

	#region item button	
	public void UpdateItemButtons () {

		int a = currentPage * ItemPerPage;

		foreach ( ItemButton itemButton in itemButtons ) {

			itemButton.gameObject.SetActive ( a < LootManager.Instance.getLoot (currentCategory).Length );

			if ( a < LootManager.Instance.getLoot (currentCategory).Length ) {

				Item item = ItemLoader.Instance.getItem (currentCategory,LootManager.Instance.getLoot (currentCategory)[a].ID);

				itemButton.Name 		= item.name;
				itemButton.Description 	= item.description;

				itemButton.ParamObj.SetActive (currentCategory != ItemLoader.ItemType.Mics);
				itemButton.gameObject.SetActive (true);
				itemButton.Param = item.value;

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
	[SerializeField] private GameObject previousPageButton;
	[SerializeField] private GameObject nextPageButton;

	public void NextPage () {
		++currentPage;
		UpdateLootUI ();
	}
	public void PreviousPage () {
		--currentPage;
		UpdateLootUI ();
	}

	private void UpdatePages () {
		maxPage = Mathf.CeilToInt ( LootManager.Instance.getLoot (currentCategory).Length / ItemPerPage);

		previousPageButton.SetActive( currentPage > 0 );
		nextPageButton.SetActive( currentPage < maxPage );
	}

	public int ItemPerPage {
		get {
			return itemButtons.Length;
		}
	}
	#endregion

	#region item usage
	public void UseItem () {

		CrewMember targetMember = CrewNavigator.Instance.SelectedMember;

		switch (currentCategory) {
		case ItemLoader.ItemType.Provisions:
			targetMember.Health += SelectedItem.value;
			// states 
			break;
		case ItemLoader.ItemType.Weapon:
			targetMember.AttackDice = SelectedItem.value;
			break;
		case ItemLoader.ItemType.Clothes:
			targetMember.ConstitutionDice = SelectedItem.value;
			break;
		}

		int removeIndex = currentPage + selectionIndex;

		LootManager.Instance.RemoveItem ( currentCategory, removeIndex);

		UpdateLootUI ();
		CrewNavigator.Instance.UpdateMembers();

	}
	public void UpdateActionButton (int index) {

		bool actionButtonActive = LootManager.Instance.getLoot (currentCategory).Length > 0 && currentCategory != ItemLoader.ItemType.Mics;
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
			return LootManager.Instance.getLoot (currentCategory)[currentPage+selectionIndex];
		}
	}
}
