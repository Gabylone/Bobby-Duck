using UnityEngine;
using System.Collections;

public class ItemLoader : MonoBehaviour {

	public static ItemLoader Instance;

	public enum ItemType {
		Provisions,
		Weapon,
		Clothes,
		Mics
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

		items[(int)currentType] = new Item[rows.Length-1];

		for ( int i = 1; i < rows.Length-1 ;++i ) {

			string[] cells = rows[i].Split (';');
			Item newItem =
				new Item (
				i-1,
				cells[0],// name
				cells[1],// description
				int.Parse(cells[2]),// value
				int.Parse(cells[3])// price
				);

			items[(int)currentType][i-1] = newItem;
		}

	}

	public Item[] getRandomLoot () {

		int globalAmount = 0;
		int[] itemAmounts = new int[4];

		// get item amount
		for (int itemType = 0; itemType < 4; ++itemType ) {
			if ( Random.value * 100 < items_AppearChance[itemType] ) {
				int itemAmount = Random.Range (1,items_MaxPerLoot[itemType]);
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
		return items[(int)itemType][Random.Range (0,items[(int)itemType].Length)];
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
