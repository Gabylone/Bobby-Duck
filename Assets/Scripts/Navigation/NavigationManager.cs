using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Directions {
	North,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest,
	None
}

public class NavigationManager : MonoBehaviour {

	public static NavigationManager Instance;

	private Directions currentDirection;

	private int shipRange = 1;

	void Awake () {
		Instance = this;
	}

	#region movement
	public void Move ( int dir ) {

		currentDirection = (Directions)dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);

	}
	public void Move ( Directions dir ) {

		currentDirection = dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);
	}

	private void MoveDelay () {

		BoatManager.Instance.SetBoatPos ();

		MapManager.Instance.SetNewPos (getDir (currentDirection));

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			Crews.playerCrew.CrewMembers[i].AddToStates ();
		}

		IslandManager.Instance.SetIsland ();

		Transitions.Instance.ScreenTransition.Switch ();

		WeatherManager.Instance.UpdateWeather ();

	}
	#endregion

	public int ShipRange {
		get {

			int range = shipRange;

			if (WeatherManager.Instance.Raining)
				range--;
			if (WeatherManager.Instance.IsNight)
				range--;
			
			return Mathf.Clamp (range,0,10);
		}
		set {
			shipRange = value;
		}
	}

	#region tools
	public Directions getDirectionToPoint ( Vector2 point ) {

		Vector2 boatPos = new Vector2 (MapManager.Instance.PosX, MapManager.Instance.PosY);
		Vector2 direction = point - boatPos;

		for (int i = 0; i < 8; ++i ) {
			if ( Vector2.Angle ( direction , NavigationManager.Instance.getDir((Directions)i) ) < 45f ) {
				return (Directions)i;
			}

		}

		Debug.Log ("coun't find a direction");
		return Directions.None;
	}
	public string getDirName ( Directions dir ) {

		switch (dir) {
		case Directions.North:
			return "au nord";
			break;
		case Directions.NorthEast:
			return "au nord est";
			break;
		case Directions.East:
			return "à l'est";
			break;
		case Directions.SouthEast:
			return "au sud est";
			break;
		case Directions.South:
			return "au sud";
			break;
		case Directions.SouthWest:
			return "au sud ouest";
			break;
		case Directions.West:
			return "à l'ouest";
			break;
		case Directions.NorthWest:
			return "au nord ouest";
			break;
		case Directions.None:
			return "nulle part";
			break;
		}

		return "nulle part";

	}
	public Vector2 getDir ( Directions dir ) {

		switch (dir) {
		case Directions.North:
			return new Vector2 (0, 1);
			break;
		case Directions.NorthEast:
			return new Vector2 (1, 1);
			break;
		case Directions.East:
			return new Vector2 (1, 0);
			break;
		case Directions.SouthEast:
			return new Vector2 (1, -1);
			break;
		case Directions.South:
			return new Vector2 (0, -1);
			break;
		case Directions.SouthWest:
			return new Vector2 (-1, -1);
			break;
		case Directions.West:
			return new Vector2 (-1, 0);
			break;
		case Directions.NorthWest:
			return new Vector2 (-1, 1);
			break;
		case Directions.None:
			return new Vector2 (0, 0);
			break;
		}

		return Vector2.zero;

	}

	#endregion

	public Directions CurrentDirection {
		get {
			return currentDirection;
		}
		set {
			currentDirection = value;
		}
	}
}
