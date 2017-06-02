﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLoot : MonoBehaviour {

	public static PlayerLoot Instance;

	private int selectedMember = 0;

	[SerializeField]
	private UIButton inventoryButton;
	[SerializeField]
	private GameObject closeButton;

	public GameObject CloseButton {
		get {
			return closeButton;
		}
	}

	[SerializeField]
	private CategoryContent inventoryCategoryContent;
	[SerializeField]
	private CategoryContent tradeCategoryContent;

	[SerializeField]
	private Crews.Side targetSide;

	[Header("LootUI")]
	[SerializeField]
	private LootUI lootUI;
	[SerializeField]
	private LootUI otherLootUI;

	[Header("Card")]
	[SerializeField]
	private GameObject inventoryCardsParent;
	private InventoryCard[] inventoryCards;

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
		inventoryCards = inventoryCardsParent.GetComponentsInChildren<InventoryCard>(true);

		// set indexes
		int a = 0;
		foreach (InventoryCard inventoryCard in inventoryCards) {
			inventoryCard.MemberIndex = a;
			++a;
		}

	}

	#region button action

	public void Eat () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		SoundManager.Instance.PlaySound (eatSound);

		targetMember.Health += lootUI.SelectedItem.value;

		int foodHealth = (int)(lootUI.SelectedItem.value * 1.5f);
		targetMember.CurrentHunger -= foodHealth;


			// ça c'est pour faire en sorte qu'il puisse prendre qu'un bouffe par tour.

//		if (CombatManager.Instance.Fighting) {
//
//			inventoryButton.Opened = false;
//
//			Close ();
//
//			CombatManager.Instance.ChangeState (CombatManager.States.StartTurn);
//			CardManager.Instance.UpdateCards ();
//
//			UpdateMembers ();
//		}

		RemoveSelectedItem ();

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

		PlayerLoot.Instance.SelectedCard.Deploy ();

		SoundManager.Instance.PlaySound (equipSound);

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
		UpdateMembers ();
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
	public void Switch () {

		lootUI.CategoryContent = inventoryCategoryContent;
		Opened = !Opened;
	}
	#endregion

	#region crew management
	public int SelectedMemberIndex {
		get {
			return selectedMember;
		}
		set {
			inventoryCards [SelectedMemberIndex].GetComponentInChildren<Button>().interactable = true;
			inventoryCards [SelectedMemberIndex].Deployed = false;

			selectedMember = value;

			inventoryCards [SelectedMemberIndex].GetComponentInChildren<Button>().interactable = false;
			inventoryCards [SelectedMemberIndex].Deployed = true;

			UpdateMembers ();
		}
	}
	public CrewMember SelectedMember {
		get {
			return Crews.playerCrew.CrewMembers[SelectedMemberIndex];
		}
	}
	#endregion

	#region Update members
	public Transform anchor;
	public float deployedDecal = 1f;
	public void UpdateMembers () {

		float decal = 0;

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);

			Vector3 pos = anchor.position - Vector3.up * (decal);

			inventoryCards [i].GetTransform.position = pos;

			decal += deployedDecal;
			if (i == SelectedMemberIndex)
				decal += deployedDecal;
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
			lootUI.Visible = value;
			MapImage.Instance.MapButton.Opened = false;
			inventoryButton.Opened = value;

			UpdateMembers ();
			SelectedMemberIndex = 0;

			// set icons
			for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

				Transform parent = value ? inventoryCards [i].IconAnchor : crewCanvas;
				Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (parent);
				Crews.playerCrew.CrewMembers[i].Icon.Overable = !value;

				if ( value == true )
					Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
				else
					Crews.playerCrew.CrewMembers[i].Icon.MoveToPoint (Crews.playerCrew.CrewMembers[i].Icon.CurrentPlacingType, 0.2f);

				inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);
			}

			if ( value == false )
				BoatUpgradeManager.Instance.CloseUpgradeMenu ();

			Karma.Instance.Visible = value;
		}
	}

	public InventoryCard[] InventoryCards {
		get {
			return inventoryCards;
		}
	}

	public InventoryCard SelectedCard {
		get {
			return inventoryCards [SelectedMemberIndex];
		}
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

	public UIButton InventoryButton {
		get {
			return inventoryButton;
		}
	}

	public ActionGroup ActionGroup {
		get {
			return actionGroup;
		}
	}
}