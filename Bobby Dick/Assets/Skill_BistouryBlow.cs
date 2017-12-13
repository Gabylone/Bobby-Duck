﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BistouryBlow : Skill {

	public int healAmount = 35;
	public int healthToHeal = 60;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.TargetFighter.Heal (healAmount);

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool allyInHelp = false;

		foreach (var item in Crews.getCrew(member.side).CrewMembers) {
			if (item.Health < healthToHeal) {
				allyInHelp = true;
			}
		}

		return allyInHelp && base.MeetsConditions (member);
	}
}
