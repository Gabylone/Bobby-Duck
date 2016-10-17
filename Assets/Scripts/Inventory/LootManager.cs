using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	private Item[][] currentLoot = new Item[4][];

	void Awake () {
		Instance = this;
		currentLoot = ItemLoader.Instance.getRandomInventory();
	}

	void Start () {
		
	}

	public Item[] getLoot (ItemLoader.ItemType itemType ){
		return currentLoot [(int)itemType];
	}

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
}
