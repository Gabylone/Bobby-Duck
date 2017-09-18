using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

public class CrewIcon : MonoBehaviour {

		// components
	[Header ("Components")]
	[SerializeField]
	private GameObject controllerTransform;

	[SerializeField]
	private GameObject faceObj;

	[SerializeField]
	private GameObject bodyObj;

	private CrewMember member;
	private Transform _transform;

	[Header("Overing")]
	[SerializeField]
	private float scaleAmount = 1.5f;
	[SerializeField]
	private float scaleDuration = 0.35f;
	private bool pointerOver = false;
	private bool overable = true;
	private bool showCard = true;

	[Header("Lerping")]
	[SerializeField]
	private float moveDuration = 1f;

	[Header("decals")]
	[SerializeField]
	private float overingDecal = 2f;
	[SerializeField]
	private float placementDecal = 1.3f;

	Crews.PlacingType currentPlacingType;
	Crews.PlacingType previousPlacingType;

	public Transform dialogueAnchor;

	void Awake () {
		_transform = transform;
	}

	#region overing
//	public void OnPointerEnter() {
//
//		if (CombatManager.Instance.Fighting) {
//			CardManager.Instance.ShowFightingCard (member);
//		}
//		return;
//	}

	public void OnPointerDown() {
		
		if (!Overable) {
			return;
		}

		if (member.Side == Crews.Side.Enemy)
			return;

		if (PlayerLoot.Instance.Opened && PlayerLoot.Instance.SelectedMember == member) {

			if ( OtherLoot.Instance.trading || OtherLoot.Instance.looting ) {
				return;

			}

			PlayerLoot.Instance.HideInventory ();

			Down ();

			showCard = true;

		} else {

			if ( !PlayerLoot.Instance.canOpen ) {
				print ("cannot open player loot");
				return;
			}

			if (StoryLauncher.Instance.PlayingStory) {
				if (OtherLoot.Instance.trading) {
					PlayerLoot.Instance.ShowInventory (CategoryContentType.PlayerTrade, member);
				} else if (OtherLoot.Instance.looting) {
					PlayerLoot.Instance.ShowInventory (CategoryContentType.PlayerLoot, member);
				} else {
					PlayerLoot.Instance.ShowInventory (CategoryContentType.Inventory , member);
				}
			} else {
				PlayerLoot.Instance.ShowInventory (CategoryContentType.Inventory , member);
			}

			showCard = false;

		}
		
	}
	#endregion

	#region bounce
	public void Up () {
		Tween.Scale ( transform , 0.3f  , 1.3f);
	}
	public void Down () {
		Tween.Scale ( transform , 0.3f  , 1f);
	}
	#endregion

	#region movement
	public void MoveToPrevPoint () {

		if ( PreviousPlacingType == Crews.PlacingType.Hidden ) {
			Debug.LogError ("wait no... previous point is hidden");
			previousPlacingType = Crews.PlacingType.Map;
		}

		MoveToPoint (previousPlacingType);
	}
	public void MoveToPoint ( Crews.PlacingType targetPlacingType ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = targetPlacingType;

		float decal = 0f;

		if (currentPlacingType == Crews.PlacingType.Discussion
			||currentPlacingType == Crews.PlacingType.SoloCombat) {
			ShowBody ();
		}

		if (targetPlacingType == Crews.PlacingType.Combat || targetPlacingType == Crews.PlacingType.Map) {
			HideBody();
			decal = member.GetIndex * placementDecal;
		}

		Vector3 targetPos = Crews.getCrew(member.Side).CrewAnchors [(int)targetPlacingType].position + Crews.playerCrew.CrewAnchors [(int)targetPlacingType].up * decal;

		HOTween.To ( GetTransform , moveDuration , "position" , targetPos , false , EaseType.Linear , 0f );
	}
	#endregion

	#region body
	public void HideBody () {
		bodyObj.SetActive (false);
	}
	public void ShowBody () {
		bodyObj.SetActive (true);
	}
	public void UpdateVisual (MemberID memberID)
	{
		GetComponent<IconVisual> ().UpdateVisual (memberID);
	}
	#endregion

	#region properties
	public Transform GetTransform {
		get {
			return _transform;
		}
	}

	public CrewMember Member {
		get {
			return member;
		}
		set {
			member = value;

			GetComponent<IconVisual> ().UpdateVisual (member.MemberID);
		}
	}

	public bool Overable {
		get {
			return overable;
		}
		set {

			if (member.Side == Crews.Side.Enemy && value)
				return;

			overable = value;

		}
	}

	public float MoveDuration {
		get {
			return moveDuration;
		}
	}
	#endregion


	public Crews.PlacingType CurrentPlacingType {
		get {
			return currentPlacingType;
		}
	}

	public Crews.PlacingType PreviousPlacingType {
		get {
			return previousPlacingType;
		}
	}

	public GameObject BodyObj {
		get {
			return bodyObj;
		}
	}

	public GameObject FaceObj {
		get {
			return faceObj;
		}
	}

	public GameObject ControllerTransform {
		get {
			return controllerTransform;
		}
	}
}