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
	private Text heartText;
	[SerializeField]
	private Image heartImage;
	[SerializeField]
	private Text attackText;
	[SerializeField]
	private Text defenseText;

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

	public virtual void Init () {
		
		_transform = cardObject.GetComponent<Transform>();

		HideCard ();
	}

	public virtual void UpdateMember ( CrewMember member ) {

		ResetCard ();

		cardObject.SetActive (true);

			// general info
		name_Text.text = member.MemberName;

			// INFO
		lvl_Text.text = member.Level.ToString ();

		lvl_Image.fillAmount = ((float)member.Xp / (float)member.StepToNextLevel);

		// STATS & Equipment

		heartImage.fillAmount = (float)member.Health / (float)member.MaxHealth;
		heartText.text = member.Health.ToString ();

		attackText.text = member.Attack.ToString ();
		defenseText.text = member.Defense.ToString ();

			// STATES
		float[] values = new float[2] { member.CurrentHunger, member.CurrentCold };
//
//		stateFeedbacks [0].fillAmount = values[0] / member.MaxState;
//		stateFeedbacks [0].color = values[0] >= member.MaxState ? Color.red : Color.white;
//		stateWarnings[0].enabled = values[0] >= member.MaxState;

//		stateAnimators[0].SetBool ("Warning",values[0] >= member.MaxState);
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
