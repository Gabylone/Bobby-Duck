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

//			foreach (var terminaison in terminaisons) {
//
//				string nameTer = terminaison.Remove (terminaison.Length);
//
//				Debug.Log ("terminaison : " + nameTer);
//
//				if ( nameTer == terminaison ) {
//					Debug.LogError ("trouvé terminaison : " + nameTer);
//					return nameTer + terminaison;
//				}
//			}

		}

		if (num == Word.Number.Plural) {
			adj += "s";
		}

		return adj;

	}

	public string _name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
