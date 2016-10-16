using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewIcon : MonoBehaviour {

	private int id = 0;

	bool pointerOver = false;

	private CrewMember member;

	[SerializeField]
	private float scaleAmount = 1.5f;
	[SerializeField]
	private float scaleDuration = 0.35f;

	[SerializeField]
	private GameObject faceObj;

	[SerializeField]
	private GameObject bodyObj;

	private bool overable = true;

	private Transform _transform;

	float timer = 0f;
	bool choosingMember = false;

	bool scaleLerp = false;

	bool moveLerp = false;
	Vector3 initPos = Vector3.zero;
	Vector3 targetPos = Vector3.zero;
	[SerializeField]
	private float moveDuration = 1f;

	[Header("decals")]
	[SerializeField]
	private float overingDecal = 2f;
	[SerializeField]
	private float placementDecal = 1f;
	Crews.PlacingType currentPlacingType;
	

	void Awake () {
		_transform = transform;
	}

	void Update () {

		if (moveLerp ) {
			MoveUpdate ();
		}

		if ( scaleLerp ) {
			
			float l = pointerOver ? (timer / scaleDuration) : 1-(timer / scaleDuration);

			GetTransform.localScale = Vector3.Lerp (Vector3.one, Vector3.one * scaleAmount, l);
//
			if (timer >= scaleDuration)
				scaleLerp = false;
		}

		if (moveLerp || scaleLerp)
			timer += Time.deltaTime;

	}
	#region selection
	public void OnPointerEnter() {

		if (!overable)
			return;

		pointerOver = true;
		scaleLerp = true;

		CardManager.Instance.ShowOvering (member);
	}

	public void OnPointerExit () {

		if (overable == false)
			return;

		pointerOver = false;
		scaleLerp = true;

		CardManager.Instance.HideOvering ();
	}
	public void OnPointerDown() {

		OnPointerExit ();

		if ( InventoryManager.Instance.Opened ) {

			InventoryManager.Instance.SelectedMember = member.GetIndex;

		} else if (choosingMember) {

			CombatManager.Instance.SetPlayerMember (Member);

		} else {

			DialogueManager.Instance.SetDialogue ("Oui ?", GetTransform);
		
		}

	}
	#endregion

	#region movement

	public void MoveToPoint ( Vector3 pos , float duration = 0.5f ) {
		
		targetPos = pos;

		MoveStart ();
	}

	public void MoveToPoint ( Crews.PlacingType placingType , float duration = 0.5f ) {

		currentPlacingType = placingType;
		
		float decal = 0f;

		if (placingType == Crews.PlacingType.Combat || placingType == Crews.PlacingType.Map) {

			decal = member.GetIndex;
		}

		targetPos = Crews.getCrew(member.Side).CrewAnchors [(int)placingType].position + Crews.playerCrew.CrewAnchors [(int)placingType].up * decal;

		MoveStart ();
	}

	private void MoveStart () {
//		overable = false;
		moveLerp = true;

		initPos = GetTransform.position;

		timer = 0f;
	}
	private void MoveUpdate () {

		float l = timer / moveDuration;

		GetTransform.position = Vector3.Lerp ( initPos , targetPos , l );

		if (timer > moveDuration)
			MoveExit ();
	}

	public void MoveExit () {
		moveLerp = false;
		overable = true;

		if (currentPlacingType == Crews.PlacingType.Discussion
			||currentPlacingType == Crews.PlacingType.SoloCombat) {
			ShowBody ();
		}

		if (currentPlacingType == Crews.PlacingType.Combat
			|| currentPlacingType == Crews.PlacingType.Map) {
			HideBody();
		}

	}
	#endregion

	#region body
	public void HideBody () {
		bodyObj.SetActive (false);
	}
	public void ShowBody () {
		bodyObj.SetActive (true);
	}
	public void HideFace () {
		faceObj.SetActive (false);
	}
	public void ShowFace () {
		faceObj.SetActive (true);
	}
	#endregion

	#region properties
	public int Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

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
		}
	}

	public bool ChoosingMember {
		get {
			return choosingMember;
		}
		set {
			choosingMember = value;
		}
	}
	public bool Overable {
		get {
			return overable;
		}
		set {
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
}
