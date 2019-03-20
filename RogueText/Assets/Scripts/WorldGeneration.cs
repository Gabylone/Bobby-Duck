using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

	public static WorldGeneration Instance;

	public int mapScale = 100;

	TileSet mapTileSet;

	void Awake () {
		Instance = this;
	}

	[Header("Roads")]
	public int originRoadAmount_min = 6;
	public int originRoadAmount_max = 8;

    // pas encore utilisé
    public class PathInfo
    {
        public float chanceChangeDirection;
        public float chanceDeviatingOnRoad;
        public float chanceBuildingTown;
    }

	public float road_ChanceChangeDirection = 3f;
	public float road_ChanceDeviatingNewRoad = 4f;

	public float chanceBuildingTown = 3f;
	public int townLenght = 4;

	[Header("Blob")]
	public float blobExpendChance = 20;
	public int blobExpendRangeMin = 1;
	public int blobExpendRangeMax = 3;
	public int blobBorderBuffer = 2;

    [Header("Rivers")]
    public int minRiverAmount = 1;
    public int maxRiverAmount = 4;

    [Header("Forests")]
	public int maxForestScale = 30;
	public int minForestScale = 10;
	public int maxForestAmount = 4;
	public int minForestAmount = 7;

	[Header("Lakes")]
	public int maxLakeScale = 30;
	public int minLakeScale = 10;
	public int maxLakeAmount = 4;
	public int minLakeAmount = 7;

	[Header("Hills")]
	public int maxHillScale = 30;
	public int minHillScale = 10;
	public int maxHillAmount = 4;
	public int minHillAmount = 7;

	[Header("Houses")]
	public int minTownScale = 2;
	public int maxTownScale = 4;
	public float houseAppearChance = 0.2f;
	public float chanceOfLoneHouseCreatingPath = 0.25f;

	[Header("Interiors")]
	public float roomAppearRate = 0.35f;
	public float[] tileTypeAppearChances;
    public float chanceClosedDoor = 1f;
    public float chanceLockedRoom = 1f;

    [Header("Empty")]
    public float emptyTileChance = 20f;

    [Header("Borders")]
	public int borderWidth = 0;

	public int loneHouseAmount = 40;

	public int blobWaitRate = 2;

	public int roadWaitRate = 1;

	public int scatteredHouseLimit = 10;

	void Start () {

		StartCoroutine(GenerateMap ());

	}

	void InitMapTiles ()
	{
		TileSet.map = new TileSet ();

        Adjective adjective = Adjective.GetRandom(Adjective.Type.Rural);

        for (int x = 0; x < Instance.mapScale; x++) {
			for (int y = 0; y < Instance.mapScale; y++) {

				Coords c = new Coords(x,y);

				Tile newTile = new Tile (c);


                if (c.x < 1)
                {
                    newTile.SetType(Tile.Type.Mountain, adjective);
                    //
                }
                else if (c.x > Instance.mapScale - 2)
                {
                    newTile.SetType(Tile.Type.Mountain, adjective);
                    //
                }
                else if (c.y < 1)
                {
                    newTile.SetType(Tile.Type.Mountain, adjective);
                    //
                }
                else if (c.y > Instance.mapScale - 2)
                {
                    newTile.SetType(Tile.Type.Mountain, adjective);
                }
                else
                {
                    newTile.SetType(Tile.Type.Plain, adjective);
                }

                if (Random.value < 0.01f)
                {
                    adjective = Adjective.GetRandom(Adjective.Type.Rural);
                }

                TileSet.map.Add (c, newTile);

			}
		}
	}

	IEnumerator GenerateMap () {
		

		if (MapTexture.Instance)
		MapTexture.Instance.InitTexture (Instance.mapScale);

        InitMapTiles();

        TileSet.Set(TileSet.map);

		yield return GenerateBorders ();

//		CreateInterior (TileSet.current.GetTile(Player.Instance.coords+new Coords(1,0)), Tile.Type.ForestCabin);
		int hillAmount = Random.Range ( minHillAmount, maxHillAmount );
		for (int i = 0; i < hillAmount; i++) {
			int hillScale = Random.Range ( minHillScale , maxHillScale );
			yield return BlobCoroutine (Tile.Type.Mountain, Tile.Type.Hill, Coords.random, hillScale);
		}

		int LakeAmount = Random.Range ( minLakeAmount, maxLakeAmount );
		for (int i = 0; i < LakeAmount; i++) {
			int lakeScale = Random.Range ( minLakeScale , maxLakeScale );
			yield return BlobCoroutine (Tile.Type.Lake, Tile.Type.Lake, Coords.random, lakeScale);
		}

		int forestAmount = Random.Range ( minForestAmount, maxForestAmount );
		for (int i = 0; i < forestAmount; i++) {
			int forestScale = Random.Range ( minForestScale , maxForestScale );
			yield return BlobCoroutine (Tile.Type.Forest, Tile.Type.Woods, Coords.random, forestScale);
		}

		Direction[] directions = new Direction[4];
		for (int i = 0; i < 4; i++) {
			directions[i] = (Direction)(i * 2);
		}


        // rivers
        int riverAmount = Random.Range(minRiverAmount, maxRiverAmount);
        for (int i = 0; i < riverAmount; i++)
        {
            Direction randomDirection = directions[Random.Range(0, directions.Length)];

            Coords riverOrigin = new Coords(0, mapScale);

            int min = 3;
            int max = mapScale - 3;

            int random = Random.Range(min, max);

            switch (randomDirection)
            {
                case Direction.North:
                    riverOrigin = new Coords( random , min);
                    break;
                case Direction.East:
                    riverOrigin = new Coords( min, random);
                    break;
                case Direction.South:
                    riverOrigin = new Coords( random , max );
                    break;
                case Direction.West:
                    riverOrigin = new Coords( max , random);
                    break;
            }

            yield return PathCoroutine(riverOrigin, new Direction[1] { randomDirection }, Tile.Type.River);
        }


        // roads
        Coords roadOrigin = new Coords((int)((float)Instance.mapScale / 2f), (int)((float)Instance.mapScale / 2f));
        Interior.Add(TileSet.current.GetTile(roadOrigin), Tile.Type.TownHouse);
        yield return PathCoroutine(roadOrigin, directions, Tile.Type.Road);


        // random houses
        yield return GenerateRandomHouses ();

        for (int x = 0; x < Instance.mapScale; x++)
        {
            for (int y = 0; y < Instance.mapScale; y++)
            {

                Coords c = new Coords(x, y);

                switch (TileSet.current.tiles[c].type)
                {
                    case Tile.Type.Plain:
                    case Tile.Type.Field:
                    case Tile.Type.Hill:
                    case Tile.Type.Mountain:
                    case Tile.Type.Forest:
                    case Tile.Type.Woods:
                    case Tile.Type.Sea:
                    case Tile.Type.Lake:
                    case Tile.Type.Beach:
                        if (Random.value * 100f < emptyTileChance)
                        {
                            TileSet.current.RemoveAt(c);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        RefreshTexture ();

		yield return new WaitForEndOfFrame ();

        //		Debug.Log ("Finished World");

        ClueManager.Instance.Init();

        Player.Instance.Init ();


    }

    IEnumerator GenerateBorders() {

		yield return new WaitForEndOfFrame ();
	}

	IEnumerator GenerateRandomHouses () {

		int a = 0;

		for (int i = 0; i < loneHouseAmount; i++) {

			Coords c = Coords.random;
			Tile tile = TileSet.current.GetTile (c);

			switch (tile.type) {

			case Tile.Type.Plain:

				if (Random.value < 0.3f) {
					
					Interior.Add (tile, Tile.Type.Farm);

					List<Tile> tiles = Tile.GetSurrouringTiles (tile, 1);

					foreach (var item in tiles) {
						switch (item.type) {
						case Tile.Type.Plain:
						case Tile.Type.Hill:
						case Tile.Type.Mountain:
						case Tile.Type.Forest:
						case Tile.Type.Woods:
						case Tile.Type.Path:
							
							item.SetType (Tile.Type.Field);
							break;
						default:
							break;
						}
					}

				} else {
					Interior.Add (tile, Tile.Type.CountryHouse);
				}
				break;

			case Tile.Type.Forest:

				Interior.Add (tile,  Tile.Type.ForestCabin);
				break;

			default:
				break;
			}

			switch (tile.type) {
			case Tile.Type.Farm:
			case Tile.Type.ForestCabin:
			case Tile.Type.CountryHouse:
				if ( Random.value * 100 < chanceOfLoneHouseCreatingPath ) {
					yield return PathCoroutine ( c, new Direction[1] { (Direction)(Random.Range(0,4) * 2) } , Tile.Type.Path );
				}
				break;
			default:
				break;
			}

			++a;
			if ( a > scatteredHouseLimit ) {
				a = 0;
				yield return new WaitForEndOfFrame ();
			}


		}

		yield return new WaitForEndOfFrame ();

	}

	#region forest
	IEnumerator BlobCoroutine (Tile.Type targetType, Tile.Type borderType, Coords origin, int scale ) {

		int leftX = 0;
		int rightX = 0;

		int midScale = (int)(scale / 2f);

		int y = midScale;

		int waitRate = 0;

		Adjective adj = Adjective.GetRandom (Adjective.Type.Rural);

		while ( true ) {

			for (int x = leftX; x < rightX; x++) {

				Coords c = origin + new Coords (x, y);
				if (!c.OutOfMap ()) {

					Tile tile = TileSet.current.GetTile (c);
					if (tile.type != Tile.Type.Hill && tile.type != Tile.Type.Mountain) {

						if (x < leftX + blobBorderBuffer || x > rightX - blobBorderBuffer) {
							if (tile.type != targetType)
								tile.SetType (borderType);

						} else {
							tile.SetType (targetType, adj);
							//
						}

					}
				}
			}

			RefreshTexture ();

			++waitRate;
			if (waitRate > blobWaitRate) {
				waitRate = 0;
				yield return new WaitForEndOfFrame ();
			}
			
			--y;

			if (y > 0) {
				if (Random.value * 100f < blobExpendChance) {
					leftX -= Random.Range ( blobExpendRangeMin , blobExpendRangeMax );
				}
				if (Random.value * 100f < blobExpendChance) {
					rightX += Random.Range ( blobExpendRangeMin , blobExpendRangeMax );
				}
			} else {

				if (Random.value * 100f < blobExpendChance) {
					leftX += Random.Range ( blobExpendRangeMin , blobExpendRangeMax );
				}
				if (Random.value * 100f < blobExpendChance) {
					rightX -= Random.Range ( blobExpendRangeMin , blobExpendRangeMax );
				}

				if (leftX > 0 && rightX < 0) {
//					Debug.Log ("la foret c'est fini");
					break;
				}
			}
		}

		yield return new WaitForEndOfFrame ();

	}

	#endregion

	#region roads
	IEnumerator PathCoroutine (Coords origin , Direction[] directions, Tile.Type tileType) {

		List<GenerationPath> paths = new List<GenerationPath>();

		// create origin paths

//		for (int i = 0; i < 1; i++) {
		for (int i = 0; i < directions.Length; i++) {

			Coords c = (Coords)directions[i];
//						Coords startCoords = Coords.random;

			Coords startCoords = origin + new Coords (c.x,c.y);

            GenerationPath originPath = new GenerationPath(startCoords, directions[i], tileType);
			paths.Add (originPath);
		}

		int a = 0;

		while (paths.Count > 0) {

			// advance paths
			for (int i = 0; i < paths.Count; i++) {

				GenerationPath item = paths [i];

				if ( !item.CanContinue () ) {
//					yield return GenerateTown (item.coords);
					paths.Remove (item);
					continue;
				}


				Tile tile = TileSet.current.GetTile (item.coords);

				switch (tile.type) {
				case Tile.Type.Plain:
				case Tile.Type.Forest:
					if (Random.value * 100f < road_ChanceChangeDirection)
						item.ChangeDirection (2);
					
					break;
				}

				switch (tile.type) {
				case Tile.Type.Plain:
					if ( Random.value * 100f < road_ChanceDeviatingNewRoad && item.rate > 2) {
						GenerationPath newPath = new GenerationPath (item.coords, item.dir, Tile.Type.Road);
						newPath.ChangeDirection (2);
						item.rate = 0;
						paths.Add (newPath);
						newPath.Continue ();
					}
					break;
				}

				item.Draw ();
				item.Continue ();

			}

			if (paths.Count == 0) {
				break;
			}

			++a;

			if (a > roadWaitRate) {

				a = 0;

				yield return new WaitForEndOfFrame ();
				RefreshTexture ();
			}

		}

	}
    #endregion

    void RefreshTexture () {
		if (MapTexture.Instance)
			MapTexture.Instance.RefreshTexture ();
	}
}

public class GenerationPath {
	
	public int rate = 0;

	bool buildingTown = false;
    int townLength = 0;

	public Coords coords;
	public Direction dir;
	public Coords dir_Coords;
	public Adjective adj;

    public Tile.Type tileType;

	public enum IsNotBridgeType {
		NotDecidedYet,
		NotBridge,
		DefenitelyBridge,
	}

	IsNotBridgeType isNotBridgeType;
	public bool CanContinue ()
	{
		if (coords.OutOfMap()) {
            //Debug.Log("break path generation of type : " + tileType + " : out of map");
			return false;
		}

		if (TileSet.current.GetTile (coords).type == Tile.Type.Road) {
            //Debug.Log("break path generation of type : " + tileType + " : hit road");
			return false;
        }

        Coords right = coords + (Coords)Coords.GetRelativeDirection(dir, Player.Facing.Right);
        if (TileSet.current.GetTile(coords).type == Tile.Type.Road)
        {
            //Debug.Log("break path generation of type : " + tileType + " : road on right");
            return false;
        }

        Coords left = coords + (Coords)Coords.GetRelativeDirection(dir, Player.Facing.Left);
        if (TileSet.current.GetTile(coords).type == Tile.Type.Road)
        {
            //Debug.Log("break path generation of type : " + tileType + " : road on left");
            return false;
        }

		return true;
	}

	public GenerationPath ( Coords _coords, Direction _dir , Tile.Type _tileType ) {

		coords = _coords;
		dir = _dir;
		dir_Coords = (Coords)dir;
		adj = Adjective.GetRandom (Adjective.Type.Rural);
        tileType = _tileType;

	}

	public void ChangeDirection (int r) {

		int span = Random.value > 0.5f ? r : -r;

		int randomDir = (int)dir + span;
		if ( randomDir >= 8 )
			randomDir -= 8;
		if (randomDir < 0)
			randomDir += 8;

		dir = (Direction)randomDir;
		dir_Coords = (Coords)dir;

	}

	public void Draw () {

		CheckBuildTown ();

		switch (TileSet.current.GetTile (coords).type) {

		case Tile.Type.Plain:
			
			    TileSet.current.GetTile (coords).SetType (tileType, adj);
			break;

		case Tile.Type.Forest:
		case Tile.Type.Woods:
			
			    TileSet.current.GetTile (coords).SetType (tileType, adj);

                break;

		case Tile.Type.Sea:
			break;

		case Tile.Type.Lake:
			
                if ( tileType == Tile.Type.River)
                {
                    isNotBridgeType = IsNotBridgeType.NotBridge;
                    break;
                }
			if (isNotBridgeType == IsNotBridgeType.NotDecidedYet) {
				if (Random.value < 0.45f) {
					isNotBridgeType = IsNotBridgeType.NotBridge;
				} else {
					isNotBridgeType = IsNotBridgeType.DefenitelyBridge;
				}
			}

			if (isNotBridgeType == IsNotBridgeType.DefenitelyBridge)
                {
			        TileSet.current.GetTile (coords).SetType (Tile.Type.Bridge, adj);
                }
                else
                {
                    TileSet.current.GetTile(coords).SetType(tileType, adj);
                }


                break;

		case Tile.Type.River:
			
			TileSet.current.GetTile (coords).SetType (Tile.Type.Bridge, adj);

			break;

		default:
//			TileSet.current.GetTile (coords).SetType (Tile.Type.Road);
			break;
		}


	}

	void CheckBuildTown ()
	{
        if (TileSet.current.GetTile(coords).type != Tile.Type.Plain)
        {
            return;
        }

        float chance = Random.value * 100f;
        if (!buildingTown)
        {
            if (chance < WorldGeneration.Instance.chanceBuildingTown)
            {
                buildingTown = true;
                townLength = 0;
            }
            
        }
        else
        {

            List<Coords> townCoords = new List<Coords>();

            townCoords.Add(coords + (Coords)Coords.GetRelativeDirection(dir, Player.Facing.Right));
            townCoords.Add(coords + (Coords)Coords.GetRelativeDirection(dir, Player.Facing.Left));

            foreach (var c in townCoords)
            {
                Tile tile = TileSet.current.GetTile(c);
                switch (tile.type)
                {
                    case Tile.Type.Forest:
                    case Tile.Type.Plain:
                        Interior.Add(tile, Tile.Type.TownHouse);
                        break;
                    default:
                        break;
                }
            }

            if (townLength >= WorldGeneration.Instance.townLenght)
            {
                buildingTown = false;
            }

            ++townLength;
        }
	}

	public void Continue () {

		coords += dir_Coords;

		++rate;



//		Debug.Log ("coords : " + coords);
//		Debug.Log ("dir : " + dir_Coords);
//

		//		if ( Random.value < 0.2f ) {
		//			coords.x += Random.value < 0.5f ? 1 : -1;
		//		}
		//
		//		if ( Random.value < 0.2f ) {
		//			coords.y += Random.value < 0.5f ? 1 : -1;
		//		}
	}
}

public enum Direction {

	North,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest,

	None
}


[System.Serializable]
public struct Coords {

    public string GetCode()
    {
        return "x:" + x + ",y:" + y;
    }

	public int x;
	public int y;

    public static Direction GetDirectionFromCoords(Coords c1, Coords c2)
    {
        Coords c = c2 - c1;

        Vector2 v = (Vector2)c;

        Direction direction = Direction.North;

        Direction closestDirection = Direction.North;

        while (direction != Direction.None)
        {
            Coords ce = (Coords)direction;

            float angle = Vector2.Angle(v, (Vector2)ce);

            float closestDirectionAngle = Vector2.Angle(v, (Vector2)((Coords)closestDirection));

            string direction_str = Coords.GetWordsDirection(direction).GetDescription(Word.Def.Defined, Word.Preposition.A, Word.Number.Singular);

            if ( angle < closestDirectionAngle)
            {
                closestDirection = direction;
            }

            ++direction;
        }
        return closestDirection;
    }

	public static Coords random {
		get {
			return new Coords ( Random.Range (1,WorldGeneration.Instance.mapScale-1) , Random.Range (1,WorldGeneration.Instance.mapScale-1) );
		}
	}
	public Coords (int x,int y) {
		this.x = x;
		this.y = y;
	}


	public static string GetPhraseDirecton (Player.Facing facing) {

        string[] directionPhrases = new string[8] {
        "devant vous",
        "quelques pas devant vous, vers la droite",
        Random.value < 0.5f ? "à votre droite" : "sur votre droite",
        "derrière vous, à droite",
        "derrière vous",
        "derrière vous, à gauche",
        Random.value < 0.5f ? "à votre gauche" : "sur votre gauche",
        "quelques pas devant vous, à votre gauche"
    };

		return directionPhrases [(int)facing];
	}

	public static Direction GetDirectionFromString ( string str ) {

		foreach (var item in System.Enum.GetValues(typeof(Direction) )) {
			if (item.ToString () == str) {
//				Debug.Log ("found direction : " + item);
				return (Direction)item;
			}
		}

		return Direction.None;

	}

	public bool OutOfMap () {
		return
			x > WorldGeneration.Instance.mapScale - 2 || x < 1 ||
			y > WorldGeneration.Instance.mapScale - 2 || y < 1;
	}

	public static Coords Zero {
		get {
			return new Coords (0, 0);
		}
	}
	// overrides
	// == !=
	public static bool operator ==( Coords c1, Coords c2) 
	{
		return c1.x == c2.x && c1.y == c2.y;
	}
	public static bool operator != (Coords c1, Coords c2) 
	{
		return !(c1 == c2);
	}

	// < >
	public static bool operator < (Coords c1, Coords c2) 
	{
		return c1.x < c2.x && c1.y < c2.y;
	}
	public static bool operator > (Coords c1, Coords c2) 
	{
		return c1.x > c2.x && c1.y > c2.y;
	}
	public static bool operator < (Coords c1, int i) 
	{
		return c1.x < i || c1.y < i;
	}
	public static bool operator > (Coords c1, int i) 
	{
		return c1.x > i || c1.y > i;
	}

	// >= <=
	public static bool operator >= (Coords c1, Coords c2) 
	{
		return c1.x >= c2.x && c1.y >= c2.y;
	}
	public static bool operator <= (Coords c1, Coords c2) 
	{
		return c1.x <= c2.x && c1.y <= c2.y;
	}
	public static bool operator >= (Coords c1, int i) 
	{
		return c1.x >= i || c1.y >= i;
	}
	public static bool operator <= (Coords c1, int i) 
	{
		return c1.x <= i || c1.y <= i;
	}

	// + -
	public static Coords operator +(Coords c1, Coords c2) 
	{
		return new Coords ( c1.x + c2.x , c1.y + c2.y );
	}
	public static Coords operator -(Coords c1, Coords c2) 
	{
		return new Coords ( c1.x - c2.x , c1.y - c2.y );
	}
	public static Coords operator +(Coords c1, int i) 
	{
		return new Coords ( c1.x + i, c1.y + i );
	}
	public static Coords operator -(Coords c1, int i) 
	{
		return new Coords ( c1.x - i, c1.y - i );
	}

	// vector2 cast

	public static explicit operator Coords(Vector2 v)  // explicit byte to digit conversion operator
	{
		return new Coords ( (int)v.x , (int)v.y );
	}
	public static explicit operator Vector2(Coords c)  // explicit byte to digit conversion operator
	{
		return new Vector2 (c.x, c.y);
	}
	//
	//		// direction cast
	//	public static explicit operator Direction(Coords c)  // explicit byte to digit conversion operator
	//	{
	//		return new Direction (c.x, c.y);
	//	}
	public static explicit operator Coords(Direction dir)  // explicit byte to digit conversion operator
	{
		switch (dir) {
		case Direction.North:
			return new Coords ( 0 , 1 );
		case Direction.NorthEast:
			return new Coords ( 1 , 1 );
		case Direction.East:
			return new Coords ( 1 , 0 );
		case Direction.SouthEast:
			return new Coords ( 1 , -1 );
		case Direction.South:
			return new Coords ( 0 , -1 );
		case Direction.SouthWest:
			return new Coords ( -1 , -1 );
		case Direction.West:
			return new Coords ( -1 , 0 );
		case Direction.NorthWest:
			return new Coords ( -1 , 1 );
		case Direction.None:
			return new Coords ( 0 , 0 );
		}

		return new Coords ();
	}

	public static Word GetWordsDirection ( Direction direction ) {
		return Item.items [(int)direction+1].word;
	}

	// string
	public override string ToString()
	{
		return "X : " + x + " / Y : " + y;
	}

    public static Player.Facing GetFacing(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Player.Facing.Front;
            case Direction.NorthEast:
                break;
            case Direction.East:
                return Player.Facing.Left;
            case Direction.SouthEast:
                break;
            case Direction.South:
                return Player.Facing.Back;
            case Direction.SouthWest:
                break;
            case Direction.West:
                return Player.Facing.Right;
            case Direction.NorthWest:
                break;
            case Direction.None:
                break;
        }

        return Player.Facing.None;

    }

    public static Direction GetRelativeDirection (Direction direction, Player.Facing facing)
	{
		int a = (int)direction + (int)facing;
		if ( a >= 8 ) {
			a -= 8;
		}

		//		Debug.Log ( "player is turned " + direction + ", so the returned dir is " + (Direction)a );

		return (Direction)a;
	}
}

