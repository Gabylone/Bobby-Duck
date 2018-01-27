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
//		Debug.Log ("setting treasure island at : " + treasureIslandCoords);
		Chunk.GetChunk (treasureIslandCoords).SetIslandData (new IslandData (StoryType.Treasure));

		// HOME
		homeIslandCoords = MapGenerator.Instance.RandomCoords;
//		Debug.Log ("setting treasure island at : " + homeIslandCoords);
		Chunk.GetChunk (homeIslandCoords).SetIslandData (new IslandData (StoryType.Home));

	}

}

