using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapData
{
	public static MapData Instance;

	public int[] 	clueIslandsXPos;
	public int[] 	clueIslandsYPos;

	public int 		treasureIslandXPos = 0;
	public int 		treasureIslandYPos = 0;

	public int 		homeIslandXPos = 0;
	public int 		homeIslandYPos = 0;

	public MapData () {
		Instance = this;
	}

	public MapData (int mapScale) {

		Instance = this;

		clueIslandsXPos = new int[ClueManager.Instance.ClueAmount];
		clueIslandsYPos = new int[ClueManager.Instance.ClueAmount];

		for ( int i = 0; i < clueIslandsXPos.Length ; ++i)
		{

			clueIslandsXPos[i] = MapGenerator.Instance.RandomX;
			clueIslandsYPos[i] = MapGenerator.Instance.RandomY;

			MapGenerator.Instance.Chunks 	[clueIslandsXPos[i], clueIslandsYPos[i]].IslandData = new IslandData(clueIslandsXPos[i],clueIslandsYPos[i]);
		}

		// TREASURE
		treasureIslandXPos = MapGenerator.Instance.RandomX;
		treasureIslandYPos = MapGenerator.Instance.RandomY;
		MapGenerator.Instance.Chunks 	[treasureIslandXPos, treasureIslandYPos].IslandData = new IslandData(treasureIslandXPos, treasureIslandYPos);

		// HOME
		homeIslandXPos = MapGenerator.Instance.RandomX;
		homeIslandYPos = MapGenerator.Instance.RandomY;

		MapGenerator.Instance.Chunks 	[homeIslandXPos, homeIslandYPos].IslandData = new IslandData(homeIslandXPos, homeIslandYPos);

	}

	public Chunk currentChunk {
		get {
			return MapGenerator.Instance.Chunks[Boats.Instance.PlayerBoatInfo.PosX , Boats.Instance.PlayerBoatInfo.PosY];
		}
	}

}

