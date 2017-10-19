using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour {

	public static Card previouslySelectedCard;

		// components
	private Transform _transform;

	[Header("UI Elements")]
	[SerializeField]
	private GameObject cardObject;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private RectTransform heartBackground;
	[SerializeField]
	private Image heartImage;

	[SerializeField]
	private GameObject heartGroup;

	[SerializeField]
	private Text defenceText;

	[SerializeField]
	private Text attackText;

	[SerializeField]
	private Text levelText;

	[SerializeField]
	private GameObject statGroup;

	public Fighter linkedFighter;

	float maxWidth = 0f;


	void Start () {

		linkedFighter.onInit += HandleOnInit;
		linkedFighter.onSelect += HandleOnSelect;
		linkedFighter.onSetTurn += HandleOnSetTurn;
		linkedFighter.onEndTurn += HandleOnEndTurn;
		linkedFighter.onShowInfo += HandleOnShowInfo;
		linkedFighter.onGetHit += HandleOnGetHit;

		if (linkedFighter.CrewMember != null)
			UpdateMember ();

		LootUI.useInventory+= HandleUseInventory;

		maxWidth = heartImage.rectTransform.rect.width;

	}

	void HandleOnEndTurn ()
	{
		Tween.Scale (transform, 0.2f, 1f);
	}

	void HandleOnInit () {

		UpdateMember (linkedFighter.CrewMember);
	}

	void HandleOnGetHit ()
	{
		UpdateMember ();
	}

	void HandleOnShowInfo ()
	{
		if (previouslySelectedCard == this) {
			HideInfo ();
			return;
		}

		if (previouslySelectedCard != null)
			previouslySelectedCard.HideInfo ();

		previouslySelectedCard = this;

		statGroup.SetActive (true);

		Tween.Bounce (transform);
	}

	public void HideInfo ()
	{
		statGroup.SetActive (false);
	}

	void HandleOnSelect ()
	{
		UpdateMember ();
	}

	void HandleOnSetTurn ()
	{
		UpdateMember ();

		Tween.Scale (transform, 0.2f, 1.6f);
	}

	void HandleUseInventory (InventoryActionType actionType)
	{
		UpdateMember (CrewMember.selectedMember);
	}

	void UpdateMember() {
		UpdateMember (linkedFighter.CrewMember);
	}

	public virtual void UpdateMember ( CrewMember member ) {

		nameText.text = member.MemberName;

		levelText.text = member.Level.ToString ();
		if (member.Level == member.maxLevel)
			levelText.text = "MAX";

//		heartImage.fillAmount = (float)member.Health / (float)member.MemberID.maxHealth;
		maxWidth = heartBackground.sizeDelta.x;
		float x = maxWidth * (float)member.Health / member.MemberID.maxHealth;
		heartImage.rectTransform.sizeDelta = new Vector2 ( x - maxWidth , heartImage.rectTransform.sizeDelta.y);

		Tween.Bounce (transform);

		attackText.text = member.Attack.ToString ();

		defenceText.text = member.Defense.ToString ();
	}

	public void ShowStats () {
		statGroup.SetActive (true);
	}

	public void HideStats () {
		statGroup.SetActive (false);
	}

	public void ShowCard () {
		//
		cardObject.SetActive (true);

	}
	public void HideCard () {
		cardObject.SetActive (false);
	}


}
