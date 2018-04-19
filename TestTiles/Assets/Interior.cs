using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior {

	public Coords coords;

	public static Dictionary<Coords, Interior> interiors= new Dictionary<Coords, Interior>();

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

	public TileSet tileSet;

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

		Coords hallway_Coords = new Coords ((int)(WorldGeneration.mapScale/2f),(int)(WorldGeneration.mapScale/2f));

		while ( roomTypes.Count > 0 ) {

			Tile newHallwayTile = new Tile (hallway_Coords);
			newHallwayTile.SetType (Tile.Type.Hallway);
			tileSet.Add (hallway_Coords, newHallwayTile);

			if ( Random.value * 100 < WorldGeneration.Instance.roomAppearRate ) {

				Coords coords = newHallwayTile.coords + new Coords (1, 0);

				Tile newRoomTile = new Tile(coords);
				Tile.Type roomType = roomTypes [Random.Range (0, roomTypes.Count)];
				newRoomTile.SetType (roomType);

				roomTypes.Remove (roomType);

				Debug.Log ("creating : " + roomType.ToString ());

				tileSet.Add ( coords, newRoomTile );
			}

			hallway_Coords += new Coords (0, 1);
		}

	}
}

