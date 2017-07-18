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

	public bool CanOpen {
		get {
			return canOpen;
		}
		set {
			canOpen = value;
		}
	}

	public GameObject CrewGroup {
		get {
			return crewGroup;
		}
	}

	[Header("Category Contents")]
	[SerializeField]
	private CategoryContent inventoryCategoryContent;
	[SerializeField]
	private CategoryContent tradeCategoryContent;

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

	void Start() {
		Init ();
	}

	private void Init () {

		// init crew cards
		inventoryCard.Init ();

		crewGroup.SetActive (false);

		lootUI.useInventory += HandleUseInventory;
	}

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

	#region button action


	public void EatItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		targetMember.Health += lootUI.SelectedItem.value;
		targetMember.CurrentHunger -= (int)(lootUI.SelectedItem.value * 1.7f);

		RemoveSelectedItem ();

		if ( CombatManager.Instance.Fighting ) {

			ateSomething = true;

			Close ();

			CombatManager.Instance.NextTurn ();
		}

	}

	public void EquipItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		if (!targetMember.CheckLevel (lootUI.SelectedItem.level))
			return;

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

		PlayerLoot.Instance.inventoryCard.UpdateMember (targetMember);

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

	public void UseItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		switch (lootUI.SelectedItem.category) {
		case ItemCategory.Provisions:
			
			break;
		case ItemCategory.Weapon:
		case ItemCategory.Clothes:

			break;
		}

	}
	#endregion

	#region crew navigator
	public void Open (int id) {
		if (!CanOpen)
			return;
		if ( opened ) {
			HideMember (selectedMemberIndex);
		}
		selectedMemberIndex = id;
		Open (inventoryCategoryContent);
	}
	public void Open (CategoryContent categorycontent) {
		
		Opened = true;
		lootUI.Show (categorycontent);

		if (openInventory != null)
			openInventory ();
		else
			Debug.LogError ("no events linked to open inventory");
	}
	public void Close () {

		if (closeInventory != null)
			closeInventory();
		else
			Debug.LogError ("no events linked to close inventory");

		Opened = false;
		lootUI.Visible = false;

		if ( CombatManager.Instance.Fighting ) {

			if (ateSomething == false)
				CombatManager.Instance.ChangeState (CombatManager.States.PlayerAction);

			ateSomething = false;
		}
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
	public bool Opened {
		get {
			return opened;
		}
		set {

			opened = value;

			crewGroup.SetActive (value);

			if ( value == true )
				MapImage.Instance.CloseMap();

			if (value)
				ShowMember (selectedMemberIndex);
			else
				HideMember (selectedMemberIndex);

			if ( value == false )
				BoatUpgradeManager.Instance.CloseUpgradeMenu ();
		}
	}

	public void ShowMember ( int i ) {
		Transform parent = inventoryCard.IconAnchor;
		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (parent);

		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
		inventoryCard.UpdateMember (Crews.playerCrew.CrewMembers[i]);
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

	public CategoryContent TradeCategoryContent {
		get {
			return tradeCategoryContent;
		}
	}

	public ActionGroup ActionGroup {
		get {
			return actionGroup;
		}
	}
}
