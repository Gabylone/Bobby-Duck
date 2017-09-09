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

	bool displaying = false;

	[SerializeField]
	private Transform iconAnchor;

	[Header("Dice")]
	public Text attackText;
	public Text defenseText;

	[Header("Card Bounds")]
	[SerializeField]
	private Vector2 cardBoundsX = new Vector2();
	[SerializeField]
	private Vector2 cardBoundsY = new Vector2();
	[SerializeField]
	private bool centerCard = false;

	public virtual void Init () {
		_transform = cardObject.GetComponent<Transform>();

		HideCard ();
	}

	public delegate void OnCardUpdate (CrewMember crewMember);
	public OnCardUpdate onCardUpdate;
	public virtual void UpdateMember ( CrewMember member ) {

		ShowCard ();

			// general info
		name_Text.text = member.MemberName;

		if (onCardUpdate != null) {
			onCardUpdate (member);
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

	void ShowCard () {
		//
		cardObject.SetActive (true);

	}
	public void HideCard () {
		cardObject.SetActive (false);
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
