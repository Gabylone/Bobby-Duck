using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ItemCategory {
	Provisions,
	Weapon,
	Clothes,
	Misc,
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
	[SerializeField]
	private int[] amountRange_Min;

	[SerializeField]
	private int[] amountRange_Max;

	int[][] levelRange;

	int currentID = 0;

	void Awake () {
		Instance = this;
	}


	public void Init () {

		allCategories = new ItemCategory[categoryAmount];

		for (int i = 0; i < allCategories.Length; ++i ) {

			allCategories [i] = (ItemCategory)i;

		}

		items = new Item[categoryAmount][];
		levelRange = new int[categoryAmount][];

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

		string maxLevelTxt = rows [rows.Length - 2].Split (';')[5];
		int maxLevel = int.Parse (maxLevelTxt);
		levelRange[(int)currentType] = new int[maxLevel+1];

		int currentLevel = 0;

		for ( int i = 1; i < items[(int)currentType].Length+1 ;++i ) {

			string[] cells = rows[i].Split (';');

			int spriteID = -1;
			if ( cells.Length > 6 ) {
				spriteID = int.Parse (cells [6]) - 1;
			}

			Item newItem =
				new Item (
				currentID,
				cells[0],// name
				cells[1],// description
				int.Parse(cells[2]),// value
				int.Parse(cells[3]),// price
				int.Parse(cells[4]),// weight
				int.Parse(cells[5]),// level
				spriteID,

				currentType // category
				);

			items[(int)currentType][i-1] = newItem;

			if ( newItem.level > currentLevel && newItem.level > 0) {

				if ( (int)currentType >= levelRange.Length )
					Debug.LogError ( "Level Range out of range : CURRENT TYPE : " + currentType + " LENGHT : " + levelRange.Length );

				if (  newItem.level-1 >= levelRange[(int)currentType].Length)
					Debug.LogError ( "Level Range out of range : "+ currentType + " ITEM LVL : " + (newItem.level-1) + " LENGHT : " + levelRange[(int)currentType].Length);
				
				levelRange [(int)currentType] [newItem.level-1] = i - 1;
				++currentLevel;
			}

			currentID++;
		}
	}

	#region random items
	// ONLY 1 CATEGORY
	public Item[] getRandomCategoryOfItem ( ItemCategory category , int mult ) {

		int itemType = (int)category;

		int itemAmount = Random.Range (amountRange_Min[itemType],amountRange_Max[itemType]) * mult;

		Item[] tmpItems = new Item[itemAmount];

		// reset mult
		for (int i = 0; i < itemAmount; ++i)
		{
		
			// c'est ici que je dis :  !!! LES OBJETS PEUVENT ETRENT DE TOUS LES NIVEAUX !!!
//			int level = Random.Range ( 0, 11 );
			int level = Random.Range ( 0, Crews.playerCrew.captain.Level + 3 );
			level = Mathf.Clamp (level, 0, 11);

			tmpItems[i] = GetRandomItemOfCertainLevel ((ItemCategory)itemType,level);
		}

		return tmpItems;
	}

	public Item getRandomItem ( ItemCategory category ) {

		int l = items [(int)category].Length;
		int index = Random.Range (0, l);

		return items[(int)category][index];
	}

	public Item GetRandomItemOfCertainLevel ( ItemCategory itemType , int targetLevel = 0 ) {

		int range1 = 0;

		int range2 = items [(int)itemType].Length;

		if ( targetLevel > 0 && targetLevel < levelRange[(int)itemType].Length ) {

			range1 = levelRange [(int)itemType][targetLevel-1];

			if ( levelRange [(int)itemType][targetLevel] > 0 )
				range2 = levelRange [(int)itemType][targetLevel];
			
		}

		int index = Random.Range (range1, range2);

		Item item = items[(int)itemType][index];

		return item;
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

	public Item createItembyName (string name, ItemCategory cat) {

		print (" ITEM : " + name + " doesn't exist, creating new one");

//		Item[] tempItems = items [(int)cat]; 
//
//		items[(int)cat] = new Item[tempItems.Length+1];

		Item newItem = getRandomItem (cat);
		newItem.name = name;

//		items [(int)cat] [tempItems.Length] = newItem;

		return newItem;
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
}

