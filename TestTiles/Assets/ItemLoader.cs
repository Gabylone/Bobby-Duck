using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemLoader : MonoBehaviour {

	public static ItemLoader Instance;

	public List<List<int>> itemAppearRate = new List<List<int>> ();

	public float[] appearRates;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
		LoadVerbAndItemData ();

		LoadItemAppearRate ();

	}

	void LoadVerbAndItemData ()
	{
		TextAsset textAsset = Resources.Load ("Items & Verbs") as TextAsset;
		string[] rows = textAsset.text.Split ('&');

		/// VERBS ///
		LoadVerbs(rows);

		/// ITEMS ///
		LoadItems(rows);
	}

	void LoadItems (string[] rows)
	{
		int itemIndex = 0;
		for (int rowIndex = 1; rowIndex < rows.Length - 1; rowIndex++) {

			Item newItem = new Item ();

			Word itemWord = new Word ();

			string row = rows[rowIndex].TrimStart ('\r', '\n');

			string[] cells = row.Split(';');

//			Debug.Log ("loaded " + cells[0]);

			itemWord.name = cells[0];
			itemWord.UpdateGenre(cells[1]);
			newItem.word = itemWord;
			newItem.row = itemIndex;


			Item.items.Add (newItem);

			++itemIndex;

		}
	}

	void LoadVerbs (string[] rows)
	{
		int itemIndex = 0;

		for (int rowIndex = 0; rowIndex < rows.Length - 1; rowIndex++) {

			string row = rows[rowIndex].TrimEnd ('\r', '\n');

			string[] cells = row.Split (';');

			if (rowIndex == 0) {
				
				for (int cellIndex = 2; cellIndex < cells.Length - 1; cellIndex++) {

					Verb newVerb = new Verb ();

					newVerb.names = cells [cellIndex].Split (new string[] { ", " }, System.StringSplitOptions.None);

//					Debug.Log ("loaded " + newVerb.names [0]);

					Verb.verbs.Add (newVerb);

				}

			} else {

				int verbIndex = 0;

				for (int cellIndex = 2; cellIndex < cells.Length - 1; cellIndex++) {

					if ( cells[cellIndex].Length >= 2 ) {

						if ( verbIndex >= Verb.verbs.Count ) {
							Debug.LogError (verbIndex + " / " + Verb.verbs.Count);
						}
						Verb.verbs [verbIndex].cellContents.Add (itemIndex,cells[cellIndex]);


					}

					++verbIndex;

				}

				++itemIndex;
			}

		}


	}

	void LoadItemAppearRate ()
	{
		TextAsset textAsset = Resources.Load ("Items Appear Rates") as TextAsset;

		string[] rows = textAsset.text.Split ('\n');

		for (int rowIndex = 1; rowIndex < rows.Length - 1; rowIndex++) {

			int itemIndex = rowIndex - 1;

			// handle row
			string row = rows [rowIndex];
			row = row.TrimEnd ('\r', '\n');

			// get item
			if ( itemIndex >= Item.items.Count ) {
				Debug.LogError ( "item index : " + itemIndex + " plus grand que liste items " + Item.items.Count );
			}
			Item item = Item.items [itemIndex];

			// split cells
			string[] cells = row.Split (';');

			if ( cells[1].Length > 0 && cells[1][0] == '/' ) {
				item.usableAnytime = true;
				continue;
			}

			for (int cellIndex = 1; cellIndex < cells.Length; cellIndex++) {

				int tileTypeIndex = cellIndex - 1;

				// get cell
				string cell = cells [cellIndex];

				if (cell.Length == 0) {
//					Debug.Log (item.name + " : la cellule est vide, aucune chances");
					continue;
				}


				Tile.Type tileType = (Tile.Type)tileTypeIndex;

				int appearRate = 0;
				if ( int.TryParse (cell, out appearRate) == false ) {
					Debug.LogError ("APPEAR RATES : la cellule : " + cell + " ne peut pas être parsée");
				} 

				item.appearRates.Add (tileType, appearRate);
			}


		}
	}
}