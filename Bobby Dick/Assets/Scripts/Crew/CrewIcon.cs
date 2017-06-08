using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewIcon : MonoBehaviour {

	private int id = 0;

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

	[Header("States")]
	[SerializeField]
	private Image hungerImage;
	[SerializeField]
	private GameObject hungerObject;

	[Header("Lerping")]
	[SerializeField]
	private float moveDuration = 1f;

	float timer = 0f;

	bool scaleLerp = false;
	bool moveLerp = false;

	Vector3 initPos = Vector3.zero;
	Vector3 targetPos = Vector3.zero;

	[Header("decals")]
	[SerializeField]
	private float overingDecal = 2f;
	private float placementDecal = 1.3f;

	Crews.PlacingType currentPlacingType;
	Crews.PlacingType previousPlacingType;

	public Transform dialogueAnchor;

	void Awake () {
		_transform = transform;
	}

	void Start () {
		UpdateIcon ();
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

	public void UpdateIcon () {

		if (currentPlacingType == Crews.PlacingType.Map) {
			float f = ((float)member.CurrentHunger / (float)member.MaxState);
			hungerImage.fillAmount = f;

			hungerObject.SetActive (f > 0.65f);
		} else {
			hungerObject.SetActive (false);
		}
	}

	#region selection
	public void OnPointerEnter() {

		if (!Overable)
			return;

		pointerOver = true;
		scaleLerp = true;

		hungerObject.SetActive (true);

		CardManager.Instance.ShowOvering (member);
	}

	public void OnPointerExit () {

		if (!Overable)
			return;

		pointerOver = false;
		scaleLerp = true;

		hungerObject.SetActive (false);

		UpdateIcon ();

		CardManager.Instance.HideOvering ();
	}
	public void OnPointerDown() {
		if (!Overable)
			return;
		OnPointerExit ();
		DialogueManager.Instance.SetDialogue ("Oui ?", member);
	}
	#endregion

	#region movement
	public void MoveToPoint ( Vector3 pos , float duration = 0.5f ) {
		
		targetPos = pos;

		MoveStart ();
	}

	public void MoveToPoint ( Crews.PlacingType placingType , float duration = 0.5f ) {

		previousPlacingType = currentPlacingType;
		currentPlacingType = placingType;

		float decal = 0f;

		if (placingType == Crews.PlacingType.Combat || placingType == Crews.PlacingType.Map) {
			decal = member.GetIndex * placementDecal;
		}

		moveDuration = duration;

		targetPos = Crews.getCrew(member.Side).CrewAnchors [(int)placingType].position + Crews.playerCrew.CrewAnchors [(int)placingType].up * decal;

		MoveStart ();
	}

	private void MoveStart () {
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
		Overable = true;

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

	public bool Overable {
		get {
			return overable;
		}
		set {
			overable = value;

			UpdateIcon ();

			if ( value == false )
				hungerObject.SetActive (false);

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

	#region body parts
	[Header("BobyParts")]
	[SerializeField]
	private Image faceImage;
	[SerializeField]
	private Image beardImage;
	[SerializeField]
	private Image bodyImage;
	[SerializeField]
	private Image hairImage;
	[SerializeField]
	private Image eyesImage;
	[SerializeField]
	private Image eyebrowsImage;
	[SerializeField]
	private Image mouthImage;
	[SerializeField]
	private Image noseImage;

	public Image FaceImage {
		get {
			return faceImage;
		}
	}

	public Image BeardImage {
		get {
			return beardImage;
		}
	}

	public Image HairImage {
		get {
			return hairImage;
		}
	}

	public Image EyesImage {
		get {
			return eyesImage;
		}
	}

	public Image EyebrowsImage {
		get {
			return eyebrowsImage;
		}
	}

	public Image MouthImage {
		get {
			return mouthImage;
		}
	}

	public Image NoseImage {
		get {
			return noseImage;
		}
	}

	public Image BodyImage {
		get {
			return bodyImage;
		}
	}

	#endregion

	public GameObject ControllerTransform {
		get {
			return controllerTransform;
		}
	}
}