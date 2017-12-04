using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Cuss : Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		if ( fighter.TargetFighter.HasStatus(Fighter.Status.Toasted) ) {
			fighter.TargetFighter.RemoveStatus (Fighter.Status.Toasted,3);
			//
		}

		fighter.TargetFighter.AddStatus (Fighter.Status.Cussed,3);

		EndSkill ();

	}
}
