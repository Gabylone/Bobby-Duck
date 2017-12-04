using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Jag : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.TargetFighter.AddStatus (Fighter.Status.Jagged, 3);
		fighter.TargetFighter.RemoveStatus (Fighter.Status.Poisonned, 3);

		EndSkill ();

	}
}
