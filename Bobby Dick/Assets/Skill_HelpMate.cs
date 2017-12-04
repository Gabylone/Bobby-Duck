using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HelpMate : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		base.TriggerSkill ();

		fighter.TargetFighter.AddStatus (Fighter.Status.Protected);

		EndSkill ();

	}
}
