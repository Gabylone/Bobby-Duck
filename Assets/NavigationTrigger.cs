using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;

	Animator animator;

	bool shownTuto = false;

	void Start () {
		animator = GetComponentInParent<Animator> ();

	}

	public void OnMouseExit() {
		if ( !Application.isMobilePlatform ) {
			animator.SetBool ("feedback", false);
		}
	}

	public void OnMouseDown() {
		NavigationManager.Instance.Move (texID);
		animator.SetTrigger ("press");
	}

	public bool EnableFeedback {
		get {
			return animator.enabled;
		}
		set {
			animator.enabled = value;

			animator.SetTrigger ("feedback");
		}
	}

	public Animator Animator {
		get {
			return animator;
		}
	}
}