using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour {

	public Fighter fighter;

	public void DestroyBearTrap () {
		Destroy (gameObject);
	}

	public void HandleOnRemoveFighterStatus (Fighter.Status status, int count)
	{
		if ( status == Fighter.Status.BearTrapped && count == 0) {
			GetComponent<Animator> ().SetTrigger ("Crush");
			fighter.onRemoveStatus -= HandleOnRemoveFighterStatus;
		}
	}
}
