using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance;

	[SerializeField]
	private Vector2 islandRange = new Vector2( 350 , 160 );

	private List<IslandData> islandDatas = new List<IslandData>();
	private int[,] islandIds;

	[SerializeField]
	private int noManSeaScale = 3;

	public int[,] IslandIds {
		get {
			return islandIds;
		}
	}

	public List<IslandData> IslandDatas {
		get {
			return islandDatas;
		}
	}

	[SerializeField]
	private int loadLimit = 1000;

	MapImage mapImage;

	// Use this for initialization
	void Start () {
		Instance = this;
	}

	public void GenerateIslands () {

		mapImage = GetComponent<MapImage> ();

		islandIds = new int[mapImage.TextureScale, mapImage.TextureScale];
		for ( int x = 0; x < mapImage.TextureScale; ++x ) {
			for ( int y = 0; y < mapImage.TextureScale; ++y )
				islandIds [x, y] = -1;
		}

		StartCoroutine (GenerateIslandsCoroutine ());
	}

	private IEnumerator GenerateIslandsCoroutine () {
		
		int currentLoad = 0;

		int islandAmount = Mathf.RoundToInt (mapImage.TextureScale / 10);
		int islandID = 0;

		#region clues & treasure island
		ClueManager.Instance.Clue_XPos = new int[ClueManager.Instance.ClueAmount];
		ClueManager.Instance.Clue_YPos = new int[ClueManager.Instance.ClueAmount];

        for ( int i = 0; i < ClueManager.Instance.ClueAmount; ++i)
        {
            int y1 = Random.Range(0, (mapImage.TextureScale / 2) - (noManSeaScale / 2));
            int y2 = Random.Range((mapImage.TextureScale / 2) + (noManSeaScale / 2), mapImage.TextureScale);
            int y = Random.value > 0.5f ? Mathf.RoundToInt(y1) : Mathf.RoundToInt(y2);
			int x = Random.Range ( 0, mapImage.TextureScale );

			islandIds 	[x, y] 	= islandID;
			++islandID;

			if ( i == 0 ) {
				ClueManager.Instance.TreasureIslandY = y;
				ClueManager.Instance.TreasureIslandX = x;
			} else {
				ClueManager.Instance.Clue_XPos [i - 1] = x;
				ClueManager.Instance.Clue_YPos [i - 1] = y;
			}

        }
		#endregion


		#region island
		for ( int y = 0; y < mapImage.TextureScale ; ++y ) {

			for (int i = 0; i < islandAmount; ++i ) {

				bool isInNoMansSea =
					y > (mapImage.TextureScale / 2) - (noManSeaScale/2)
					&& y < (mapImage.TextureScale / 2) + (noManSeaScale/2);

				if ( isInNoMansSea == false ) {
					int x = Random.Range ( 0, mapImage.TextureScale );

					if (islandIds[x,y] == -1) {
						
						islandIds 	[x, y] 	= islandID;

						Vector2 islandPos = new Vector2(Random.Range (-islandRange.x,islandRange.x) , Random.Range(-islandRange.y ,islandRange.y) );
						islandDatas.Add ( new IslandData( islandPos) );

						++islandID;
					}
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

		#endregion

	}
}
