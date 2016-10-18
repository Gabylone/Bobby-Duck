using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	private Item[][] currentLoot = new Item[4][];

	public void Randomize () {
		currentLoot = ItemLoader.Instance.getRandomInventory();
	}

	public Item[] getCategory (ItemLoader.ItemType itemType ){
		return currentLoot [(int)itemType];
	}

	#region add & remove items
	public void AddItem ( ItemLoader.ItemType category, Item newItem ) {

		Item[] newItems = new Item[currentLoot[(int)category].Length+1];

		for (int i = 0; i < currentLoot [(int)category].Length; ++i)
			newItems [i] = currentLoot [(int)category] [i];

		newItems [newItems.Length - 1] = newItem;

		currentLoot [(int)category] = newItems;
	}

	public void RemoveItem ( ItemLoader.ItemType category, int index ) {

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
