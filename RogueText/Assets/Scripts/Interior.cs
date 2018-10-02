using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Interior {

	public Coords coords;

    public static Interior current;

	public static Dictionary<Coords, Interior> interiors= new Dictionary<Coords, Interior>();

    public TileSet tileSet;

    public static void Add ( Tile tile , Tile.Type type) {

		tile.SetType (type);

		Interior newInterior = new Interior ();

		newInterior.coords = tile.coords;
		interiors.Add (tile.coords, newInterior);
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

        DisplayWeather.Instance.DisplayCurrentWeather();


    }

    public void ExitByWindow()
    {
        Coords tCoords = TileSet.map.playerCoords + (Coords)Player.Instance.direction;

        Player.Instance.coords = tCoords;

        Exit();
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

        DisplayWeather.Instance.DisplayCurrentWeather();

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
                newHallwayTile.AddItem(Item.FindByName("porte"));
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

	}
}
