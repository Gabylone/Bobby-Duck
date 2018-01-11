using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LootManager : MonoBehaviour {

	public static LootManager Instance;

	[SerializeField]
	private CategoryContent defaultCategoryContent;
	[SerializeField]
	private CategoryContent tradeCategoryContent_Player;
	[SerializeField]
	private CategoryContent tradeCategoryContent_Other;

	[SerializeField]
	private CategoryContent lootCategoryContent_Player;
	[SerializeField]
	private CategoryContent lootCategoryContent_Other;

	[SerializeField]
	private CategoryContent tradeCategoryContent_Combat;

	[SerializeField]
	private CategoryContent inventoryCategoryContent;

	public delegate void UdpateLoot();
	public UdpateLoot updateLoot;

	private Loot playerLoot;
	private Loot otherLoot;

	[SerializeField]
	private Sprite[] foodSprites;

	[SerializeField]
	private Sprite[] weaponSprites;

	[SerializeField]
	private Sprite[] clotheSprites;

	[SerializeField]
	private Sprite[] miscSprites;

	void Awake (){
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.AddToInventory:
			AddToInventory ();
			break;
		case FunctionType.RemoveFromInventory:
			RemoveFromInventory ();
			break;
		case FunctionType.CheckInInventory:
			CheckInInventory ();
			break;
		}
	}



	public void CreateNewLoot () {
		Loot playerLoot = new Loot (0, 0);
		playerLoot.Randomize (new ItemCategory[1] {ItemCategory.Provisions},1);
//		playerLoot.Randomize (ItemLoader.allCategories,10);

		setLoot (Crews.Side.Player, playerLoot);
	}

	public Loot PlayerLoot {
		get {
			return playerLoot;
		}
	}

	public Loot OtherLoot {
		get {
			return otherLoot;
		}
	}

	public Loot getLoot (Crews.Side side) {
		return side == Crews.Side.Player ? playerLoot : otherLoot;
	}

	public void setLoot ( Crews.Side side , Loot targetLoot) {
		if (side == Crews.Side.Player) {
			playerLoot = targetLoot;
		} else {
			otherLoot = targetLoot;
		}
	}

	public Loot GetIslandLoot () {
		return GetIslandLoot (1);
	}
	public Loot GetIslandLoot (int mult) {

		int row = StoryReader.Instance.Row;
		int col = StoryReader.Instance.Col;

		var tmpLoot = StoryReader.Instance.CurrentStoryHandler.GetLoot (row, col);

		if (tmpLoot == null) {

			Loot newLoot = new Loot (row , col);

			ItemCategory[] categories = getLootCategoriesFromCell ();

			newLoot.Randomize (categories,mult);

			StoryReader.Instance.CurrentStoryHandler.SetLoot (newLoot);

			return newLoot;

		}

		return tmpLoot;
	}

	public ItemCategory[] getLootCategoriesFromCell () {

		string cellParams = StoryFunctions.Instance.CellParams;

		if ( cellParams.Length < 2 ) {
			return ItemLoader.allCategories;
		}

		string[] cellParts = cellParams.Split ('/');

		ItemCategory[] categories = new ItemCategory[cellParts.Length];

		int index = 0;

		foreach ( string cellPart in cellParts ) {

			categories [index] = getLootCategoryFromString(cellPart);

			++index;
		}

		return categories;
	}

	public ItemCategory getLootCategoryFromString ( string arg ) {

		switch (arg) {
		case "Food":
			return ItemCategory.Provisions;
			break;
		case "Weapons":
			return ItemCategory.Weapon;
			break;
		case "Clothes":
			return ItemCategory.Clothes;
			break;
		case "Misc":
			return ItemCategory.Misc;
			break;
		}

		Debug.LogError ("getLootCategoryFromString : couldn't find category in : " + arg);

		return ItemCategory.Misc;

	}

	#region item
	public delegate void OnRemoveItemFromInventory (Item item);
	public static OnRemoveItemFromInventory onRemoveItemFromInventory;
	void RemoveFromInventory () {

		string cellParams = StoryFunctions.Instance.CellParams;

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);
		StoryReader.Instance.NextCell ();

		if ( LootManager.Instance.getLoot(Crews.Side.Player).allItems [(int)targetCat].Count == 0 ) {
			Debug.LogError ( "REMOVE IN INVENTORY : la catégorie visée est vide : ignorement" );
			StoryReader.Instance.UpdateStory ();
			return;
		}

		Item item = LootManager.Instance.getLoot(Crews.Side.Player).allItems [(int)targetCat] [0];
		if (cellParams.Contains ("<")) {
			string itemName = cellParams.Split ('<') [1];
			itemName = itemName.Remove (itemName.Length - 6);
			item = LootManager.Instance.getLoot (Crews.Side.Player).allItems [(int)targetCat].Find (x => x.name == itemName);
		}

		if (onRemoveItemFromInventory != null) {
			onRemoveItemFromInventory (item);
		}

		LootManager.Instance.getLoot(Crews.Side.Player).RemoveItem (item);

	}

	public delegate void OnAddToInventory (Item item);
	public static OnAddToInventory onAddToInventory;
	void AddToInventory () {

		string cellParams = StoryFunctions.Instance.CellParams;

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		Item item = null;

		if (cellParams.Contains ("<")) {
			string itemName = cellParams.Split ('<') [1];
			itemName = itemName.Remove (itemName.Length - 6);
			item = System.Array.Find (ItemLoader.Instance.getItems (targetCat), x => x.name == itemName);

			if (item == null) {
				Debug.LogError ("item : " + itemName + " was not found, returning random");
				item = ItemLoader.Instance.getRandomItem (targetCat);
			}
				
		} else {
			item = ItemLoader.Instance.getRandomItem (targetCat);
		}

		if (onAddToInventory != null) {
			onAddToInventory (item);
		}

		getLoot(Crews.Side.Player).AddItem (item);
	}

	void CheckInInventory () {
		
		string cellParams = StoryFunctions.Instance.CellParams;

		StoryReader.Instance.NextCell ();

		ItemCategory targetCat = getLootCategoryFromString (cellParams.Split('/')[1]);

		if (cellParams.Contains ("<")) {
			
			string itemName = cellParams.Split ('<') [1];
			itemName = itemName.Remove (itemName.Length - 6);

			Item item = LootManager.Instance.getLoot (Crews.Side.Player).allItems [(int)targetCat].Find (x => x.name == itemName);

			if (item == null) {
				StoryReader.Instance.SetDecal (1);
			}

		} else {
			if (LootManager.Instance.getLoot (Crews.Side.Player).allItems [(int)targetCat].Count == 0) {
				StoryReader.Instance.SetDecal (1);
			}
		}

		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	public CategoryContent GetCategoryContent (CategoryContentType catContentType) {
		switch (catContentType) {
		case CategoryContentType.Inventory:
			return inventoryCategoryContent;
			break;

		case CategoryContentType.OtherLoot:
			return lootCategoryContent_Other;
			break;
		case CategoryContentType.PlayerLoot:
			return lootCategoryContent_Player;
			break;
		case CategoryContentType.PlayerTrade:
			return tradeCategoryContent_Player;
			break;
		case CategoryContentType.OtherTrade:
			return tradeCategoryContent_Other;
			break;
		case CategoryContentType.Combat:
			return tradeCategoryContent_Combat;
			break;
		}
		print ("category content reached zero");
		return defaultCategoryContent;
	}

	public delegate void OnWrongLevelEvent ();
	public OnWrongLevelEvent onWrongLevelEvent;

	public void OnWrontLevel ()
	{
		if ( onWrongLevelEvent != null ) {
			onWrongLevelEvent ();
		}
	}

	public Sprite getItemSprite (ItemCategory cat,int id) {

		switch (cat) {
		case ItemCategory.Provisions:

			if (foodSprites.Length == 0) {
				return null;
			}

			return foodSprites [id];

			break;
		case ItemCategory.Weapon:

			if (weaponSprites.Length == 0) {
				return null;
			}

			return weaponSprites [id];

			break;
		case ItemCategory.Clothes:

			if (clotheSprites.Length == 0) {
				return null;
			}

			return clotheSprites [id];

			break;
		case ItemCategory.Misc:

			if (miscSprites.Length == 0) {
				return null;
			}

			return miscSprites [id];

			break;
		default:

			if (miscSprites.Length == 0) {
				return null;
			}

			return miscSprites [id];

			break;
		}
	}
}


public enum CategoryContentType {
	PlayerTrade,
	OtherTrade,
	Inventory,
	PlayerLoot,
	OtherLoot,
	Combat,
}
