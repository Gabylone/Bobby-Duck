using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {


	bool moving = false;

	Vector2 deltaMousePos;

	public float moveSpeed = 1f;

	Vector3 middle = Vector3.zero;

	bool locked = false;

	// Use this for initialization
	void Start () {
		middle = new Vector3 ( Grid.scaleX/2f , Grid.height/2f , Grid.scaleY/2f );

		CubeSide.onTouch += HandleOnTouch;
	}

	void HandleOnTouch (Direction side, Vector3 pos)
	{
		Lock ();
	}

	void Lock () {
		locked = true;
	}

	// Update is called once per frame
	void Update () {

		if ( Input.GetMouseButtonDown(0) ) {
			StartMove ();
		}

		if (moving) {
			UpdateMove ();
		}

		transform.LookAt ( middle );

	}

	void StartMove() {

		if (locked) {
			locked = false;
			return;
		}

		deltaMousePos = Input.mousePosition;

		moving = true;
	}
	void UpdateMove() {

		Vector2 deltaPos = (Vector2)Input.mousePosition - deltaMousePos;

//		transform.Translate ( deltaPos * Time.deltaTime * moveSpeed );
		transform.RotateAround( middle , Vector3.up , deltaPos.x * Time.deltaTime * moveSpeed );
		transform.RotateAround( middle , Vector3.right , deltaPos.y * Time.deltaTime * moveSpeed );

		deltaMousePos = Input.mousePosition;


		if ( Input.GetMouseButtonUp(0) ) {
			ExitMove ();
		}
	}
	void ExitMove () {
		moving = false;
	}
}
