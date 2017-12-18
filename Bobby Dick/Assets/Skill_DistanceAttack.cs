using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DistanceAttack : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		fighter.TargetFighter.GetHit (fighter, fighter.crewMember.Attack);

		EndSkill ();

	}
}
