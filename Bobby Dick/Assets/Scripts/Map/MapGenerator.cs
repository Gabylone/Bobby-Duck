using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance;


	[SerializeField]
	private int mapScale = 100;

	[SerializeField]
	private int loadLimit = 1000;

	public int islandID;

	private MapData mapData;

	void Awake () {
		Instance = this;
	}

	public void GenerateIslands () {
		CreateNewMap ();
	}

	#region map data
	private void CreateNewMap () {

		Chunk.chunks.Clear ();

		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {

				Coords c = new Coords (x, y);

				Chunk.chunks.Add (c, new Chunk ());
				Chunk.chunks[c].State = ChunkState.UndiscoveredSea;
			}
		}

		mapData = new MapData (MapScale);

		int islandAmount = Mathf.RoundToInt (mapScale / 10);

		for ( int y = 0; y < mapScale ; ++y ) {

			for (int i = 0; i < islandAmount; ++i ) {

				int x = Random.Range ( 0, mapScale );

				Coords c = new Coords ( x , y );

				if (Chunk.GetChunk(c).State == ChunkState.UndiscoveredSea) {
					Chunk.GetChunk (c).SetIslandData (new IslandData (StoryType.Normal));
				}
			}


		}
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

	#region load & save
	public void LoadIslandsData () {
		
//		MapData.Instance = new MapData ();

		MapData.Instance = SaveManager.Instance.GameData.mapData;

		Chunk[][] ou = SaveTool.Instance.LoadWorldChunks ();

		Chunk.chunks = FromChunkArray(ou);

	}

	public void SaveImportantIslandPositions () {
		
		SaveManager.Instance.GameData.mapData = MapData.Instance;

	}

	public Chunk[][] toChunkArray ( Dictionary<Coords,Chunk> chunkDico ) {

		Chunk[][] tmpChunks = new Chunk[MapGenerator.Instance.MapScale][];

		for (int i = 0; i < MapGenerator.Instance.MapScale; i++) {
			tmpChunks[i] = new Chunk[MapGenerator.Instance.MapScale];
		}

		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {
				tmpChunks [x] [y] = chunkDico [new Coords (x, y)];
			}
		}

		return tmpChunks;
	}

	public Dictionary<Coords,Chunk> FromChunkArray ( Chunk[][] bufferChunks ) {

		Dictionary<Coords,Chunk> chunkDico = new Dictionary<Coords, Chunk> ();
		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {
				chunkDico.Add (new Coords (x, y), bufferChunks [x] [y]);
			}
		}

		return chunkDico;
	}
	#endregion

	public int MapScale {
		get {
			return mapScale;
		}
	}
}
