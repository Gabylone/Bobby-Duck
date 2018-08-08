using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CrewInventory : MonoBehaviour {

	public static CrewInventory Instance;

	public delegate void OpenInventory (CrewMember member);
	public OpenInventory onOpenInventory;
	public delegate void CloseInventory ();
	public CloseInventory onCloseInventory;

	private CrewMember selectedMember;

	[Header("Groups")]
	[SerializeField]
	public bool canOpen = true;

	public MenuButtons menuButtons;

	public void Lock () {
		canOpen = false;
	}
	public void Unlock () {
		canOpen = true;
	}

	[SerializeField]
	private Transform crewCanvas;

	private bool ateSomething = false;

	[SerializeField]
	private ActionGroup actionGroup;

	public bool opened = false;

	void Awake () {
		Instance = this;

        onCloseInventory = null;
        onOpenInventory = null;
        StatButton.onClickStatButton = null;
	}

	public void Init () {

		LootUI.useInventory += HandleUseInventory;

		StoryLauncher.Instance.onPlayStory += HandlePlayStory;
		StoryLauncher.Instance.onEndStory += HandleEndStory;

		RayBlocker.onTouchRayBlocker += HandleOnTouchRayBlocker;
	}

	void HandleOnTouchRayBlocker ()
	{
		if (menuButtons.opened)
			HideInventory ();
	}


	#region events
	void HandleEndStory ()
	{
//		canOpen = true;
	}

	void HandlePlayStory ()
	{
//		canOpen = false;
		HideInventory ();
	}
	#endregion

	#region event handling
	void HandleUseInventory (InventoryActionType actionType)
	{
		switch (actionType) {
		case InventoryActionType.Eat:
			EatItem ();
			break;
		case InventoryActionType.Equip:
			EquipItem ();
			break;
        case InventoryActionType.Unequip:
            UnequipItem();
            break;
            case InventoryActionType.Throw:
			ThrowItem ();
			break;
		case InventoryActionType.Sell:
			SellItem ();
			break;
		default:
			break;
		}

	}
    #endregion

    #region button action
    public void EatItem () {

		Item foodItem = LootUI.Instance.SelectedItem;

		int hunger = 0;
		int health = 0;

		switch (foodItem.spriteID) {
		// légume
		case 0:
			hunger = (int) ((float)Crews.maxHunger/3f);
			health = 25;
			break;
		// poisson
		case 1:
			hunger = (int) ((float)Crews.maxHunger/2f);
			health = 50;
			break;
		// viande
		case 2:
			hunger = (int) ((float)Crews.maxHunger/1.5f);
			health = 75;
			break;
		default:
			break;
		}

		if (CrewMember.GetSelectedMember.Health >= CrewMember.GetSelectedMember.MemberID.maxHealth - 15) {
			CrewMember.GetSelectedMember.CurrentHunger -= hunger;
		}
		CrewMember.GetSelectedMember.AddHealth (health);

		if (OtherInventory.Instance.type == OtherInventory.Type.None)
			LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
		else
			LootManager.Instance.OtherLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	void EquipItem ()
	{
		Item item = LootUI.Instance.SelectedItem;

		if ( CrewMember.GetSelectedMember.CheckLevel (item.level) == false ) {
			return;
		}

		if (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) != null)
			LootManager.Instance.PlayerLoot.AddItem (CrewMember.GetSelectedMember.GetEquipment(item.EquipmentPart) );

		CrewMember.GetSelectedMember.SetEquipment (item);

		if ( OtherInventory.Instance.type == OtherInventory.Type.None )
			LootManager.Instance.PlayerLoot.RemoveItem (item);
		else 
			LootManager.Instance.OtherLoot.RemoveItem (item);

	}

    public delegate void OnRemoveItemFromMember(Item item);
    public static OnRemoveItemFromMember onRemoveItemFromMember;

    private void UnequipItem()
    {
        LootManager.Instance.getLoot(Crews.Side.Player).AddItem(LootUI.Instance.SelectedItem);

        CrewMember.GetSelectedMember.RemoveEquipment(LootUI.Instance.SelectedItem.EquipmentPart);

        if (onRemoveItemFromMember != null)
            onRemoveItemFromMember(LootUI.Instance.SelectedItem);
    }

    public void ThrowItem () {

		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);
	}

	public void SellItem () {

		int price = 1 + (int)(LootUI.Instance.SelectedItem.price / 3f);

		GoldManager.Instance.AddGold (price);
		LootManager.Instance.OtherLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}
	#endregion

	#region properties
	public void ShowInventory ( CategoryContentType catContentType ) {
		ShowInventory (catContentType, Crews.playerCrew.captain);
	}
	public void ShowInventory ( CategoryContentType catContentType , CrewMember crewMember ) {

		if (!canOpen) {
			return;
		}

		// event
		onOpenInventory (crewMember);

			// set bool
		opened = true;

		LootUI.Instance.UpdateLootUI ();

		if (LootUI.Instance.visible) {
			LootUI.Instance.UpdateLootUI ();
		} else {
//			LootUI.Instance.Close ();
			ShowMenuButtons ();
		} 


	}
	public void HideInventory () {

			// set bool
		opened = false;

			// event
		onCloseInventory();

		// hide elements
		HideMenuButtons ();

	}
	#endregion

	#region menu buttons
	public void ShowMenuButtons () {
		menuButtons.Show ();
	}
	public void HideMenuButtons () {
		menuButtons.Hide();
	}
	#endregion

	#region inventory button
	public void Open () {
		
		LootUI.Instance.Show (CategoryContentType.Inventory,Crews.Side.Player);

		HideMenuButtons ();
	}
	#endregion

}
