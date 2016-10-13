using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CrewManager : MonoBehaviour {
	
	Crews.PlacingType previousPlacingType = Crews.PlacingType.Map;
	Crews.PlacingType currentPlacingType = Crews.PlacingType.Map;

	List<CrewMember> crewMembers = new List<CrewMember> ();

	[SerializeField] private Crews.Side side;
	[SerializeField] private Transform[] crewAnchors;

	private bool placingCrew = false;

	[SerializeField] private float placingDuration = 0.5f;

	private float timer = 0f;

	[SerializeField]
	private Vector3[] crewDecals = new Vector3 [2] { new Vector3(0.3f,0f),new Vector3(0f, 0.35f)};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//
//		if (Input.GetKeyDown (KeyCode.P) && this.gameObject.name == "PlayerCrew") {
//			Crews.playerCrew.RemoveMember (crewMembers[0]);
//			Crews.playerCrew.UpdateCrew (Crews.PlacingType.Map);
//		}
	}

	#region crew placement
	public void UpdateCrew ( Crews.PlacingType placingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = placingType;

		foreach ( CrewMember member in crewMembers ) {
			member.Icon.MoveToPoint (placingType);
		}
	}

	public Vector3 GetMemberPos ( Crews.PlacingType placingType , int memberIndex ) {
		return crewAnchors [(int)currentPlacingType].position + (crewDecals [(int)currentPlacingType] * memberIndex);
	}
	public float PlacingDuration {
		get {
			return placingDuration;
		}
		set {
			placingDuration = value;
		}
	}
	public Transform[] CrewAnchors {
		get {
			return crewAnchors;
		}
	}
	#endregion

	#region crew list
	public void AddMember ( CrewMember member ) {
		crewMembers.Add (member);
	}
	public List<CrewMember> CrewMembers {
		get {
			return crewMembers;
		}
	}
	public CrewMember captain {
		get {
			return crewMembers [0];
		}
	}
	public void RemoveMember ( CrewMember member ) {

		Destroy (member.IconObj);

		crewMembers.Remove (member);
	}
	public void Hide () {
		for (int i = 0; i < crewMembers.Count; ++i) {
			RemoveMember (crewMembers[i]);
		}
	}
	#endregion

	#region creation
	public void CreateRandomCrew () {
		
		Hide ();

		CrewCreator.Instance.TargetSide = side;

		CrewMember crewMember;

		int count = Random.Range (2,7);
		if (side == Crews.Side.Enemy)
			count = 1;

		for (int i = 0; i < count; ++i ) {
			CrewMember member = CrewCreator.Instance.NewMember ();
			AddMember (member);
		}
	}
	public void CreateRandomMember () {

		crewMembers.Clear ();

		CrewCreator.Instance.TargetSide = side;

		CrewMember crewMember = CrewCreator.Instance.NewMember ();
		AddMember (crewMember);

		crewMember.Icon.MoveToPoint (Crews.PlacingType.Discussion);
	}
	#endregion

	#region property

	#endregion


}
