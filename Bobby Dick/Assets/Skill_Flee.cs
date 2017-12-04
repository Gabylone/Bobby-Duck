using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Flee : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		DiceManager.Instance.onEndThrow += HandleOnEndThrow;

		DiceManager.Instance.ThrowDice (DiceTypes.DEX, fighter.crewMember.GetStat(Stat.Dexterity));

	}

	void HandleOnEndThrow ()
	{

		if ( DiceManager.Instance.HighestResult == 6 ) {

			fighter.Fade ();
			fighter.combatFeedback.Display("Sucess!", Color.green);

			CombatManager.Instance.DeleteFighter (fighter);

		} else if ( DiceManager.Instance.HighestResult == 1 ) {

			fighter.combatFeedback.Display("Crit\nFail!", Color.magenta);
			fighter.AddStatus (Fighter.Status.KnockedOut);

		} else {

			fighter.combatFeedback.Display("Fail!",Color.red);

		}

		DiceManager.Instance.onEndThrow -= HandleOnEndThrow;

		EndSkill ();
	}
}
