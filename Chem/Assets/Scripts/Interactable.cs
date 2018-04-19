using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

	public bool canInteract = true;

	public delegate void OnInteract ();
	public static OnInteract onInteract;

	public delegate void OnEnterInteractable(Transform target);
	public static OnEnterInteractable onEnterInteractable;

	public delegate void OnExitInteractable();
	public static OnExitInteractable onExitInteractable;

	bool containsPlayer = false;

	void OnTriggerEnter2D ( Collider2D other ) {
		if (other.tag == "Player") {
			Tween.Bounce (transform);
			if (onEnterInteractable != null ) {
				onEnterInteractable (transform);
			}
		}
	}

	void OnTriggerStay2D ( Collider2D other ) {
		if (other.tag == "Player" && canInteract  ) {
			if ( Input.GetButtonDown("Fire2") ) {
				Interact ();
			}
		}
	}

	void OnTriggerExit2D ( Collider2D other ) {
		if (other.tag == "Player") {
			if ( onExitInteractable != null ) {
				onExitInteractable ();
			}
		}
	}

	public virtual void Interact () {
		if ( onInteract != null ) {
			onInteract ();
		}
	}
}
