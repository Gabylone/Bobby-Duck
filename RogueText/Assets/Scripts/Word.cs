using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word {

	public bool startsWithVowel = false;

	public string locationPrep = "";

	public string name = "";

	public Genre genre;

	public Adjective.Type adjType;

    public bool used = false;

    public Number number = Number.None;

	public enum Number {
        None,

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

    bool red = false;

    public void SetColor()
    {
        red = true;
    }

	public string GetDescription ( Def def, Preposition prep ) {
		return GetDescription (def, prep, Number.Singular);
	}
	public string GetDescription ( Def def ) {
		return GetDescription (def, Preposition.None, Number.Singular);
	}
	public string GetDescription ( Def def , Preposition prep , Number num ) {
        return GetDescription(def, prep, num, TextColor.None);
	}
    public string GetDescription(Def def, Preposition prep, Number num, TextColor textColor)
    {
        string article = GetArticle(def, prep, num);
        if (prep == Preposition.Loc)
        {
            article = locationPrep + " " + GetArticle(def, Preposition.None, num);
        }

        string placeName = GetName(num, textColor);

        return article + " " + placeName;
    }

    public string GetName()
    {
        return GetName(Number.Singular);
    }
    public string GetName (Number number) {

        if ( used)
        {
            return GetName(number, TextColor.Green);
        }
        else
        {
            return GetName(number, TextColor.Blue);
        }

    }

    public string GetName(Number _number, TextColor c )
    {
        string str = name.ToLower();

        if (_number == Number.Plural)
            str += "s";

        if (str.Contains("("))
        {
            str = str.Remove(str.IndexOf("(") - 1);
        }

        if ( c == TextColor.None)
        {
            return str;
        }
        else
        {
            return "<" + TextManager.Instance.GetColorChar(c) + str + ">";
        }

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
			Debug.LogError ("pas trouvé de genre pour l'item : " + name + " ( content : " + str + ")");
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
			name [0] == 'u'
            ||
            name [0] == 'é') {
			startsWithVowel = true;
		}

        // NUMBER
		if (number == Number.Plural) {
			
			switch (proposition) {
			case Preposition.None:
				return "des";
			case Preposition.De:
				if (def == Def.Defined)
					return "des";
				else
					return "de";
			case Preposition.A:
				return "aux";
			default:
				break;
			}

		}

        // DEF

        /*if ( Interior.current != null)
        {
            Debug.Log("il y a un intérieur donc défini");
            def = Def.Defined;
        }*/

		if ( def == Def.Defined ) {

			if (startsWithVowel) {

				switch (proposition) {
				case Preposition.None:
					return "l'";
				case Preposition.De:
					return "de l'";
				case Preposition.A:
					return "à l'";
				default:
					break;
				}

			} else {
				
				switch (genre) {
				case Genre.Masculine:

					switch (proposition) {
					case Preposition.None:
						return "le";
					case Preposition.De:
						return "du";
					case Preposition.A:
						return "au";
					default:
						break;
					}

					break;
				case Genre.Feminine:

					switch (proposition) {
					case Preposition.None:
						return "la";
					case Preposition.De:
						return "de la";
					case Preposition.A:
						return "à la";
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
				case Preposition.De:
					if ( startsWithVowel )
						return "de l'";
					else
						return "du";
				case Preposition.A:
					return "à un";
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
				case Preposition.A:
					return "à une";
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