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

        Display("Loading world");
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
            default:
			break;
		}
	}

    private void DisplayTimeOfDay()
    {
        string str = "Il est " + TimeManager.Instance.timeOfDay + "h...";

        Display(str);
    }

    private void DescribeItem()
    {
        Item item = Action.last.item;

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

        Display(str);

        
    }

    private void DescribeExterior()
    {
        Coords tCoords = TileSet.map.playerCoords + (Coords)Player.Instance.direction;

        Tile tile = TileSet.map.GetTile(tCoords);

        string str = "Par la fenêtre, vous apercevez " + tile.GetDescription();

        Display(str);

    }

    void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
		Clear ();
	}
}
