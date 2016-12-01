using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLoot : MonoBehaviour {

	public static PlayerLoot Instance;

	private int selectedMember = 0;

	[SerializeField]
	private CategoryContent inventoryCategoryContent;
	[SerializeField]
	private CategoryContent tradeCategoryContent;

	[SerializeField]
	private Crews.Side targetSide;

	[Header("LootUI")]
	[SerializeField]
	private LootUI lootUI;

	[Header("Card")]
	[SerializeField]
	private GameObject inventoryCardsParent;
	private InventoryCard[] inventoryCards;
	private Vector3 cardOrigin;

	[SerializeField]
	private Transform crewCanvas;

	[Header("Sounds")]
	[SerializeField] private AudioClip eatSound;
	[SerializeField] private AudioClip equipSound;

	bool opened = false;

	void Awake () {
		Instance = this;
	}

	void Start() {
		Init ();
	}

	private void Init () {

		// init crew cards
		inventoryCards = inventoryCardsParent.GetComponentsInChildren<InventoryCard>();

		// set indexes
		int a = 0;
		foreach (InventoryCard inventoryCard in inventoryCards) {
			inventoryCard.MemberIndex = a;
			++a;
		}

		cardOrigin = inventoryCards [0].GetTransform.localPosition;
	}

	#region button action
	public void Action () {
		if (OtherLoot.Instance.Trading) {
			OtherLoot.Instance.SellItem ( lootUI.SelectedItem.category ,lootUI.ItemIndex);
		} else {
			UseItem ();
		}
	}
	public void UseItem () {

		CrewMember targetMember = PlayerLoot.Instance.SelectedMember;

		switch (lootUI.SelectedItem.category) {
		case ItemCategory.Provisions:

			SoundManager.Instance.PlaySound (eatSound);

			targetMember.Health += lootUI.SelectedItem.value;
			targetMember.CurrentHunger -= (lootUI.SelectedItem.value * 2);

			UpdateMembers ();
			break;
		case ItemCategory.Weapon:
		case ItemCategory.Clothes:
		case ItemCategory.Shoes:

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
			case ItemCategory.Shoes:
				part = CrewMember.EquipmentPart.Shoes;
				break;
			}

			if (targetMember.GetEquipment(part) != null)
				LootManager.Instance.PlayerLoot.AddItem (targetMember.GetEquipment(part) );

			targetMember.SetEquipment (part, lootUI.SelectedItem);

			PlayerLoot.Instance.SelectedCard.Deploy ();

			SoundManager.Instance.PlaySound (equipSound);

			break;
		}

		LootManager.Instance.PlayerLoot.RemoveItem ( lootUI.SelectedItem);
		lootUI.UpdateLootUI ();
		PlayerLoot.Instance.UpdateMembers();

	}
	#endregion

	#region crew navigator
	public void Switch () {
		if (opened)
			Close ();
		else
			Open ();
	}
	private void Open () {

			// Members
		UpdateMembers ();
		SelectedMemberIndex = 0;

			// set icons
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (inventoryCards[i].IconAnchor);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
			Crews.playerCrew.CrewMembers[i].Icon.HideBody ();
			Crews.playerCrew.CrewMembers[i].Icon.Overable = false;
		}

			// loot
		opened = true;
		lootUI.CategoryContent = inventoryCategoryContent;
		lootUI.Visible = true;

	}

	private void Close () {

		opened = false;
		lootUI.Visible = false;
		
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			
			Crews.playerCrew.CrewMembers[i].Icon.MoveToPoint (Crews.playerCrew.CrewMembers[i].Icon.CurrentPlacingType, 0.2f);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (crewCanvas);
			Crews.playerCrew.CrewMembers[i].Icon.Overable = true;
			
			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);
		}
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
	public void UpdateMembers () {

		int decal = 0;

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);

			Vector3 pos = cardOrigin - Vector3.up * (decal);

			inventoryCards [i].GetTransform.localPosition = pos;

			decal += 100;
			if (i == SelectedMemberIndex)
				decal += 100;
		}
	}
	#endregion

	#region properties
	public bool Opened {
		get {
			return opened;
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
}
