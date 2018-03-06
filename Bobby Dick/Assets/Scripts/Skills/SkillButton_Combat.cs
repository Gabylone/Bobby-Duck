using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class SkillButton_Combat : SkillButton {


	public GameObject energyGroup;
	public Text uiText_Energy;

	// charge
	public Image chargeFillImage;
	public GameObject chargeGroup;
	public Text chargeText;

	public float timeToShowDescriptionFeedback = 0.2f;

	bool canTriggerSkill = true;

	bool touching = false;

	public override void Start ()
	{
		base.Start ();
	}

	public override void SetSkill (Skill _skill)
	{
		base.SetSkill (_skill);

		CheckSkill ();
	}

	void Enable () {
		canTriggerSkill = true;
		skillImage.color = Color.black;
	}

	void Disable() {
		canTriggerSkill = false;
		skillImage.color = new Color ( 1,1,1,0.35f );
	}

	void CheckSkill ()
	{
		if (skill.energyCost == 0) {
			energyGroup.SetActive (false);
		} else {
			energyGroup.SetActive (true);
			uiText_Energy.text = "" + skill.energyCost;
		}

		Fighter fighter = CombatManager.Instance.currentFighter;

		Enable ();

		// ENERGY
		if (skill.MeetsRestrictions (fighter.crewMember) == false) {
			Disable ();
		}

		if ( skill.energyCost > fighter.crewMember.energy ) {
			Disable ();
		}

		// CHARGE
		int charge = fighter.crewMember.charges[skill.GetSkillIndex(fighter.crewMember)];
		UpdateCharge (charge);

	}

	void UpdateCharge (int charge) {
		if (charge > 0) {
			Disable ();
			chargeGroup.SetActive (true);
			chargeFillImage.fillAmount = (float)charge / (float)skill.initCharge;
			chargeText.text = "" + charge;
		} else {
			chargeGroup.SetActive (false);
		}
	}

	public void OnPointerDown () {

		touching = true;

		CancelInvoke("TriggerSkillDelay");
		CancelInvoke ("SkillDelayFeedback");
		Invoke ("TriggerSkillDelay" , timeToShowDescription);	
		Invoke ("SkillDelayFeedback", timeToShowDescriptionFeedback);

	}

	void SkillDelayFeedback() {

		if (!touching)
			return;
		
		chargeGroup.SetActive (true);
		chargeFillImage.fillAmount = 0f;
		chargeText.text = "";

		HOTween.Kill (chargeFillImage);
		HOTween.To ( chargeFillImage , timeToShowDescription - timeToShowDescriptionFeedback, "fillAmount", 1f );

	}

	void TriggerSkillDelay () {

		if ( touching )
			ShowDescription ();

		touching = false;

	}

	public void OnPointerUp () {

		if (canTriggerSkill && touching) {
			skill.Trigger (CombatManager.Instance.currentFighter);
		}

		HideDescription ();
		HOTween.Kill (chargeFillImage);
		CrewMember member = CombatManager.Instance.currentFighter.crewMember;
		int charge = member.charges[skill.GetSkillIndex(member)];
		UpdateCharge (charge);

		touching = false;

	}
}
