using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public static bool anyTargeted = false;

	public GameObject arrowGroup;

	public Directions direction;

	bool targeted = false;

	bool inside = false;

	void Start () {
		Swipe.onSwipe += HandleOnSwipe;
	}

	void Update () {
		if (targeted) {

			if (NavigationManager.Instance.FlagControl.UpdatingPosition) {
				if (!inside) {
					NavigationManager.Instance.FlagControl.PlaceFlagOnScreen ();
					Targeted = false;
				}
			}

		}
	}

	void HandleOnSwipe (Directions direction)
	{
		if ( direction == this.direction ) {

			NavigationManager.Instance.FlagControl.UpdatingPosition = false;
			NavigationManager.Instance.FlagControl.FlagImage.transform.position = arrowGroup.transform.position;
//			NavigationManager.Instance.FlagControl.PlaceFlagOnWorld (transform.localPosition);

			Target ();
		}
	}

	void OnTriggerStay2D ( Collider2D other ) {
		if (other.tag == "Player" && targeted ) {
			NavigationManager.Instance.ChangeChunk (direction);
			Targeted = false;
		}
	}

	void Target ()
	{
		

		Targeted = true;

		Tween.Bounce (NavigationManager.Instance.FlagControl.FlagImage.transform);
	}

	public void OnMouseOver() {

		if (targeted == false) {
			if (NavigationManager.Instance.FlagControl.UpdatingPosition) {
				Target ();
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

			if (value == false) {
				arrowGroup.SetActive (false);
			}
			NavigationManager.Instance.FlagControl.FlagImage.sprite = targeted ? NavigationManager.Instance.arrowSprites [(int)direction] : NavigationManager.Instance.flagSprite;

		}
	}
}