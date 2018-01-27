using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HeadsOrTail : Skill {

	

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		if (Random.value > 0.5f) {
			fighter.combatFeedback.Display ("FACE :)");
			fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack , 2f);
		} else {
			fighter.combatFeedback.Display ("PILE :(");
		}

		EndSkill ();

	}
}
