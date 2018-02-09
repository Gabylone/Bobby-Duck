using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BearTrap : Skill {

	public GameObject beapTrapPrefab;

	public float healthLost = 15f;

	public Vector2 decalToFighter = new Vector2(130,70);

	public override void ApplyEffect ()
	{
		if (fighter.HasStatus (Fighter.Status.BearTrapped))
			return;

		base.ApplyEffect ();

		fighter.AddStatus (Fighter.Status.BearTrapped);

		GameObject bearTrapObj = Instantiate (beapTrapPrefab, fighter.transform.parent) as GameObject;

		if (fighter.crewMember.side == Crews.Side.Enemy) {
			bearTrapObj.transform.localPosition = new Vector2 (-decalToFighter.x , decalToFighter.y );
		} else {
			bearTrapObj.transform.localPosition = new Vector2 (decalToFighter.x , decalToFighter.y );
		}

//		foreach (var bearTrapImage in  bearTrapObj.GetComponentsInChildren<Image>()) {
//			
//		}

		bearTrapObj.transform.localScale = Vector3.one;

		fighter.onRemoveStatus += bearTrapObj.GetComponent<BearTrap> ().HandleOnRemoveFighterStatus;
		bearTrapObj.GetComponent<BearTrap> ().fighter = fighter;

		EndSkill ();

	}

	public override bool MeetsRestrictions (CrewMember member)
	{
		bool bearTrapped = CombatManager.Instance.currentFighter.HasStatus (Fighter.Status.BearTrapped);

		return bearTrapped == false && base.MeetsRestrictions (member);
	}

	public override bool MeetsConditions (CrewMember member)
	{
		bool bearTrapped = CombatManager.Instance.currentFighter.HasStatus (Fighter.Status.BearTrapped);

		return !bearTrapped && base.MeetsConditions (member);
	}
}
