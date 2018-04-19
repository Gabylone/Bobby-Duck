using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlane : MonoBehaviour {

	public static Vector3 touchPos;

	Collider col;

	// Use this for initialization
	void Start () {
		CubeSide.onTouch += HandleOnTouch;
		CubeSide.onExitTouch += HandleOnExitTouch;

		col = GetComponent<Collider> ();

		HandleOnExitTouch ();
	}

	void HandleOnExitTouch ()
	{
		col.enabled = false;
	}

	void HandleOnTouch (Direction side, Vector3 pos)
	{
		col.enabled = true;

		transform.position = pos;

		switch (side) {
		case Direction.Front:
		case Direction.Back:
			transform.forward = Vector3.forward;
			break;
		case Direction.Right:
		case Direction.Left:
			transform.forward = Vector3.right;
			break;
		case Direction.Top:
		case Direction.Bottom:
			transform.forward = -Vector3.up;
			break;
		default:
			break;
		}
	}

	public LayerMask layerMask;
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		if ( Physics.Raycast ( Camera.main.ScreenPointToRay (Input.mousePosition), out hit , 100f ,layerMask ) ) {

			touchPos = hit.point;

		}

	}
}
