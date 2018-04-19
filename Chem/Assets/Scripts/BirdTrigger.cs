using UnityEngine;
using System.Collections;

public class BirdTrigger : Interactable {

	Bird bird;

	void Start () {
		bird = GetComponentInParent<Bird> ();

		bird.onChangeState += HandleOnChangeState;
		bird.onChangeDirection += HandleOnChangeDirection;
	}

	void HandleOnChangeDirection ()
	{
		if (bird.direction == Character.Instance.direction) {
			GetComponent<BoxCollider2D> ().enabled = true;
			//
		} else {
			GetComponent<BoxCollider2D> ().enabled = false;

		}
	}

	void HandleOnChangeState ()
	{
		if (bird.currentState == Controller.State.state1) {
			GetComponent<BoxCollider2D> ().enabled = true;
		} else {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	public override void Interact ()
	{
		base.Interact ();

		bird.Die ();
		canInteract = false;
	}
}
