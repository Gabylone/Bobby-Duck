using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySkillButtons : MonoBehaviour {

	SkillButton_Inventory[] skillButtons;

	public GameObject statGroup;

	// Use this for initialization
	void Start () {
		
		skillButtons = GetComponentsInChildren<SkillButton_Inventory> (true);

		SkillMenu.onShowSkillMenu += HandleOnShowCharacterStats;
		HandleOnShowCharacterStats ();
	}

	void HandleOnShowCharacterStats ()
	{
		if (CrewMember.GetSelectedMember == null)
			return;

        //StartCoroutine (ShowSkillButtons());
        ShowSkillButtons();
	}

	public float timeBetweenButtons = 0.5f;

    void ShowSkillButtons() {
    //IEnumerator ShowSkillButtonsCoroutine () {

		HideButtons ();

		List<Skill> skills = SkillManager.getJobSkills (CrewMember.GetSelectedMember.job);

		int a = 0;

		foreach (var item in skillButtons) {

			item.Show ();

			item.HideDescription ();

			item.SetSkill (skills [a]);

			item.Invoke ("ShowDescription", timeBetweenButtons / 1.5f );

			++a;

			//yield return new WaitForSeconds (timeBetweenButtons);

		}

		statGroup.SetActive (true);
		Tween.Bounce (statGroup.transform);

		//yield return new WaitForSeconds (timeBetweenButtons);

	}

	void HideButtons ()
	{
		foreach (var item in skillButtons) {
			item.gameObject.SetActive (false);
		}

		statGroup.SetActive (false);
	}

	void UpdateSkillButtons () {

		Skill[] skills;

		for (int skillButtonIndex = 0; skillButtonIndex < skillButtons.Length; skillButtonIndex++) {

			skillButtons [0].SetSkill ( SkillManager.getJobSkills ( CrewMember.GetSelectedMember.job)[skillButtonIndex] );


		}

	}

}
