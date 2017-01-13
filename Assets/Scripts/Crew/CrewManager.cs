using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CrewManager : MonoBehaviour {

	Crew managedCrew;

	Crews.PlacingType previousPlacingType = Crews.PlacingType.Map;
	Crews.PlacingType currentPlacingType = Crews.PlacingType.Map;

	List<CrewMember> crewMembers = new List<CrewMember> ();

	[SerializeField] private Crews.Side side;
	[SerializeField] private Transform[] crewAnchors;

	private int memberCapacity = 2;
	[SerializeField]
	private int maxMember = 8;

	private bool placingCrew = false;

	[SerializeField] private float placingDuration = 0.5f;

	private float timer = 0f;

	[SerializeField]
	private Vector3[] crewDecals = new Vector3 [2] { new Vector3(0.3f,0f),new Vector3(0f, 0.35f)};

	#region crew placement
	public void HideCrew () {
		foreach (CrewMember member in crewMembers) {
			member.Icon.MoveToPoint (Crews.PlacingType.Hidden);
		}
	}
	public void ShowCrew () {
		foreach (CrewMember member in crewMembers) {
			member.Icon.MoveToPoint (member.Icon.PreviousPlacingType);
		}
	}
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

		if (crewMembers.Count == memberCapacity)
			return;

		managedCrew.Add (member.MemberID);
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

		managedCrew.Remove (member.MemberID);
		crewMembers.Remove (member);

		if ( CrewMembers.Count == 0 ) {
			if ( side == Crews.Side.Player ) {
				GameManager.Instance.GameOver ();
			}
		}
	}
	public void DeleteCrew () {

		if (crewMembers.Count == 0)
			return;

		foreach ( CrewMember member in CrewMembers )
			Destroy (member.IconObj);

		crewMembers.Clear ();
	}
	public void Hide () {
		UpdateCrew (Crews.PlacingType.Hidden);
	}
	#endregion

	#region creation
	public void setCrew (Crew crew) {

		DeleteCrew ();

		CrewCreator.Instance.TargetSide = side;

		for (int i = 0; i < crew.MemberIDs.Count; ++i ) {
			CrewMembers.Add (CrewCreator.Instance.NewMember (crew.MemberIDs[i]));
		}

		ManagedCrew = crew;

		UpdateCrew (Crews.PlacingType.Combat);
	}
	#endregion

	#region property
	public int MaxMember {
		get {
			return maxMember;
		}
	}

	public int MemberCapacity {
		get {
			return memberCapacity;
		}
		set {
			memberCapacity = Mathf.Clamp (value, 0, MaxMember);
			BoatUpgradeManager.Instance.UpdateCrewImages ();
		}
	}
	#endregion

	public Crew ManagedCrew {
		get {
			return managedCrew;
		}
		set {
			managedCrew = value;
		}
	}
}
