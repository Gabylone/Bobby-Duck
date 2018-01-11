using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton_Inventory : SkillButton {

	public GameObject padlockObj;
	public Text padlockText;

	public override void Start ()
	{
		base.Start ();

		onUnlockSkill += HandleOnUnlockSkill;
	}

	void HandleOnUnlockSkill ()
	{
		if (skill != null)
			SetSkill (skill);
	}

	public delegate void OnUnlockSkill ();
	public static OnUnlockSkill onUnlockSkill;
	public void OnPointerDown () {

		Tween.Bounce (skillImage.transform);

		if ( CrewMember.selectedMember.SkillPoints >= GetSkillCost() ) {

			CrewMember.selectedMember.SkillPoints -= GetSkillCost ();

			CrewMember.selectedMember.MemberID.specialSkillsIndexes.Add ( SkillManager.getSkillIndex(skill) );

			if (onUnlockSkill != null)
				onUnlockSkill ();

			Unlock ();

		} else {
			padlockObj.GetComponent<Animator> ().SetTrigger ("giggle");
			Tween.Bounce (padlockObj.transform);
		}

	}

	void Unlock () {

		button.interactable = false;

		Tween.Bounce ( padlockObj.transform );
		Invoke ("HidePadlock", Tween.defaultDuration);

	}

	void HidePadlock () {
		padlockObj.SetActive (false);
	}

	void Lock ()
	{
		button.interactable = true;

		padlockObj.SetActive (true);

//		image.color = Color.gray;

		padlockText.text = "" + GetSkillCost ();
	}

	public override void SetSkill (Skill _skill)
	{
		base.SetSkill (_skill);

		if (CrewMember.selectedMember.specialSkills.Find (x => x.type == _skill.type) == null) {

			Lock ();

		} else {
			
			Unlock ();

		}
	}

	int GetSkillCost ()
	{
		return CrewMember.selectedMember.specialSkills.Count;
	}
}
