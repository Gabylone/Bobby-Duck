using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

	public delegate void OnSwipe (Directions direction);
	public static OnSwipe onSwipe;

	Vector2 prevPoint;

	public Transform test;

	public float minimumDistance = 0.1f;

	public float minimumTime = 0.5f;

	bool swiping = false;

	float timer = 0f;

	// Use this for initialization
	void Start () {
		WorldTouch.pointerDownEvent += HandlePointerDownEvent;
	}

	void HandlePointerDownEvent ()
	{
		Swipe_Start ();
	}

	void HandleOnTouchWorld ()
	{
//		Swipe_Start ();
	}
	
	// Update is called once per frame
	void Update () {

//		if ( InputManager.Instance.OnInputDown() ) {
//			Swipe_Start ();
//		}

		if (swiping) {
			Swipe_Update ();
		}

	}

	void Swipe_Start() {

		print ("starting swipe");

		swiping = true;
		timer = 0f;

		prevPoint = InputManager.Instance.GetInputPosition ();
	}

	void Swipe_Update() {

		float dis = Vector3.Distance ( prevPoint , InputManager.Instance.GetInputPosition() );

		if (dis > minimumDistance && timer < minimumTime) {

			Vector2 dir = (Vector2)InputManager.Instance.GetInputPosition () - prevPoint;
			Directions direction = NavigationManager.Instance.getDirectionFromVector (dir);

//			print ("swipe : " + direction.ToString() );

			print ("swiping : " + direction);

			if ( onSwipe!=null )
				onSwipe (direction);

			Swipe_Exit ();
		}

		timer += Time.deltaTime;

//		prevPoint = InputManager.Instance.GetInputPosition ();

		if ( InputManager.Instance.OnInputExit() ) {
			print ("leaving touch ( swipe exit)");
			Swipe_Exit ();
		}
	}

	void Swipe_Exit () {
		swiping = false;
	}
}
