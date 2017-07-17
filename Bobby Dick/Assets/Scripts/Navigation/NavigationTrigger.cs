using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public int texID = 0;

	bool targeted = false;

	bool inside = false;

	void OnTriggerStay2D ( Collider2D other ) {

		if (other.tag == "Player" && targeted ) {

			NavigationManager.Instance.ChangeChunk ((Directions)texID);

			Targeted = false;

		}

	}

	void Update () {
		if (targeted) {
			if (NavigationManager.Instance.FlagControl.UpdatingPosition) {
				if ( !inside )
					Targeted = false;
			}

		}
	}

	public void OnMouseOver() {

		if (targeted == false) {
			if (NavigationManager.Instance.FlagControl.UpdatingPosition) {
				Targeted = true;
				NavigationManager.Instance.FlagControl.FlagImage.GetComponent<Animator> ().SetTrigger ("bounce");
			}
		}

		inside = true;
	}

	public void OnMouseExit () {
		inside = false;
	}

	public bool Targeted {
		get {
			return targeted;
		}
		set {
			targeted = value;

			NavigationManager.Instance.FlagControl.FlagImage.sprite = targeted ? NavigationManager.Instance.arrowSprites [texID] : NavigationManager.Instance.flagSprite;

		}
	}
}