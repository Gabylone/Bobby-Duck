using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewNavigator : MonoBehaviour {

	public static CrewNavigator Instance;

	private int selectedMember = 0;

	[Header("Card")]
	[SerializeField]
	private GameObject inventoryCardsParent;
	private Card[] inventoryCards;
	Vector3[] iconPositions;

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
		inventoryCards = inventoryCardsParent.GetComponentsInChildren<Card>();

		int a = 0;
		iconPositions = new Vector3[inventoryCards.Length];
		foreach (Card inventoryCard in inventoryCards) {
			iconPositions [a] = inventoryCard.IconAnchor.position;
			++a;
		}
	}

	#region inventory states
	public void Open () {

		opened = true;
		lootObj.SetActive (true);

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (inventoryCards[i].IconAnchor);
//			Crews.playerCrew.CrewMembers [i].Icon.MoveToPoint (iconPositions[i], 0.2f);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
			Crews.playerCrew.CrewMembers[i].Icon.HideBody ();
			Crews.playerCrew.CrewMembers[i].Icon.Overable = false;
		}

		UpdateMembers ();

		SelectedMemberIndex = 0;

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
			inventoryCards [SelectedMemberIndex].GetComponent<InventoryCard> ().Deployed = false;
			selectedMember = value;

			inventoryCards [SelectedMemberIndex].GetComponentInChildren<Button>().interactable = false;
			inventoryCards [SelectedMemberIndex].GetComponent<InventoryCard> ().Deployed = true;
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
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);
		}
	}
	#endregion

	#region properties
	public bool Opened {
		get {
			return opened;
		}
	}
	#endregion
}
