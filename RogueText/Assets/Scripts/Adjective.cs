using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adjective {

	public static List<List<Adjective>> adjectives = new List<List<Adjective>>();

 

	public enum Type
	{
		Rural,
		Urbain,

		Any,
	}

	public string GetName ( Word.Genre genre , Word.Number num) {

		string adj = _name;
		if (genre == Word.Genre.Feminine) {

			if ( _name[_name.Length-1] != 'e' ) {
				adj += 'e';
			}

		}

		if (num == Word.Number.Plural) {
			adj += "s";
		}

		return adj;

	}

	public string _name;

	public static Adjective GetRandom ( Adjective.Type type ) {
		return adjectives [(int)type] [Random.Range (0, adjectives [(int)type].Count)];
	}
}
