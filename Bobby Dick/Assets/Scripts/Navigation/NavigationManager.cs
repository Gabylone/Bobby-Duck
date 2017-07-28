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

	private Directions currentDirection;

	public Directions CurrentDirection {
		get {
			return currentDirection;
		}
	}

	void Awake () {
		Instance = this;
	}

	void Start () {
		InitPlayerBoatConctrol ();
	}

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

		currentDirection = newDirection;
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

		Vector2 direction = point - (Vector2)NavigationManager.CurrentCoords;

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

	public static Coords CurrentCoords {
		get {
			return Boats.Instance.PlayerBoatInfo.CurrentCoords;
		}
	}
	public static Coords PreviousCoords {
		get {
			return Boats.Instance.PlayerBoatInfo.PreviousCoords;
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
	   
}
