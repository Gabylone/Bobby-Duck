using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Tilable {

    /*public static Sprite[] tileSprites;

    public static Sprite[] GetTileSprites()
    {
        if ( tileSprites == null)
        {
            tileSprites = Resources.LoadAll<Sprite>("Graphs/tiles");
        }

        return tileSprites;
    }*/

    public static Dictionary<Coords, Tile> tiles = new Dictionary<Coords, Tile>();

    public static int maxY = 0;
    public static int minY = 0;
    public static int maxX = 0;
    public static int minX = 0;

    public override void Start()
    {
        base.Start();

        if (tiles.ContainsKey (coords) == false )
            tiles.Add(coords, this);


        /*GetComponentInChildren<SpriteRenderer>().sprite = GetTileSprites()[Random.Range(0, GetTileSprites().Length)];

        if ( Random.value < 0.5f)
        {
            transform.localScale = new Vector3(1,-1,1);
        }*/
    }

}
