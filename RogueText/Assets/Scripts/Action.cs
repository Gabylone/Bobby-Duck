
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Action
{

    public enum Type
    {

        None,

        Move,
        MoveRel,
        Look,
        DisplayInventory,
        CloseInventory,
        Enter,
        GoOut,
        Eat,
        Drink,
        DrinkAndRemove,
        Sleep,
        Take,
        Throw,
        AddToInventory,
        RemoveFromInventory,
        AddToTile,
        RemoveFromTile,
        Require,
        Display,
        GiveClue,
        MoveAway,
        OpenContainer,
        CloseContainer,
        Equip,
        Unequip,
        DescribeExterior,
        DisplayTimeOfDay,
        ExitByWindow,
        DescribeItem,
        PointNorth,
        RemoveLastItem,
        CheckStat,
        ReplaceItem,
        Craft,
        ReadRecipe,
        DisplayHelp,
    }

    public static Action current;

    public Type type;

    public List<string> contents = new List<string>();
    public List<int> ints = new List<int>();

    public Verb verb;

    public Item primaryItem;
    public Item secundaryItem;

}