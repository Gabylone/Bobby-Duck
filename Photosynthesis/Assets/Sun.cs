using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	public enum Direction {
		LeftUp,
		Up,
		RightUp,
		DownRight,
		Down,
		DownLeft,
	}

	public static Direction direction;

	void Start () {
		
		Game.onNextTurn += HandleOnNextTurn;

		Game.onNextPhase += HandleOnNextPhase;

		Game.onNextPlayer+= HandleOnNextPlayer;

	}

	void HandleOnNextPlayer (Player player)
	{
		if (Game.phase == Game.Phase.Photosynthesis) {
			StartCoroutine (CheckSoilsCoroutine ());
		}
	}

	IEnumerator CheckSoilsCoroutine () {
		
		foreach (var soil in Soil.soils) {

			if (Game.currentPlayer.playerColor == soil.associatedPlayer.playerColor) {

				switch (soil.currentContent) {
				case Soil.Content.SmallTree:
				case Soil.Content.MediumTree:
				case Soil.Content.TallTree:

					yield return soil.AddSunPointsCoroutine ();

					break;
				}

			}

		}

		yield return new WaitForSeconds (0.5f);

	}

	void HandleOnNextPhase (Game.Phase phase)
	{
		
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow))
			direction++;
	}

	void HandleOnNextTurn (int turn)
	{
		direction = (Direction)turn;
//		print ("sun switches direction : " + direction);
		foreach (var item in Soil.soils) {
			item.ClearShadows ();
		}

		foreach (var item in Soil.soils) {
			item.CheckShadow ();
		}
	}

}

[System.Serializable]
public struct Coord {
	public int x;
	public int y;

	public Coord ( int x , int y ) {
		this.x = x;
		this.y = y;
	}

	public static bool operator ==( Coord c1, Coord c2) 
	{
		if (object.ReferenceEquals(c1, null))
		{
			return object.ReferenceEquals(c2, null);
		}

		return c1.Equals(c2);
	}
	public static bool operator !=( Coord c1, Coord c2) 
	{
		return !(c1 == c2);
	}
	public static Coord operator +(Coord c1, Coord c2) 
	{
		return new Coord ( c1.x + c2.x , c1.y + c2.y );
	}
	public static Coord operator -(Coord c1, Coord c2) 
	{
		return new Coord ( c1.x - c2.x , c1.y - c2.y );
	}
	public override string ToString () {
		return "X : " + x + " / Y : " + y;
	}
	public bool IsNull () {
		return x == 0 && y == 0;
	}

	public static Coord GetCoord ( Sun.Direction direction , int amount ) {

		Coord coord;

		switch (direction) {
		case Sun.Direction.LeftUp:
			coord = new Coord (-1, 1);
			break;
		case Sun.Direction.Up:
			coord = new Coord (0, 2);
			break;
		case Sun.Direction.RightUp:
			coord = new Coord (1, 1);
			break;
		case Sun.Direction.DownRight:
			coord = new Coord (1, -1);
			break;
		case Sun.Direction.Down:
			coord = new Coord (0, -2);
			break;
		case Sun.Direction.DownLeft:
			coord = new Coord (-1, -1);
			break;
		default:
			coord = new Coord (0, 0);
			break;
		}

		coord = new Coord (coord.x * amount, coord.y * amount);

		return coord;

	}


}
