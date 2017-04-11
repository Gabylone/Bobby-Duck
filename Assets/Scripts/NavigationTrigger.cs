using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;

	Animator animator;

//	void Start () {
//		animator = GetComponentInParent<Animator> ();
//
//	}
//
//	public void OnMouseEnter() {
//		if ( !Application.isMobilePlatform ) {
//			animator.SetBool ("feedback", true);
//		}
//	}
//
//	public void OnMouseExit() {
//		if ( !Application.isMobilePlatform ) {
//			animator.SetBool ("feedback", false);
//		}
//	}
//
//	public void OnMouseDown() {
//		NavigationManager.Instance.Move (texID);
//		animator.SetTrigger ("press");
//	}
//
	void OnTriggerEnter2D ( Collider2D other ) {

		if (other.tag == "Player") {

			NavigationManager.Instance.Move (texID);

		}

	}

	public bool EnableFeedback {
		get {
			return animator.enabled;
		}
		set {
			animator.enabled = value;
		}
	}

	public Animator Animator {
		get {
			return animator;
		}
	}
}