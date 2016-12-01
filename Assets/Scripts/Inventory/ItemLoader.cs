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
	private string pathToCSVs = "Items/CSVs";
	private TextAsset[] files;

	private Item[][] items;

	[Header("Chances")]
	[SerializeField] private float[] items_AppearChance;
	[SerializeField] private int[] items_MaxPerLoot;

	int[][] levelRange;

	int currentID = 0;

	public int levelTest = 1;

	public void Init () {
		
		Instance = this;

		allCategories = new ItemCategory[categoryAmount];

		for (int i = 0; i < allCategories.Length; ++i ) {

			allCategories [i] = (ItemCategory)i;

		}

		items = new Item[categoryAmount][];
		LevelRange = new int[categoryAmount][];

		files = new TextAsset[Resources.LoadAll (pathToCSVs, typeof(TextAsset)).Length];
		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (pathToCSVs, typeof(TextAsset) )) {
			files[index] = textAsset;
			++index;
		}

		foreach ( TextAsset file in files ) {
			LoadItems (file);
			++currentType;
		}
	}

	void LoadItems (TextAsset data) {

		string[] rows = data.text.Split ('\n');

		items[(int)currentType] = new Item[rows.Length-2];

		string maxLevelTxt = rows [rows.Length - 3].Split (';')[5];
		int maxLevel = int.Parse (maxLevelTxt);
		LevelRange[(int)currentType] = new int[maxLevel+1];

		int currentLevel = 0;

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
				int.Parse(cells[5]),// level

				currentType // category
				);

			items[(int)currentType][i-1] = newItem;

			if ( newItem.level > currentLevel && newItem.level > 0) {
				LevelRange [(int)currentType] [newItem.level-1] = i - 1;
				++currentLevel;
			}

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

			int maxLevel = 1;
			if ( Crews.playerCrew != null )
				maxLevel = Crews.playerCrew.captain.Level + Random.Range (-1, 2);
			
			maxLevel = Mathf.Clamp ( maxLevel , 1 , 1000 );
			for (int i = 0; i < itemAmount; ++i)
			{
				randomItems [itemType] [i] = getRandomItemMaxLevel ((ItemCategory)itemType, maxLevel);
			}

		}

		return randomItems;
	}

	public Item getRandomItem ( ItemCategory itemType ) {

		int l = items [(int)itemType].Length;
		int index = Random.Range (0, l);

		return items[(int)itemType][index];
	}

	public Item getRandomItemMaxLevel ( ItemCategory itemType , int maxLevel = 0 ) {

		int l = items [(int)itemType].Length;

		if ( maxLevel > 0 && maxLevel < LevelRange[(int)itemType].Length ) {
			if ( LevelRange [(int)itemType][maxLevel] > 0 )
				l = LevelRange [(int)itemType][maxLevel];
		}


		int index = Random.Range (0, l);

		return items[(int)itemType][index];
	}

	public Item getRandomItemSpecLevel ( ItemCategory itemType , int level = 0 ) {

		int range1 = 0;
		int range2 = items [(int)itemType].Length;
		if ( level > 0 && level < LevelRange[(int)itemType].Length ) {
			range1 = LevelRange [(int)itemType][level-1];
			if ( LevelRange [(int)itemType][level] > 0 )
				range2 = LevelRange [(int)itemType][level];
		}

		int index = Random.Range (range1, range2);

		return items[(int)itemType][index];
	}

	public int getRandomIDSpecLevel ( ItemCategory itemType , int level = 0 ) {

		int range1 = 0;
		int range2 = items [(int)itemType].Length;
		if ( level > 0 && level < LevelRange[(int)itemType].Length ) {
			range1 = LevelRange [(int)itemType][level-1];
			if ( LevelRange [(int)itemType][level] > 0 )
				range2 = LevelRange [(int)itemType][level];
		}

		return Random.Range (range1, range2);
	}

	public Item[] getItems ( ItemCategory itemType ) {
		return items[(int)itemType];
	} 

	public Item getItem ( ItemCategory itemType , int itemID ) {
		if ( itemID >= items[(int)itemType].Length ) {
			Debug.LogError ( "item id out of range " + " ID : " + itemID + " / cat : " + itemType + " L : " + items[(int)itemType].Length );
			return items [(int)itemType][0];
		}
		return items[(int)itemType][itemID];
	}

	public Item[][] Items {
		get {
			return items;
		}
	}
	#endregion

	public int CategoryAmount {
		get {
			return categoryAmount;
		}
	}

	public int[][] LevelRange {
		get {
			return levelRange;
		}
		set {
			levelRange = value;
		}
	}
}

