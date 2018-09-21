using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilable : MonoBehaviour {

    public Coords coords;

    public static int spriteBuffer = 5;

    public virtual void Start()
    {
        coords = new Coords(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));


    }

    public void UpdateSprites()
    {
        int minimumSpriteOrder = (Tile.maxY - (7+coords.y) );

        Debug.Log("minimum sprite order  :" + minimumSpriteOrder);

        int spriteIndex = 0;

        foreach (var rend in GetComponentsInChildren<SpriteRenderer>())
        {
            int i = minimumSpriteOrder *spriteIndex;
            rend.sortingOrder = i;

            Debug.Log(name + " //// " + "coords y : " + coords.y + " / sprite order : " + i);

            ++spriteIndex;
        }
    }

}

[System.Serializable]
public struct Coords
{

    public int x;
    public int y;

    public Coords(int x, int y)
    {
        this.x = x;
        this.y = y;
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

    // string
    public override string ToString()
    {
        return "X : " + x + " / Y : " + y;
    }
}
