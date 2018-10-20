using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class DisplaySurroundingTiles : TextTyper {

    public static DisplaySurroundingTiles Instance;

    public TextColor placeTextColor = TextColor.Pink;


    List<string> positionPhrases;
    List<string> visionPhrases;
    List<string> locationPhrases;

    int surroundTileIndex;



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

        // init
		Clear ();

        positionPhrases = LocationLoader.Instance.positionPhrases.ToList();
        visionPhrases = LocationLoader.Instance.visionPhrases.ToList();
        locationPhrases = LocationLoader.Instance.locationPhrases.ToList();

        // get tiles
        GetSurroundingTiles();

        string str = "";

        List<string> phrases = new List<string>();

        // get text
		foreach (var surroundingTile in surroundingTiles) {
            phrases.Add ( GetSurroundingTileDescription(surroundingTile) );
        }

        // display text
        foreach (var item in phrases)
        {
            str += WithCaps(item);

            if (surroundTileIndex < surroundingTiles.Count - 1)
            {
                str += "\n";
            }
        }

        //Display(str);
        textToType = str;
        //UpdateText ();

	}

    void GetSurroundingTiles()
    {
        surroundingTiles.Clear();

        List<Player.Facing> facings = new List<Player.Facing>();
        facings.Add(Player.Facing.Front);
        facings.Add(Player.Facing.Right);
        facings.Add(Player.Facing.Left);

        foreach (var facing in facings)
        {
            Direction dir = Player.Instance.GetDirection(facing);

            Coords targetCoords = Player.Instance.coords + (Coords)dir;

            Tile tile = TileSet.current.GetTile(targetCoords);

            if (tile == null)
                continue;

            if (tile.locked)
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
    }


    string GetLocationDescription(Tile tile)
    {
        // same tile
        if (tile.type == Tile.current.type)
        {
            if (Location.GetLocation(tile.type).continuationType == Location.ContinuationType.Single)
            {
                string article = tile.word.GetArticle(Word.Def.Undefined, Word.Preposition.None, Word.Number.Singular);
                return article + " autre " + tile.word.GetName(Word.Number.Singular, placeTextColor);
            }
            else
            {
                return tile.word.GetDescription(Word.Def.Defined, Word.Preposition.None, Word.Number.Singular, placeTextColor);
            }
        }
        else
        {
            Word.Def def = Interior.current != null ? Word.Def.Defined : Word.Def.Undefined;
            return tile.word.GetDescription(def, Word.Preposition.None, Word.Number.Singular, placeTextColor);
        }
    }

    DescriptionType currentDescriptionType = DescriptionType.None;

    string GetSurroundingTileDescription(SurroudingTile_Facing surroundingTile)
    {
        currentDescriptionType = (DescriptionType)Random.Range(0, (int)DescriptionType.None);

        string locationDescription = GetLocationDescription(surroundingTile.tile);
        string directonStr = surroundingTile.GetDirectionText();

        // ENTOURE //
        if (surroundingTile.facings.Count == 3)
            return GetSurroundedDescription(surroundingTile.tile);

        string link = GetLink();


        if ( Tile.current.type == surroundingTile.tile.type && Location.GetLocation(Tile.current.type).continuationType == Location.ContinuationType.Continued )
        {
            if ( Random.value < 0.5f)
            {
                return directonStr + ", " + locationDescription + " continue";
            }
            else
            {
                return locationDescription + " continue " + directonStr;
            }
        }

        switch (currentDescriptionType)
        {
            case DescriptionType.DirectionThenLocation:
                return directonStr + "  " + link + " " + locationDescription;
            case DescriptionType.LocationThenDirection:
                return locationDescription + " " + link + " " + directonStr;
            case DescriptionType.OnlyLocation:
                return directonStr + ", " + locationDescription;
        }

        return "probleme avec le type de description ";

    }

    public enum DescriptionType
    {
        DirectionThenLocation,
        LocationThenDirection,
        OnlyLocation,

        None,
    }

    string GetLink()
    {
        if (currentDescriptionType == DescriptionType.LocationThenDirection)
        {
            int id = Random.Range(0, locationPhrases.Count);
            string locationPhrase = locationPhrases[id];
            locationPhrases.RemoveAt(id);

            return locationPhrase;
        }

        if (Random.value < 0.5f)
        {
            int id = Random.Range(0, visionPhrases.Count);
            string visionPhrase = visionPhrases[id];
            visionPhrases.RemoveAt(id);

            return visionPhrase;
        }
        else
        {
            int id = Random.Range(0, locationPhrases.Count);
            string locationPhrase = locationPhrases[id];
            locationPhrases.RemoveAt(id);

            return locationPhrase;
        }
    }

    string GetSurroundedDescription(Tile tile)
    {
        string description = tile.word.GetDescription(Word.Def.Undefined, Word.Preposition.De, Word.Number.Singular, placeTextColor);

        if (Tile.current.type == tile.type)
        {
            return "Vous êtes toujours au milieu " + description;
        }
        else
        {
            return "Vous êtes au milieu " + description;
        }
    }

    public struct SurroudingTile_Facing
    {
        /// <summary>
        /// DECLARATION
        /// </summary>
        public Tile tile;

        public List<Player.Facing> facings;
        public string GetDirectionText()
        {
            string str = "";

            int i = 0;

            foreach (var facing in facings)
            {
                string directionWord = Coords.GetPhraseDirecton(facing);

                str += directionWord;

                if (i < facings.Count - 1)
                {
                    if (facings.Count == 2)
                    {
                        str += " et ";
                    }
                }

                i++;
            }

            return str;
        }
    }

    public struct SurroundingTile_Direction
    {

        public Tile tile;

        public List<Direction> dirs;

    }
}
