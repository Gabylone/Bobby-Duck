using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	public delegate void OnTriggerSkill ( Skill skill );
	public static OnTriggerSkill onTriggerSKill;

	public Image skillImage;

	public GameObject energyGroup;
	public GameObject descriptionGroup;

	public Text uiText_SkillName;
	public Text uiText_Description;

	public Text uiText_Energy;

	public Skill skill;

	public Image image;
	public Button button;

	bool touching = false;

	public float timeToShowDescription = 0.5f;

	public virtual void Start () {
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

	#region description
	public void ShowDescription ()
	{
		descriptionGroup.SetActive (true);

 		uiText_SkillName.text = skill.name;
		uiText_Description.text = skill.description;

		Tween.Bounce ( descriptionGroup.transform );
	}

	public void HideDescription ()
	{
		descriptionGroup.SetActive (false);
	}
	#endregion

	public virtual void SetSkill (Skill _skill)
	{
		skill = _skill;

		skillImage.sprite = SkillManager.skillSprites [(int)skill.type];

		uiText_SkillName.text = _skill.type.ToString ();

		UpdateEnergyCost ();
	}

	void UpdateEnergyCost ()
	{
		if (energyGroup == null)
			return;

		if (skill.energyCost == 0) {
			energyGroup.SetActive (false);
		} else {
			energyGroup.SetActive (true);
			uiText_Energy.text = "" + skill.energyCost;
		}

		Fighter fighter = CombatManager.Instance.currentFighter;

		if ( skill.energyCost > fighter.crewMember.energy || skill.MeetsRestrictions(fighter.crewMember) == false ) {
			GetComponent<Image> ().raycastTarget = false;
		} else {

			GetComponent<Image> ().raycastTarget = true;

		}
	}

	public void Show () {
		gameObject.SetActive (true);
		Tween.Bounce (transform);
	}

	public void Hide () 
	{
		gameObject.SetActive (false);
	}

}
