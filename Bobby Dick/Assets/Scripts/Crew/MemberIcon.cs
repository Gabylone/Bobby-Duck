using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class MemberIcon : MonoBehaviour {

	public int index = 0;

		// components
	[Header ("Components")]
	public GameObject group;
	public GameObject faceGroup;
	public GameObject bodyGroup;

	public Animator animator;

	private Transform _transform;

	public bool overable = true;

	public float moveDuration = 1f;

	public float bodyScale = 1f;

	public float initScale;

	public Crews.PlacingType currentPlacingType = Crews.PlacingType.None;
	public Crews.PlacingType previousPlacingType  = Crews.PlacingType.None;

	public Transform dialogueAnchor;

	public CrewMember member;

	void Start () {
		_transform = transform;
	}

	public void SetMember (CrewMember member) {

		this.member = member;

		animator = GetComponentInChildren<Animator> ();
		initScale = group.transform.localScale.x;
	
		HideBody ();
		InitVisual (member.MemberID);

	}

	#region overing
	public void OnPointerDown() {

		if (member.side == Crews.Side.Enemy) {
			StoryInput.Instance.LockFromMember ();
			GetComponentInChildren<StatGroup> ().Display (member);
			return;
		}

		if ( !CrewInventory.Instance.canOpen ) {
			print ("cannot open player loot");
			return;
		}

		if (StoryLauncher.Instance.PlayingStory) {

			switch (OtherInventory.Instance.type) {
			case OtherInventory.Type.None:
				CrewInventory.Instance.ShowInventory (CategoryContentType.Inventory , member);
				break;
			case OtherInventory.Type.Loot:
				CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot, member);
				break;
			case OtherInventory.Type.Trade:
				CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerTrade, member);
				break;
			}

		} else {
			CrewInventory.Instance.ShowInventory (CategoryContentType.Inventory , member);
		}
		
	}
	#endregion

	#region movement
	public void MoveToPoint ( Crews.PlacingType targetPlacingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = targetPlacingType;

		float decal = 0f;

		Vector3 targetPos = Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].position;

		if (currentPlacingType == Crews.PlacingType.Map) {
			int index = member.GetIndex;
			if (index < 0) {
				Debug.LogError ("index : " + index + " mapanchors :" + Crews.getCrew (member.side).mapAnchors.Length);
				Debug.LogError ("membre à probleme  "+ member.MemberName);
				Debug.LogError ("current membre  "+ CrewMember.GetSelectedMember.MemberName);
			}
			targetPos = Crews.getCrew (member.side).mapAnchors [member.GetIndex].position;
		}

//		print ("moviong target : " + Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].name);

		HOTween.To ( transform , moveDuration , "position" , targetPos , false , EaseType.Linear , 0f );

		switch (currentPlacingType) {
		case Crews.PlacingType.Map:
		case Crews.PlacingType.Hidden:
		case Crews.PlacingType.None:
			HideBody ();
			break;
		case Crews.PlacingType.Combat:
		case Crews.PlacingType.Inventory:
		case Crews.PlacingType.Discussion:
			ShowBody ();
			break;
		default:
			break;
		}
	}
	#endregion

	#region body
	public void HideBody () {
		
		bodyGroup.SetActive (false);
		animator.SetBool ("enabled", false);

//		Vector3 targetScale = Vector3.one * initScale;
//		if (member.side == Crews.Side.Player)
//			targetScale.x = -targetScale.x;
//
//		HOTween.To ( group.transform , moveDuration / 2f , "localScale" , targetScale );

	}
	public void ShowBody () {
		
		bodyGroup.SetActive (true);
		animator.SetBool ("enabled", true);
//
//		Vector3 targetScale = Vector3.one * bodyScale;
//		if (member.side == Crews.Side.Player)
//			targetScale.x = -bodyScale;
//		
//		HOTween.To (group.transform, moveDuration / 2f, "localScale", targetScale);

	}

	public void InitVisual (Member memberID)
	{
		GetComponent<IconVisual> ().InitVisual (memberID);
	}
	#endregion
}