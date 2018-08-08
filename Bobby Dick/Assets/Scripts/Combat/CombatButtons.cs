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

        CombatManager.Instance.onFightEnd += Hide;

		defaultSkillButtons = defaultGroup.GetComponentsInChildren<SkillButton> (true);
		skillButtons = skillGroup.GetComponentsInChildren<SkillButton> (true);

        Hide();

	}

	void HandleOnChangeState (CombatManager.States currState, CombatManager.States prevState)
	{
        Hide();

        if (CombatManager.Instance.fighting == false)
            return;

		if ( currState == CombatManager.States.PlayerActionChoice ) {

			OpenDefaultButtons ();
		}

	}

    void Hide()
    {
        defaultGroup.SetActive(false);
        skillGroup.SetActive(false);
    }

	public void OpenSkills () {

        defaultGroup.SetActive(false);
        skillGroup.SetActive(true);

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

			if (skillIndex < member.SpecialSkills.Count) {

				item.gameObject.SetActive (true);

				item.SetSkill (member.SpecialSkills[skillIndex]);

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

		int a = 0;
		foreach (var item in defaultSkillButtons) {
			
			item.SetSkill (member.DefaultSkills[a]);
			Tween.Bounce (item.transform);

			++a;
		}

		ResetOpenSkillButtons ();

		jobImage.sprite = SkillManager.jobSprites[(int)member.job];
	}

	void ResetOpenSkillButtons ()
	{
		CrewMember member = CombatManager.Instance.currentFighter.crewMember;

		Tween.Bounce (openSkillButton.transform);

		openSkillButton.interactable = false;
		foreach (var item in member.SpecialSkills ) {
			if ( member.energy >= item.energyCost ) {
				openSkillButton.interactable = true;
				break;
			}
		}
	}
}
