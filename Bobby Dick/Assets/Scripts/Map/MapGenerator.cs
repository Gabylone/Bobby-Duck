using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance;

	[SerializeField]
	private int mapScale = 100;

	[SerializeField]
	private int loadLimit = 1000;

	public int islandsPerCol;

	public int islandID;

	public DiscoveredCoords discoveredCoords;

	public int islandCreation_LoadLimit= 1;


	void Awake () {
		Instance = this;

        MinimapChunk.onTouchMinimapChunk = null;
	}

	#region map data
	public void CreateNewMap () {

		islandsPerCol = Mathf.RoundToInt (mapScale / 10);

		discoveredCoords = new DiscoveredCoords ();

		InitChunks();

		CreateTreasureIsland ();

		CreateHomeIsland ();

		FormulaManager.Instance.CreateNewClues ();

		StartCoroutine (CreateNormalIslands ());

	}

	public void LoadMap() {

		discoveredCoords = SaveTool.Instance.LoadFromCurrentMap ("discovered coords.xml", "DiscoveredCoords") as DiscoveredCoords;

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

	public void InitChunks() {
		
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
//		SaveManager.Instance.GameData.homeCoords = Coords.Zero;
//		SaveManager.Instance.GameData.homeCoords = new Coords(mapScale-1,mapScale-1);

		Chunk.GetChunk (SaveManager.Instance.GameData.homeCoords).InitIslandData (new IslandData (StoryType.Home));
	}
	IEnumerator CreateNormalIslands () {
//	void CreateNormalIslands () {


		int max = (int)((float)(islandsPerCol * mapScale));
		LoadingScreen.Instance.StartLoading ("Création îles",max );

		int l = 0;
		int a = 0;

		for ( int y = 0; y < mapScale ; ++y ) {

			for (int i = 0; i < islandsPerCol; ++i ) {

				int x = Random.Range ( 0, mapScale );

				Coords c = new Coords ( x , y );

				Chunk targetChunk = Chunk.GetChunk (c);

				if (targetChunk.state == ChunkState.UndiscoveredSea) {
					targetChunk.InitIslandData (new IslandData (StoryType.Normal) );
				}


				++l;
				++a;
				if ( l > islandCreation_LoadLimit ) {
					l = 0;
					yield return new WaitForEndOfFrame ();
				}
				LoadingScreen.Instance.Push (a);
//				yield return new WaitForEndOfFrame ();
			}

		}

		yield return new WaitForEndOfFrame ();

		if (GameManager.Instance.saveOnStart)
			SaveManager.Instance.SaveAllIslands ();
		else
			LoadingScreen.Instance.End ();

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