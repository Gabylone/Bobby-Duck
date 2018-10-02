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

    public static Interactable currInteractable;

	bool containsPlayer = false;

	void OnTriggerEnter2D ( Collider2D other ) {
		if (other.tag == "Player") {
            Enter();
		}
	}

	void OnTriggerStay2D ( Collider2D other ) {
		if (other.tag == "Player" && canInteract  ) {
			if ( Input.GetButtonDown("Fire2") && currInteractable == this) {
				Interact ();
			}
		}
	}

	void OnTriggerExit2D ( Collider2D other ) {
		if (other.tag == "Player") {
            Leave();
		}
	}

	public virtual void Interact () {
		if ( onInteract != null ) {
			onInteract ();
		}
	}

    public void Enter()
    {
        if( currInteractable != null )
        {
            currInteractable.Leave();
        }

        currInteractable = this;

        Tween.Bounce(transform);

        if (onEnterInteractable != null)
        {
            onEnterInteractable(transform);
        }
    }

    public void Leave()
    {
        if (onExitInteractable != null)
        {
            onExitInteractable();
        }
    }
}
