using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player Instance;

	public Direction direction;

    // STATES
	public int health = 0;
    public int maxHealth = 10;

    public int hunger_rate = 5;
    public int hunger_currRate = 0;
	public int hunger = 0;
    public int maxHunger = 20;

    public int thirst_rate = 5;
    public int thirst_currRate = 0;
	public int thirst = 0;
    public int maxThirst = 10;

    public int sleep_rate = 4;
    public int sleep_currRate = 0;
	public int sleep = 0;
	public int maxSleep = 40;

	// COORDS
	public Coords prevCoords = new Coords(-1,-1);
	public Coords coords = new Coords (-1,-1);

	// EVENTS
	public delegate void OnPlayerMove ( Coords prevCoords , Coords newCoords);
	public static OnPlayerMove onPlayerMove;

    public delegate void OnPlayerInit();
    public OnPlayerInit onPlayerInit;
    
	void Awake () {
		Instance = this;
	}


	public void Init () {

		coords = Interior.interiors.Values.ElementAt (0).coords;

		Move (Direction.None);

        Equipment equipment = new Equipment();
        equipment.Init();

		ActionManager.onAction+= HandleOnAction;

        if ( onPlayerInit != null)
        {
            onPlayerInit();
        }

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
		case Action.Type.Look:
			break;
		case Action.Type.Enter:
			EnterCurrentInterior ();
			break;
        case Action.Type.ExitByWindow:
            Interior.current.ExitByWindow();
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
        if (Interior.current == null)
        {
            Interior.Get(coords).Enter();
        }
        else
        {
            Interior.current.ExitByDoor();
        }
	}

	public void Move ( Direction dir ) {

		Coords targetCoords = coords + (Coords)dir;

		if (!CanMoveForward (targetCoords)) {
			Debug.Log ("cannot move forward");
			return;
		}

		if (coords.x > 0) {
			prevCoords = coords;
		}
		coords += (Coords)dir;

		Tile.previous = Tile.current;

		Tile.current = TileSet.current.GetTile (coords);

		if ( Tile.current.visited == false) {
			Tile.current.GenerateItems ();
		}

		if (dir == Direction.None)
			dir = Direction.North;
		
		direction = dir;

		UpdateStates ();

		if (onPlayerMove != null)
			onPlayerMove (prevCoords ,coords);

		Tile.current.visited = true;

	}

	void HandleOnMoveAction ()
	{
		DisplayFeedback.Instance.Display ("Vous vous déplacez...");

		float moveDelay = 2f;
		Invoke ("MoveDelay",moveDelay);
	}

	void MoveDelay () {
		Move ((Direction)Action.last.ints[0]);
	}

	bool CanMoveForward (Coords c)
	{
		Tile targetTile = TileSet.current.GetTile (c);

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
        if ( Interior.current != null )
        {
            return;
        }

        //++sleep;
        ++thirst_currRate;
        ++sleep_currRate;
        ++hunger_currRate;

        if ( hunger_currRate == hunger_rate)
        {
            hunger_currRate = 0;
            AddHunger(1);
        }

        if ( sleep_currRate == sleep_rate)
        {
            sleep_currRate = 0;
            AddSleep(1);
        }

        if ( thirst_currRate == thirst_rate)
        {
            thirst_currRate = 0;
            AddThirst(1);
        }

	}
    #endregion

    #region hunger
    void Eat ()
	{
        RemoveHunger(Action.last.ints[0]);

        if (Action.last.ints.Count > 1)
        {
            health -= Action.last.ints[1];
        }

        string str = "";

        string name = "" + Action.last.item.word.GetDescription(Word.Def.Defined);

        if (hunger == 3)
        {
            str = name + " ne vous a pas vraiment nourri, il va faloir manger dans peu de temps...";
        }
        else if (hunger == 2)
        {
            str = name + " vous permet d'attendre quelques heures, mais ce n'était pas très consistent";
        }
        else if (hunger == 1)
        {
            str = name + " ne vous rassasis pas mais vous êtes satisfait";
        }
        else
        {
            str = name + " vous rempli totalement le ventre, vous êtes rassasié";
        }

        hunger_currRate = 0;

        DisplayFeedback.Instance.Display(str);

        Item.Remove(Action.last.item);

	}
    public void RemoveHunger ( int i)
    {
        hunger -= i;

        hunger = Mathf.Clamp(hunger, 0, 4);
    }
    public void AddHunger(int i)
    {
        hunger += i;

        hunger = Mathf.Clamp(hunger, 0, 4);
    }
    #endregion

    #region sleep
    public void RemoveSleep(int i)
    {
        sleep -= i;

        sleep = Mathf.Clamp(sleep, 0, 4);
    }
    public void AddSleep(int i)
    {
        sleep += i;

        sleep = Mathf.Clamp(sleep, 0, 4);
    }
    void Sleep()
    {
        RemoveSleep(Action.last.ints[0]);

        string str = "";

        if (sleep == 3)
        {
            str = "Vous n'avez pas très bien dormis, mais récupérez un peu d'énergie";
        }
        else if (sleep == 2)
        {
            str = "Vous avez dormis et récupérez un peu d'énergie";
        }
        else if (sleep == 3)
        {
            str = "Après quelques heures de sommeil, vous vous sentez légèrement reposé";
        }
        else
        {
            str = "Vous vous sentez entièrement reposé";
        }

        sleep_currRate = 0;

        DisplayDescription.Instance.ClearAll();

        DisplayFeedback.Instance.Display(str);

        TimeManager.Instance.timeOfDay = 6;
        TimeManager.Instance.NextDay();

    }
    void SleepDelay()
    {
        DisplayDescription.Instance.DisplayTileDescription();
    }
    #endregion

    #region thirst
    public void RemoveThirst(int i)
    {
        thirst -= i;

        thirst = Mathf.Clamp(thirst, 0, 4);
    }
    public void AddThirst(int i)
    {
        thirst += i;

        thirst = Mathf.Clamp(thirst, 0, 4);
    }
    void Drink ()
	{
        RemoveThirst(4);

        thirst_currRate = 0;

        DisplayFeedback.Instance.Display(Action.last.item.word.GetDescription(Word.Def.Defined, Word.Preposition.None) + " vous déshydrate, vous n'avez plus soif");

        Item.Remove(Action.last.item);

    }
	#endregion

	public Direction GetDirection ( Facing facing ) {

		int a = (int)direction + (int)facing;
		if ( a >= 8 ) {
			a -= 8;
		}

		return (Direction)a;
	}

    public enum Facing
    {
        Front,
        FrontRight,
        Right,
        BackRight,
        Back,
        BackLeft,
        Left,
        FrontLeft,

        None,

        Current,
    }


}


/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interior {

	public Coords coords;

    public static Interior current;

	public static Dictionary<Coords, Interior> interiors= new Dictionary<Coords, Interior>();

    public TileSet tileSet;

    public static void Add ( Tile tile , Tile.Type type) {

		tile.SetType (type);

		Interior newInterior = new Interior ();

		newInterior.coords = tile.coords;
		interiors.Add (tile.coords, newInterior);
	}

	public static Interior Get (Coords coords)
	{
		return interiors[coords];
	}

    

    #region generation
    public void Genererate() {

		tileSet = new TileSet ();

		List<Tile.Type> roomTypes = new List<Tile.Type> ();

		Tile.Type type = Tile.Type.LivingRoom;

		for (int i = 0; i < WorldGeneration.Instance.tileTypeAppearChances.Length; i++) {
			
			if ( Random.value * 100 < WorldGeneration.Instance.tileTypeAppearChances[i] ) {

				roomTypes.Add (type);

			}

			++type;

		}

		Coords hallway_Coords = new Coords ((int)(WorldGeneration.Instance.mapScale/2f),(int)(WorldGeneration.Instance.mapScale/2f));

        int hallWayIndex = 0;

		while ( roomTypes.Count > 0 ) {

			Tile newHallwayTile = new Tile (hallway_Coords);

			newHallwayTile.SetType (Tile.Type.Hallway);

            if (hallWayIndex  == 0)
            {
                newHallwayTile.AddItem(Item.FindByName("porte"));
            }

            tileSet.Add (hallway_Coords, newHallwayTile);

			if ( Random.value * 100 < WorldGeneration.Instance.roomAppearRate ) {

				Coords coords = newHallwayTile.coords + new Coords (1, 0);

				Tile newRoomTile = new Tile(coords);
				Tile.Type roomType = roomTypes [Random.Range (0, roomTypes.Count)];
				newRoomTile.SetType (roomType);

				roomTypes.Remove (roomType);

				tileSet.Add ( coords, newRoomTile );
			}

			hallway_Coords += new Coords (0, 1);

            ++hallWayIndex;
        }

        // GENERATING BUNKER

        if ( coords == ClueManager.Instance.bunkerCoords)
        {
            int a = Random.Range(1, tileSet.tiles.Count);
            Item bunkerItem = Item.FindByName("tableau");
            tileSet.tiles.Values.ElementAt(a).items.Add(bunkerItem);
        }

	}
#endregion

}*/
