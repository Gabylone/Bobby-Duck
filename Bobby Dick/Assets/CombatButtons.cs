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

			defaultGroup.SetActive (true);

			UpdateDefaultButtons ();
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
		defaultGroup.SetActive (true);
		skillGroup.SetActive (false);

		Tween.Bounce (defaultGroup.transform);

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

		foreach (var item in defaultSkillButtons) {
			item.SetSkill (item.skill);
			Tween.Bounce (item.transform);
		}

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
