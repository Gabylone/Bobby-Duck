using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public static int scaleX = 1;
	public static int scaleY = 5;
	public static int height = 5;

	// Use this for initialization
	void Start () {

		for (int x = 0; x < scaleX; x++) {

			for (int y = 0; y < scaleY; y++) {

				for (int h = 0; h < height; h++) {

					GridCoord gridCoord = new GridCoord ();
					gridCoord.x = x;
					gridCoord.y = y;
					gridCoord.h = h;
					gridCoord.state = GridCoord.State.Empty;

					GridCoord.gridCoords.Add ( gridCoord );

				}

			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public static Vector3 GetVectorDir ( Direction direction ) {

		switch (direction) {
		case Direction.Front:
			return Vector3.forward;
			break;
		case Direction.Back:
			return Vector3.back;
			break;
		case Direction.Right:
			return Vector3.right;
			break;
		case Direction.Left:
			return Vector3.left;
			break;
		case Direction.Top:
			return Vector3.up;
			break;
		case Direction.Bottom:
			return Vector3.down;
			break;
		default:
			return Vector3.zero;
			break;
		}

	}

	public static Vector3 GetLimit (Direction direction, Vector3 p) {

		switch (direction) {

		case Direction.Front:
			p.z = scaleY;
			break;

		case Direction.Back:
			p.z = 0f;
			break;

		case Direction.Right:
			p.x = scaleX;
			break;

		case Direction.Left:
			p.x = 0f;
			break;

		case Direction.Top:
			p.y = height;
			break;

		case Direction.Bottom:
			p.y = 0f;
			break;

		default:
			
			break;
		}

		return p;

	}
}

public class GridCoord {

	public enum State {
		Empty,

		Blocked,
	}

	public State state;

	public int x = 0;

	public int y = 0;

	public int h = 0;

	public static List<GridCoord> gridCoords = new List<GridCoord>();

//	public static GridCoord getCoord (GridCoord gridCoord ) {
//
//
//	}

}

