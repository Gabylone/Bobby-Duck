using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapData
{
	public static MapData Instance;

	public Coords[] clueIslandsCoords;

	public Coords 	treasureIslandCoords;

	public Coords 	homeIslandCoords;

	public MapData () {
		Instance = this;
	}

	public MapData (int mapScale) {

		Instance = this;

		clueIslandsCoords = new Coords[ClueManager.Instance.ClueAmount];

		for ( int i = 0; i < clueIslandsCoords.Length ; ++i)
		{
			clueIslandsCoords [i] = MapGenerator.Instance.RandomCoords;

			MapGenerator.Instance.GetChunk(clueIslandsCoords[i]).IslandData = new IslandData(clueIslandsCoords[i]);
		}

		// TREASURE
		treasureIslandCoords = MapGenerator.Instance.RandomCoords;
		MapGenerator.Instance.GetChunk(treasureIslandCoords).IslandData = new IslandData(treasureIslandCoords);

		// HOME
		homeIslandCoords = MapGenerator.Instance.RandomCoords;
//		homeIslandCoords = new Coords(0,0);
		MapGenerator.Instance.GetChunk(homeIslandCoords).IslandData = new IslandData(homeIslandCoords);

	}

}

