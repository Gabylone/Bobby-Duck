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
	private Color discoveredIsland_Color;
	[SerializeField]
	private Color visitedIsland_Color;
	[SerializeField]
	private Color discoveredSea_Color;
	[SerializeField]
	private Color undiscoveredSea_Color;
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

				if (chunk.State == ChunkState.VisitedIsland)
					print ("initing visited island");
			}


		}
		UpdateTexture (texture);

		UpdateBoatSurroundings ();
	}
	#endregion

	#region update boat surroundings
	public void UpdateBoatSurroundings () {

		Texture2D texture = (Texture2D)targetImage.mainTexture;

		int shipRange = Boats.Instance.PlayerBoatInfo.ShipRange;

		int posX = Boats.Instance.PlayerBoatInfo.PosX;
		int posY = Boats.Instance.PlayerBoatInfo.PosY;

		int mapScale = MapGenerator.Instance.MapScale;

		Chunk previousChunk = MapGenerator.Instance.Chunks [Boats.Instance.PlayerBoatInfo.PreviousPosX, Boats.Instance.PlayerBoatInfo.PreviousPosY];
		SetPixel (texture,Boats.Instance.PlayerBoatInfo.PreviousPosX, Boats.Instance.PlayerBoatInfo.PreviousPosY, getChunkColor (previousChunk));

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				int pX = posX + x;
				int pY = posY + y;

				if (pX < mapScale && pX >= 0 &&
					pY < mapScale && pY >= 0) {


					Chunk chunk = MapGenerator.Instance.Chunks [pX, pY];

					Color color = Color.red;

					switch (chunk.State) {
					case ChunkState.UndiscoveredSea:
						chunk.State = ChunkState.DiscoveredSea;
						break;
					case ChunkState.UndiscoveredIsland:
						print ("mark island as discoverd");
						chunk.State = ChunkState.DiscoveredIsland;
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
			if ( boatInfo.PosX <= Boats.Instance.PlayerBoatInfo.PosX + Boats.Instance.PlayerBoatInfo.ShipRange && boatInfo.PosX >= Boats.Instance.PlayerBoatInfo.PosX -Boats.Instance.PlayerBoatInfo.ShipRange &&
				boatInfo.PosY <= Boats.Instance.PlayerBoatInfo.PosY + Boats.Instance.PlayerBoatInfo.ShipRange && boatInfo.PosY >= Boats.Instance.PlayerBoatInfo.PosY - Boats.Instance.PlayerBoatInfo.ShipRange) {
				SetPixel (texture,boatInfo.PosX, boatInfo.PosY, Color.green);
			} else {
				SetPixel (texture,boatInfo.PreviousPosX, boatInfo.PreviousPosY, getChunkColor(MapGenerator.Instance.Chunks[boatInfo.PreviousPosX,boatInfo.PreviousPosY]) );
				SetPixel (texture,boatInfo.PosX, boatInfo.PosY, getChunkColor(MapGenerator.Instance.Chunks[boatInfo.PosX,boatInfo.PosY]) );
			}
		}

		UpdateTexture (texture);
	}

	private Color getChunkColor_Reveal (Chunk chunk) {

		switch (chunk.State) {
		case ChunkState.UndiscoveredSea:
			return discoveredSea_Color;
			break;
		case ChunkState.DiscoveredSea:
			return discoveredSea_Color;
			break;
		case ChunkState.UndiscoveredIsland:
			return discoveredIsland_Color;
			break;
		case ChunkState.DiscoveredIsland:
			return discoveredIsland_Color;
			break;
		case ChunkState.VisitedIsland:
			return visitedIsland_Color;
			break;
		default:
			return Color.black;
			break;
		}
	}
	private Color getChunkColor (Chunk chunk) {

		switch (chunk.State) {
		case ChunkState.UndiscoveredSea:
			return undiscoveredSea_Color;
			break;
		case ChunkState.DiscoveredSea:
			return discoveredSea_Color;
			break;
		case ChunkState.UndiscoveredIsland:
			return undiscoveredSea_Color;
			break;
		case ChunkState.DiscoveredIsland:
			return discoveredIsland_Color;
			break;
		case ChunkState.VisitedIsland:
			return visitedIsland_Color;
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
		Vector2 boatPos = new Vector2 (Boats.Instance.PlayerBoatInfo.PosX,Boats.Instance.PlayerBoatInfo.PosY);
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
		PlayerLoot.Instance.Close ();
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
