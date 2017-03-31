using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;

	Animator animator;

	bool shownTuto = false;

	void Start () {
		animator = GetComponentInParent<Animator> ();

	}
	public void OnMouseEnter() {

		if ( !Application.isMobilePlatform ) {
//			animator.SetBool ("feedback", true);
		}

//		NavigationManager.Instance.CursorEnters (texID);

//		Cursor.SetCursor(arrowTextures[texID], hotSpot, CursorMode.Auto);
	}
	public void OnMouseExit() {
		if ( !Application.isMobilePlatform ) {
			animator.SetBool ("feedback", false);

		}

//		NavigationManager.Instance.CursorExits (texID);
//		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	public void OnMouseDown() {
		NavigationManager.Instance.Move (texID);
		animator.SetTrigger ("press");
	}

	void Update () {
		//
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