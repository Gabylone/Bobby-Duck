using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PledgeOfFeast : Skill {

	public int energyAmount = 10;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		fighter.TargetFighter.crewMember.AddEnergy (energyAmount);

		EndSkill ();

	}
}
