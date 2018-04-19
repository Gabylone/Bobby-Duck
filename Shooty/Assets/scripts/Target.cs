using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Selectable {

	public override void Selected ()
	{
		base.Selected ();

		Tween.Bounce (transform);

		foreach ( Soldier soldier in Soldier_Player.selectedSoldiers ) {
			soldier.AimAtTarget (transform);
		}
	}

}
