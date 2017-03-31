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

	[SerializeField]
	private Image flagImage;

	bool isInNoMansSea = false;
	bool hasBeenWarned = false;

	void Awake () {
		Instance = this;
	}

	#region movement
	public void Move ( int dir ) {

		currentDirection = (Directions)dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);

		if (!hasShownTuto) {
			hasShownTuto = true;
			IslandManager.Instance.DisableTuto ();
		}

	}
	public void Move ( Directions dir ) {

		currentDirection = dir;

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("MoveDelay", Transitions.Instance.ScreenTransition.Duration);
	}
	public void UpdateTime () {
		for (int i = 0; i < Crews.playerCrew.CrewMembers.Count; ++i ) {
			Crews.playerCrew.CrewMembers[i].AddToStates ();
		}
	}

	bool lostOcean_Warned = false;

	private void MoveDelay () {

		BoatManager.Instance.SetBoatPos ();

		MapManager.Instance.SetNewPos (getDir (currentDirection));

		UpdateTime ();

		IslandManager.Instance.SetIsland ();

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

	}
	#endregion

	#region triggers
	bool hasShownTuto = false;

	public Texture2D[] arrowTextures;
	public Vector2 hotSpot = Vector2.zero;

	public void CursorEnters (int texID) {
		Cursor.SetCursor(arrowTextures[texID], hotSpot, CursorMode.ForceSoftware);
	}
	public void CursorExits (int texID) {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
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
}
