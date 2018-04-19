using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSet {
	
	public Dictionary<Coords, Tile> tiles = new Dictionary<Coords, Tile> ();

	public void Add (Coords c, Tile newTile)
	{
		tiles.Add (c, newTile);
	}
}

public class Tile {

	public Coords coords;
	public Type type;

	public List<Item> items = new List<Item>();

	public static TileSet currentTileSet;

	public Tile ( Coords coords ) {
		this.coords = coords;
	}

	public static Tile current {
		get {
			return Tile.GetTile (Player.Instance.coords);
		}
	}

	public void SetType ( Type type ) {
		this.type = type;
		MapTexture.Instance.Paint (coords, type);
	}

	public static List<Tile> GetSurrouringTiles (Tile tile, int range)
	{
		List<Tile> tiles = new List<Tile> ();

		for (int x = -range; x <= range; x++) {
			for (int y = -range; y <= range; y++) {

				Coords c = tile.coords + new Coords (x, y);

				if (c == tile.coords || c.OutOfMap())
					continue;

				tiles.Add (GetTile (c));

			}
		}

		return tiles;
	}

	#region items
	public void GenerateItems () {

		foreach (var item in Item.items) {

			if (item.appearRates.ContainsKey (type)) {

				if ( Random.value * 100f < item.appearRates[type] ) {
					items.Add (item);
				}

			}

		}

	}
	#endregion

	#region tile set

	public static TileSet GetCurrentTileSet {
		get {
			return currentTileSet;
		}
	}

	public static void SetTileSet ( TileSet tileSet ) {
		currentTileSet = tileSet;
	}

	public static Tile GetTile ( Coords coords ) {
		if (currentTileSet.tiles.ContainsKey (coords) == false) {
//			Debug.LogError ("dico does not contain coords : " + coords.ToString ());
			return null;
		}

		return currentTileSet.tiles [coords];
	}
	#endregion

	public Word word {
		get {
			return Word.GetLocationWord (type);
		}
	}

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

