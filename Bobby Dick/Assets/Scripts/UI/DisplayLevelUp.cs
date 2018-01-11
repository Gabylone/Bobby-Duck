using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLevelUp : MonoBehaviour {

	[SerializeField]
	private GameObject group;

	[SerializeField]
	private Text statText;

	// Use this for initialization
	void Start () {

		group.SetActive (false);

		GetComponentInParent<MemberIcon> ().member.onLevelUp += HandleOnLevelUp;
		GetComponentInParent<MemberIcon> ().member.onLevelUpStat += HandleOnLevelUpStat;
		SkillButton_Inventory.onUnlockSkill += HandleOnUnlockSkill;


	}

	void HandleOnUnlockSkill ()
	{
		if (GetComponentInParent<MemberIcon> ().member == CrewMember.selectedMember) {
			UpdateStatText (CrewMember.selectedMember);
		}
	}

	void HandleOnLevelUp (CrewMember member)
	{
		Show ();

		statText.text = member.SkillPoints.ToString();
	}

	void HandleOnLevelUpStat (CrewMember member)
	{
		UpdateStatText (member);
	}

	void UpdateStatText (CrewMember member)
	{
		statText.text = member.SkillPoints.ToString();

		if (member.SkillPoints == 0) {
			Hide ();
		}
	}

	void Show () {
		group.SetActive (true);
	}

	void Hide () {
		group.SetActive (false);
	}
}
