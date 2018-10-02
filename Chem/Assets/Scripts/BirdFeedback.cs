using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFeedback : MonoBehaviour {

	public GameObject group;

	public float displayDuration = 1.5f;

	Bird bird;

	void Start () {
		bird = GetComponentInParent<Bird> ();

		bird.onChangeState += HandleOnChangeState;
		Hide ();
	}

	void HandleOnChangeState () {
		if ( bird.currentState == Bird.State.moving ) {
			Show ();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void Hide ()
	{
		group.SetActive (false);
	}

	void Show () {

		group.SetActive (true);

		Tween.Bounce (group.transform);

		Invoke ("Hide" , displayDuration);

	}
}
