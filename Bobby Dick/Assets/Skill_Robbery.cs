using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Robbery : Skill {

	public int goldStolen = 30;

	public int minimumGoldToSteal = 15;

	public override void Start ()
	{
		base.Start ();
	}

	public override void ApplyEffect ()
	{
		base.ApplyEffect ();

		if ( fighter.crewMember.side == Crews.Side.Enemy ) {

			GoldManager.Instance.GoldAmount -= goldStolen;

		} else {

			GoldManager.Instance.GoldAmount += goldStolen;

			//
		}

		fighter.combatFeedback.Display ("+ " + goldStolen + " or", Color.yellow);
		fighter.TargetFighter.combatFeedback.Display ("- " + goldStolen + " or", Color.red);

		EndSkill ();

	}

	public override bool MeetsConditions (CrewMember member)
	{

		bool hasMinimumGold = false;

		if (GoldManager.Instance.GoldAmount > minimumGoldToSteal)
			hasMinimumGold = true;

		return hasMinimumGold && base.MeetsConditions (member);
	}
}
