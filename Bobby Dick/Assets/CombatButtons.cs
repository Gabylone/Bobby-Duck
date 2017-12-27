using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatButtons : MonoBehaviour {

	SkillButton[] defaultSkillButtons;
	SkillButton[] skillButtons;

	public Button openSkillButton;
	public Image jobImage;

	public GameObject defaultGroup;
	public GameObject skillGroup;


	// Use this for initialization
	void Start () {
		
		CombatManager.Instance.onChangeState += HandleOnChangeState;

		defaultSkillButtons = defaultGroup.GetComponentsInChildren<SkillButton> (true);
		skillButtons = skillGroup.GetComponentsInChildren<SkillButton> (true);

		defaultGroup.SetActive (false);
		skillGroup.SetActive (false);

	}

	void HandleOnChangeState (CombatManager.States currState, CombatManager.States prevState)
	{
		defaultGroup.SetActive (false);
		skillGroup.SetActive (false);

		if ( currState == CombatManager.States.PlayerActionChoice ) {

			OpenDefaultButtons ();
		}

	}

	public void OpenSkills () {
		
		defaultGroup.SetActive (false);
		skillGroup.SetActive (true);

		UpdateSkillButtons ();

		foreach (var item in skillButtons) {
			Tween.Bounce (item.transform);
		}

	}

	public void CloseSkills () {
		skillGroup.SetActive (false);

		OpenDefaultButtons ();

	}

	void OpenDefaultButtons () {
		defaultGroup.SetActive (true);

		UpdateDefaultButtons ();
	}

	void UpdateSkillButtons ()
	{
		CrewMember member = CombatManager.Instance.currentFighter.crewMember;

		int skillIndex = 0;

		foreach (var item in skillButtons) {

			if (skillIndex < member.specialSkills.Count) {

				item.gameObject.SetActive (true);

				item.SetSkill (member.specialSkills[skillIndex]);

			} else {
				item.gameObject.SetActive (false);
			}

			skillIndex++;
		}

	}

	void UpdateDefaultButtons ()
	{
		// check if player has enought energy
		CrewMember member = CombatManager.Instance.currentFighter.crewMember;

		if ( member.GetEquipment(CrewMember.EquipmentPart.Weapon).spriteID == 0 ) {
			// pistolet
			defaultSkillButtons[0].SetSkill( SkillManager.getSkill(Skill.Type.DistanceAttack) );
//			Debug.Log ("il a un pistolet");
		} else {
			// sword
			defaultSkillButtons[0].SetSkill (SkillManager.getSkill(Skill.Type.CloseAttack));
//			Debug.Log ("il a une épée");
		}

		foreach (var item in defaultSkillButtons) {
			item.SetSkill (item.skill);
			Tween.Bounce (item.transform);
		}
		Tween.Bounce (openSkillButton.transform);

		openSkillButton.interactable = false;
		foreach (var item in member.specialSkills ) {
			if ( member.energy >= item.energyCost ) {
				openSkillButton.interactable = true;
				break;
			}
		}

		jobImage.sprite = SkillManager.jobSprites[(int)member.job];
	}
}
