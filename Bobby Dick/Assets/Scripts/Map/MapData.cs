using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapData
{
	public static MapData Instance;

	public Coords 	treasureIslandCoords;

	public Coords 	homeIslandCoords;

	public MapData () {
		Instance = this;
	}

	public MapData (int mapScale) {

		Instance = this;

		// TREASURE
		treasureIslandCoords = MapGenerator.Instance.RandomCoords;
		MapGenerator.Instance.GetChunk(treasureIslandCoords).IslandData = new IslandData(IslandType.Treasure);

		// HOME
		homeIslandCoords = MapGenerator.Instance.RandomCoords;
		MapGenerator.Instance.GetChunk(homeIslandCoords).IslandData = new IslandData(IslandType.Home);

	}

}

