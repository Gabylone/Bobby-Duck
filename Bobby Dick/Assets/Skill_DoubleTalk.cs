using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DoubleTalk : Skill {

	public int healthToFlee = 60;

	public override void Start ()
	{
		base.Start ();
	}

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		DiceManager.Instance.onEndThrow += HandleOnEndThrow;

		DiceManager.Instance.ThrowDice (DiceTypes.DEX, fighter.crewMember.GetStat(Stat.Dexterity));

	}

	void HandleOnEndThrow ()
	{
		if ( DiceManager.Instance.HighestResult == 6 ) {

			fighter.combatFeedback.Display("Sucess!", Color.green);

			foreach ( var fighterItem in CombatManager.Instance.getCurrentFighters(fighter.crewMember.side) ) {
				fighterItem.Fade ();
				CombatManager.Instance.DeleteFighter (fighterItem);
			}


		} else {

			fighter.combatFeedback.Display("Fail!",Color.red);

		}

		DiceManager.Instance.onEndThrow -= HandleOnEndThrow;

		EndSkill ();
	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool allyInHelp = false;

		int count = 0;

		foreach (var item in Crews.getCrew(member.side).CrewMembers) {
			if (item.Health < healthToFlee) {
				++count;
				if ( count > 1 )
					allyInHelp = true;
			}
		}

		return allyInHelp && base.MeetsConditions (member);
	}
}
