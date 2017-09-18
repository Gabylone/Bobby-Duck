using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLoot : MonoBehaviour {

	public static PlayerLoot Instance;

	public delegate void OpenInventory (CrewMember member);
	public OpenInventory openInventory;
	public delegate void CloseInventory ();
	public CloseInventory closeInventory;

	private CrewMember selectedMember;

	private int selectedMemberIndex = 0;

	[SerializeField]
	private GameObject infoGroup;

	[Header("Groups")]
	[SerializeField]
	private GameObject crewGroup;
	public bool canOpen = true;
	[SerializeField]
	private GameObject closeButtonObj;

	public void Lock () {
		canOpen = false;
	}
	public void Unlock () {
		canOpen = true;
	}

	[Header("LootUI")]
	[SerializeField]
	private LootUI lootUI;
	[SerializeField]
	private LootUI otherLootUI;


	[SerializeField]
	private Transform crewCanvas;

	private bool ateSomething = false;

	[SerializeField]
	private ActionGroup actionGroup;

	bool opened = false;

	void Awake () {
		Instance = this;
	}

	public void Init () {
//		HideCrewGroup ();

		lootUI.useInventory += HandleUseInventory;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStory;
		StoryLauncher.Instance.endStoryEvent += HandleEndStory;
		DisplayItem_Crew.onRemoveItemFromMember += HandleOnRemoveItemFromMember;
	}

	void HandleOnRemoveItemFromMember (Item item)
	{
		lootUI.UpdateLootUI ();
	}

	#region crew group
	public void ShowCrewGroup () {
		crewGroup.SetActive (true);
		//
	}
	public void HideCrewGroup () {
		crewGroup.SetActive (false);
		//
	}
	#endregion

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

		SelectedMember.Health += lootUI.SelectedItem.value;

		int i = (int)(lootUI.SelectedItem.value * 1.5f);

		SelectedMember.CurrentHunger -= i;

		RemoveSelectedItem ();

	}

	public void EquipItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		if (!targetMember.CheckLevel (lootUI.SelectedItem.level)) {
			return;
		}

		CrewMember.EquipmentPart part = CrewMember.EquipmentPart.Clothes;
		switch (lootUI.SelectedItem.category) {
		case ItemCategory.Weapon:
			part = CrewMember.EquipmentPart.Weapon;
			break;
		case ItemCategory.Clothes:
			part = CrewMember.EquipmentPart.Clothes;
			break;
		}

		if (targetMember.GetEquipment(part) != null)
			LootManager.Instance.PlayerLoot.AddItem (targetMember.GetEquipment(part) );

		targetMember.SetEquipment (part, lootUI.SelectedItem);

		RemoveSelectedItem ();
		print ("debor y'a ça");

	}

	public void ThrowItem () {

		RemoveSelectedItem ();
	}

	public void SellItem () {

		GoldManager.Instance.GoldAmount += lootUI.SelectedItem.price;

		LootManager.Instance.OtherLoot.AddItem (lootUI.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (lootUI.SelectedItem);

		lootUI.UpdateLootUI ();
		otherLootUI.UpdateLootUI ();

	}

	private void RemoveSelectedItem () {
		LootManager.Instance.PlayerLoot.RemoveItem (lootUI.SelectedItem);
		lootUI.UpdateLootUI ();
	}
	#endregion

	#region crew management
	public CrewMember SelectedMember {
		get {
			return selectedMember;
		}
		set {
			if ( selectedMember != null ) {
				selectedMember.Icon.Down ();
			}

			selectedMember = value;

			selectedMember.Icon.Up ();
		}
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

		SelectedMember = crewMember;

			// set bool
		opened = true;

			// event
		openInventory (crewMember);

			// update crew position
		Crews.getCrew (Crews.Side.Player).UpdateCrew (Crews.PlacingType.Map);


			// close map
		MapImage.Instance.Close ();

			// show elements
		ShowCrewGroup();
		lootUI.Hide ();
		infoGroup.SetActive (true);

		if ( catContentType != CategoryContentType.Inventory ) {
			lootUI.Show (catContentType);
			infoGroup.SetActive (false);
		}

	}
	public void HideInventory () {

		if (Opened == false)
			return;

			// set bool
		opened = false;


		if (StoryLauncher.Instance.PlayingStory) {
			Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
		} else {
			Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.PlacingType.Map);
		}

		foreach (CrewMember member in Crews.getCrew(Crews.Side.Player).CrewMembers)
			member.Icon.Down ();

			// event
		closeInventory();

			// hide elements
		HideCrewGroup();

		lootUI.Hide ();

	}

	public void OpenLoot () {
		lootUI.Show (CategoryContentType.Inventory);
		infoGroup.SetActive (false);

		QuestMenu.Instance.Close ();
		BoatUpgradeManager.Instance.CloseUpgradeMenu ();
	}

	public void CloseLoot () {
		lootUI.Hide ();
		infoGroup.SetActive (true);
	}

	public bool Opened {
		get {
			return opened;
		}
	}

	public LootUI LootUI {
		get {
			return lootUI;
		}
	}

	#endregion

	public ActionGroup ActionGroup {
		get {
			return actionGroup;
		}
	}

}
