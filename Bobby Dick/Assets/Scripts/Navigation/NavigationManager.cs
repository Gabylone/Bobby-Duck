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


	[SerializeField]
	public Sprite[] arrowSprites;

	[SerializeField]
	public Sprite flagSprite;

		// singleton
	public static NavigationManager Instance;

		// player boat info
	[SerializeField]
	private Boat playerBoat;

	private bool changingChunk = false;

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
	private Transform[] anchors;

	[SerializeField]
	private Transform[] otherAnchors;

	[SerializeField]
	private WheelControl wheelControl;

	[SerializeField]
	private FlagControl flagControl;

	public delegate void ChunkEvent ();
	public ChunkEvent EnterNewChunk;

	void Awake () {
		Instance = this;
	}

	void Start () {
		InitPlayerBoatConctrol ();
	}

	private void InitPlayerBoatConctrol ()
	{
		// get boat control
		wheelControl.WheelTransform.gameObject.SetActive (navigationSystem == NavigationSystem.Wheel);
		flagControl.FlagImage.gameObject.SetActive (navigationSystem == NavigationSystem.Flag);

		wheelControl.gameObject.SetActive (navigationSystem == NavigationSystem.Wheel);
		flagControl.gameObject.SetActive (navigationSystem == NavigationSystem.Flag);
	}

	#region movement
	public void ChangeChunk ( Directions newDirection ) {


			// set new boat direction
		PlayerBoatInfo.Instance.currentDirection = newDirection;
		PlayerBoatInfo.Instance.PosX += (int)getDir (newDirection).x;
		PlayerBoatInfo.Instance.PosY += (int)getDir (newDirection).y;

		if (EnterNewChunk != null) {
			EnterNewChunk ();
		}

//		print (getDirectionToPoint (new Vector2(MapData.Instance.treasureIslandXPos,MapData.Instance.treasureIslandYPos)).ToString());

	}
	#endregion

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

		Vector2 boatPos = new Vector2 (PlayerBoatInfo.Instance.PosX, PlayerBoatInfo.Instance.PosY);
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
	public Directions SwitchDirection ( Directions direction ) {

		switch (direction) {
		case Directions.North:
			return Directions.South;
			break;
		case Directions.NorthEast:
			return Directions.SouthWest;
			break;
		case Directions.East:
			return Directions.West;
			break;
		case Directions.SouthEast:
			return Directions.NorthWest;
			break;
		case Directions.South:
			return Directions.North;
			break;
		case Directions.SouthWest:
			return Directions.NorthEast;
			break;
		case Directions.West:
			return Directions.East;
			break;
		case Directions.NorthWest:
			return Directions.SouthEast;
			break;
		case Directions.None:
			return Directions.North;
			break;
		default:
			return Directions.None;
			break;
		}

	}
	#endregion

	#region properties
	public GameObject NavigationTriggers {
		get {
			return navigationTriggers;
		}
	}

	public NavigationSystem CurrentNavigationSystem {
		get {
			return navigationSystem;
		}
		set {
			navigationSystem = value;
		}
	}

	public WheelControl WheelControl {
		get {
			return wheelControl;
		}
		set {
			wheelControl = value;
		}
	}

	public FlagControl FlagControl {
		get {
			return flagControl;
		}
		set {
			flagControl = value;
		}
	}
	#endregion

	public Transform[] Anchors {
		get {
			return anchors;
		}
	}

	public Transform[] OtherAnchors {
		get {
			return otherAnchors;
		}
	}
}
