using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_GrapeShot : Skill {
	
	public int attackCount = 4;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		StartCoroutine (SkillCoroutine ());

	}

	IEnumerator SkillCoroutine () {

		for (int count = 0; count < attackCount; count++) {

			Fighter targetFighter = CombatManager.Instance.getCurrentFighters (Crews.otherSide (fighter.crewMember.side))
				[Random.Range (0, CombatManager.Instance.getCurrentFighters (Crews.otherSide (fighter.crewMember.side)).Count)];

			targetFighter.GetHit (fighter, fighter.crewMember.Attack * 0.3f);

			TriggerAnimation ();

			yield return new WaitForSeconds ( animationTime );


		}

		yield return new WaitForEndOfFrame ();

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{
		bool moreThanOneMember = CombatManager.Instance.getCurrentFighters (Crews.otherSide (member.side)).Count > 1;

		return moreThanOneMember && base.MeetsConditions (member);
	}
}
