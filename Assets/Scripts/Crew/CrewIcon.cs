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

	void Awake () {
		_transform = transform;
	}

	void Update () {

		if (moveLerp ) {
			MoveUpdate ();
		}

		if ( scaleLerp ) {
			
			timer += Time.deltaTime;

			float l = pointerOver ? (timer / scaleDuration) : 1-(timer / scaleDuration);

			GetTransform.localScale = Vector3.Lerp (Vector3.one, Vector3.one * scaleAmount, l);
//
			if (timer >= scaleDuration)
				scaleLerp = false;
		}

	}
	#region selection
	public void OnPointerEnter () {

		if (overable == false)
			return;

		pointerOver = true;
		scaleLerp = true;
		timer = 0f;

		CardManager.Instance.ShowOvering (member);
	}

	public void OnPointerExit () {
		
		pointerOver = false;
		scaleLerp = true;
		timer = 0f;

		CardManager.Instance.HideOvering ();
	}
	public void OnPointerDown() {

		if (choosingMember) {

			CombatManager.Instance.SetPlayerMember (Member);

		} else {

			DialogueManager.Instance.SetDialogue ("Oui ?", GetTransform);
		
		}

		OnPointerExit ();
	}
	#endregion

	#region movement
	Crews.PlacingType currentPlacingType;
	public void MoveToPoint ( Crews.PlacingType placingType , float duration = 0.5f ) {

		currentPlacingType = placingType;

		float decal = 0f;
		if (placingType == Crews.PlacingType.Combat || placingType == Crews.PlacingType.Map)
			decal = member.GetIndex;

		targetPos = Crews.getCrew(member.Side).CrewAnchors [(int)placingType].position + Crews.playerCrew.CrewAnchors [(int)placingType].up * decal;

		MoveStart ();
	}

	private void MoveStart () {
		overable = false;
		moveLerp = true;

		initPos = GetTransform.position;

		timer = 0f;
	}
	private void MoveUpdate () {

		timer += Time.deltaTime;

		float l = timer / moveDuration;

		GetTransform.position = Vector3.Lerp ( initPos , targetPos , l );

		if (timer > moveDuration)
			MoveExit ();
	}

	public void MoveExit () {
		moveLerp = false;
		overable = true;

		if (currentPlacingType == Crews.PlacingType.Discussion) {
			ShowBody ();
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
	#endregion


}
