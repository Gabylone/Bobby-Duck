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

	public static Dictionary<Coords,Chunk> chunks = new Dictionary<Coords, Chunk>();

	void Awake () {
		Instance = this;
	}

	public void GenerateIslands () {
		CreateMapData ();
	}

	#region map data
	private void CreateMapData () {

		for (int x = 0; x < mapScale; x++) {
			for (int y = 0; y < mapScale; y++) {

				Coords c = new Coords (x, y);

				chunks.Add (c, new Chunk ());
				chunks [c].State = ChunkState.UndiscoveredSea;
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

					Coords c = new Coords ( x , y );

					if (GetChunk(c).State == ChunkState.UndiscoveredSea) {
						GetChunk(c).IslandData = new IslandData(c);
					}
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

		chunks = fromChunkArray(SaveManager.Instance.CurrentData.chunks);

	}

	public void SaveIslandsData () {
		SaveManager.Instance.CurrentData.mapData = MapData.Instance;
		SaveManager.Instance.CurrentData.chunks = toChunkArray(chunks);
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

	public Dictionary<Coords,Chunk> fromChunkArray ( Chunk[][] bufferChunks ) {

		Dictionary<Coords,Chunk> chunkDico = new Dictionary<Coords, Chunk> ();
		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {
				chunkDico.Add (new Coords (x, y), bufferChunks [x] [y]);
			}
		}

		return chunkDico;
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

	public Chunk CurrentChunk {
		get {
			return chunks [NavigationManager.CurrentCoords];
		}
	}


	public Chunk GetChunk (Coords c) {
		if (chunks.ContainsKey (c) == false) {

			Debug.LogError ("LES COORDONNEES " + c.ToString() + " ne sont pas dans le dico");

			return chunks [new Coords ()];
		}
		return chunks [c];
	}
}
