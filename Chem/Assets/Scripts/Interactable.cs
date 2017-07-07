using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

	private bool canInteract = true;

	void OnTriggerStay2D ( Collider2D other ) {
		if (other.tag == "Player" && CanInteract) {

			Feedback.Instance.Place ( transform.position );

			if ( Input.GetButtonDown("Fire2") ) {
				Interact ();
			}
		}
	}

	void OnTriggerExit2D ( Collider2D other ) {
		if (other.tag == "Player") {
			Feedback.Instance.Visible = false;
		}
	}

	public virtual void Interact () {
		
	}

	public bool CanInteract {
		get {
			return canInteract;
		}
		set {
			canInteract = value;

			if (value == false)
				Feedback.Instance.Visible = false;
		}
	}
}
