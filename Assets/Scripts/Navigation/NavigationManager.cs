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

	[Header("Boat")]
	[SerializeField]
	private Transform boatTransform;
	[SerializeField]
	private Vector2 boatBounds = new Vector2 ( 350f, 164f );
	[SerializeField]
	private float boatSpeed = 0.3f;

	private int shipRange = 1;

	[Header("Island")]
	[SerializeField] private Image islandImage;

	[Header("Weather")]
	[Header("Rain")]
	[SerializeField] private Image rainImage;

	bool raining = false;

	private int currentRain = 0;
	[SerializeField] private int rainRate = 20;
	[SerializeField] private int rainDuration = 5;

	[Header("Night")]
	[SerializeField] private Image nightImage;

	bool isNight = false;

	private int currentNight = 0;
	[SerializeField] private int nightRate = 8;
	[SerializeField] private int nightDuration = 4;

	[Header ("Sounds")]
	[SerializeField] private AudioClip rainSound;
	[SerializeField] private AudioClip daySound;



	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		PlaySound ();
	}
	
	// Update is called once per frame
	void Update () {
		
		boatTransform.Translate ( getDir(currentDirection) * boatSpeed * Time.deltaTime , Space.World );

		if (Input.GetKeyDown (KeyCode.A)) {
			for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i) {
				Crews.playerCrew.CrewMembers [i].AddToStates ();
				Crews.playerCrew.CrewMembers [i].AddXP(1);
			}

			PlayerLoot.Instance.UpdateMembers ();
		}
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

		SetBoatPos ();

		MapManager.Instance.SetNewPos (getDir (currentDirection));

		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			Crews.playerCrew.CrewMembers[i].AddToStates ();
		}

		SetIsland ();

		Transitions.Instance.ScreenTransition.Switch ();

		UpdateWeather ();

	}
	private void SetBoatPos () {

		boatTransform.localPosition = -new Vector2(getDir (currentDirection).x * boatBounds.x, getDir (currentDirection).y * boatBounds.y);

	}
	private void SetIsland () {
		
		islandImage.gameObject.SetActive ( MapManager.Instance.NearIsland );
		if ( MapManager.Instance.NearIsland )
			islandImage.transform.localPosition = MapManager.Instance.CurrentIsland.Position;

	}
	#endregion

	#region weather
	private void UpdateWeather () {

		// rain image
		currentRain++;
		int r1 = Raining ? rainDuration : rainRate;
		if (currentRain == r1)
			Raining = !Raining;

		currentNight++;
		int r2 = IsNight ? nightDuration : nightRate;
		if (currentNight == r2)
			IsNight = !IsNight;
	}
	public void PlaySound () {
		if (!IslandManager.Instance.OnIsland) {
			SoundManager.Instance.PlayAmbiance (raining ? rainSound : daySound);
		}
	}
	#endregion

	public int ShipRange {
		get {

			int range = shipRange;
			if (raining)
				range--;
			if (isNight)
				range--;
			
			return Mathf.Clamp (shipRange,0,10);
		}
		set {
			shipRange = value;
		}
	}

	public bool Raining {
		get {
			return raining;
		}
		set {
			raining = value;
			currentRain = 0;
			rainImage.gameObject.SetActive ( value );
			PlaySound ();
		}
	}

	public bool IsNight {
		get {
			return isNight;
		}
		set {
			isNight = value;
			currentNight = 0;
			nightImage.gameObject.SetActive (value);
			PlaySound ();
		}
	}

	#region tools
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
}
