﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Holoville.HOTween;

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
	private GameObject energyGroup;
	[SerializeField]
	private GameObject[] energyPoints;

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

	public Image jobImage;

	float maxWidth = 0f;

//	void Awake () {
	void Start() {

		linkedFighter.onInit += HandleOnInit;
		linkedFighter.onSelect += HandleOnSelect;
		linkedFighter.onSetTurn += HandleOnSetTurn;
		linkedFighter.onEndTurn += HandleOnEndTurn;

		linkedFighter.onShowInfo += HandleOnShowInfo;
		linkedFighter.onGetHit += HandleOnGetHit;

		linkedFighter.onChangeState += HandleOnChangeState;


		if (linkedFighter.crewMember != null)
			UpdateMember ();

		LootUI.useInventory+= HandleUseInventory;

		maxWidth = heartImage.rectTransform.rect.width;

	}

	void HandleOnChangeState (Fighter.states currState, Fighter.states prevState)
	{
		if (currState == Fighter.states.triggerSkill) {
			UpdateEnergyBar (linkedFighter.crewMember);

			Tween.Bounce (energyGroup.transform);
		}
	}


	void HandleOnEndTurn ()
	{
		Tween.Scale (transform, 0.2f, 1f);
	}

	void HandleOnInit () {

		UpdateMember (linkedFighter.crewMember);
	}

	void HandleOnGetHit ()
	{
		UpdateMember ();
	}

	void HandleOnShowInfo ()
	{
		if (previouslySelectedCard == this) {
			previouslySelectedCard = null;
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
		UpdateMember (linkedFighter.crewMember);
	}

	public virtual void UpdateMember ( CrewMember member ) {

		nameText.text = member.MemberName;

		levelText.text = member.Level.ToString ();
		if (member.Level == member.maxLevel)
			levelText.text = "MAX";

		maxWidth = heartBackground.sizeDelta.x;

		float health_Width = maxWidth * (float)member.Health / (float)member.MemberID.maxHealth;
		heartImage.rectTransform.sizeDelta = new Vector2 ( health_Width , heartImage.rectTransform.sizeDelta.y);

		attackText.text = member.Attack.ToString ();
		defenceText.text = member.Defense.ToString ();

		if (SkillManager.jobSprites.Length <= (int)member.job)
			print ("skill l : " + SkillManager.jobSprites.Length + " / member job " + (int)member.job);
		jobImage.sprite = SkillManager.jobSprites[(int)member.job];

		UpdateEnergyBar (member);

		Tween.Bounce (transform);

	}
	public Color energyColor_Full;
	public Color energyColor_Empty;
	int currentEnergy = 0;
	void UpdateEnergyBar(CrewMember member) {

		float scaleAmount = 0.8f;

		float dur = 0.5f;

		int a = 0;

		foreach (var item in energyPoints) {
			
			if (a < member.energy) {
				
				item.transform.localScale = Vector3.one * scaleAmount;

				HOTween.To ( item.transform , dur , "localScale" , Vector3.one );
				HOTween.To ( item.GetComponent<Image>() , dur , "color" , energyColor_Full);

//				item.SetActive (true);
			} else {

				HOTween.To ( item.transform , dur , "localScale" , Vector3.one * scaleAmount);
				HOTween.To ( item.GetComponent<Image>() , dur , "color" , energyColor_Empty);

//				item.SetActive (false);
			}
			++a;
		}

//		currentEnergy = member.energy;
	}

	public void ShowStats () {
		statGroup.SetActive (true);
	}

	public void HideStats () {
		statGroup.SetActive (false);
	}

	public void ShowCard () {
		cardObject.SetActive (true);

	}
	public void HideCard () {
		cardObject.SetActive (false);
	}


}
