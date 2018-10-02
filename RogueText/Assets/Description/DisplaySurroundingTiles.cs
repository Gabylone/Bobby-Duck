using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct SurroudingTile_Facing {

	public Tile tile;

	public List<Player.Facing> facings;

}

public struct SurroundingTile_Direction{

	public Tile tile;

	public List<Direction> dirs;

}

public class DisplaySurroundingTiles : TextTyper {

    public static DisplaySurroundingTiles Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    public override void Start ()
	{
        base.Start();

		Player.onPlayerMove += HandleOnPlayerMove;
	}

	void HandleOnPlayerMove (Coords prevCoords, Coords newCoords)
	{
        //UpdateCurrentTileDescription();
	}

	List<SurroudingTile_Facing> surroundingTiles = new List<SurroudingTile_Facing> ();

    public Player.Facing GetFacingWithTile(string str)
    {
        if (str == Tile.current.word.name)
        {
            return Player.Facing.Current;
        }

        SurroudingTile_Facing surr = surroundingTiles.Find( x => x.tile.word.name.StartsWith(str) );

        if (surr.tile == null)
        {
            return Player.Facing.None;
        }
        else
        {
            return surr.facings[0];
        }
    }

    public override void UpdateCurrentTileDescription()
    {
        base.UpdateCurrentTileDescription();

		Clear ();

        surroundingTiles.Clear();

		List<Player.Facing> facings = new List<Player.Facing> ();
		facings.Add (Player.Facing.Front);
		facings.Add (Player.Facing.Right);
		facings.Add (Player.Facing.Left);

        Word.Def def = Word.Def.Undefined;

        if (Interior.current != null)
        {
            def = Word.Def.Defined;
        }

        foreach (var facing in facings)
        {

            Direction dir = Player.Instance.GetDirection(facing);
            Coords targetCoords = Player.Instance.coords + (Coords)dir;

            Tile tile = TileSet.current.GetTile(targetCoords);

            if (tile == null)
                continue;

            SurroudingTile_Facing newSurrTile = surroundingTiles.Find(x => x.tile.type == tile.type);

            if (newSurrTile.tile == null)
            {

                newSurrTile.tile = tile;

                newSurrTile.facings = new List<Player.Facing>();
                newSurrTile.facings.Add(facing);

                surroundingTiles.Add(newSurrTile);

            }
            else
            {

                newSurrTile.facings.Add(facing);

            }

        }

        List<string> positionPhrases = LocationLoader.Instance.positionPhrases.ToList();
        List<string> visionPhrases = LocationLoader.Instance.visionPhrases.ToList();
        List<string> locationPhrases = LocationLoader.Instance.locationPhrases.ToList();

        string str = "";

        TextColor placeTextColor = TextColor.Pink;

		int surroundTileIndex = 0;
		foreach (var surroundingTile in surroundingTiles) {

            int facingIndex = 0;
            // ENTOURE //
			if (surroundingTile.facings.Count == 3) {

				if ( Random.value < 0.5f ) {

					string[] surroundedPhrases = new string[2] {
						"entouré",
						"encerclé"
					};

					Tile tile = surroundingTile.tile;

					string surroundedPhrase = surroundedPhrases [Random.Range (0, surroundedPhrases.Length)];

					if (Random.value > 0.5f) {

						//string adjective = tile.GetAdjectives()[0].GetName (tile.word.genre, Word.Number.Plural);
						string description = surroundingTile.tile.word.GetDescription (def, Word.Preposition.De, Word.Number.Plural, placeTextColor);
                        str += "Vous êtes " + surroundedPhrase + " " + description;


                    } else {

						string adjective = tile.GetAdjectives()[0].GetName (tile.word.genre, Word.Number.Singular);

						if ( Tile.current.type == surroundingTile.tile.type ) {
							string description = surroundingTile.tile.word.GetDescription (def, Word.Preposition.None, Word.Number.Singular, placeTextColor);
                            ///str += "Vous êtes " + surroundedPhrase + " par " + description + " " + adjective;
                            str += "Vous êtes " + surroundedPhrase + " par " + description;

                        } else {
							string description = surroundingTile.tile.word.GetDescription (def, Word.Preposition.None, Word.Number.Singular, placeTextColor);
                            str += "Vous êtes " + surroundedPhrase + " par " + description;
						}


					}

				} else {

					string adjective = surroundingTile.tile.GetAdjectives()[0].GetName (surroundingTile.tile.word.genre, Word.Number.Plural);
					string description = surroundingTile.tile.word.GetDescription (def, Word.Preposition.De, Word.Number.Plural, placeTextColor);
                    str += "Vous êtes au milieu " + description + " " + adjective;


                }

			}
            // DESCRIPTION PRECISE
            else {

                // facing part //

				string directionPart = "";
				string placePart = "";

				foreach (var facing in surroundingTile.facings) {

					string direction_str = Coords.GetPhraseDirecton (facing);

					directionPart += direction_str;

					if (facingIndex < surroundingTile.facings.Count - 1) {
						if (surroundingTile.facings.Count == 2) {
							directionPart += " et ";
						}
					}

					facingIndex++;
				}

				bool facingsFirst = Random.value < 0.3f;

                #region same tile
                if (surroundingTile.tile.type == Tile.current.type)
                {

                    // SAME TILE !!!
                    
                    if (Location.GetLocation(surroundingTile.tile.type).continuationType == Location.ContinuationType.Single)
                    {
                        string article = surroundingTile.tile.word.GetArticle(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);
                        placePart = article + " autre " + surroundingTile.tile.word.GetName(Word.Number.Singular, placeTextColor);


                        if (facingsFirst)
                        {


                            string s = "";
                            if (Random.value < 0.5f)
                            {
                                int id = Random.Range(0, visionPhrases.Count);
                                string visionPhrase = visionPhrases[id];
                                visionPhrases.RemoveAt(id);

                                s = visionPhrase;
                            }
                            else
                            {
                                int id = Random.Range(0, locationPhrases.Count);
                                string locationPhrase = locationPhrases[id];
                                locationPhrases.RemoveAt(id);

                                s = locationPhrase;
                            }

                            placePart = s + " " + placePart;

                        }
                        else
                        {

                            int id = Random.Range(0, locationPhrases.Count);
                            string locationPhrase = locationPhrases[id];
                            locationPhrases.RemoveAt(id);

                            placePart = placePart + " " + locationPhrase;

                        }

                    }
                    else
                    {
                        string description = surroundingTile.tile.word.GetDescription(Word.Def.Defined, Word.Preposition.None, Word.Number.Singular, placeTextColor);
                        placePart = description + " continue";
                    }

                }
                #endregion
                else
                #region normal
                {

					string adjective = surroundingTile.tile.GetAdjectives()[0].GetName (surroundingTile.tile.word.genre, Word.Number.Singular);
                    string description = surroundingTile.tile.word.GetDescription(def, Word.Preposition.None, Word.Number.Singular, placeTextColor);

                    if ( facingsFirst ){

                        string s = "";
                        if (Random.value < 0.5f)
                        {
                            int id = Random.Range(0, visionPhrases.Count);
                            string visionPhrase = visionPhrases[id];
                            visionPhrases.RemoveAt(id);

                            s = visionPhrase;
                        }
                        else
                        {
                            int id = Random.Range(0, locationPhrases.Count);
                            string locationPhrase = locationPhrases[id];
                            locationPhrases.RemoveAt(id);

                            s = locationPhrase;
                        }

                        placePart = s + " " + description;

					} else {

                        int id = Random.Range(0, locationPhrases.Count);
                        string locationPhrase = locationPhrases[id];
                        locationPhrases.RemoveAt(id);


                        placePart = description + " " + locationPhrase;
					}

				}

                bool noFacing = Random.value < 0.5f;

                if (facingsFirst) {
                    str += WithCaps(directionPart) + ", " + placePart;

                } else {
                    str += WithCaps(placePart) + " " + directionPart;
                }

            }
    #endregion

            if (surroundTileIndex < surroundingTiles.Count - 1)
            {
                str += "\n";
                str += "\n";
            }

            surroundTileIndex++;
		}

        Display(str);
		//UpdateText ();

	}
}
