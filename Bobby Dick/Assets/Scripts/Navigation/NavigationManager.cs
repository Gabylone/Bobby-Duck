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

	[SerializeField]
	private Transform[] anchors;

	[SerializeField]
	private Transform[] otherAnchors;
	public delegate void ChunkEvent ();
	public ChunkEvent EnterNewChunk;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryLauncher.Instance.playStoryEvent += HandlePlayStory;
		StoryLauncher.Instance.endStoryEvent += HandleEndStory;

	}

	#region event handler
	void HandleEndStory ()
	{
		navigationTriggers.SetActive (true);
	}

	void HandlePlayStory ()
	{
		navigationTriggers.SetActive (false);
	}
	#endregion

	void Update () {
		if ( Input.GetKeyDown(KeyCode.DownArrow) ) {
			ChangeChunk (Directions.South);
		}
		if ( Input.GetKeyDown(KeyCode.UpArrow) ) {
			ChangeChunk (Directions.North);
		}
		if ( Input.GetKeyDown(KeyCode.LeftArrow) ) {
			ChangeChunk (Directions.West);
		}
		if ( Input.GetKeyDown(KeyCode.RightArrow) ) {
			ChangeChunk (Directions.East);
		}
	}

	#region movementf
	public void ChangeChunk ( Directions newDirection ) {

		Boats.PlayerBoatInfo.Move (newDirection);

		if (EnterNewChunk != null) {
			EnterNewChunk ();
		}

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

		Vector2 direction = point - (Vector2)Boats.PlayerBoatInfo.coords;

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
	public Coords getNewCoords ( Directions dir ) {

		switch (dir) {
		case Directions.North:
			return new Coords ( 0 , 1 );
		case Directions.NorthEast:
			return new Coords ( 1 , 1 );
		case Directions.East:
			return new Coords ( 1 , 0 );
		case Directions.SouthEast:
			return new Coords ( 1 , -1 );
		case Directions.South:
			return new Coords ( 0 , -1 );
		case Directions.SouthWest:
			return new Coords ( -1 , -1 );
		case Directions.West:
			return new Coords ( -1 , 0 );
		case Directions.NorthWest:
			return new Coords ( -1 , 1 );
		case Directions.None:
			return new Coords ( 0 , 0 );
		}

		return new Coords ();

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
	public Transform GetOppositeAnchor ( Directions direction ) {
		return otherAnchors[(int)GetOppositeDirection(direction)];
	}

	#endregion

	#region properties
	public GameObject NavigationTriggers {
		get {
			return navigationTriggers;
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

	public static Coords CurrentCoords {
		get {
			return Boats.PlayerBoatInfo.coords;
		}
	}

	public static Coords PreviousCoords {
		get {
			return Boats.PlayerBoatInfo.PreviousCoords;
		}
	}

	public static Directions GetOppositeDirection ( Directions direction ) {

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

}

[System.Serializable]
public struct Coords {
	
	public int x;
	public int y;

	public Coords (int x,int y) {
		this.x = x;
		this.y = y;
	}

	public static Coords current {
		get {
			return Boats.PlayerBoatInfo.coords;
		}
	}

	public static Coords Zero {
		get {
			return new Coords (0, 0);
		}
	}

	// overrides
		// == !=
	public static bool operator ==( Coords c1, Coords c2) 
	{
		return c1.x == c2.x && c1.y == c2.y;
	}
	public static bool operator != (Coords c1, Coords c2) 
	{
		return !(c1 == c2);
	}

		// < >
	public static bool operator < (Coords c1, Coords c2) 
	{
		return c1.x < c2.x && c1.y < c2.y;
	}
	public static bool operator > (Coords c1, Coords c2) 
	{
		return c1.x > c2.x && c1.y > c2.y;
	}
	public static bool operator < (Coords c1, int i) 
	{
		return c1.x < i || c1.y < i;
	}
	public static bool operator > (Coords c1, int i) 
	{
		return c1.x > i || c1.y > i;
	}

		// >= <=
	public static bool operator >= (Coords c1, Coords c2) 
	{
		return c1.x >= c2.x && c1.y >= c2.y;
	}
	public static bool operator <= (Coords c1, Coords c2) 
	{
		return c1.x <= c2.x && c1.y <= c2.y;
	}
	public static bool operator >= (Coords c1, int i) 
	{
		return c1.x >= i || c1.y >= i;
	}
	public static bool operator <= (Coords c1, int i) 
	{
		return c1.x <= i || c1.y <= i;
	}

		// + -
	public static Coords operator +(Coords c1, Coords c2) 
	{
		return new Coords ( c1.x + c2.x , c1.y + c2.y );
	}
	public static Coords operator -(Coords c1, Coords c2) 
	{
		return new Coords ( c1.x - c2.x , c1.y - c2.y );
	}
	public static Coords operator +(Coords c1, int i) 
	{
		return new Coords ( c1.x + i, c1.y + i );
	}
	public static Coords operator -(Coords c1, int i) 
	{
		return new Coords ( c1.x - i, c1.y - i );
	}

		// vector2 cast

	public static explicit operator Coords(Vector2 v)  // explicit byte to digit conversion operator
	{
		return new Coords ( (int)v.x , (int)v.y );
	}
	public static explicit operator Vector2(Coords c)  // explicit byte to digit conversion operator
	{
		return new Vector2 (c.x, c.y);
	}
//
//		// direction cast
//	public static explicit operator Directions(Coords c)  // explicit byte to digit conversion operator
//	{
//		return new Directions (c.x, c.y);
//	}
	public static explicit operator Coords(Directions dir)  // explicit byte to digit conversion operator
	{
		switch (dir) {
		case Directions.North:
			return new Coords ( 0 , 1 );
		case Directions.NorthEast:
			return new Coords ( 1 , 1 );
		case Directions.East:
			return new Coords ( 1 , 0 );
		case Directions.SouthEast:
			return new Coords ( 1 , -1 );
		case Directions.South:
			return new Coords ( 0 , -1 );
		case Directions.SouthWest:
			return new Coords ( -1 , -1 );
		case Directions.West:
			return new Coords ( -1 , 0 );
		case Directions.NorthWest:
			return new Coords ( -1 , 1 );
		case Directions.None:
			return new Coords ( 0 , 0 );
		}

		return new Coords ();
	}

		// string
	public override string ToString()
	{
		return "X : " + x + " / Y : " + y;
	}

	public static Coords GetClosest ( Coords originCoords ) {
		
		int radius = 1;

		while ( radius < MapGenerator.Instance.MapScale ) {

			for (int x = -radius; x < radius; x++) {
				for (int y = -radius; y < radius; y++) {

					if (x == 0 && y == 0)
						continue;

					Coords coords = new Coords (originCoords.x + x, originCoords.y + y);

					if (coords > MapGenerator.Instance.MapScale || coords <= 0) {
						continue;
					}

					Chunk chunk = Chunk.GetChunk (coords);

					if (chunk.IslandData != null) {
						return coords;
					}


				}
			}

			++radius;

			if (radius > 10) {
				Debug.Log ("Get closest island reached 10 : breaking");
				break;
			}

		}

		Debug.Log ("could not find closest island, returning current");

		return current;

	}
}
