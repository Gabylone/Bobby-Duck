
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	public Coords bunkerCoords;

    public Coords clueCoords;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
    public void Init()
    {

        int bunkerID = Random.Range(1, Interior.interiors.Count);
        bunkerCoords = Interior.interiors.Values.ElementAt(bunkerID).coords;

        int clueID = Random.Range(1, Interior.interiors.Count);
        if ( clueID == bunkerID)
        {
            if ( clueID == 1)
            {
                clueID++;
            }
            else
            {
                clueID--;
            }
        }
        clueCoords = Interior.interiors.Values.ElementAt(clueID).coords;

        ActionManager.onAction += HandleOnAction;

	}

    private void HandleOnAction(Action action)
    {
        switch (action.type)
        {
            case Action.Type.GiveClue:
                DisplayBunkerSurrounding();
                break;
            case Action.Type.MoveAway:
                CheckMoveAway();
                break;
            default:
                break;
        }
    }

    private void CheckMoveAway()
    {
        if (Interior.current.coords == bunkerCoords)
        {
            Item.Remove(Action.current.primaryItem);

            DisplayDescription.Instance.Display
                ("Derrière le tableau, un trou béant se dévoile.\n" +
                "Après avoir rampé de longues minutes, une grotte apparait.\n" +
                "Des centaines de gens vivent ici.\n" +
                "Ils marchandent, parlent, dorment et flannent. C'est le début d'une nouvelle ère\n" +
                "\n" +
                "Vous avez gagné.");

            DisplayInput.Instance.EndInput();


        }
    }

	public void GetClueText() {

		string str = "";

	}

    void DisplayBunkerSurrounding ()
	{
		List<DisplaySurroundingTiles.SurroundingTile_Direction> surroundingTiles = new List<DisplaySurroundingTiles.SurroundingTile_Direction> ();

		List<Direction> directions = new List<Direction> ();

        int clueAmount = Random.Range(1, 2);

        Direction randomDirection = (Direction)Random.Range(0, 8);

		string positionPhrase = LocationLoader.Instance.positionPhrases [Random.Range (0, LocationLoader.Instance.positionPhrases.Length)];
		string locationPhrase = LocationLoader.Instance.locationPhrases [Random.Range (0, LocationLoader.Instance.locationPhrases.Length)];

		string str = "";

        string facingPart = "";
        string placePart = "";

        string direction_str = Coords.GetWordsDirection(randomDirection).GetDescription(Word.Def.Defined, Word.Preposition.A, Word.Number.Singular);

        facingPart += direction_str;

        Tile tile = TileSet.map.GetTile( bunkerCoords + (Coords)randomDirection);

        string description = tile.word.GetDescription(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);
        string adjective = tile.GetAdjectives()[0].GetName(tile.word.genre, Word.Number.Singular);

        placePart = description + " " + adjective + " " + locationPhrase;

        str = placePart + " " + facingPart;

        DisplayFeedback.Instance.Display("On parle ici d'un bunker où les gens sont en sécurité." +
            "\n" +
            "On dit qu'" + str + " de l'abri");

	}

}
