using UnityEngine;
using System.Collections;
using System;

public class Loot {


	private Item[][] currentLoot = new Item[4][];

	public void Randomize () {
		currentLoot = ItemLoader.Instance.getRandomInventory();
	}

	public Item[] getCategory (ItemCategory category ){
		return currentLoot [(int)category];
	}

	public Item[] getCategory (ItemCategory[] categories){

		int lenght = 0;
		for (int i = 0; i < categories.Length; ++i) {
			lenght += currentLoot [(int)categories [i]].Length;
		}

		Item[] items = new Item[lenght];

		int index = 0;

		foreach ( ItemCategory itemType in categories ) {

			for (int i = 0; i < currentLoot[(int)itemType].Length; ++i ) {
				
				items [index] = currentLoot [(int)itemType] [i];
//				Debug.Log (items[index].name);
				++index;
			}

		}

		return items;
	}

	#region add & remove items
	public void AddItem ( Item newItem ) {

		Item[] newItems = new Item[currentLoot[(int)newItem.category].Length+1];

		for (int i = 0; i < currentLoot [(int)newItem.category].Length; ++i)
			newItems [i] = currentLoot [(int)newItem.category] [i];

		newItems [newItems.Length - 1] = newItem;

		currentLoot [(int)newItem.category] = newItems;
	}

	public void RemoveItem ( Item itemToRemove ) {

		ItemCategory category = itemToRemove.category;
		int index = Array.FindIndex (currentLoot [(int)category], x => x.name.Length == itemToRemove.name.Length);

		Item[] newItems = new Item[currentLoot[(int)category].Length-1];

		int a = 0;
		for (int i = 0; i < currentLoot [(int)category].Length; ++i) {

			if (i != index) {
				newItems [a] = currentLoot [(int)category] [i];
				++a;
			}
		}

		currentLoot [(int)category] = newItems;
	}
	#endregion
}
