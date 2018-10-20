using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interior {

	public Coords coords;

    public static Interior current;

	public static Dictionary<Coords, Interior> interiors= new Dictionary<Coords, Interior>();

    public TileSet tileSet;

    public static void Add(Tile tile, Tile.Type type)
    {

        tile.SetType(type);

        Interior newInterior = new Interior();

        newInterior.coords = tile.coords;
        interiors.Add(tile.coords, newInterior);

        if (Random.value < WorldGeneration.Instance.chanceClosedDoor)
        {
            tile.AddItem(Item.FindByName("porte (f)"));
            tile.AddItem(Item.FindByName("clé"));
            tile.AddItem(Item.FindByName("clé"));
        }
        else
        {
            tile.AddItem(Item.FindByName("porte (o)"));
            tile.AddItem(Item.FindByName("clé"));
            tile.AddItem(Item.FindByName("clé"));
        }

    }

	public static Interior Get (Coords coords)
	{
		return interiors[coords];
	}

    #region enter / exit
    public void Enter()
    {
        current = this;


        if (tileSet == null)
        {
            tileSet = new TileSet();
            Genererate();
        }

        TileSet.map.playerCoords = Player.Instance.coords;

        TileSet.Set(tileSet);



        Player.Instance.coords = new Coords((int)(WorldGeneration.Instance.mapScale / 2f), (int)(WorldGeneration.Instance.mapScale / 2f));

        Player.Instance.Move(Direction.None);

        TimeManager.Instance.ChangeMovesPerHour(4);

    }

    public void ExitByWindow()
    {
        //Coords tCoords = TileSet.map.playerCoords + (Coords)Player.Instance.direction;
        Coords tCoords = TileSet.map.playerCoords;

        Tile tile = TileSet.map.GetTile(tCoords);

        if ( tile!= null)
        {
            Player.Instance.coords = tCoords;
            Exit();
        }
        else
        {
            DisplayFeedback.Instance.Display("la fenêtre est bloquée par une haie");
        }
        
    }

    public void ExitByDoor()
    {
        Player.Instance.coords = TileSet.map.playerCoords;

        Exit();
    }

    void Exit()
    {
        Interior.current = null;

        TileSet.Set(TileSet.map);

        Player.Instance.Move(Direction.None);

        TimeManager.Instance.ChangeMovesPerHour(10);


    }
    #endregion

    public void Genererate() {

		tileSet = new TileSet ();

		List<Tile.Type> roomTypes = new List<Tile.Type> ();

		Tile.Type type = Tile.Type.LivingRoom;

		for (int i = 0; i < WorldGeneration.Instance.tileTypeAppearChances.Length; i++) {
			
			if ( Random.value * 100 < WorldGeneration.Instance.tileTypeAppearChances[i] ) {

				roomTypes.Add (type);

			}

			++type;

		}

		Coords hallway_Coords = new Coords ((int)(WorldGeneration.Instance.mapScale/2f),(int)(WorldGeneration.Instance.mapScale/2f));

        int a = 0;

		while ( roomTypes.Count > 0 ) {

			Tile newHallwayTile = new Tile (hallway_Coords);
			newHallwayTile.SetType (Tile.Type.Hallway);
			tileSet.Add (hallway_Coords, newHallwayTile);

            if ( a == 0)
            {
                newHallwayTile.AddItem(Item.FindByName("porte (o)"));
            }

            if ( Random.value * 100 < WorldGeneration.Instance.roomAppearRate ) {

				Coords coords = newHallwayTile.coords + new Coords (1, 0);

				Tile newRoomTile = new Tile(coords);
				Tile.Type roomType = roomTypes [Random.Range (0, roomTypes.Count)];
				newRoomTile.SetType (roomType);

				roomTypes.Remove (roomType);

				tileSet.Add ( coords, newRoomTile );
			}

			hallway_Coords += new Coords (0, 1);

            ++a;

        }

        // GENERATING BUNKER

        if ( coords == ClueManager.Instance.bunkerCoords)
        {
            int i = Random.Range(1, tileSet.tiles.Count);
            Item bunkerItem = Item.FindByName("tableau");
            tileSet.tiles.Values.ElementAt(i).items.Add(bunkerItem);
        }

        if (coords== ClueManager.Instance.clueCoords)
        {
            int i = Random.Range(1, tileSet.tiles.Count);
            Item clueItem = Item.FindByName("radio");
            tileSet.tiles.Values.ElementAt(i).items.Add(clueItem);
        }

        // ADDING DOORS
        AddDoors(tileSet);

	}
    
    void AddDoors(TileSet tileset)
    {
        foreach (var tile in tileset.tiles.Values)
        {
            if (tile.type != Tile.Type.Hallway)
            {
                AddDoors(tileSet, tile);
            }
        }
    }
    void AddDoors(TileSet tileset, Tile tile)
    {

        if (Random.value < WorldGeneration.Instance.chanceLockedRoom)
        {
            tile.locked = true;

            Direction[] surr = new Direction[4] {
                        Direction.North, Direction.West, Direction.South, Direction.West
                    };

            foreach (var dir in surr)
            {
                Coords c = tile.coords + (Coords)dir;
                Tile adjTile = tileset.GetTile(c);

                if (adjTile != null)
                {
                    switch (dir)
                    {
                        case Direction.North:
                            tile.AddItem(Item.FindByName("porte (o)(n)"));
                            adjTile.AddItem(Item.FindByName("porte (o)(s)"));
                            break;
                        case Direction.East:
                            tile.AddItem(Item.FindByName("porte (o)(e)"));
                            adjTile.AddItem(Item.FindByName("porte (o)(w)"));
                            break;
                        case Direction.South:
                            tile.AddItem(Item.FindByName("porte (o)(s)"));
                            adjTile.AddItem(Item.FindByName("porte (o)(n)"));
                            break;
                        case Direction.West:
                            tile.AddItem(Item.FindByName("porte (o)(w)"));
                            adjTile.AddItem(Item.FindByName("porte (o)(e)"));
                            break;
                        case Direction.None:
                            break;
                        default:
                            break;
                    }

                }

            }

        }
    }
}
