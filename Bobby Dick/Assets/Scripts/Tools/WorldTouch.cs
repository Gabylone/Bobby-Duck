using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTouch : MonoBehaviour {

	public delegate void OnTouchWorld ();
	public static OnTouchWorld onTouchWorld;

	public delegate void PointerDownEvent ();
	public static PointerDownEvent pointerDownEvent;

	bool touching = false;

	float timeToTouch = 0.25f;

	// Use this for initialization
	void Start () {
		Swipe.onSwipe += HandleOnSwipe;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	void HandleChunkEvent ()
	{
		
	}

	void HandleOnSwipe (Directions direction)
	{
		timer = timeToTouch + 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (touching) {
			timer += Time.deltaTime;
		}
	}

	float timer = 0f;

	public void OnPointerDown () {

		touching = true;

		timer = 0f;

		if (pointerDownEvent != null) {
			pointerDownEvent ();
		}
	}


	public void OnPointerUp () {

		touching = false;

		if (timer > timeToTouch) {
			return;
		}

		if (onTouchWorld != null) {
			onTouchWorld ();
		}
	}
}
