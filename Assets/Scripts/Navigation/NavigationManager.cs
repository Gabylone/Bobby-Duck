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

	private bool isInNoMansSea = false;
	private bool hasBeenWarned = false;

	private bool moving = false;

	[SerializeField]
	private GameObject navigationTriggers;

	/// <summary>
	/// Navigation system.
	/// </summary>
	public enum NavigationSystem
	{
		Wheel,
		Flag
	}

	[SerializeField]
	private NavigationSystem navigationSystem;

	[SerializeField]
	private WheelControl wheelControl;

	[SerializeField]
	private FlagControl flagControl;

	[SerializeField]
	private Vector2 boatBounds = new Vector2 ( 290f , 125f );

	[SerializeField]
	private Boat playerBoat;

	void Awake () {
		Instance = this;
	}

	void Start () {
		foreach ( Animator animator in navigationTriggers.GetComponentsInChildren<Animator>() ){
			animator.SetBool ("feedback", true);
		}

			// get boat control
		wheelControl.WheelTransform.gameObject.SetActive (navigationSystem == NavigationSystem.Wheel);
		flagControl.FlagImage.gameObject.SetActive (navigationSystem == NavigationSystem.Flag);

		wheelControl.gameObject.SetActive (navigationSystem == NavigationSystem.Wheel);
		flagControl.gameObject.SetActive (navigationSystem == NavigationSystem.Flag);
	}

	#region movement
	public void Move ( int dir ) {

		if (moving)
			return;
		
		moving = true;

		currentDirection = (Directions)dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);

	}

	private void MoveDelay () {

		playerBoat.SetBoatPos ();

		MapManager.Instance.SetNewPos (getDir (currentDirection));

		UpdateTime ();

		IslandManager.Instance.UpdateIslandPosition ();

		Transitions.Instance.ScreenTransition.Switch ();

		WeatherManager.Instance.UpdateWeather ();

		bool isInNoMansSea =
			MapManager.Instance.PosY > (MapImage.Instance.TextureScale / 2) - (MapGenerator.Instance.NoManSeaScale/2)
			&& MapManager.Instance.PosY < (MapImage.Instance.TextureScale / 2) + (MapGenerator.Instance.NoManSeaScale/2);

		if (isInNoMansSea ) {

			if (!hasBeenWarned) {
				DialogueManager.Instance.ShowNarrator ("Le bateau entre dans la Grande Mer... Pas de terres en vue à des lieus d'ici. Mieux vaut être bien préparé, la traversée sera longue.");

				hasBeenWarned = true;
			}

		} else {
			if (hasBeenWarned) {
				DialogueManager.Instance.ShowNarrator ("Le bateau quitte les eaux de la Grande Mer... Déjà les premières îles apparaissent à l'horizon. Ouf...");
			}

			hasBeenWarned = false;

		}

//		DialogueManager.Instance.SetDialogue ( "Capitaine ! );

//		/// debug pour tout le temps savoir ou est le trésor
//
//		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
//		string directionPhrase = NavigationManager.Instance.getDirName (dir);
//
//		Debug.Log (directionPhrase);

		moving = false;
	}
	#endregion


	public void UpdateTime () {
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			Crews.playerCrew.CrewMembers[i].AddToStates ();
		}
	}

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


	public Directions getDirectionFromVector ( Vector2 dir ) {

		for (int i = 0; i < 8; ++i ) {
			if ( Vector2.Angle ( NavigationManager.Instance.getDir( (Directions)i ) , dir ) < 22f ) {
				return (Directions)i;
			}
		}
		return Directions.None;
	}
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
		case Directions.NorthEast:
			return "au nord est";
		case Directions.East:
			return "à l'est";
		case Directions.SouthEast:
			return "au sud est";
		case Directions.South:
			return "au sud";
		case Directions.SouthWest:
			return "au sud ouest";
		case Directions.West:
			return "à l'ouest";
		case Directions.NorthWest:
			return "au nord ouest";
		case Directions.None:
			return "nulle part";
		}

		return "nulle part";

	}
	public Vector2 getDir ( Directions dir ) {

		switch (dir) {
		case Directions.North:
			return new Vector2 (0, 1);
		case Directions.NorthEast:
			return new Vector2 (1, 1);
		case Directions.East:
			return new Vector2 (1, 0);
		case Directions.SouthEast:
			return new Vector2 (1, -1);
		case Directions.South:
			return new Vector2 (0, -1);
		case Directions.SouthWest:
			return new Vector2 (-1, -1);
		case Directions.West:
			return new Vector2 (-1, 0);
		case Directions.NorthWest:
			return new Vector2 (-1, 1);
		case Directions.None:
			return new Vector2 (0, 0);
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

	public GameObject NavigationTriggers {
		get {
			return navigationTriggers;
		}
	}

	public Vector2 BoatBounds {
		get {
			return boatBounds;
		}
	}
}
