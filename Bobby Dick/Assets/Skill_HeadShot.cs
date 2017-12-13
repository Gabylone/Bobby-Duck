using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HeadShot : Skill {

	bool onDelay = false;

	public int healthToAttack = 30;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		if (onDelay) {

			onDelay = false;
			hasTarget = false;

			fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack * 3f);

			EndSkill ();
			//

		} else {

			fighter.AddStatus (Fighter.Status.PreparingAttack);

			fighter.onSkillDelay += HandleOnSkillDelay;

			EndSkill ();

		}

	}

	void HandleOnSkillDelay (Fighter delayFighter)
	{
		hasTarget = true;
		onDelay = true;


		Trigger (delayFighter);


	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool allyHealthIsCritical = false;

		foreach (var item in Crews.getCrew(Crews.otherSide(member.side)).CrewMembers) {
			if (item.Health > healthToAttack) {
				allyHealthIsCritical = true;
			}
		}

		return allyHealthIsCritical && base.MeetsConditions (member);
	}
}
