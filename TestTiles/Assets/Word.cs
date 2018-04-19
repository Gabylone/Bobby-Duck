using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word {

//	public static Word[] itemWords;
	public static Word[] locationWords;
	public bool startsWithVowel = false;

	public string locationPrep = "";

	public string name = "";

	public Genre genre;

	public Adjective.Type adjType;

	public Adjective adj;

	public Adjective GetAdjective () {
		
		if (adj == null) {
			adj = Adjective.adjectives [(int)adjType] [Random.Range (0, Adjective.adjectives [(int)adjType].Count)];
		}

		return adj;
	}

	public static Word GetLocationWord ( Tile.Type type ) {

		int typeIndex = (int)type;

		Word word = locationWords [typeIndex - 1];

		return word;
	}


	public enum Number {
		Singular,
		Plural,
	}

	public enum Genre {

		None,

		Masculine,
		Feminine,

	}
	public enum Def {
		Defined,
		Undefined
	}
	public enum Preposition {
		None,
		De,
		A,
		Loc,
	}

	public string GetDescription ( Def def, Preposition prep ) {
		return GetDescription (def, prep, Number.Singular);
	}
	public string GetDescription ( Def def ) {
		return GetDescription (def, Preposition.None, Number.Singular);
	}
	public string GetDescription ( Def def , Preposition prep , Number num ) {

		string article = GetArticle (def, prep, num);
		if ( prep == Preposition.Loc ) {
			article = locationPrep + " " + GetArticle (def, Preposition.None, num);
		}

//		string adj = GetAdjective ().GetName(genre, num);

		string placeName = GetName(num);

		//return article + " " + adj + " " + placeName;
		return article + " " + placeName;
//		return article + " " + placeName + " " + adj ;
	}

	public string GetName (Number number) {
		string str = name.ToLower ();
		if (number == Number.Plural)
			str += "s";

//		return "<color=red>" + str + "</color>"; 
		return str;
	}


	public void UpdateGenre ( string str ) {
		switch (str) {
		case "ms":
			genre = Genre.Masculine;
			break;
		case "fs" :
			genre = Genre.Feminine;
			break;
		default:
			Debug.LogError ("pas trouvé de genre pour : " + str);
			break;
		}

	}

	public void UpdateAdjectiveType ( string str ) {

		for (int i = 0; i < (int)Adjective.Type.Any; i++) {

			Adjective.Type a = (Adjective.Type)i;
			if ( a.ToString() == str ) {
				adjType = a;
				return;
			}

		}

		Debug.LogError("pas trouvé adj type pour " + str);

	}

	public string GetArticle ( Def def , Preposition proposition , Number number ) {

		if (
			name [0] == 'a'
			||
			name [0] == 'e'
			||
			name [0] == 'i'
			||
			name [0] == 'o'
			||
			name [0] == 'u') {
			startsWithVowel = true;
		}

		if (number == Number.Plural) {
			
			switch (proposition) {
			case Preposition.None:
				return "des";
				break;
			case Preposition.De:
				return "des";
				break;
			case Preposition.A:
				return "aux";
				break;
			default:
				break;
			}

		}

		if ( def == Def.Defined ) {

			if (startsWithVowel) {

				switch (proposition) {
				case Preposition.None:
					return "l'";
					break;
				case Preposition.De:
					return "de l'";
					break;
				case Preposition.A:
					return "à l'";
					break;
				default:
					break;
				}

			} else {
				
				switch (genre) {
				case Genre.Masculine:

					switch (proposition) {
					case Preposition.None:
						return "le";
						break;
					case Preposition.De:
						return "du";
						break;
					case Preposition.A:
						return "au";
						break;
					default:
						break;
					}

					break;
				case Genre.Feminine:

					switch (proposition) {
					case Preposition.None:
						return "la";
						break;
					case Preposition.De:
						return "de la";
						break;
					case Preposition.A:
						return "à la";
						break;
					default:
						break;
					}

					break;
				default:
					break;
				}
			}

		} else {

			switch (genre) {
			case Genre.Masculine:

				switch (proposition) {
				case Preposition.None:
					return "un";
					break;
				case Preposition.De:
					if ( startsWithVowel )
						return "de l'";
					else
						return "du";
					break;
				case Preposition.A:
					return "à un";
					break;
				default:
					break;
				}

				break;
			case Genre.Feminine:

				switch (proposition) {
				case Preposition.None:
					return "une";
					break;
				case Preposition.De:
					return "d'une";
					break;
				case Preposition.A:
					return "à une";
					break;
				default:
					break;
				}

				break;
			default:
				break;
			}

		}

		return "pas trouvé d'aricle";
	}
}