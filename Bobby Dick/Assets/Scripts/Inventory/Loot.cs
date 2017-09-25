using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Loot {

	public int id = 0;
	public int row = 0;
	public int col = 0;

	public List<List<Item>> allItems = new List<List<Item>>();

	public int weight = 0;

	public Loot()
	{
		
	}

	public Loot(int _row, int _col)
	{
		row = _row;
		col = _col;

		for (int i = 0; i < 4; i++) {
			allItems.Add (new List<Item> ());
		}

	}

	public void Randomize ( ItemCategory[] categories ) {

		foreach ( Item[] items in ItemLoader.Instance.getRandomLoot (categories) ) {
			foreach ( Item item in items )
				AddItem (item);
		}

	}

	#region add & remove items
	public void AddItem ( Item newItem ) {

		allItems [(int)newItem.category].Add (newItem);

		weight += newItem.weight;

		if ( LootManager.Instance.updateLoot != null )
			LootManager.Instance.updateLoot ();

	}

	public void RemoveItem ( Item itemToRemove ) {

		allItems [(int)itemToRemove.category].Remove (itemToRemove);

		weight -= itemToRemove.weight;

		if ( LootManager.Instance.updateLoot != null )
			LootManager.Instance.updateLoot ();

	}
	#endregion


}
