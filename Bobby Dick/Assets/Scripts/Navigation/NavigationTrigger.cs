using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public static bool anyTargeted = false;

	public GameObject arrowGroup;

	public Directions direction;

	bool targeted = false;

	void Start () {
	
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		Swipe.onSwipe += HandleOnSwipe;

		WorldTouch.onTouchWorld += HandleOnTouchWorld;
	}

	void HandleOnTouchWorld ()
	{
		Targeted = false;
	}

	void HandleChunkEvent ()
	{
		Targeted = false;
	}

	void HandleOnSwipe (Directions direction)
	{
		if ( direction == this.direction ) {
			PlayerBoat.Instance.SetTargetPos (GetComponent<RectTransform>());
			Target ();
		}
	}

	void OnTriggerEnter2D ( Collider2D other ) {
		if (other.tag == "Player" && targeted ) {
			NavigationManager.Instance.ChangeChunk (direction);
			Targeted = false;
		}
	}

	void Target ()
	{
		Tween.Bounce (arrowGroup.transform);

		Targeted = true;
	}

	public bool Targeted {
		get {
			return targeted;
		}
		set {
			targeted = value;
			arrowGroup.SetActive (value);
		}
	}
}