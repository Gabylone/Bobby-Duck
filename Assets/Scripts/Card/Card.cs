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
	private Text lvl_Text;

	[SerializeField]
	private Text name_Text;

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

	void Start () {
		Init ();
	}

	public void Init () {
		_transform = cardObject.GetComponent<Transform>();
		HideCard ();
	}

	public void UpdateMember ( CrewMember member ) {

		ResetCard ();

		cardObject.SetActive (true);

		name_Text.text = member.MemberName;
		lvl_Text.text = member.Level.ToString ();

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

	}

	public void PlaceCard (Vector3 pos) {

		pos.x = Mathf.Clamp ( pos.x , cardBoundsX.x, cardBoundsX.y );
		pos.y = Mathf.Clamp ( pos.y , cardBoundsY.x, cardBoundsY.y );

		GetTransform.position = pos;

		Vector3 dir = GetTransform.position - Vector3.zero;
		GetTransform.right = -dir;

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
