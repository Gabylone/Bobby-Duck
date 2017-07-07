using UnityEngine;
using System.Collections;

public enum Border {
	Top,
	Right,
	Bottom,
	Left
}

public class CameraTrigger : MonoBehaviour {

	[SerializeField]
	private Border border;

	public delegate void TouchBorder (Border border);
	public static TouchBorder touchBorder;


	void OnTriggerEnter2D ( Collider2D coll ){

		if (coll.tag == "Player" ) {

//			touchBorder (border);

		}

	}
}
