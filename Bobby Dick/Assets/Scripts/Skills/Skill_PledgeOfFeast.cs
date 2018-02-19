using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PledgeOfFeast : Skill {

	public int energyAmount = 10;

	

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		fighter.TargetFighter.crewMember.AddEnergy (energyAmount);
		fighter.TargetFighter.ShowInfo ();

		EndSkill ();

	}
}
