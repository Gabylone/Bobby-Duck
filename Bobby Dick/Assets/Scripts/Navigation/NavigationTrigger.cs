using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public static bool anyTargeted = false;

	public GameObject arrowGroup;

	public Directions direction;

	RectTransform rectTransform;

	bool targeted = false;

	void Start () {
	
		rectTransform = GetComponent<RectTransform> ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		Swipe.onSwipe += HandleOnSwipe;

		WorldTouch.onTouchWorld += HandleOnTouchWorld;

		StoryLauncher.Instance.onStartStory += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{
		Targeted = false;
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
		Targeted = false;

		if ( direction == this.direction ) {
			PlayerBoat.Instance.SetTargetPos (rectTransform);
			Target ();

//			Invoke ("HandleOnSwipeDelay" , 0.7f);
		}
	}

//	void HandleOnSwipeDelay () {
//		NavigationManager.Instance.ChangeChunk (direction);
//		Targeted = false;
//	}

	void OnTriggerStay2D ( Collider2D other ) {
		if (other.tag == "Player" && targeted ) {
			NavigationManager.Instance.ChangeChunk (direction);
			Targeted = false;
		}
	}
//
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