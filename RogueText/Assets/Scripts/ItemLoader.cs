using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemLoader : MonoBehaviour {

	public static ItemLoader Instance;

    public int containerLimit = 0;

	void Awake () {
		Instance = this;
	}

    // Use this for initialization
    void Start()
    {

        LoadVerbAndItemData();

        string[] placeAppearRatesRows =
            GetRows(new TextAsset[2] {

        Resources.Load("Items Appear Rates") as TextAsset,

        Resources.Load("Containers Appear Rates") as TextAsset });

        LoadItemAppearRate(placeAppearRatesRows, Item.AppearRate.Type.Tile);

        string[] containerAppearRatesRows = GetRows(Resources.Load("Item Rates In Containers") as TextAsset);

        LoadItemAppearRate(containerAppearRatesRows, Item.AppearRate.Type.Container);

        LoadItemPositions();


    }

    private void LoadItemPositions()
    {
        TextAsset textAsset_ItemPositions = Resources.Load("Items_ItemPositions") as TextAsset;
        string[] rows_ItemPositions = textAsset_ItemPositions.text.Split('\n');

        string[] itemPositions = rows_ItemPositions[0].Split(';');

        int rowIndex = 0;

        foreach (var item in Item.items)
        {
            string[] cells = rows_ItemPositions[rowIndex].Split(';');

            for (int cellIndex = 1; cellIndex < cells.Length; cellIndex++)
            {
                if (cells[cellIndex].Contains("1"))
                {
                    //Debug.Log("pour l'item : " + item.word.name + " la phrase : " + itemPositions[cellIndex]);

                    item.itemPositions.Add(itemPositions[cellIndex]);
                }
            }

            ++rowIndex;

        }
    }

    void LoadVerbAndItemData ()
	{
        string[] rows = GetRows(
            new TextAsset[2] {
        Resources.Load("Items & Verbs") as TextAsset,
        Resources.Load("Containers & Verbs") as TextAsset
            });

        /// ITEMS ///
        LoadItems(rows);

        /// VERBS ///
        LoadVerbs(rows);
	}

	void LoadItems (string[] rows)
	{
		int itemIndex = 0;
		for (int rowIndex = 3; rowIndex < rows.Length; rowIndex++) {


			Item newItem = new Item ();

			Word itemWord = new Word ();

			string row = rows[rowIndex].TrimStart ('\r', '\n');

            string[] cells = row.Split(';');

			itemWord.name = cells[0];

			itemWord.UpdateGenre(cells[1]);

            if (cells[2].Length > 0)
            {
                int weight = 1;

                if (int.TryParse(cells[2] , out weight) == false)
                {
                    Debug.Log("item weight : " + cells[2] + " does not parse");
                }

                newItem.weight = weight;
            }

            if (cells[3].Length > 0)
            {
                int param = 1;

                if ( int.TryParse(cells[3],out param) == false )
                {
                    Debug.Log("item parameter : " + cells[3] + " does not parse");
                }

                newItem.value = param;
            }

            // word
            newItem.word = itemWord;
			newItem.row = itemIndex;

            // adjective
            /*newItem.adjective = Adjective.GetRandom(Adjective.Type.Item);*/

			Item.items.Add (newItem);

			++itemIndex;

		}
	}

	void LoadVerbs (string[] rows)
	{
		int itemIndex = 0;

        for (int rowIndex = 0; rowIndex < rows.Length ; rowIndex++) {

			string row = rows[rowIndex].TrimEnd ('\r', '\n');
            string[] cells = row.Split (';');

			if (rowIndex < 3) {

                int verbIndex = 0;

				for (int cellIndex = 4; cellIndex < cells.Length; cellIndex++) {

                    // create verbs
                    if ( rowIndex == 0)
                    {
                        Verb newVerb = new Verb();

                        newVerb.names = cells[cellIndex].Split(new string[] { ", " }, System.StringSplitOptions.None);

                        Verb.verbs.Add(newVerb);
                    }
                    // assign questions
                    else if (rowIndex == 1)
                    {
                        Verb.verbs[verbIndex].question = cells[cellIndex];
                    }
                    // assign help phrase
                    else if (rowIndex == 2)
                    {
                        Verb.verbs[verbIndex].helpPhrase = cells[cellIndex];
                    }

                    verbIndex++;

                }

            }
            else {

				int verbIndex = 0;

				for (int cellIndex = 4; cellIndex < cells.Length; cellIndex++) {

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

    private string[] GetRows(TextAsset[] textAssets)
    {
        List<string> tmpRows = new List<string>();

        int textAssetIndex = 0;

        foreach (var textAsset in textAssets)
        {
            string[] rows;

            if (textAssetIndex == 0)
            {
                rows = GetRows(textAsset);
                containerLimit = rows.Length;
            }
            else
            {
                rows = GetRows(textAsset, ParseType.CropFirstLign);
            }

            foreach (var item in rows)
            {
                tmpRows.Add(item);
            }

            ++textAssetIndex;
        }

        return tmpRows.ToArray();
    }

    public enum ParseType
    {
        Normal,
        CropFirstLign,
    }

    private string[] GetRows ( TextAsset textAsset)
    {
        return GetRows(textAsset, ParseType.Normal);
    }
    private string[] GetRows ( TextAsset textAsset , ParseType parseType )
    {
        List<string> tmpRows = new List<string>();

        int a = 0;

        string[] split1 = textAsset.text.Split('&');

        foreach (var item in split1)
        {
            if ( a < 1 && parseType == ParseType.CropFirstLign)
            {
                ++a;
                continue;
            }

            if (a < split1.Length - 1)
            {
                tmpRows.Add(item);
            }

            ++a;
        }

        return tmpRows.ToArray();
    }

    void LoadItemAppearRate(string[] rows, Item.AppearRate.Type type )
    {
        for (int rowIndex = 1; rowIndex < rows.Length - 1; rowIndex++)
        {
            // handle row
            string row = rows[rowIndex];
            row = row.TrimEnd('\r', '\n');

            int itemIndex = rowIndex - 1;

            // get item
            if (itemIndex >= Item.items.Count)
            {
                Debug.LogError("item index : " + itemIndex + " plus grand que liste items " + Item.items.Count);
            }

            Item item = Item.items[itemIndex];

            // split cells
            string[] cells = row.Split(';');

            if (cells[1].Length > 0 && cells[1][0] == '/')
            {
                item.usableAnytime = true;
                continue;
            }

            for (int cellIndex = 1; cellIndex < cells.Length; cellIndex++)
            {
                // get cell
                string cell = cells[cellIndex];

                if (cell.Length == 0)
                {
                    continue;
                }

                string amountStr = "";

                if (cell.Contains("*"))
                {
                    amountStr = cell.Split('*')[0];
                    cell = cell.Split('*')[1];
                }

                int appearRate = 0;
                if (int.TryParse(cell, out appearRate) == false)
                {
                    Debug.LogError("APPEAR RATES : la cellule : " + cell + " ne peut pas être parsée");
                }

                Item.AppearRate newAppearRate = new Item.AppearRate();

                // rate
                newAppearRate.rate = appearRate;

                // id 
                newAppearRate.id = type ==
                    Item.AppearRate.Type.Container
                    ?
                    (containerLimit + cellIndex - 2)
                    :
                    cellIndex;

                // type
                newAppearRate.type = type;

                // amount
                if (amountStr.Length > 0)
                {
                    int a = int.Parse(amountStr);

                    newAppearRate.amount = a;
                }
                else
                {
                    newAppearRate.amount = 1;
                }

                item.appearRates.Add(newAppearRate);


            }


        }
    }
}