using UnityEngine;
using System.Collections;

public class BirdTrigger : Interactable {

	public override void Interact ()
	{
		base.Interact ();

		GetComponentInParent<Bird> ().Die ();

		CanInteract = false;
	}
}
