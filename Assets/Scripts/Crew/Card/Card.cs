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

	[Header("Equipment")]
	[SerializeField]
	private Image weaponImage;
	[SerializeField]
	private Image clothImage;

	[Header("Dice")]
	[SerializeField]
	private Image[] heart;
	[SerializeField]
	private Image[] attackDice;
	[SerializeField]
	private Image[] speedDice;
	[SerializeField]
	private Image[] constDice;

	[Header("Card Bounds")]
	[SerializeField]
	private Vector2 cardBoundsX = new Vector2();
	[SerializeField]
	private Vector2 cardBoundsY = new Vector2();

	void Start () {
		
		_transform = cardObject.GetComponent<Transform>();

		HideCard ();
	}

	void Update () {
		
	}

	public void UpdateMember ( CrewMember member ) {

		ResetCard ();

		cardObject.SetActive (true);

		name_Text.text = member.MemberName;
		lvl_Text.text = member.Level.ToString ();

		Image[][] images = new Image[4][] {
			heart,
			attackDice,
			speedDice,
			constDice,
		};

		// icon
		memberIcon = Instantiate (member.IconObj) as GameObject;
		memberIcon.transform.SetParent (GetTransform);
		memberIcon.transform.position = iconAnchor.position;
		memberIcon.transform.localScale = Vector3.one * 0.7f;
		memberIcon.GetComponent<CrewIcon> ().HideBody ();

		memberIcon.GetComponent<CrewIcon> ().enabled = false;

		// 
		foreach (Image image in memberIcon.GetComponentsInChildren<Image>())
			image.raycastTarget = false;

		//
		int a = 0;
		foreach ( Image[] dice in images ) {

			int d = 0;
			foreach (Image die in dice) {
				die.enabled = d < member.getDiceValues[a];

				++d;
			}

			d = 0;
			++a;
		}

	}

	public void PlaceCard (Vector3 pos) {

		pos.x = Mathf.Clamp ( pos.x , cardBoundsX.x, cardBoundsX.y );
		pos.y = Mathf.Clamp ( pos.y , cardBoundsY.x, cardBoundsY.y );

		GetTransform.position = pos;

	}

	public void HideCard () {
		ResetCard ();
		cardObject.SetActive (false);
	}

	private void ResetCard () {
		name_Text.text = "";
		lvl_Text.text = "";

		weaponImage.sprite = null;
		clothImage.sprite = null;

		Image[][] images = new Image[4][] {
			heart,
			attackDice,
			speedDice,
			constDice,
		};

		if (memberIcon != null) { 
			Destroy (memberIcon);
		}

		foreach ( Image[] dice in images ) {
			foreach ( Image die in dice )
				die.enabled = false;
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
}
