using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BistouryBlow : Skill {

	public int healAmount = 35;
	public int healthToHeal = 60;

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		fighter.TargetFighter.Heal (healAmount);

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{

		Fighter weakestFighter = CombatManager.Instance.currEnemyFighters [0];
		foreach (var item in CombatManager.Instance.currEnemyFighters) {
			if ( item.crewMember.Health < weakestFighter.crewMember.Health ) {
				weakestFighter = item;
			}
		}

		bool allyInHelp = false;

		if ( weakestFighter.crewMember.Health < healthToHeal ) {
			preferedTarget = weakestFighter;
			allyInHelp = true;
		}

		return allyInHelp && base.MeetsConditions (member);
	}
}
