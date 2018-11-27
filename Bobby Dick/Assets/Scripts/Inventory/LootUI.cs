using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum InventoryActionType {

	Eat,
	Equip,

    PurchaseAndEquip,
    Unequip,

    Throw,
	Sell,
	Buy,
	PickUp,

}

public class LootUI : MonoBehaviour {

	public static LootUI Instance;

	public RectTransform scollViewRectTransform;

	Loot handledLoot;

	private Item selectedItem = null;

	public delegate void OnSetSelectedItem ();
	public static OnSetSelectedItem onSetSelectedItem;

	public Item SelectedItem {
		
		get {

			return selectedItem;
		}

		set {
			
			selectedItem = value;
			selectedItemDisplay.HandledItem = value;

			UpdateActionButton (value);

			if ( onSetSelectedItem != null  ) {
				onSetSelectedItem ();
			}

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

	public bool visible = false;

	[Header("Item Buttons")]
	[SerializeField]
	private GameObject itemButtonGroup;
	private DisplayItem_Loot[] displayItems = new DisplayItem_Loot[0];
	public DisplayItem_Loot selectedItemDisplay;

	[Header("Categories")]
	[SerializeField] private Button[] categoryButtons;
	private CategoryContent categoryContent;
	public CategoryContentType categoryContentType;
	public Transform selectedParent;
	public Transform initParent;

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

        onSetSelectedItem = null;
        onHideLoot = null;
        onShowLoot = null;
        useInventory = null;

		Init ();
	}

	void Start () {
		CrewInventory.onRemoveItemFromMember += HandleOnRemoveItemFromMember;

		RayBlocker.onTouchRayBlocker += HandleOnTouchRayBlocker;
	}

	void HandleOnTouchRayBlocker ()
	{
		if (visible)
			Close ();
	}

	public void DeselectCurrentItem(){
		if (DisplayItem_Loot.selectedDisplayItem != null) {
			DisplayItem_Loot.selectedDisplayItem.Deselect ();
		}
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
	public Crews.Side currentSide;
	public delegate void OnShowLoot ();
	public static OnShowLoot onShowLoot;
	public void Show (CategoryContentType _catContentType,Crews.Side side) {

		SelectedItem = null;

		currentSide = side;

		handledLoot = LootManager.Instance.getLoot(side);

		categoryContent = LootManager.Instance.GetCategoryContent(_catContentType);
		categoryContentType = _catContentType;

		InitButtons (_catContentType);
		InitCategory();

		visible = true;
		lootObj.SetActive (true);

		UpdateLootUI ();

		//Tween.Bounce ( lootObj.transform , 0.2f , 1.05f);

		CrewInventory.Instance.HideMenuButtons ();

		if (onShowLoot != null)
			onShowLoot ();
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

	public delegate void OnHideLoot ();
	public static OnHideLoot onHideLoot;
	void Hide () {
		lootObj.SetActive (false);
	}
	public void Close () {
		
		visible = false;

		if (OtherInventory.Instance.type == OtherInventory.Type.Loot || OtherInventory.Instance.type == OtherInventory.Type.Trade) {

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();

			CrewInventory.Instance.HideInventory ();

			Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);

		} else {
			CrewInventory.Instance.ShowMenuButtons ();
		}

		OtherInventory.Instance.type = OtherInventory.Type.None;

		Hide ();

		if (onHideLoot != null) {
			onHideLoot ();
		}
	}
	#endregion

	void HandleOnRemoveItemFromMember (Item item)
	{
		UpdateLootUI ();
	}

	#region item button	
	public void UpdateItemButtons () {

        int displayItemIndex = 0;

        if ( (currentCat == ItemCategory.Clothes || currentCat == ItemCategory.Weapon) && currentPage == 0 && currentSide == Crews.Side.Player)
        {
            Item equipedItem = CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Weapon);
            if ( currentCat == ItemCategory.Clothes)
                equipedItem = CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Clothes);

            if ( equipedItem != null)
            {
                displayItems[displayItemIndex].HandledItem = equipedItem;

                ++displayItemIndex;
            }

        }

		int a = currentPage * ItemPerPage;

		for (int i = displayItemIndex; i < ItemPerPage; ++i ) {

			DisplayItem_Loot displayItem = displayItems [i];

			displayItem.gameObject.SetActive ( a < handledLoot.AllItems [(int)currentCat].Count );

			if ( a < handledLoot.AllItems [(int)currentCat].Count ) {

				Item item = handledLoot.AllItems[(int)currentCat][a];
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

			if ( handledLoot.AllItems[catIndex].Count > 0 ) {
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

		scollViewRectTransform.anchoredPosition = Vector2.zero;

	}

	public void UpdateLootUI () {

		if (!visible)
			return;

		UpdateItemButtons ();
		UpdateCategoryButtons ();

		selectedItemDisplay.HandledItem = selectedItemDisplay.HandledItem;


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

			categoryButtons [buttonIndex].transform.SetParent (initParent);

			// no items in category
			if ( handledLoot.AllItems[buttonIndex].Count == 0 ) {


                if ( currentSide == Crews.Side.Enemy)
                {
                    categoryButtons[buttonIndex].interactable = false;
                    continue;
                }

                if ( buttonIndex == (int)ItemCategory.Clothes )
                {
                    if ( CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Clothes) != null) {
                        categoryButtons[buttonIndex].interactable = true;
                        continue;
                    }
                }

                if (buttonIndex == (int)ItemCategory.Weapon)
                {
                    if (CrewMember.GetSelectedMember.GetEquipment(CrewMember.EquipmentPart.Weapon) != null)
                    {
                        categoryButtons[buttonIndex].interactable = true;
                        continue;
                    }
                }

                categoryButtons[buttonIndex].interactable = false;

            }
            else
			{
				categoryButtons [buttonIndex].interactable = true;
			}
		}

		categoryButtons [(int)currentCat].interactable = false;
		categoryButtons [(int)currentCat].transform.SetParent (selectedParent);
	}
	#endregion

	#region page navigation
	public int ItemPerPage {
		get {
			return displayItems.Length;
		}
	}
	#endregion

	#region action button
	public void InventoryAction ( InventoryActionType inventoryActionType ) {

		if (useInventory != null)
			useInventory (inventoryActionType);
		else
			print ("no function liked to the event : use inventory");

		UpdateLootUI ();

		if (DisplayItem_Loot.selectedDisplayItem != null)
			DisplayItem_Loot.selectedDisplayItem.Deselect ();

		SelectedItem = null;

	}
	public void UpdateActionButton (Item item) {

		if ( item == null ) {
            actionGroup.HideAll();
			return;
		}

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
