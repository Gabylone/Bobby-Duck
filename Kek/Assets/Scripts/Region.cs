using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Region {

    public static Region selected;

    public int level = 0;

    public enum State
    {
        Invaded,

        Conquered,

        Attacked,
    }

    public static Dictionary<Coords, Region> regions = new Dictionary<Coords, Region>();

    public Coords coords;
    public int difficulty = 1;

    public State state;

    internal void SetState(State state)
    {
        this.state = state;

        if ( state == State.Conquered) {
            RegionManager.Instance.conqueredRegionCount++;
        }


    }
}

public enum Direction
{
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


[System.Serializable]
public struct Coords
{
    public static Direction[] allDirections = new Direction[8]
    {
        Direction.North,
        Direction.NorthEast,
        Direction.East,
        Direction.SouthEast,
        Direction.South,
        Direction.SouthWest,
        Direction.West,
        Direction.NorthWest
    };

    public int x;
    public int y;

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Direction GetDirectionFromString(string str)
    {

        foreach (var item in System.Enum.GetValues(typeof(Direction)))
        {
            if (item.ToString() == str)
            {
                //				Debug.Log ("found direction : " + item);
                return (Direction)item;
            }
        }

        return Direction.None;

    }

    public static Coords Zero
    {
        get
        {
            return new Coords(0, 0);
        }
    }
    // overrides
    // == !=
    public static bool operator ==(Coords c1, Coords c2)
    {
        return c1.x == c2.x && c1.y == c2.y;
    }
    public static bool operator !=(Coords c1, Coords c2)
    {
        return !(c1 == c2);
    }

    // < >
    public static bool operator <(Coords c1, Coords c2)
    {
        return c1.x < c2.x && c1.y < c2.y;
    }
    public static bool operator >(Coords c1, Coords c2)
    {
        return c1.x > c2.x && c1.y > c2.y;
    }
    public static bool operator <(Coords c1, int i)
    {
        return c1.x < i || c1.y < i;
    }
    public static bool operator >(Coords c1, int i)
    {
        return c1.x > i || c1.y > i;
    }

    // >= <=
    public static bool operator >=(Coords c1, Coords c2)
    {
        return c1.x >= c2.x && c1.y >= c2.y;
    }
    public static bool operator <=(Coords c1, Coords c2)
    {
        return c1.x <= c2.x && c1.y <= c2.y;
    }
    public static bool operator >=(Coords c1, int i)
    {
        return c1.x >= i || c1.y >= i;
    }
    public static bool operator <=(Coords c1, int i)
    {
        return c1.x <= i || c1.y <= i;
    }

    // + -
    public static Coords operator +(Coords c1, Coords c2)
    {
        return new Coords(c1.x + c2.x, c1.y + c2.y);
    }
    public static Coords operator -(Coords c1, Coords c2)
    {
        return new Coords(c1.x - c2.x, c1.y - c2.y);
    }
    public static Coords operator +(Coords c1, int i)
    {
        return new Coords(c1.x + i, c1.y + i);
    }
    public static Coords operator -(Coords c1, int i)
    {
        return new Coords(c1.x - i, c1.y - i);
    }

    // vector2 cast

    public static explicit operator Coords(Vector2 v)  // explicit byte to digit conversion operator
    {
        return new Coords((int)v.x, (int)v.y);
    }
    public static explicit operator Vector2(Coords c)  // explicit byte to digit conversion operator
    {
        return new Vector2(c.x, c.y);
    }
    //
    //		// direction cast
    //	public static explicit operator Direction(Coords c)  // explicit byte to digit conversion operator
    //	{
    //		return new Direction (c.x, c.y);
    //	}
    public static explicit operator Coords(Direction dir)  // explicit byte to digit conversion operator
    {
        switch (dir)
        {
            case Direction.North:
                return new Coords(0, 1);
            case Direction.NorthEast:
                return new Coords(1, 1);
            case Direction.East:
                return new Coords(1, 0);
            case Direction.SouthEast:
                return new Coords(1, -1);
            case Direction.South:
                return new Coords(0, -1);
            case Direction.SouthWest:
                return new Coords(-1, -1);
            case Direction.West:
                return new Coords(-1, 0);
            case Direction.NorthWest:
                return new Coords(-1, 1);
            case Direction.None:
                return new Coords(0, 0);
        }

        return new Coords();
    }

    // string
    public override string ToString()
    {
        return "X : " + x + " / Y : " + y;
    }

}