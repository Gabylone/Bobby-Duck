using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	[SerializeField]
	private Vector2 islandRange = new Vector2( 330 , 120 );

	[SerializeField]
	private int noManSeaScale = 3;

	[SerializeField]
	private int loadLimit = 1000;

	MapImage mapImage;

	// Use this for initialization
	void Start () {

	}

	public void GenerateIslands () {

		mapImage = GetComponent<MapImage> ();

		MapManager.Instance.CheckIsland = new bool[mapImage.TextureScale, mapImage.TextureScale];
		MapManager.Instance.IslandPositions = new Vector2[mapImage.TextureScale, mapImage.TextureScale];
		MapManager.Instance.IslandLoots = new Loot [mapImage.TextureScale, mapImage.TextureScale];

		StoryLoader.Instance.IslandStories = new Story[mapImage.TextureScale,mapImage.TextureScale];

		StartCoroutine (GenerateIslandsCoroutine ());
	}

	private IEnumerator GenerateIslandsCoroutine () {
		
		int currentLoad = 0;

		int islandAmount = Mathf.RoundToInt (mapImage.TextureScale / 10);

		int y1 = Random.Range (0, (mapImage.TextureScale / 2) - (noManSeaScale / 2));
		int y2 = Random.Range ((mapImage.TextureScale / 2) + (noManSeaScale / 2), mapImage.TextureScale);
		int treasureY = Random.value > 0.5f ? Mathf.RoundToInt (y1) : Mathf.RoundToInt (y2);

		for ( int y = 0; y < mapImage.TextureScale ; ++y ) {

			for (int i = 0; i < islandAmount; ++i ) {

				int x = Random.Range ( 0, mapImage.TextureScale );



				bool isInNoMansSea =
					y > (mapImage.TextureScale / 2) - (noManSeaScale/2)
					&& y < (mapImage.TextureScale / 2) + (noManSeaScale/2);

				if (!MapManager.Instance.CheckIsland[x,y]
					&& isInNoMansSea == false) {

					if ( i == 0 && y == treasureY ) {
						//
					}

					MapManager.Instance.CheckIsland 	[x, y] 	= true;
					MapManager.Instance.IslandPositions [x, y] 	= new Vector2 ( Random.Range (-islandRange.x,islandRange.x), Random.Range (-islandRange.y, islandRange.y) );

				}

				++currentLoad;

				if (currentLoad > loadLimit) {
					yield return new WaitForEndOfFrame ();
					currentLoad = 0;
				}
			}


		}

		yield return new WaitForEndOfFrame ();

		mapImage.InitImage ();
		NavigationManager.Instance.Move (Directions.None);

	}
}
