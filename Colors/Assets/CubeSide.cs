using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSide : MonoBehaviour {

	public enum Type {
		Block,
		Deviate,
	}

	public Direction side;
	public Type type;

	Renderer rend;

	public delegate void OnTouch (Direction side, Vector3 pos);
	public static OnTouch onTouch;

	public delegate void OnExitTouch ();
	public static OnExitTouch onExitTouch;

	bool touched = false;

	void Start () {
		rend = GetComponent<Renderer> ();
		rend.material.color = Color.white;
	}

	void Update () {
		if (touched) {
			if ( Input.GetMouseButtonUp(0) ) {
				MouseExit ();
			}
		}
	}

	void MouseExit () {
		touched = false;

		rend.material.color = Color.white;

		if (onExitTouch != null)
			onExitTouch ();
	}

	void OnMouseDown () {
		
		if ( onTouch != null )
			onTouch (side,transform.position);

		touched = true;

		rend.material.color = Color.yellow;
	}

	public void DeviateRay ()
	{
		
	}
}
public enum Direction {
	Front,
	Back,
	Right,
	Left,
	Top,
	Bottom
}