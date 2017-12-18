using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Leap: Skill {

	bool onDelay = false;

	public int healthToAttack = 30;

	public override void Start ()
	{
		base.Start ();
	}

	public override void InvokeSkill ()
	{
		if (onDelay) {
			print ("lui donne de l'energie");
			fighter.crewMember.energy += energyCost;
		}
		base.InvokeSkill ();
	}

	public override void ApplyEffect ()
	{

		base.ApplyEffect ();

		if (onDelay) {

			onDelay = false;
			hasTarget = false;
			goToTarget = false;

			fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack * 6f);

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
		onDelay = true;
		hasTarget = true;
		goToTarget = true;

		Trigger (delayFighter);

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool allyHealthIsCritical = true;

		foreach (var item in Crews.getCrew(Crews.otherSide(member.side)).CrewMembers) {
			if (item.Health > healthToAttack) {
				allyHealthIsCritical = false;
			}
		}

		return allyHealthIsCritical == false && base.MeetsConditions (member);
	}
}
