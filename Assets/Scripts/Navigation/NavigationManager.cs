using UnityEngine;
using System.Collections;

public enum Directions {
	North,
	East,
	South,
	West,
}

public class NavigationManager : MonoBehaviour {

	public static NavigationManager Instance;

	private Directions currentDirection;

	[SerializeField]
	private Transform boatTransform;
	[SerializeField]
	private float boatSpeed = 0.3f;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		boatTransform.Translate ( getDir(currentDirection) * boatSpeed * Time.deltaTime , Space.World );
	}

	#region movement
	public void Move ( int dir ) {

		currentDirection = (Directions)dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);
	}

	private void MoveDelay () {

		SetBoatPos ();

		MapManager.Instance.SetNewPos (getDir (currentDirection));

		Transitions.Instance.ScreenTransition.Switch ();
	}
	private void SetBoatPos () {

		switch (currentDirection) {
		case Directions.North:
			boatTransform.localPosition = new Vector2 ( 0 , -164f );
			break;
		case Directions.East:
			boatTransform.localPosition = new Vector2 ( -350f , 0 );
			break;
		case Directions.South:
			boatTransform.localPosition = new Vector2 ( 0 , 164f );
			break;
		case Directions.West:
			boatTransform.localPosition = new Vector2 ( 350f , 0);
			break;
		}
	}
	#endregion

	private Vector2 getDir ( Directions dir ) {
		
		switch (dir) {
		case Directions.North:
			return new Vector2 (0, 1);
			break;
		case Directions.East:
			return new Vector2 (1, 0);
			break;
		case Directions.South:
			return new Vector2 (0, -1);
			break;
		case Directions.West:
			return new Vector2 (-1, 0);
			break;
		}

		return Vector2.zero;

	}
}
