using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Dynamite : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		foreach (var targetFighter in CombatManager.Instance.getCurrentFighters (Crews.otherSide (fighter.crewMember.side))) {

			targetFighter.GetHit (fighter, fighter.crewMember.Attack / 2f);

		}

		EndSkill ();

	}

}
