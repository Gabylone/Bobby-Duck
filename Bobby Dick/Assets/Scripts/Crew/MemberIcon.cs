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

	Vector3 initScale;

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
		initScale = group.transform.localScale;
	
		HideBody ();
		UpdateVisual (member.MemberID);

	}

	#region overing
	public void OnPointerDown() {

		if (member.side == Crews.Side.Enemy) {
			StoryInput.Instance.LockFromMember ();
			GetComponentInChildren<StatGroup> ().Display (member);
			return;
		}

		if (CrewInventory.Instance.Opened && CrewMember.selectedMember == member) {

			if ( OtherInventory.Instance.type != OtherInventory.Type.None ) {
				return;
			}

			CrewInventory.Instance.HideInventory ();

		} else {

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
		
	}
	#endregion

	#region bounce
	public void Up () {

//		Transform t = transform.parent;
//
//		transform.SetParent(transform.parent.parent);
//
//		transform.SetParent(t);
//
//		Tween.Scale ( transform , 0.3f  , 1.3f);

//		MoveToPoint (Crews.PlacingType.Discussion);

	}

	public void Down () {

//		MoveToPoint (Crews.PlacingType.Map);

//		Tween.Scale ( transform , 0.3f  , 1f);

	}

	#endregion

	#region movement
	public void MoveToPoint ( Crews.PlacingType targetPlacingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = targetPlacingType;

		float decal = 0f;

		Vector3 targetPos = Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].position;

		if (currentPlacingType == Crews.PlacingType.Discussion
			||currentPlacingType == Crews.PlacingType.SoloCombat) {
			ShowBody ();
		} else {
			HideBody();
		}

		if ( currentPlacingType == Crews.PlacingType.Map )
			targetPos = Crews.getCrew (member.side).mapAnchors [member.GetIndex].position;

//		print ("moviong target : " + Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].name);

		HOTween.To ( transform , moveDuration , "position" , targetPos , false , EaseType.Linear , 0f );
	}
	#endregion

	#region body
	public void HideBody () {
		bodyGroup.SetActive (false);
		animator.SetBool ("enabled", false);

		Vector3 targetScale = initScale;
//		Vector3 targetScale = Vector3.one * initScale;
//		if (member.side == Crews.Side.Player)
//			targetScale.x = -1;

//		HOTween.To ( group.transform , moveDuration / 2f , "localScale" , targetScale );

	}
	public void ShowBody () {
		bodyGroup.SetActive (true);
		animator.SetBool ("enabled", true);

		Vector3 targetScale = Vector3.one * bodyScale;
		if (member.side == Crews.Side.Player)
			targetScale.x = -bodyScale;
		
//		HOTween.To (group.transform, moveDuration / 2f, "localScale", targetScale);

	}

	public void UpdateVisual (Member memberID)
	{
		GetComponent<IconVisual> ().UpdateVisual (memberID);
	}
	#endregion
}