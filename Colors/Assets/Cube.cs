using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Cube : MonoBehaviour {

	public static Cube selected;

	Vector3 deltaTouchDir = Vector3.zero;

	// Use this for initialization
	void Start () {
		CubeSide.onTouch += HandleOnTouch;
		CubeSide.onExitTouch += HandleOnExitTouch;
	}

	bool moving = false;

	void HandleOnExitTouch ()
	{
		moving = false;

		selected = null;

		SnapToGrid ();

		Tween.Bounce (transform);
	}

	void HandleOnTouch (	Direction side, Vector3 pos)
	{
		Tween.Bounce (transform);

		Invoke ("HandleOnTouch_Delay", 0.1f);

		selected = this;
	}
	void HandleOnTouch_Delay () {
		
		deltaTouchDir = (transform.position-CubePlane.touchPos);
		moving = true;

	}
	// Update is called once per frame
	void Update () {

		if ( moving ) {

			transform.position = CubePlane.touchPos + deltaTouchDir;
		}

	}

	void SnapToGrid () {

		HOTween.To (transform, 0.5f, "position", GridDisplay.RoundToGrid(transform.position), false, EaseType.EaseOutBounce, 0f);
	}


	void OnDrawGizmos () {

		Gizmos.DrawSphere ( CubePlane.touchPos , 0.1f );
	
	}

}
