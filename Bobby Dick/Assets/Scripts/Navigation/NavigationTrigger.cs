using UnityEngine;
using System.Collections;

public class NavigationTrigger : MonoBehaviour {

	public static bool anyTargeted = false;

	public GameObject arrowGroup;

	public Directions direction;

	Transform _transform;

	bool targeted = false;

	void Start () {

        _transform = GetComponent<Transform> ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		Swipe.onSwipe += HandleOnSwipe;

		WorldTouch.onPointerDown += HandleOnTouchWorld;

		StoryLauncher.Instance.onPlayStory += HandlePlayStoryEvent;

        Targeted = false;
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

            Vector3 corner = NavigationManager.Instance.GetCornerPosition(direction);
            Vector3 p = corner + (corner - PlayerBoat.Instance.getTransform.position).normalized * 2f;

            PlayerBoat.Instance.SetTargetPos(p);

			Target ();
		}
	}

	void OnTriggerStay ( Collider other ) {
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