using UnityEngine;
using System.Collections;

public class ItemLoader : MonoBehaviour {

	public static ItemLoader Instance;

	public enum ItemType {
		Provisions,
		Weapon,
		Clothes,
		Mics,

		None,
		Any,
	}
	ItemType currentType = ItemType.Provisions;

	[SerializeField]
	private TextAsset[] files;

	private Item[][] items = new Item[4][];

	[Header("Chances")]
	[SerializeField] private float[] items_AppearChance;
	[SerializeField] private int[] items_MaxPerLoot;

	void Awake () {
		
		Instance = this;

		foreach ( TextAsset file in files ) {
			LoadItems (file);
			++currentType;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadItems (TextAsset data) {

		string[] rows = data.text.Split ('\n');

		items[(int)currentType] = new Item[rows.Length-2];

		for ( int i = 1; i < items[(int)currentType].Length+1 ;++i ) {

			string[] cells = rows[i].Split (';');
			Item newItem =
				new Item (
				i-1,
				cells[0],// name
				cells[1],// description
				int.Parse(cells[2]),// value
				int.Parse(cells[3])// price
				);

//			Debug.Log (newItem.name);
			items[(int)currentType][i-1] = newItem;
		}


	}

	public Item[][] getRandomInventory () {

		Item[][] randomItems = new Item[4][];

		// get item amount
		for (int itemType = 0; itemType < 4; ++itemType ) {
			if (Random.value * 100 < items_AppearChance [itemType]) {
				int itemAmount = Random.Range (1, items_MaxPerLoot [itemType]+1);
				randomItems [itemType] = new Item[itemAmount];
				for (int i = 0; i < itemAmount; ++i) {
					randomItems [itemType] [i] = getRandomItem ((ItemType)itemType);
				}
			} else {
				randomItems [itemType] = new Item[0];
			}
		}

		return randomItems;
	}

	public Item[] getRandomLoot () {

		int globalAmount = 0;
		int[] itemAmounts = new int[4];

		// get item amount
		for (int itemType = 0; itemType < 4; ++itemType ) {
			if ( Random.value * 100 < items_AppearChance[itemType] ) {
				int itemAmount = Random.Range (1,items_MaxPerLoot[itemType]+1);
				itemAmounts[itemType] = itemAmount;
				globalAmount += itemAmount;
			}
		}

		Item[] loot = new Item[globalAmount];

		int a = 0;
		for (int itemType = 0; itemType < 4; ++itemType ) {
			for (int i = 0; i < itemAmounts[itemType]; ++i )
				loot[a] = getRandomItem((ItemType)itemType);
		}

		return loot;
	}

	public Item getRandomItem ( ItemType itemType ) {
		
		int index = Random.Range (0, items [(int)itemType].Length);

		// Debug.Log ("random item : " + items [(int)itemType] [index].name);

		return items[(int)itemType][index];
	} 

	public Item[] getItems ( ItemType itemType ) {
		return items[(int)itemType];
	} 

	public Item getItem ( ItemType itemType , int itemID ) {
		return items[(int)itemType][itemID];
	}

	public Item[][] Items {
		get {
			return items;
		}
	}

}
