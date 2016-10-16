using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour {

	public static InventoryManager Instance;

	[Header("Selection")]
	[SerializeField]
	private RectTransform select_Transform;
	private int selectedMember = 0;

	[Header("Card")]
	[SerializeField]
	private GameObject inventoryCardsParent;
	private Card[] inventoryCards;

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
		inventoryCards = inventoryCardsParent.GetComponentsInChildren<Card>();
	}

	#region inventory states
	public void Open () {

		opened = true;
		lootObj.SetActive (true);

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {

//			Crews.playerCrew.CrewMembers[i].Icon.MoveToPoint (inventoryCards[i].IconAnchor.position);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (inventoryCards[i].IconAnchor);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.localPosition = Vector3.zero;
			Crews.playerCrew.CrewMembers[i].Icon.HideBody ();
			Crews.playerCrew.CrewMembers[i].Icon.Overable = false;
			
			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);

		}

		UpdateSelection ();

	}

	public void Close () {
		
		opened = false;
		lootObj.SetActive (false);
		
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			
			Crews.playerCrew.CrewMembers[i].Icon.MoveToPoint (Crews.playerCrew.CrewMembers[i].Icon.CurrentPlacingType);
			Crews.playerCrew.CrewMembers[i].Icon.GetTransform.SetParent (crewCanvas);
			Crews.playerCrew.CrewMembers[i].Icon.Overable = true;
			
			inventoryCards[i].UpdateMember (Crews.playerCrew.CrewMembers[i]);
			
		}
		
	}
	#endregion

	#region crew management
	private void UpdateSelection () {
		select_Transform.SetParent (inventoryCards[selectedMember].IconAnchor);
		select_Transform.localPosition = Vector3.zero;
	}
	public int SelectedMember {
		get {
			return selectedMember;
		}
		set {
			selectedMember = value;
			UpdateSelection ();
		}
	}
	#endregion

	#region loot management
	#endregion

	#region properties
	public bool Opened {
		get {
			return opened;
		}
	}
	#endregion
}
