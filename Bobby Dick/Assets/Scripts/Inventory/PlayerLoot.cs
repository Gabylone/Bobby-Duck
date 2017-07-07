using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLoot : MonoBehaviour {

	public static PlayerLoot Instance;

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

	[Header("Sounds")]
	[SerializeField] private AudioClip eatSound;
	[SerializeField] private AudioClip equipSound;
	[SerializeField] private AudioClip sellSound;

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
	}

	#region button action

	bool ateSomething = false;

	public void Eat () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		SoundManager.Instance.PlaySound (eatSound);

		targetMember.Health += lootUI.SelectedItem.value;
		targetMember.CurrentHunger -= (int)(lootUI.SelectedItem.value * 1.7f);

		RemoveSelectedItem ();

		if ( CombatManager.Instance.Fighting ) {

			ateSomething = true;

			Close ();

			CombatManager.Instance.NextTurn ();
		}

	}

	public void Equip () {

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

		SoundManager.Instance.PlaySound (equipSound);

		PlayerLoot.Instance.inventoryCard.UpdateMember (targetMember);

		RemoveSelectedItem ();
	}

	public void Throw () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		SoundManager.Instance.PlaySound (equipSound);

		RemoveSelectedItem ();
	}

	public void Sell () {

		SoundManager.Instance.PlaySound (sellSound);

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
		Opened = true;
		lootUI.Show (inventoryCategoryContent);
	}
	public void Open (CategoryContent categorycontent) {
		Opened = true;
		lootUI.Show (categorycontent);
	}
	public void Close () {

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

			MapImage.Instance.MapButton.Opened = false;

			if (value)
				ShowMember (selectedMemberIndex);
			else
				HideMember (selectedMemberIndex);

			if ( value == false )
				BoatUpgradeManager.Instance.CloseUpgradeMenu ();

			Karma.Instance.Visible = value;
		}
	}

	public void ShowMember ( int i ) {
		Transform parent = inventoryCard.IconAnchor;
		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (parent);

		Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
//		inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);
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
