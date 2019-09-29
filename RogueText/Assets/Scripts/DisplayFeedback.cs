using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFeedback : TextTyper {

	public static DisplayFeedback Instance;

	void Awake () {
        Instance = this;
	}

	public override void Start ()
	{
		base.Start ();

		Player.onPlayerMove += HandleOnPlayerMove;

		ActionManager.onAction += HandleOnAction;
    }

    public override void Display(string str)
    {
        base.Display(str);
    }

    void HandleOnAction (Action action)
	{
		switch (action.type) {
		case Action.Type.Display:
			    Display (action.contents [0]);
			    break;
            case Action.Type.DescribeExterior:
                DescribeExterior();
                break;
            case Action.Type.DisplayTimeOfDay:
                DisplayTimeOfDay();
                break;
            case Action.Type.DescribeItem:
                DescribeItem();
                break;
            case Action.Type.PointNorth:
                PointNorth();
                break;
            case Action.Type.DisplayHelp:
                DisplayHelp();
                break;
            default:
			break;
		}
	}

    private void DisplayHelp()
    {
        string str = "";

        Display(str);
    }

    private void DisplayTimeOfDay()
    {
        string str = "";

        if ( TimeManager.Instance.timeOfDay == 12)
        {
            str = "Il est midi";
        }
        else if(TimeManager.Instance.timeOfDay == 0)        {
            str = "Il est minuit";
        }
        else if (TimeManager.Instance.timeOfDay < 12)
        {
            str = "Il est " + TimeManager.Instance.timeOfDay + "h du matin";
        }
        else 
        {
            str = "Il est " + (TimeManager.Instance.timeOfDay-12) + "h du soir";
        }

        Display(str);
    }


    private void PointNorth()
    {
        string facing = Coords.GetPhraseDirecton(Coords.GetFacing(Player.Instance.direction));
        string str = "Le nord est " + facing;

        Display(str);
    }

    private void DescribeItem()
    {
        Item item = Action.current.primaryItem;

        string str = "";
        int count = 0;

        foreach (var verb in Verb.verbs)
        {
            if (verb.cellContents.ContainsKey(item.row))
            {
                if( verb.helpPhrase.Length > 2)
                {
                    if ( count == 0)
                    {
                        str += verb.helpPhrase.Replace("ITEM", item.word.GetDescription(Word.Def.Defined, Word.Preposition.None));
                    }
                    else
                    {
                        if ( item.word.genre == Word.Genre.Masculine)
                        {
                            str += "\n" + verb.helpPhrase.Replace("ITEM", "il");
                        }
                        else
                        {
                            str += "\n" + verb.helpPhrase.Replace("ITEM", "elle");
                        }
                    }

                    ++count;
                }
                
            }
        }

        if ( count == 0)
        {
            Display(str);
        }
        else
        {
            Display("Vous ne vous pouvez pas faire grand chose avec " + item.word.GetDescription(Word.Def.Defined, Word.Preposition.None));
        }

    }

    private void DescribeExterior()
    {
        Direction dir = Direction.East;

        if (Player.Instance.coords.x < 0)
        {
            dir = Direction.West;
        }

        Coords tCoords = TileSet.map.playerCoords + (Coords)dir;

        Tile tile = TileSet.map.GetTile(tCoords);

        string str = "Par la fenêtre, vous apercevez " + tile.GetDescription();

        if ( tile == null)
        {
            str = "la fenêtre est bloquée par une haie, vous ne voyez rien...";
        }

        Display(str);

    }

    void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
		Clear ();
	}
}
