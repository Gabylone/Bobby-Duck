using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BistouryBlow : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.TargetFighter.Heal (35);

		EndSkill ();

	}
}
