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

	[SerializeField]
	private MapData mapData;

	void Awake () {
		Instance = this;
	}

	public void GenerateIslands () {
		CreateMapData ();
	}

	#region map data
	private void CreateMapData () {
		
		mapData = new MapData (MapScale);

		for ( int x = 0; x < mapScale; ++x ) {
			for ( int y = 0; y < mapScale; ++y )
				MapData.Instance.chunks [x, y].state = State.UndiscoveredSea;
		}

		int islandAmount = Mathf.RoundToInt (mapScale / 10);

		for ( int y = 0; y < mapScale ; ++y ) {

			for (int i = 0; i < islandAmount; ++i ) {

				bool isInNoMansSea =
					y > (mapScale / 2) - (noManSeaScale/2)
					&& y < (mapScale / 2) + (noManSeaScale/2);

				if ( isInNoMansSea == false ) {
					int x = Random.Range ( 0, mapScale );

					if (MapData.Instance.chunks[x,y].state == State.UndiscoveredSea) {
						MapData.Instance.chunks [x, y].IslandData = new IslandData(x,y);
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

	}

	public void SaveIslandsData () {
		SaveManager.Instance.CurrentData.mapData = MapData.Instance;
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
