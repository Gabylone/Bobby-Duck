using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapData
{
	public static MapData Instance;

	public Chunk[,] chunks;

	public int[] clueIslandsXPos;
	public int[] clueIslandsYPos;

	public int treasureIslandXPos = 0;
	public int treasureIslandYPos = 0;

	public int homeIslandXPos = 0;
	public int homeIslandYPos = 0;

	public MapData () {
		Instance = this;
	}

	public MapData (int mapScale) {

		Instance = this;

		chunks = new Chunk[mapScale, mapScale];
		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {
				chunks [x, y] = new Chunk (x,y);
			}
		}

		clueIslandsXPos = new int[ClueManager.Instance.ClueAmount];
		clueIslandsYPos = new int[ClueManager.Instance.ClueAmount];

		for ( int i = 0; i < clueIslandsXPos.Length ; ++i)
		{

			clueIslandsXPos[i] = MapGenerator.Instance.RandomX;
			clueIslandsYPos[i] = MapGenerator.Instance.RandomY;

			chunks 	[clueIslandsXPos[i], clueIslandsYPos[i]].IslandData = new IslandData(clueIslandsXPos[i],clueIslandsYPos[i]);
		}

		// TREASURE
		treasureIslandXPos = MapGenerator.Instance.RandomX;
		treasureIslandYPos = MapGenerator.Instance.RandomY;

		chunks 	[treasureIslandXPos, treasureIslandXPos].IslandData = new IslandData(treasureIslandXPos, treasureIslandXPos);

		// HOME
		homeIslandXPos = MapGenerator.Instance.RandomX;
		homeIslandYPos = MapGenerator.Instance.RandomY;

		chunks 	[homeIslandXPos, homeIslandYPos].IslandData = new IslandData(homeIslandXPos, homeIslandYPos);

	}

	public Chunk currentChunk {
		get {
			return chunks[PlayerBoatInfo.Instance.PosX , PlayerBoatInfo.Instance.PosY];
		}
	}

}

