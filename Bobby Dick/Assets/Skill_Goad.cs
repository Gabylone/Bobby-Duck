using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Goad : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.TargetFighter.AddStatus (Fighter.Status.Provoking,3);

		EndSkill ();

	}
}
