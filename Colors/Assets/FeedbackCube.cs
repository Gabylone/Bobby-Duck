using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackCube : MonoBehaviour {

	public GameObject group;

	bool showingFeedback = false;

	// Use this for initialization
	void Start () {
		CubeSide.onTouch += HandleOnTouch;
		CubeSide.onExitTouch += HandleOnExitTouch;
	}

	void HandleOnTouch (Direction side, Vector3 pos)
	{
		group.SetActive (true);

		showingFeedback = true;
	}

	void HandleOnExitTouch ()
	{
		group.SetActive (false);
		showingFeedback = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ( showingFeedback ) {

			transform.position = GridDisplay.RoundToGrid (Cube.selected.transform.position);

		}
	}
}
