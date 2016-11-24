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
			// CLUES
		for ( int i = 0; i < ClueManager.Instance.ClueAmount; ++i)
        {
            
			IslandManager.Instance.ClueIslandsXPos[i] = RandomX;
			IslandManager.Instance.ClueIslandsYPos[i] = RandomY;

			islandIds 	[IslandManager.Instance.ClueIslandsXPos[i], IslandManager.Instance.ClueIslandsYPos[i]] 	= islandID;
			++islandID;

        }

			// TREASURE
		IslandManager.Instance.TreasureIslandXPos = RandomX;
		IslandManager.Instance.TreasureIslandYPos = RandomY;

		islandIds 	[IslandManager.Instance.TreasureIslandXPos, IslandManager.Instance.TreasureIslandYPos] 	= islandID;
		++islandID;

			// HOME
		IslandManager.Instance.HomeIslandXPos = RandomX;
		IslandManager.Instance.HomeIslandYPos = RandomY;

		MapManager.Instance.PosX = IslandManager.Instance.HomeIslandXPos;
		MapManager.Instance.PosY = IslandManager.Instance.HomeIslandYPos;

		islandIds 	[IslandManager.Instance.HomeIslandXPos, IslandManager.Instance.HomeIslandYPos] 	= islandID;
		++islandID;
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

		IslandManager.Instance.SetIsland ();
		StoryLoader.Instance.CurrentIslandStory = StoryLoader.Instance.Stories[1];

		mapImage.InitImage ();
		MapManager.Instance.UpdateImage ();

		IslandManager.Instance.Enter ();

		StoryLoader.Instance.CurrentIslandStory = null;


		#endregion

	}

	public int RandomX {
		get {
			return Random.Range ( 0, mapImage.TextureScale );
		}
	}

	public int RandomY {
		get {

			int y1 = Random.Range(0, (mapImage.TextureScale / 2) - (noManSeaScale / 2));
			int y2 = Random.Range((mapImage.TextureScale / 2) + (noManSeaScale / 2), mapImage.TextureScale);

			return Random.value > 0.5f ? Mathf.RoundToInt(y1) : Mathf.RoundToInt(y2);
		}
	}

}
