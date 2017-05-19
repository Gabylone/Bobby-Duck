using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;

	Animator animator;
//
	void OnTriggerEnter2D ( Collider2D other ) {

		if (other.tag == "Player") {

			NavigationManager.Instance.ChangeChunk (texID);

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