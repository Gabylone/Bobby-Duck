using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewNavigator : MonoBehaviour {

	public static CrewNavigator Instance;

	private int selectedMember = 0;

	[SerializeField]
	private LootUI lootUI;

	[Header("Card")]
	[SerializeField]
	private GameObject inventoryCardsParent;
	private InventoryCard[] inventoryCards;
	Vector3[] iconPositions;
	private Vector3 cardOrigin;

	[SerializeField]
	private Transform crewCanvas;

	[Header("Loot")]
	[SerializeField]
	private GameObject lootObj;

	bool opened = false;

	void Awake () {
		Instance = this;
	}

	void Start() {

		lootObj.SetActive (false);
		inventoryCards = inventoryCardsParent.GetComponentsInChildren<InventoryCard>();

		int a = 0;
		iconPositions = new Vector3[inventoryCards.Length];
		foreach (InventoryCard inventoryCard in inventoryCards) {
			iconPositions [a] = inventoryCard.IconAnchor.position;
			inventoryCard.MemberIndex = a;
			++a;
		}

		cardOrigin = inventoryCards [0].GetTransform.localPosition;
	}

	#region use item
	public void UseItem () {

		CrewMember targetMember = CrewNavigator.Instance.SelectedMember;

		switch (lootUI.CurrentCategory) {
		case ItemLoader.ItemType.Provisions:
			targetMember.Health += lootUI.SelectedItem.value;
			// states 
			break;
		case ItemLoader.ItemType.Weapon:
			targetMember.AttackDice = lootUI.SelectedItem.value;
			if (targetMember.Equipment [0] != null)
				LootManager.playerLoot.AddItem (lootUI.CurrentCategory, targetMember.Equipment [0]);

			targetMember.Equipment [0] = lootUI.SelectedItem;

			CrewNavigator.Instance.SelectedCard.Deploy ();
			break;
		case ItemLoader.ItemType.Clothes:
			targetMember.ConstitutionDice = lootUI.SelectedItem.value;
			if (targetMember.Equipment [2] != null)
				LootManager.playerLoot.AddItem (lootUI.CurrentCategory, targetMember.Equipment [2]);

			targetMember.Equipment [2] = lootUI.SelectedItem;
			CrewNavigator.Instance.SelectedCard.Deploy ();
			break;
		}

		int removeIndex = lootUI.CurrentPage + lootUI.SelectionIndex;

		LootManager.playerLoot.RemoveItem ( lootUI.CurrentCategory, removeIndex);

		lootUI.UpdateLootUI ();
		CrewNavigator.Instance.UpdateMembers();

	}
	#endregion

	#region inventory states
	public void Open () {

		opened = true;
		lootObj.SetActive (true);

		UpdateMembers ();
		SelectedMemberIndex = 0;

		// set icons
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (inventoryCards[i].IconAnchor);
//			Crews.playerCrew.CrewMembers [i].Icon.MoveToPoint (iconPositions[i], 0.2f);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
			Crews.playerCrew.CrewMembers[i].Icon.HideBody ();
			Crews.playerCrew.CrewMembers[i].Icon.Overable = false;
		}



	}

	public void Close () {
		
		opened = false;
		lootObj.SetActive (false);
		
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

	public GameObject LootObj {
		get {
			return lootObj;
		}
	}

	public LootUI LootUI {
		get {
			return lootUI;
		}
	}
	#endregion
}
