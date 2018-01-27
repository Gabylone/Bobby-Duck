using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Flee : Skill {

	public int healthToFlee = 30;

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		DiceManager.Instance.onEndThrow += HandleOnEndThrow;

		DiceManager.Instance.ThrowDice (DiceTypes.DEX, fighter.crewMember.GetStat(Stat.Dexterity));

	}

	void HandleOnEndThrow ()
	{

		if ( DiceManager.Instance.HighestResult == 6 ) {

			fighter.Fade ();
			fighter.combatFeedback.Display("Sucess!", Color.green);

			CombatManager.Instance.DeleteFighter (fighter);
			CombatManager.Instance.StartNewTurn ();


		} else if ( DiceManager.Instance.HighestResult == 1 ) {


			fighter.combatFeedback.Display("Crit\nFail!", Color.magenta);
			fighter.AddStatus (Fighter.Status.KnockedOut);

			fighter.crewMember.energy = 0;

			EndSkill ();


		} else {

			fighter.combatFeedback.Display("Fail!",Color.red);

			EndSkill ();

		}

		DiceManager.Instance.onEndThrow -= HandleOnEndThrow;

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool fewHealth = member.Health < healthToFlee;

		bool meetsChance = Random.value < 0.65f;

		return meetsChance && fewHealth && base.MeetsConditions (member);
	}
}
