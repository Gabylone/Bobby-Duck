using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public static List<Location> locations = new List<Location>();

    public static Location GetLocation(Tile.Type type)
    {
        return locations[(int)type - 1];
    }

    public string GetItemPosition()
    {
        return itemPositions[Random.Range(0, itemPositions.Count)];
    }

    public Tile.Type type;

    public enum ContinuationType
    {
        Single,
        Continued,
    }

    public ContinuationType continuationType;

    public List<string> itemPositions = new List<string>();

    public Word word;
}

public class LocationLoader : MonoBehaviour
{

    public static LocationLoader Instance;

    public string[] positionPhrases;
    public string[] visionPhrases;
    public string[] locationPhrases;


    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        TextAsset textAsset = Resources.Load("Locations") as TextAsset;

        string[] rows_Locations = textAsset.text.Split('\n');

        int locationIndex = 0;
        int lenght = rows_Locations.Length - 1;

        for (int rowIndex = 1; rowIndex < lenght; rowIndex++)
        {
            Location newLocation = new Location();

            /// WORD ///
			Word newWord = new Word();
            string row = rows_Locations[rowIndex];
            row = row.TrimEnd('\r', '\n');

            string[] cells = row.Split(';');

            newWord.name = cells[1].ToLower();
            newWord.UpdateGenre(cells[2]);
            newWord.locationPrep = cells[3];
            newWord.UpdateAdjectiveType(cells[4]);

            newLocation.word = newWord;

            /// CONTINUATION TYPE ///
            if (cells[5] == "continued")
                newLocation.continuationType = Location.ContinuationType.Continued;
            else
                newLocation.continuationType = Location.ContinuationType.Single;

            Location.locations.Add(newLocation);

            ++locationIndex;

        }


        LoadItemPositions();

    }

    private void LoadItemPositions()
    {
        TextAsset textAsset_ItemPositions = Resources.Load("Locations_ItemPositions") as TextAsset;
        string[] rows_ItemPositions = textAsset_ItemPositions.text.Split('\n');

        string[] itemPositions = rows_ItemPositions[0].Split(';');

        int rowIndex = 1;

        foreach (var location in Location.locations)
        {
            string[] cells = rows_ItemPositions[rowIndex].Split(';');

            for (int cellIndex = 1; cellIndex < cells.Length; cellIndex++)
            {
                if ( cells[cellIndex].Contains("1"))
                {
                   //Debug.Log("cell content : " + cells[cellIndex]);
                   //Debug.Log("adding phrases (" + itemPositions[cellIndex] + ") to location : (" + location.word.name + ")");
                   //Debug.Log("ROW : " + rows_ItemPositions[rowIndex]);
                   location.itemPositions.Add(itemPositions[cellIndex]);
                }
            }

            ++rowIndex;

        }
    }
}
