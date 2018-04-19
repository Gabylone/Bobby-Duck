using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationLoader : MonoBehaviour {

	public static LocationLoader Instance;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		TextAsset textAsset = Resources.Load ("Places") as TextAsset;

		string[] rows = textAsset.text.Split ('\n');

		int placeIndex = 0;

		Word.locationWords = new Word[rows.Length-1];

		for (int rowIndex = 1; rowIndex < Word.locationWords.Length; rowIndex++) {

			Word newWord = new Word ();

			string row = rows [rowIndex];
			row = row.TrimEnd ('\r', '\n');

			string[] cells = row.Split (';');

			newWord.name = cells [1].ToLower();
			newWord.UpdateGenre (cells [2]);
			newWord.locationPrep = cells [3];
			newWord.UpdateAdjectiveType (cells [4]);

			Word.locationWords [placeIndex] = newWord;

			++placeIndex;

		}

	}
}
