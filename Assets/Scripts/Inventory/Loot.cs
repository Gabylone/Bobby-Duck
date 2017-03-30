using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Loot {

	public int id = 0;
	public int row = 0;
	public int col = 0;
	public Item[][] loot = new Item[4][];

	public int weight = 0;

	public Item[][] getLoot {
		get {
			return loot;
		}
	}

	public Loot()
	{
		
	}

	public Loot(int _row, int _col)
	{
		row = _row;
		col = _col;

		loot = new Item[4][];
		for (int i= 0; i < loot.Length; ++i )
			loot[i] = new Item[0];
	}

	public void Randomize ( ItemCategory category ) {
		ItemCategory[] cats = new ItemCategory [1] { category };
		Randomize (cats);
	}

	public void Randomize ( ItemCategory[] categories ) {
		foreach ( Item[] items in ItemLoader.Instance.getRandomLoot (categories) ) {
			foreach ( Item item in items )
				AddItem (item);
		}
	}

	#region loot handling
	public Item[] getCategory (ItemCategory category ){
		return loot [(int)category];
	}

	public Item[] getCategory (ItemCategory[] categories){

		int lenght = 0;
		for (int i = 0; i < categories.Length; ++i) {
			lenght += loot [(int)categories [i]].Length;
		}

		Item[] items = new Item[lenght];

		int index = 0;

		foreach ( ItemCategory itemType in categories ) {

			for (int i = 0; i < loot[(int)itemType].Length; ++i ) {
				
				items [index] = loot [(int)itemType] [i];
				++index;
			}

		}

		return items;
	}

	public int[] getCategoryIDs (ItemCategory[] categories ){


		int lenght = 0;
		for (int i = 0; i < categories.Length; ++i)
			lenght += loot [(int)categories [i]].Length;

		int[] ids = new int[ lenght ];

		int index = 0;

		foreach ( ItemCategory itemType in categories ) {

			for (int i = 0; i < loot[(int)itemType].Length; ++i ) {

				ids [index] = loot [(int)itemType] [i].ID;
				++index;
			}

		}

		return ids;
	}
	#endregion

	#region add & remove items
	public void AddItem ( Item newItem ) {

		Item[] newItems = new Item[loot[(int)newItem.category].Length+1];

		for (int i = 0; i < loot [(int)newItem.category].Length; ++i)
			newItems [i] = loot [(int)newItem.category] [i];

		newItems [newItems.Length - 1] = newItem;

		loot [(int)newItem.category] = newItems;

		weight += newItem.weight;
		WeightManager.Instance.UpdateDisplay ();
	}

	public void RemoveItem ( Item itemToRemove ) {

		ItemCategory category = itemToRemove.category;

		int index = Array.FindIndex (loot [(int)category], x => x.ID == itemToRemove.ID);

		Item[] newItems = new Item[loot[(int)category].Length-1];

		int a = 0;
		for (int i = 0; i < loot [(int)category].Length; ++i) {

			if (i != index) {
				newItems [a] = loot [(int)category] [i];
				++a;
			}
		}

		loot [(int)category] = newItems;

		weight -= itemToRemove.weight;
		WeightManager.Instance.UpdateDisplay ();



	}
	#endregion


}
