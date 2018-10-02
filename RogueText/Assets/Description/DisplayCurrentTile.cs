using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCurrentTile : TextTyper {

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		Player.onPlayerMove += HandleOnPlayerMove;
	}

	void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
        //UpdateCurrentTileDescription();
	}

    public override void UpdateCurrentTileDescription()
    {
        base.UpdateCurrentTileDescription();
		Clear ();

		Tile currentTile = Tile.current;

		Word word = currentTile.word;

        string str = GetIntroText();

		if (!Tile.SameAsPrevious ()) {

            str += " ";
			int adjCount = 0;
			foreach (var item in currentTile.GetAdjectives()) {

                str += item.GetName(word.genre, Word.Number.Singular);

                if (currentTile.GetAdjectives ().Count > 1 && adjCount < currentTile.GetAdjectives().Count -1 ) {
                    str += " et ";
				}

				adjCount ++;
			}

		}

        Display(str);
	}

	string GetIntroText ()
	{
        if (Tile.SameAsPrevious() && Location.GetLocation(Tile.current.type).continuationType == Location.ContinuationType.Continued ){

			string[] continuePhrases = new string[2] {
				"Vous continuez " + Tile.current.word.GetDescription (Word.Def.Defined, Word.Preposition.Loc, Word.Number.Singular),
				"Vous êtes encore " + Tile.current.word.GetDescription (Word.Def.Defined, Word.Preposition.Loc, Word.Number.Singular),
			};
			return continuePhrases [Random.Range (0, continuePhrases.Length)];

		} else if (Tile.current.visited == false) {

			string[] discoverPhrases = new string[3] {
				"Vous découvrez " + Tile.current.word.GetDescription (Word.Def.Undefined, Word.Preposition.None , Word.Number.Singular),
				"Vous vous trouvez " + Tile.current.word.GetDescription (Word.Def.Undefined, Word.Preposition.Loc, Word.Number.Singular),
				"Vous êtes " + Tile.current.word.GetDescription (Word.Def.Undefined, Word.Preposition.Loc, Word.Number.Singular)
			};

			return discoverPhrases [Random.Range (0, discoverPhrases.Length)];
			//
		} else {

			string[] goBack = new string[2] {
				"Vous vous retrouvez " + Tile.current.word.GetDescription (Word.Def.Undefined, Word.Preposition.Loc, Word.Number.Singular),
				"Vous revoilà " + Tile.current.word.GetDescription (Word.Def.Undefined, Word.Preposition.Loc, Word.Number.Singular)
				//"Vous découvrez" + word.GetDescription(
			};

			return goBack [Random.Range (0, goBack.Length)];

		}

	}

}
