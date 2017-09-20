using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewInventory : MonoBehaviour {

	public static CrewInventory Instance;

	public delegate void OpenInventory (CrewMember member);
	public OpenInventory openInventory;
	public delegate void CloseInventory ();
	public CloseInventory closeInventory;

	private CrewMember selectedMember;

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

		LootUI.useInventory += HandleUseInventory;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStory;
		StoryLauncher.Instance.endStoryEvent += HandleEndStory;

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

		CrewMember.selectedMember.Health += LootUI.Instance.SelectedItem.value;

		int i = (int)(LootUI.Instance.SelectedItem.value * 1.5f);

		CrewMember.selectedMember.CurrentHunger -= i;

		RemoveSelectedItem ();

	}

	public void EquipItem () {

		CrewMember targetMember = CrewMember.selectedMember;

		if (!targetMember.CheckLevel (LootUI.Instance.SelectedItem.level)) {
			return;
		}

		CrewMember.EquipmentPart part = CrewMember.EquipmentPart.Clothes;
		switch (LootUI.Instance.SelectedItem.category) {
		case ItemCategory.Weapon:
			part = CrewMember.EquipmentPart.Weapon;
			break;
		case ItemCategory.Clothes:
			part = CrewMember.EquipmentPart.Clothes;
			break;
		}

		if (targetMember.GetEquipment(part) != null)
			LootManager.Instance.PlayerLoot.AddItem (targetMember.GetEquipment(part) );

		targetMember.SetEquipment (part, LootUI.Instance.SelectedItem);

		RemoveSelectedItem ();

	}

	public void ThrowItem () {

		RemoveSelectedItem ();
	}

	public void SellItem () {

		GoldManager.Instance.GoldAmount += LootUI.Instance.SelectedItem.price;

		LootManager.Instance.OtherLoot.AddItem (LootUI.Instance.SelectedItem);
		LootManager.Instance.PlayerLoot.RemoveItem (LootUI.Instance.SelectedItem);

	}

	private void RemoveSelectedItem () {
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

		CrewMember.setSelectedMember (crewMember);

			// set bool
		opened = true;

			// event
		openInventory (crewMember);

			// update crew position
		Crews.getCrew (Crews.Side.Player).UpdateCrew (Crews.PlacingType.Map);

			// show elements
		ShowCrewGroup();

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

		LootUI.Instance.Hide ();

	}

	public void Open () {
		
		LootUI.Instance.Show (CategoryContentType.Inventory);

		QuestMenu.Instance.Close ();
		BoatUpgradeManager.Instance.CloseUpgradeMenu ();
	}

	public void CloseLoot () {
		LootUI.Instance.Hide ();
	}

	public bool Opened {
		get {
			return opened;
		}
	}
	#endregion

}
