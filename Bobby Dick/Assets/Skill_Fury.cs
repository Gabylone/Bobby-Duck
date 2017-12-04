using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fury: Skill {

	public int energyPerTurnAdded = 20;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		fighter.AddStatus (Fighter.Status.Enraged, 3);

		fighter.onRemoveStatus += HandleOnRemoveStatus;

		fighter.crewMember.energyPerTurn += energyPerTurnAdded;

		EndSkill ();

	}

	void HandleOnRemoveStatus (Fighter.Status status, int count)
	{
		if ( status == Fighter.Status.Enraged && count == 0 ) {
//			fighter.onRemoveStatus -= 
			energyPerTurnAdded -= energyPerTurnAdded;
		}
	}

}
