using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance;

	[SerializeField]
	private int mapScale = 100;

	[SerializeField]
	private int loadLimit = 1000;

	int islandsPerCol;

	public int islandID;

	public DiscoveredCoords discoveredCoords;

	void Awake () {
		Instance = this;
	}

	public void GenerateIslands () {
		CreateNewMap ();
	}

	#region map data
	private void CreateNewMap () {

		islandsPerCol = Mathf.RoundToInt (mapScale / 10);

		discoveredCoords = new DiscoveredCoords ();

		InitMap();

		CreateTreasureIsland ();

		CreateHomeIsland ();

		FormulaManager.Instance.CreateNewClues ();

		CreateNormalIslands ();

	}

	public void LoadMap() {

		discoveredCoords = SaveTool.Instance.LoadFromPath ("discovered coords.xml", "DiscoveredCoords") as DiscoveredCoords;

		Chunk.chunks.Clear ();

		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {
				
				Coords c = new Coords (x, y);

				Chunk.chunks.Add (c, new Chunk ());
				//				Chunk.chunks[c].stae
			}
		}

		foreach (var item in discoveredCoords.coords) {

			Chunk.GetChunk (item).state = ChunkState.DiscoveredSea;

		}
	}

	public void InitMap() {
		
		Chunk.chunks.Clear ();

		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {
				Coords c = new Coords (x, y);

				Chunk.chunks.Add (c, new Chunk ());
//				Chunk.chunks[c].stae
			}
		}
	}

	void CreateTreasureIsland () {
		SaveManager.Instance.GameData.treasureCoords = MapGenerator.Instance.RandomCoords;
		Chunk.GetChunk (SaveManager.Instance.GameData.treasureCoords).InitIslandData (new IslandData (StoryType.Treasure));
	}
	void CreateHomeIsland () {
		SaveManager.Instance.GameData.homeCoords = MapGenerator.Instance.RandomCoords;
		Chunk.GetChunk (SaveManager.Instance.GameData.homeCoords).InitIslandData (new IslandData (StoryType.Home));
	}
//	IEnumerator CreateNormalIslands () {
	void CreateNormalIslands () {

		int loadLimit = 1;

//		LoadingScreen.Instance.StartLoading ("Création îles",islandsPerCol * mapScale - islandsPerCol);

		int l = 0;

		for ( int y = 0; y < mapScale ; ++y ) {

			for (int i = 0; i < islandsPerCol; ++i ) {

				int x = Random.Range ( 0, mapScale );

				Coords c = new Coords ( x , y );

				Chunk targetChunk = Chunk.GetChunk (c);

				if (targetChunk.state == ChunkState.UndiscoveredSea) {
					targetChunk.InitIslandData (new IslandData (StoryType.Normal) );
				}

//				yield return new WaitForEndOfFrame ();
				++l;
//				LoadingScreen.Instance.Push (l);
			}

		}

		SaveManager.Instance.SaveAllIslands ();

//		yield return new WaitForEndOfFrame ();

	}
	#endregion

	#region tools
	public Coords RandomCoords{
		get {
			return new Coords (Random.Range ( 0, mapScale ),Random.Range ( 0, mapScale ));
		}
	}
	public int RandomX {
		get {
			return Random.Range ( 0, mapScale );
		}
	}

	public int RandomY {
		get {
			return Random.Range ( 0, mapScale );
		}
	}
	#endregion

	public int MapScale {
		get {
			return mapScale;
		}
	}
}

public class DiscoveredCoords {
	public List<Coords> coords = new List<Coords>();

	public DiscoveredCoords () {
		//
	}
}