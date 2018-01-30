using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Loot {

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

	public void Randomize ( ItemCategory[] categories, int mult) {

		// for each categories in cell
		foreach (var category in categories) {
			
			Item[] items = ItemLoader.Instance.getRandomCategoryOfItem (category, mult);

			foreach ( Item item in items ) {
				AddItem (item);
			}

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
//	public List<Item> GetItemsFromCategory ( ItemCategory cat ) {
//		return allItems [(int)cat];
//	}
	public void EmptyCategory ( ItemCategory cat ) {
//		for (int i = 0; i < allItems[(int)cat].Count; i++) {
//			RemoveItem (allItems [(int)cat][i]);
//		}

	}
	#endregion

	public bool IsEmpty ()
	{
		foreach (var item in allItems) {
			if (item.Count > 0)
				return false;
		}

		return true;
	}
}
