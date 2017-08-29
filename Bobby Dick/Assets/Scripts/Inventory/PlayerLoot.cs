using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLoot : MonoBehaviour {

	public static PlayerLoot Instance;

	public delegate void OpenInventory ();
	public OpenInventory openInventory;
	public delegate void CloseInventory ();
	public CloseInventory closeInventory;

	private int selectedMemberIndex = 0;

	[Header("Groups")]
	[SerializeField]
	private GameObject crewGroup;
	private bool canOpen = true;
	[SerializeField]
	private GameObject closeButtonObj;

	public bool CanOpen {
		get {
			return canOpen;
		}
		set {
			canOpen = value;
		}
	}

	[Header("LootUI")]
	[SerializeField]
	private LootUI lootUI;
	[SerializeField]
	private LootUI otherLootUI;

	[Header("Card")]
	[SerializeField]
	private InventoryCard inventoryCard;

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

		// init crew cards
		inventoryCard.Init ();

		HideCrewGroup ();

		lootUI.useInventory += HandleUseInventory;

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
		canOpen = true;
	}

	void HandlePlayStory ()
	{
		canOpen = false;
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

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		targetMember.Health += lootUI.SelectedItem.value;

		int i = (int)(lootUI.SelectedItem.value * 1.5f);

		targetMember.CurrentHunger -= i;

		RemoveSelectedItem ();

	}

	public void EquipItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		if (!targetMember.CheckLevel (lootUI.SelectedItem.level)) {
			Tween.Bounce (inventoryCard.Lvl_Image.transform);
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
	}

	public void ThrowItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

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
			return Crews.playerCrew.CrewMembers[selectedMemberIndex];
		}
	}
	#endregion

	#region properties
	public void ShowInventory (CategoryContentType catContentType ) {

		if (!CanOpen) {
			return;
		}

			// set bool
		opened = true;

			// event
		openInventory ();

			// update member
		inventoryCard.UpdateMember (Crews.playerCrew.CrewMembers[selectedMemberIndex]);

			// update crew position
		Crews.getCrew (Crews.Side.Player).UpdateCrew (Crews.PlacingType.Map);

			// close map
		MapImage.Instance.CloseMap ();

			// show elements
		ShowCrewGroup();
		lootUI.Show (catContentType);
	}
	public void HideInventory () {

		if (Opened == false)
			return;

			// set bool
		opened = false;

		Crews.getCrew (Crews.Side.Player).captain.Icon.MoveToPoint (Crews.playerCrew.captain.Icon.PreviousPlacingType);

		foreach (CrewMember member in Crews.getCrew(Crews.Side.Player).CrewMembers)
			member.Icon.Down ();

			// event
		closeInventory();

			// hide elements
		HideCrewGroup();
		lootUI.Hide ();

	}

	public bool Opened {
		get {
			return opened;
		}
	}

	public void ShowMember ( int i ) {
		Transform parent = inventoryCard.IconAnchor;
		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (parent);
		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
		Crews.playerCrew.CrewMembers [i].Icon.Overable = false;
	}

	public void HideMember (int i ) {
		Transform parent = crewCanvas;
		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (parent);

		Crews.playerCrew.CrewMembers[i].Icon.MoveToPoint (Crews.playerCrew.CrewMembers[i].Icon.CurrentPlacingType, 0.2f);
		Crews.playerCrew.CrewMembers [i].Icon.Overable = true;
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

	public int SelectedMemberIndex {
		get {
			return selectedMemberIndex;
		}
		set {

			Crews.playerCrew.CrewMembers [selectedMemberIndex].Icon.Down ();


			selectedMemberIndex = value;

			Crews.playerCrew.CrewMembers [value].Icon.Up ();

		}
	}

}
