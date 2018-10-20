using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSet {

    public static TileSet current;
    public static TileSet map;

    public Coords playerCoords = Coords.Zero;

    public List<Container> containers = new List<Container>();

    public Dictionary<Coords, Tile> tiles = new Dictionary<Coords, Tile> ();

	public void Add (Coords c, Tile newTile)
	{
		tiles.Add (c, newTile);
	}

    public static void Set (TileSet _tileSet)
    {
        current = _tileSet;
    }

    public Tile GetTile(Coords coords)
    {
        if (tiles.ContainsKey(coords) == false)
        {
            return null;
        }

        return tiles[coords];
    }

    public void RemoveAt (Coords c)
    {
        tiles.Remove(c);
        MapTexture.Instance.Paint(c, Color.black);
    }

}

public class Tile {

	public Coords coords;
	public Type type;

	public bool visited = false;

	public static Tile current;
    public static Tile previous;
    public bool locked = false;

    public static bool itemsChanged = false;

	public static bool SameAsPrevious (){

		if (previous == null)
			return false;

		return Tile.current.type == Tile.previous.type;
	}

	public List<Item> items = new List<Item>();
    public List<Container> containers = new List<Container>();

	public Word word;
	public List<Adjective> adjs = new List<Adjective>();
	public Tile ( Coords coords ) {
		this.coords = coords;
	}

	public List<Adjective> GetAdjectives () {
		return adjs;
	}
	void SetRandomAdjectives(){
		
		//int amount = Random.value < 0.3f ? 2 : 1;
		int amount = 1;

		adjs.Clear ();

		for (int i = 0; i < amount; i++) {
			Adjective newAdj = Adjective.GetRandom (word.adjType);
			adjs.Add (newAdj);
		}
	}

	public void SetType ( Type type ) {
		
		this.type = type;

        MapTexture.Instance.Paint (coords, type);

        word = Location.GetLocation(type).word;

		SetRandomAdjectives ();
	}

	public void SetType ( Type type , Adjective adj ) {
		
		SetType (type);

		if (adjs.Count == 0)
			adjs.Add (adj);
		else
			adjs [0] = adj;

	}

	public static List<Tile> GetSurrouringTiles (Tile tile, int range)
	{
		List<Tile> tiles = new List<Tile> ();

		for (int x = -range; x <= range; x++) {
			for (int y = -range; y <= range; y++) {

				Coords c = tile.coords + new Coords (x, y);

				if ( c == tile.coords || c.OutOfMap() )
					continue;

				tiles.Add (TileSet.current.GetTile(c) );

			}
		}

		return tiles;
	}

	#region items
	public void GenerateItems () {

		foreach (var item in Item.items) {

            Item.AppearRate appearRate = item.appearRates.Find(x => x.type == Item.AppearRate.Type.Tile && x.id == (int)type);

			if ( appearRate != null ) {

                for (int i = 0; i < appearRate.amount; i++)
                {
                    if (Random.value * 100f < appearRate.rate)
                    {
                        Item newItem = item;

                        items.Add(newItem);

                    }
                }

			}

		}

	}
    public void RemoveItem( Item item )
    {
        items.Remove(item);

        itemsChanged = true;

    }
    public void AddItem ( Item item)
    {
        items.Add(item);

        //itemsChanged= true;
    }
    #endregion

    #region get description
    public string GetDescription()
    {
        return word.GetDescription(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular, TextColor.None)
            + " "
            + GetAdjectives()[0].GetName(word.genre, Word.Number.Singular);
    }
    #endregion

    #region tile set
    #endregion

    public enum Type {

		None,

		// plains
		Plain,
		Field,
		Clearing,

		// hills
		Hill,
		Mountain,

		// forests
		Forest,
		Woods,

		// water
		Sea,
		Lake,
		River,
		Beach,

		// roads
		Road,
		TownRoad,
		CoastalRoad,
		Path,
		Bridge,

		// houses
		TownHouse,
		Farm,
		ForestCabin,
		CountryHouse,

		// Interiors
		Hallway,
		Stairs,

		LivingRoom,
		Kitchen,
		DiningRoom,
		ChildBedroom,
		Bedroom,
		Bathroom,
		Toilet,
		Attic,
		Basement,
		Cellar,

	}
}

