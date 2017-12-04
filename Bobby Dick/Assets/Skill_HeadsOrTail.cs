using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HeadsOrTail : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		if (Random.value > 0.5f) {
			fighter.combatFeedback.Display ("FACE !");
			fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack * 2.5f);
		} else {
			fighter.combatFeedback.Display ("PILE !");
		}

		EndSkill ();

	}
}
