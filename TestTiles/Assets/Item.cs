using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {


	public static List<Item> items = new List<Item>();

	public int row;

	public Word word;

	public bool usableAnytime = false;

	public Dictionary<Tile.Type,int> appearRates = new Dictionary<Tile.Type, int>();

	public static Item FindInTile ( string str ) {
		return Tile.current.items.Find (x => x.word.name == str );
	}

	public static Item FindUsableAnytime ( string str ) {
		return items.Find (x => x.word.name == str && x.usableAnytime);
	}
}

