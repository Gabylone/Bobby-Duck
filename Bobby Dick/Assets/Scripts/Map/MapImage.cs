using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapImage : MonoBehaviour {

	public static MapImage Instance;

	[Header("General")]
	[SerializeField]
	private Image targetImage;
	[SerializeField]
	private bool revealMap = false;
	[SerializeField]
	[Range(0,1)]
	private float mapTransparency = 0.5f;

	[Header("Colors")]
	[SerializeField]
	private Color unvisitedIslandColor;
	[SerializeField]
	private Color visitedIslandColor;
	[SerializeField]
	private Color discoveredColor;
	[SerializeField]
	private Color undiscoveredColor;
	[SerializeField]
	private Color boatPositionColor;

	[SerializeField]
	private float maxImagePosition = 250f;

	void Awake() {
		Instance = this;
	}

	void Start () {
		if (revealMap)
			NavigationManager.Instance.EnterNewChunk += InitImage;
		else
			NavigationManager.Instance.EnterNewChunk += UpdateBoatSurroundings;

	}

	#region initialization
	public void InitImage () {

		Texture2D texture = new Texture2D (MapGenerator.Instance.MapScale, MapGenerator.Instance.MapScale);

		for ( int x = 0; x < MapGenerator.Instance.MapScale ; ++x ) {
			
			for (int y = 0; y < MapGenerator.Instance.MapScale; ++y ) {

				Chunk chunk = MapData.Instance.chunks [x, y];

				if (revealMap) {

					texture.SetPixel (x, y, (chunk.state == State.UndiscoveredIsland ) ? Color.yellow : Color.blue);

				} else {
					texture.SetPixel (x, y, getChunkColor (chunk) );
				}
			}
		}

		foreach ( BoatInfo boatInfo in Boats.Instance.BoatInfos ) {
			texture.SetPixel (boatInfo.PosX, boatInfo.PosY, Color.green);
		}
//
		UpdateTexture (texture);

		UpdateBoatSurroundings ();
	}
	#endregion

	#region update boat surroundings
	public void UpdateBoatSurroundings () {
		
		Texture2D texture = (Texture2D)targetImage.mainTexture;

		int shipRange = PlayerBoatInfo.Instance.ShipRange;

		int posX = PlayerBoatInfo.Instance.PosX;
		int posY = PlayerBoatInfo.Instance.PosY;

		int mapScale = MapGenerator.Instance.MapScale;

		Chunk previousChunk = MapData.Instance.chunks [PlayerBoatInfo.Instance.PreviousPosX, PlayerBoatInfo.Instance.PreviousPosY];
		texture.SetPixel (previousChunk.x, previousChunk.y, getChunkColor (previousChunk));

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				int pX = posX + x;
				int pY = posY + y;

				if (pX < mapScale && pX >= 0 &&
					pY < mapScale && pY >= 0) {


					Chunk chunk = MapData.Instance.chunks [pX, pY];

					Color color = Color.red;

					switch (chunk.state) {
					case State.UndiscoveredSea:
						chunk.state = State.DiscoveredSea;
						break;
					case State.UndiscoveredIsland:
						chunk.state = State.DiscoveredIsland;
						break;
					}

					texture.SetPixel (pX, pY, getChunkColor (chunk) );

				}

			}

		}

		UpdateTexture (texture);

	}

	private Color getChunkColor (Chunk chunk) {

		if ( chunk.x == PlayerBoatInfo.Instance.PosX && chunk.y == PlayerBoatInfo.Instance.PosY  ) {
			return Color.red;
		}

		switch (chunk.state) {
		case State.UndiscoveredSea:
			return undiscoveredColor;
			break;
		case State.DiscoveredSea:
			return discoveredColor;
			break;
		case State.UndiscoveredIsland:
			return undiscoveredColor;
			break;
		case State.DiscoveredIsland:
			return unvisitedIslandColor;
			break;
		case State.VisitedIsland:
			return visitedIslandColor;
			break;
		default:
			return Color.black;
			break;
		}
	}
	#endregion

	#region image
	private void UpdateTexture (Texture2D texture) {

		texture.filterMode = FilterMode.Point;

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, MapGenerator.Instance.MapScale ,  MapGenerator.Instance.MapScale) , Vector2.one * 0.5f );
	}
	#endregion

	#region properties
	public Image TargetImage {
		get {
			return targetImage;
		}
	}
	#endregion
}
