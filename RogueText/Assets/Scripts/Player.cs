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

    /// <summary>
    /// hunger
    /// </summary>
    public int hunger_HoursToNextStep = 5;
    public int hunger_HourCount = 0;
	public int hunger_CurrentStep = 0;
    public int hunger_MaxStep = 20;

    /// <summary>
    /// thirst
    /// </summary>
    public int thirst_HoursToNextStep = 5;
    public int thirst_CurrentHour = 0;
	public int thirst_CurrentStep = 0;
    public int thirst_MaxStep = 10;

    /// <summary>
    /// sleep
    /// </summary>
    public int sleep_HoursToNextStep = 4;
    public int sleep_CurrentHour = 0;
	public int sleep_CurrentStep = 0;
	public int sleep_MaxStep = 40;

    /// <summary>
    /// STATs
    /// </summary>
    public Stats stats;

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

        int startInteriorID = Random.Range(0, Interior.interiors.Count);
        coords = Interior.interiors.Values.ElementAt(startInteriorID).coords;

        //coords = Interior.interiors.Values.ElementAt (0).coords;

		Move (Direction.None);

        // equipement
        Equipment equipment = new Equipment();
        equipment.Init();

        // stats
        stats = new Stats();

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
        case Action.Type.DrinkAndRemove:
            DrinkAndRemove();
            break;
        case Action.Type.Drink:
			Drink ();
			break;
        case Action.Type.CheckStat:
            CheckStat();
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
            Debug.Log("exiting");
            Interior.current.ExitByDoor();
        }
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

		Tile.previous = Tile.current;

		Tile.current = TileSet.current.GetTile (coords);

		if ( Tile.current.visited == false) {
			Tile.current.GenerateItems ();
		}

		if (dir == Direction.None)
			dir = Direction.North;
		
		direction = dir;

		if (onPlayerMove != null)
			onPlayerMove (prevCoords ,coords);

		Tile.current.visited = true;

	}
    

	void HandleOnMoveAction ()
	{
		//DisplayFeedback.Instance.Display ("Vous vous déplacez...");

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
	public void UpdateStates ()
	{
        //++sleep;
        ++thirst_CurrentHour;
        ++sleep_CurrentHour;
        ++hunger_HourCount;

        if ( hunger_HourCount == hunger_HoursToNextStep)
        {
            hunger_HourCount = 0;
            AddHunger(1);
        }

        if ( sleep_CurrentHour == sleep_HoursToNextStep)
        {
            sleep_CurrentHour = 0;
            AddSleep(1);
        }

        if ( thirst_CurrentHour == thirst_HoursToNextStep)
        {
            thirst_CurrentHour = 0;
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

        string name = "" + Action.last.primaryItem.word.GetDescription(Word.Def.Defined);

        if (hunger_CurrentStep == 3)
        {
            str = name + " ne vous a pas vraiment nourri, il va faloir manger dans peu de temps...";
        }
        else if (hunger_CurrentStep == 2)
        {
            str = name + " vous permet d'attendre quelques heures, mais ce n'était pas très consistent";
        }
        else if (hunger_CurrentStep == 1)
        {
            str = name + " ne vous rassasis pas mais vous êtes satisfait";
        }
        else
        {
            str = name + " vous rempli totalement le ventre, vous êtes rassasié";
        }

        hunger_HourCount = 0;

        DisplayFeedback.Instance.Display(str);

        Item.Remove(Action.last.primaryItem);

	}
    public void RemoveHunger ( int i)
    {
        hunger_CurrentStep -= i;

        hunger_CurrentStep = Mathf.Clamp(hunger_CurrentStep, 0, 4);
    }
    public void AddHunger(int i)
    {
        hunger_CurrentStep += i;

        hunger_CurrentStep = Mathf.Clamp(hunger_CurrentStep, 0, 4);
    }
    #endregion

    #region sleep
    public void RemoveSleep(int i)
    {
        sleep_CurrentStep -= i;

        sleep_CurrentStep = Mathf.Clamp(sleep_CurrentStep, 0, 4);
    }
    public void AddSleep(int i)
    {
        sleep_CurrentStep += i;

        sleep_CurrentStep = Mathf.Clamp(sleep_CurrentStep, 0, 4);
    }
    void Sleep()
    {
        RemoveSleep(Action.last.ints[0]);

        string str = "";

        if (sleep_CurrentStep == 3)
        {
            str = "Vous n'avez pas très bien dormis, mais récupérez un peu d'énergie";
        }
        else if (sleep_CurrentStep == 2)
        {
            str = "Vous avez dormis et récupérez un peu d'énergie";
        }
        else if (sleep_CurrentStep == 3)
        {
            str = "Après quelques heures de sommeil, vous vous sentez légèrement reposé";
        }
        else
        {
            str = "Vous vous sentez entièrement reposé";
        }

        sleep_CurrentHour = 0;

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
        thirst_CurrentStep -= i;

        thirst_CurrentStep = Mathf.Clamp(thirst_CurrentStep, 0, 4);
    }
    public void AddThirst(int i)
    {
        thirst_CurrentStep += i;

        thirst_CurrentStep = Mathf.Clamp(thirst_CurrentStep, 0, 4);
    }
    void DrinkAndRemove()
    {
        Drink();

        Item.Remove(Action.last.primaryItem);

    }
    void Drink ()
	{
        RemoveThirst(4);

        thirst_CurrentHour = 0;

        DisplayFeedback.Instance.Display(Action.last.primaryItem.word.GetDescription(Word.Def.Defined, Word.Preposition.None) + " vous déshydrate, vous n'avez plus soif");

        Item.Remove(Action.last.primaryItem);

    }
    #endregion

	public Direction GetDirection ( Facing facing ) {

		int a = (int)direction + (int)facing;
		if ( a >= 8 ) {
			a -= 8;
		}

		return (Direction)a;
	}
    public Facing GetFacing(Direction dir)
    {

        int a = (int)dir - (int)direction;
        if (a < 0)
        {
            a += 8;
        }

        return (Facing)a;
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

    #region stats
    void CheckStat()
    {
        string str = Action.last.contents[0];

        Stats.Type statType = Stats.Type.Strengh;

        switch (str)
        {
            case "STR":
                statType = Stats.Type.Strengh;
                break;
            case "DEX":
                statType = Stats.Type.Dexterity;
                break;
            case "CHA":
                statType = Stats.Type.Charisma;
                break;
            case "CON":
                statType = Stats.Type.Constitution;
                break;
            default:
                break;
        }

        if ( stats.GetStat(statType) < Action.last.ints[0])
        {
			ActionManager.Instance.BreakAction ();
            DisplayFeedback.Instance.Display("Vous n'avez pas assez de : " + statType);
        }
    }
    #endregion

}

public class Stats
{
    public enum Type
    {
        Strengh,
        Dexterity,
        Charisma,
        Constitution,
    }

    public int[] values = new int[4];

    public int GetStat(Type t)
    {
        return values[(int)t];
    }
}