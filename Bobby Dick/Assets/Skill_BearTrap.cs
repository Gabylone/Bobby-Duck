using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BearTrap : Skill {

	public GameObject beapTrapPrefab;

	public override void Start ()
	{
		base.Start ();
	}

	public override void TriggerSkill ()
	{
		if (fighter.HasStatus (Fighter.Status.BearTrapped))
			return;

		base.TriggerSkill ();

		fighter.AddStatus (Fighter.Status.BearTrapped);

		GameObject bearTrapObj = Instantiate (beapTrapPrefab, fighter.transform.parent) as GameObject;
		bearTrapObj.transform.position = fighter.transform.position + (fighter.BodyTransform.right * 1.5f) - (Vector3.up * 1.24f);
		bearTrapObj.transform.localScale = Vector3.one;

		fighter.onRemoveStatus += bearTrapObj.GetComponent<BearTrap> ().HandleOnRemoveFighterStatus;
		bearTrapObj.GetComponent<BearTrap> ().fighter = fighter;

		EndSkill ();

	}
}
