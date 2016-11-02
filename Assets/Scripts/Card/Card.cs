using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {

		// components
	private Transform _transform;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject cardObject;

	[SerializeField]
	private Text name_Text;

	[Header("Level")]
	[SerializeField]
	private Text lvl_Text;
	[SerializeField]
	private Image lvl_Image;

	bool displaying = false;

	[SerializeField]
	private Transform iconAnchor;

	private GameObject memberIcon;

	[Header("Dice")]
	[SerializeField]
	private Image heart;
	[SerializeField]
	private Image attackDice;
	[SerializeField]
	private Image speedDice;
	[SerializeField]
	private Image constDice;

	[Header("Card Bounds")]
	[SerializeField]
	private Vector2 cardBoundsX = new Vector2();
	[SerializeField]
	private Vector2 cardBoundsY = new Vector2();
	[SerializeField]
	private bool centerCard = false;

	// states => hunger ; cold ( array way )
	[Header("States")]
	[SerializeField]
	private Image[] stateFeedbacks;
	[SerializeField]
	private Image[] stateWarnings;
	[SerializeField]
	private Animator[] stateAnimators;

	void Awake () {
		Init ();
	}

	public void Init () {
		_transform = cardObject.GetComponent<Transform>();
		HideCard ();
	}

	public void UpdateMember ( CrewMember member ) {

		ResetCard ();

		cardObject.SetActive (true);

			// general info
		name_Text.text = member.MemberName;

			// INFO
		lvl_Text.text = member.Level.ToString ();

		lvl_Image.fillAmount = ((float)member.Xp / (float)member.StepToNextLevel);

		// STATS & Equipment
		Image[] images = new Image[4]{
			heart,
			attackDice,
			speedDice,
			constDice,
		};

		int a = 0;
		foreach ( Image dice in images ) {
			dice.GetComponentInChildren<Text>().text = member.getDiceValues[a].ToString ();
			++a;
		}

		images [0].fillAmount = (float)member.Health / (float)member.MaxHealth;

			// STATES
		float[] values = new float[2] { member.CurrentHunger, member.CurrentCold };

		for (int i = 0; i < stateFeedbacks.Length ; ++i ) {
			stateFeedbacks [i].fillAmount = values[i] / member.MaxState;
			stateWarnings[i].enabled = values[i] >= member.MaxState;
			stateAnimators[i].SetBool ("Warning",values[i] >= member.MaxState);
		}
	}

	public void PlaceCard (Vector3 pos) {

		pos.x = Mathf.Clamp ( pos.x , cardBoundsX.x, cardBoundsX.y );
		pos.y = Mathf.Clamp ( pos.y , cardBoundsY.x, cardBoundsY.y );

		GetTransform.position = pos;

		if (centerCard) {
			Vector3 dir = GetTransform.position - Vector3.zero;
			GetTransform.right = -dir;
		}

	}

	public void HideCard () {
		ResetCard ();
		cardObject.SetActive (false);
	}

	private void ResetCard () {
		name_Text.text = "";
		lvl_Text.text = "";

		Image[] images = new Image[4] {
			heart,
			attackDice,
			speedDice,
			constDice,
		};

		if (memberIcon != null) { 
			Destroy (memberIcon);
		}
	}

	public void EndDisplay () {
		//
	}

	public Transform GetTransform {
		get {
			return _transform;
		}
		set {
			_transform = value;
		}
	}

	public Transform IconAnchor {
		get {
			return iconAnchor;
		}
	}
}
