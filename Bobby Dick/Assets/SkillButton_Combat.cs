using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton_Combat : SkillButton {


	public GameObject energyGroup;
	public Text uiText_Energy;

	// charge
	public Image chargeFillImage;
	public GameObject chargeGroup;
	public Text chargeText;

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
		chargeGroup.SetActive (false);
		skillImage.color = Color.black;
		GetComponent<Button>().interactable = true;
	}

	void Disable() {
		GetComponent<Button> ().interactable = false;
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
		if (skill.currentCharge > 0) {
			Disable ();
			chargeGroup.SetActive (true);
			chargeFillImage.fillAmount = (float)skill.currentCharge / (float)skill.initCharge;
			chargeText.text = "" + skill.currentCharge;
		}


	}

	public void OnPointerDown () {

		touching = true;

		Invoke ("TriggerSkillDelay" , timeToShowDescription);	

	}

	void TriggerSkillDelay () {

		if ( touching )
			ShowDescription ();

		touching = false;

	}

	public void OnPointerUp () {

		if (touching) {

			skill.Trigger (CombatManager.Instance.currentFighter);

		} else {
			HideDescription ();

		}

		touching = false;


	}
}
