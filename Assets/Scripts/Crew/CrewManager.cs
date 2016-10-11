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

	/*#region MemberLerp
	private void MemberLerp_Start () {

		lerpPos = getMember (targetMember).IconObj.transform.position;

		CardManager.Instance.ShowFightingCard (targetMember);

		DialogueManager.Instance.SetDialogue ("A l'abordage !", getMember (targetMember).IconObj.transform);

	}
	private void MemberLerp_Update () {

		Vector3 lerp = Vector3.Lerp (lerpPos, Crews.getCrew (targetMember).CrewAnchors [(int)Crews.PlacingType.SoloCombat].position, timeInState);
		getMember (targetMember).IconObj.transform.position = lerp;

		if ( timeInState > 1 ) {

			getMember(targetMember).Icon.ShowBody ();
			getMember(targetMember).Icon.Overable = false;

			if ( firstTurn ) {
				if ( targetMember == Turns.Player ) {
					targetMember = Turns.Enemy;
					ChangeState (States.MemberLerp);
				} else {
					targetMember = Turns.Player;
					ChangeState (States.StartTurn);
				}
			} else {
				ChangeState (States.StartTurn);
			}

		}
	}
	private void MemberLerp_Exit () {

	}
	#endregion*/

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
	#endregion

	#region creation
	public void CreateRandomCrew () {
		
		crewMembers.Clear ();

		CrewCreator.Instance.TargetSide = side;

		CrewMember crewMember;

		int count = Random.Range (2,7);

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
