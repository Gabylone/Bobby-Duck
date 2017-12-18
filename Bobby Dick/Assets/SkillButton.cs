using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	public delegate void OnTriggerSkill ( Skill skill );
	public static OnTriggerSkill onTriggerSKill;

	public Image skillImage;

	public GameObject energyGroup;

	public Text uiText_SkillName;
	public Text uiText_Description;

	public Text uiText_Energy;

	public Skill skill;

	bool touching = false;

	public float timeToShowDescription = 0.5f;

	void Start () {
		HideDescription ();
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

			if (onTriggerSKill != null)
				onTriggerSKill (skill);

		} else {
			HideDescription ();

		}

		touching = false;


	}

	public GameObject descriptionGroup;

	void ShowDescription ()
	{
		descriptionGroup.SetActive (true);

 		uiText_SkillName.text = skill.name;
		uiText_Description.text = skill.description;

		Tween.Bounce ( descriptionGroup.transform );
		Tween.Bounce ( transform );
	}

	void HideDescription ()
	{
		descriptionGroup.SetActive (false);
	}

	public void SetSkill (Skill _skill)
	{
		skill = _skill;

		skillImage.sprite = SkillManager.skillSprites [(int)skill.type];

		uiText_SkillName.text = _skill.type.ToString ();

		if ( skill.energyCost == 0 )
			energyGroup.SetActive (false);
		else
			uiText_Energy.text = "" + skill.energyCost;

		Fighter fighter = CombatManager.Instance.currentFighter;
		if ( skill.energyCost > fighter.crewMember.energy ) {
			GetComponent<Button> ().interactable = false;
		} else {
			GetComponent<Button> ().interactable = true;
		}
	}

}
