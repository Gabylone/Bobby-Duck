using UnityEngine;
using System.Collections;

public enum ItemCategory {
	Provisions,
	Weapon,
	Clothes,
	Shoes,
	Mics,
}

public class ItemLoader : MonoBehaviour {

	public static ItemLoader Instance;


	[SerializeField]
	private int categoryAmount = 5;

	ItemCategory currentType = ItemCategory.Provisions;
	public static ItemCategory[] allCategories;

	[SerializeField]
	private TextAsset[] files;

	private Item[][] items;

	[Header("Chances")]
	[SerializeField] private float[] items_AppearChance;
	[SerializeField] private int[] items_MaxPerLoot;

	int currentID = 0;

	public void Init () {
		
		Instance = this;

		allCategories = new ItemCategory[categoryAmount];

		for (int i = 0; i < allCategories.Length; ++i ) {

			allCategories [i] = (ItemCategory)i;

		}

		items = new Item[categoryAmount][];

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
				currentID,
				cells[0],// name
				cells[1],// description
				int.Parse(cells[2]),// value
				int.Parse(cells[3]),// price
				int.Parse(cells[4]),// weight

				currentType // category
				);

//			Debug.Log (newItem.name);
			items[(int)currentType][i-1] = newItem;

			currentID++;
		}


	}

	#region random items
	public Item[][] getRandomLoot ( ItemCategory[] categories ) {

		Item[][] randomItems = new Item[categoryAmount][];
		for (int i = 0; i < randomItems.Length; ++i ) {
			randomItems[i] = new Item[0];
		}

		// get item amount
		foreach ( ItemCategory cat in categories ) {

			int itemType = (int)cat;
			
			int itemAmount = Random.Range (1, items_MaxPerLoot [itemType]+1);

			randomItems [itemType] = new Item[itemAmount];

			for (int i = 0; i < itemAmount; ++i)
			{
				randomItems [itemType] [i] = getRandomItem ((ItemCategory)itemType);
			}

		}

		return randomItems;
	}

	public Item getRandomItem ( ItemCategory itemType ) {
		
		int index = Random.Range (0, items [(int)itemType].Length);

		return items[(int)itemType][index];
	} 

	public Item[] getItems ( ItemCategory itemType ) {
		return items[(int)itemType];
	} 

	public Item getItem ( ItemCategory itemType , int itemID ) {
		return items[(int)itemType][itemID];
	}

	public Item[][] Items {
		get {
			return items;
		}
	}
	#endregion

}

