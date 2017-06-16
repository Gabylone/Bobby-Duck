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
	private UIButton mapButton;

	private bool opened = false;

	[SerializeField]
	private float maxImagePosition = 250f;

	[SerializeField]
	private int pixelFactor = 2;

	[Header ("Center On Boat")]
	[SerializeField]
	private RectTransform contentTransform;
	[SerializeField]
	private float maxContentPosition = 150f;


	void Awake() {
		Instance = this;
	}

	#region initialization
	public void Init () {
		NavigationManager.Instance.EnterNewChunk += UpdateBoatSurroundings;
	}

	public void Reset ()
	{
		
	}

	public void InitImage () {

		Texture2D texture = new Texture2D (MapGenerator.Instance.MapScale, MapGenerator.Instance.MapScale);
//
		for ( int x = 0; x < MapGenerator.Instance.MapScale; ++x ) {

			for (int y = 0; y < MapGenerator.Instance.MapScale; ++y ) {

				Chunk chunk = MapGenerator.Instance.Chunks [x, y];
				SetPixel (texture,x,y, revealMap ? getChunkColor_Reveal (chunk) : getChunkColor (chunk));

			}


		}
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

		Chunk previousChunk = MapGenerator.Instance.Chunks [PlayerBoatInfo.Instance.PreviousPosX, PlayerBoatInfo.Instance.PreviousPosY];
		SetPixel (texture,PlayerBoatInfo.Instance.PreviousPosX, PlayerBoatInfo.Instance.PreviousPosY, getChunkColor (previousChunk));

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				int pX = posX + x;
				int pY = posY + y;

				if (pX < mapScale && pX >= 0 &&
					pY < mapScale && pY >= 0) {


					Chunk chunk = MapGenerator.Instance.Chunks [pX, pY];

					Color color = Color.red;

					switch (chunk.state) {
					case State.UndiscoveredSea:
						chunk.state = State.DiscoveredSea;
						break;
					case State.UndiscoveredIsland:
						chunk.state = State.DiscoveredIsland;
						break;
					}

					SetPixel (texture,pX, pY, getChunkColor (chunk));


				}

			}

		}

		SetPixel (texture,posX, posY, Color.red);

		UpdateTexture (texture);

		CheckForBoats ();

	}

	public void CheckForBoats ()
	{
		Texture2D texture = (Texture2D)targetImage.mainTexture;

		foreach ( OtherBoatInfo boatInfo in Boats.Instance.OtherBoatInfos ) {
			if ( boatInfo.PosX <= PlayerBoatInfo.Instance.PosX + PlayerBoatInfo.Instance.ShipRange && boatInfo.PosX >= PlayerBoatInfo.Instance.PosX -PlayerBoatInfo.Instance.ShipRange &&
				boatInfo.PosY <= PlayerBoatInfo.Instance.PosY + PlayerBoatInfo.Instance.ShipRange && boatInfo.PosY >= PlayerBoatInfo.Instance.PosY - PlayerBoatInfo.Instance.ShipRange) {
				SetPixel (texture,boatInfo.PosX, boatInfo.PosY, Color.green);
			} else {
//				SetPixel (texture,boatInfo.PosX, boatInfo.PosY, Color.magenta );
				SetPixel (texture,boatInfo.PreviousPosX, boatInfo.PreviousPosY, getChunkColor(MapGenerator.Instance.Chunks[boatInfo.PosX,boatInfo.PosY]) );
				SetPixel (texture,boatInfo.PosX, boatInfo.PosY, getChunkColor(MapGenerator.Instance.Chunks[boatInfo.PosX,boatInfo.PosY]) );
			}
		}

		UpdateTexture (texture);
	}

	private Color getChunkColor_Reveal (Chunk chunk) {

		switch (chunk.state) {
		case State.UndiscoveredSea:
			return discoveredColor;
			break;
		case State.DiscoveredSea:
			return discoveredColor;
			break;
		case State.UndiscoveredIsland:
			return unvisitedIslandColor;
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
	private Color getChunkColor (Chunk chunk) {

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
	private void SetPixel (Texture2D text, int x,int y,Color c) {
		for (int iX = 0; iX < pixelFactor; iX++) {
			for (int iY = 0; iY < pixelFactor; iY++) {
				text.SetPixel ((pixelFactor*x)+iX,(pixelFactor*y)+iY, c);
			}
		}
	}
	private void UpdateTexture (Texture2D texture) {

		texture.filterMode = FilterMode.Point;
		texture.anisoLevel = 0;
		texture.mipMapBias = 0;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, MapGenerator.Instance.MapScale,  MapGenerator.Instance.MapScale) , Vector2.one * 0.5f );
	}
	public void CenterOnBoat () {
		Vector2 boatPos = new Vector2 (PlayerBoatInfo.Instance.PosX,PlayerBoatInfo.Instance.PosY);
		boatPos = (boatPos * maxContentPosition) / MapGenerator.Instance.MapScale;

		boatPos -= Vector2.one * (maxContentPosition / 2);

		boatPos = -boatPos;

		contentTransform.localPosition = boatPos;
	}
	#endregion

	#region properties
	public Image TargetImage {
		get {
			return targetImage;
		}
	}
	#endregion

	public void OpenMap () {
		mapButton.Opened = true;
		CenterOnBoat ();
	}
	public void CloseMap () {
		mapButton.Opened = false;
	}

	public UIButton MapButton {
		get {
			return mapButton;
		}
	}
}
