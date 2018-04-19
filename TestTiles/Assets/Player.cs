using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player Instance;

	public Direction direction;

	// STATES
	public int health = 10;
	public int hunger = 0;
	public int maxHunger = 20;
	public int thirst = 0;
	public int maxThirst = 10;
	public int sleep = 0;
	public int maxSleep = 40;

	// COORDS
	public Coords prevCoords = new Coords(-1,-1);
	public Coords coords = new Coords (-1,-1);

	// EVENTS
	public delegate void OnPlayerMove ( Coords prevCoords , Coords newCoords);
	public static OnPlayerMove onPlayerMove;

	void Awake () {
		Instance = this;
	}

	public void Init () {

		coords = Interior.interiors.Values.ElementAt (0).coords;

		Move (Direction.None);

		ActionManager.onAction+= HandleOnAction;

		onPlayerMove += HandleOnPlayerMove;
	}

	void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
		UpdateStates ();
	}

	void HandleOnAction (Action action)
	{
		switch (action.type) {
		case Action.Type.Move:
			Move ((Direction)Action.last.ints[0]);
			break;
		case Action.Type.MoveRel:
			Move (GetDirection((Facing)Action.last.ints[0]));
			break;
		case Action.Type.LookAround:
			break;
		case Action.Type.Enter:
			EnterCurrentInterior ();
			break;
		case Action.Type.GoOut:
			break;
		case Action.Type.Eat:
			Eat ();
			break;
		case Action.Type.Drink:
			Drink ();
			break;
		case Action.Type.Sleep:
			Sleep();
			break;
		default:
			break;
		}
	}

	void EnterCurrentInterior ()
	{
		Interior interior = Interior.Get ( coords);

		if ( interior.tileSet == null ) {
			interior.tileSet = new TileSet();
			interior.Genererate();
		}

		Tile.SetTileSet(interior.tileSet);

		coords = new Coords ((int)(WorldGeneration.mapScale/2f),(int)(WorldGeneration.mapScale/2f));

		Move (Direction.None);
	}

	public void Move ( Direction dir ) {

		Coords targetCoords = coords + (Coords)dir;

		if (!CanMoveForward (targetCoords)) {
			return;
		}

		if (coords.x > 0) {
			prevCoords = coords;
		}
		coords += (Coords)dir;

		if ( Tile.current.items.Count == 0 ) {
			Tile.current.GenerateItems ();
		}


		if (dir == Direction.None)
			dir = Direction.North;
		direction = dir;

		if (onPlayerMove != null)
			onPlayerMove (prevCoords ,coords);

	}

	void HandleOnMoveAction ()
	{
		DisplayDescription.Instance.Clear ();
		DisplayFeedback.Instance.Display ("Vous vous déplacez...");

		float moveDelay = 2f;
		Invoke ("MoveDelay",moveDelay);
	}

	void MoveDelay () {
		Move ((Direction)Action.last.ints[0]);
	}

	bool CanMoveForward (Coords c)
	{
		Tile targetTile = Tile.GetTile (c);

		if ( targetTile == null ) {
			DisplayFeedback.Instance.Display ("Vous ne pouvez pas aller par là");
			return false;
		}

		switch (targetTile.type) {
		case Tile.Type.Hill:
			DisplayFeedback.Instance.Display ("La coline est trop haute, impossible de passer");
			return false;
		case Tile.Type.Mountain:
			DisplayFeedback.Instance.Display ("La pente est trop raide, il faut faire demi tour");
			return false;
		case Tile.Type.Sea:
			DisplayFeedback.Instance.Display ("Le niveau de la mer est trop haut, impossible d'avancer");
			return false;
			break;
		case Tile.Type.Lake:
			DisplayFeedback.Instance.Display ("Le lac est trop profond, impossible d'avancer sans bateau");
			return false;
		case Tile.Type.River:
			DisplayFeedback.Instance.Display ("Le courant de la rivère est trop fort, impossible d'avancer");
			return false;
		default:
			break;
		}

		return true;
	}

	#region states
	void UpdateStates ()
	{
		++sleep;
		++hunger;
		++thirst;
//
//		sleep = Mathf.Clamp (sleep, 0, maxSleep);
//		hunger = Mathf.Clamp (hunger, 0, maxHunger);
//		thirst = Mathf.Clamp (thirst, 0, maxThirst);
	}

	void Eat ()
	{
		int i = 1;
		if ( Action.last.ints.Count > 0 ) {
			i = Action.last.ints [0];
		}

		hunger-= i;
	}
	void Sleep ()
	{
		int i = 1;
		if ( Action.last.ints.Count > 0 ) {
			i = Action.last.ints [0];
		}

		sleep -= i;
	}
	void Drink ()
	{
		int i = 1;
		if ( Action.last.ints.Count > 0 ) {
			i = Action.last.ints [0];
		}

		thirst -= i;
	}
	#endregion

	public Direction GetDirection ( Facing facing ) {

		int a = (int)direction + (int)facing;
		if ( a >= 8 ) {
			a -= 8;
		}

//		Debug.Log ( "player is turned " + direction + ", so the returned dir is " + (Direction)a );

		return (Direction)a;

	}

	public enum Facing {
		Front,
		FrontRight,
		Right,
		BackRight,
		Back,
		BackLeft,
		Left,
		FrontLeft,
	}


}
