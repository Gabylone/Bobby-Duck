using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RhumRound : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		foreach (var targetFighter in CombatManager.Instance.getCurrentFighters (fighter.crewMember.side) ) {

			targetFighter.Heal (25);

		}

		EndSkill ();

	}
}
