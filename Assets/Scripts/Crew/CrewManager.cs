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

	private int memberCapacity = 2;
	[SerializeField]
	private int maxMember = 8;

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
		
	}

	#region crew placement
	public void HideCrew () {
		foreach ( CrewMember member in crewMembers )
			member.Icon.HideFace ();
	}
	public void ShowCrew () {
		foreach ( CrewMember member in crewMembers )
			member.Icon.ShowFace ();
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

		if (crewMembers.Count == memberCapacity) {
			return;
		}

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
	public void DeleteCrew () {

		for (int i = 0; i < crewMembers.Count; i++ ) {
			RemoveMember (crewMembers [i]);
		}

		crewMembers.Clear ();
	}
	public void Hide () {
		UpdateCrew (Crews.PlacingType.Hidden);
	}
	#endregion

	#region creation
	public void CreateRandomCrew () {
		
		Hide ();

		CrewCreator.Instance.TargetSide = side;

		int count = Random.Range (2,MemberCapacity);

		for (int i = 0; i < count; ++i ) {
			CrewMember member = CrewCreator.Instance.NewMember ();
			AddMember (member);
		}

		UpdateCrew (Crews.PlacingType.Combat);
	}
	public void CreateRandomMember () {

		DeleteCrew ();

		CrewCreator.Instance.TargetSide = side;

		CrewMember crewMember = CrewCreator.Instance.NewMember ();
		AddMember (crewMember);

		crewMember.Icon.MoveToPoint (Crews.PlacingType.Discussion);
	}
	#endregion

	#region property

	#endregion

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
			BoatManager.Instance.UpdateCrewImages ();
		}
	}
}
