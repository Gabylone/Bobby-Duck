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

	private Transform _transform;

	public bool overable = true;

	public float moveDuration = 1f;

	public Crews.PlacingType currentPlacingType;
	public Crews.PlacingType previousPlacingType;

	public Transform dialogueAnchor;

	public CrewMember member;

	void Start () {
		_transform = transform;
	}

	public void SetMember (CrewMember member) {

		this.member = member;

		UpdateVisual (member.MemberID);
	}

	#region overing
	public void OnPointerDown() {
		
		if (member.side == Crews.Side.Enemy)
			return;

		if (CrewInventory.Instance.Opened && CrewMember.selectedMember == member) {

			if ( OtherInventory.Instance.type != OtherInventory.Type.None ) {
				return;
			}

			CrewInventory.Instance.HideInventory ();

			Down ();

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

		Transform t = transform.parent;

		transform.SetParent(transform.parent.parent);

		transform.SetParent(t);

		Tween.Scale ( transform , 0.3f  , 1.3f);

	}

	public void Down () {
		
		Tween.Scale ( transform , 0.3f  , 1f);

	}
	#endregion

	#region movement
	public void MoveToPoint ( Crews.PlacingType targetPlacingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = targetPlacingType;

		float decal = 0f;

		if (currentPlacingType == Crews.PlacingType.Discussion
			||currentPlacingType == Crews.PlacingType.SoloCombat) {
			ShowBody ();
		}

		Vector3 targetPos = Crews.getCrew(member.side).CrewAnchors [(int)targetPlacingType].position;

		if (targetPlacingType == Crews.PlacingType.Combat || targetPlacingType == Crews.PlacingType.Map) {
			
			HideBody();

			targetPos = Crews.getCrew (member.side).mapAnchors [member.GetIndex].position;
		}


		HOTween.To ( transform , moveDuration , "position" , targetPos , false , EaseType.Linear , 0f );
	}
	#endregion

	#region body
	public void HideBody () {
		bodyGroup.SetActive (false);
	}
	public void ShowBody () {
		bodyGroup.SetActive (true);
	}
	public void UpdateVisual (Member memberID)
	{
		GetComponent<IconVisual> ().UpdateVisual (memberID);
	}
	#endregion
}