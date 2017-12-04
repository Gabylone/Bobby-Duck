using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SkipTurn: Skill {

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{

		base.TriggerSkill ();

		CombatManager.Instance.NextTurn ();
	}

}
