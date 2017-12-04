using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Cosh : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack);

		fighter.TargetFighter.AddStatus (Fighter.Status.KnockedOut);

		EndSkill ();

	}
}
