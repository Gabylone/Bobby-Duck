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

	void Start () {
		_transform = transform;
	}

	#region overing
	public void OnPointerDown() {
		
		if (Member.side == Crews.Side.Enemy)
			return;

		if (CrewInventory.Instance.Opened && CrewMember.selectedMember == Member) {

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
					CrewInventory.Instance.ShowInventory (CategoryContentType.Inventory , Member);
					break;
				case OtherInventory.Type.Loot:
					CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerLoot, Member);
					break;
				case OtherInventory.Type.Trade:
					CrewInventory.Instance.ShowInventory (CategoryContentType.PlayerTrade, Member);
					break;
				}

			} else {
				CrewInventory.Instance.ShowInventory (CategoryContentType.Inventory , Member);
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

		print (Member.MemberName);
		Vector3 targetPos = Crews.getCrew(Member.side).CrewAnchors [(int)targetPlacingType].position + Crews.playerCrew.CrewAnchors [(int)targetPlacingType].up * decal;

		if (targetPlacingType == Crews.PlacingType.Combat || targetPlacingType == Crews.PlacingType.Map) {
			HideBody();

			targetPos = Crews.getCrew (Member.side).mapAnchors [Member.GetIndex].position;
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
	public void UpdateVisual (MemberID memberID)
	{
		GetComponent<IconVisual> ().UpdateVisual (memberID);
	}
	#endregion

	#region properties
	public CrewMember Member {
		get {
			print ("crew : " + Crews.playerCrew.name);
			return Crews.playerCrew.CrewMembers[index];
		}
	}
	#endregion
}