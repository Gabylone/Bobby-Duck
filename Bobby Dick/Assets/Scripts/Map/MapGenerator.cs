using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance;

	[SerializeField]
	private MapImage mapImage;

	[SerializeField]
	private int mapScale = 100;

	[SerializeField]
	private int noManSeaScale = 3;

	[SerializeField]
	private int loadLimit = 1000;

	public int islandID;

	private MapData mapData;
	private Chunk[,] chunks;

	public Chunk[,] Chunks {
		get {
			return chunks;
		}
		set {
			chunks = value;
		}
	}

	void Awake () {
		Instance = this;
	}

	public void GenerateIslands () {
		CreateMapData ();
	}

	#region map data
	private void CreateMapData () {

		MapGenerator.Instance.Chunks = new Chunk[mapScale,mapScale];

		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {
				Chunks [x, y] = new Chunk ();
				Chunks [x, y].State = ChunkState.UndiscoveredSea;
			}
		}

		mapData = new MapData (MapScale);

		int islandAmount = Mathf.RoundToInt (mapScale / 10);

		for ( int y = 0; y < mapScale ; ++y ) {

			for (int i = 0; i < islandAmount; ++i ) {

				bool isInNoMansSea =
					y > (mapScale / 2) - (noManSeaScale/2)
					&& y < (mapScale / 2) + (noManSeaScale/2);

				if ( isInNoMansSea == false ) {
					int x = Random.Range ( 0, mapScale );

					if (Chunks[x,y].State == ChunkState.UndiscoveredSea) {
						Chunks [x, y].IslandData = new IslandData(x,y);
					}
				}
			}


		}
	}
	#endregion

	#region tools
	public int RandomX {
		get {
			return Random.Range ( 0, mapScale );
		}
	}

	public int RandomY {
		get {

			int y1 = Random.Range(0, (mapScale / 2) - (noManSeaScale / 2));
			int y2 = Random.Range((mapScale / 2) + (noManSeaScale / 2), mapScale);

			return Random.value > 0.5f ? Mathf.RoundToInt(y1) : Mathf.RoundToInt(y2);
		}
	}
	#endregion

	#region load & save
	public void LoadIslandsData () {
		MapData.Instance = new MapData ();

		MapData.Instance = SaveManager.Instance.CurrentData.mapData;

		Chunks = fromChunkArray (SaveManager.Instance.CurrentData.chunkArray);

		foreach (Chunk cun in chunks) {
			if (cun.State == ChunkState.VisitedIsland)
				print ("load visited island");
		}

		foreach (Chunk cun in chunks) {
			if (cun.State == ChunkState.DiscoveredIsland)
				print ("load discovered island");
		}

		foreach (Chunk cun in chunks) {
			if (cun.State == ChunkState.DiscoveredSea)
				print ("load discovered sea");
		}

	}

	public void SaveIslandsData () {
		SaveManager.Instance.CurrentData.mapData = MapData.Instance;
		SaveManager.Instance.CurrentData.chunkArray = toChunkArray (Chunks);

	}
	public Chunk[][] toChunkArray ( Chunk[,] bufferChunks ) {

		Chunk[][] tmpChunks = new Chunk[MapGenerator.Instance.MapScale][];
		for (int i = 0; i < MapGenerator.Instance.MapScale; i++) {
			tmpChunks[i] = new Chunk[MapGenerator.Instance.MapScale];
		}

		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {
				tmpChunks [x] [y] = bufferChunks [x, y];
			}
		}

		return tmpChunks;
	}

	public Chunk[,] fromChunkArray ( Chunk[][] bufferChunks ) {

		Chunk[,] tmpChunks = new Chunk[MapGenerator.Instance.MapScale,MapGenerator.Instance.MapScale];

		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {
				tmpChunks [x,y] = bufferChunks [x][y];
			}
		}

		return tmpChunks;
	}
	#endregion

	public int NoManSeaScale {
		get {
			return noManSeaScale;
		}
	}

	public int MapScale {
		get {
			return mapScale;
		}
	}
}
