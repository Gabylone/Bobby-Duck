using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayMinimap : MonoBehaviour {

	public GameObject minimapChunkPrefab;

	public GameObject parent;

	public Image minimapImage;

	public RectTransform boatRectTransform;

	public RectTransform scrollView;

	public float overallDecal = 100f;

	public Transform minimapChunkParent;

	private Vector2 initScale = Vector2.zero;
	private Vector2 initPos = Vector2.zero;

	public float centerTweenDuration = 0.5f;

	public float openOverallMapDuration = 0.5f;

	Vector2 scale;

	public int range = 1;

	public float mapScaleBuffer;

	// Use this for initialization
	void Start () {
//		scale = minimapChunkPrefab.GetComponent<RectTransform>().sizeDelta;

		InitMap ();
		UpdateBackgroundImage ();

		initPos = scrollView.anchoredPosition;
		initScale = scrollView.sizeDelta;

		boatRectTransform.sizeDelta = scale;

		NavigationManager.Instance.EnterNewChunk += UpdateMinimap;

		UpdateMinimap ();
	}

	void Update () {
		if ( Input.GetKeyDown(KeyCode.L) ) {
			OpenOverallMap ();
		}
	}

	void CenterOnBoat() {
		CenterMap (Boats.PlayerBoatInfo.coords);
	}
	void CenterMap (Coords coords)
	{
		float x = minimapImage.rectTransform.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
		x -= scale.x/2f;
		x = Mathf.Clamp (x,0, minimapImage.rectTransform.rect.width - scrollView.rect.width);

		float y = minimapImage.rectTransform.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
		y -= scale.y/2f;
		y = Mathf.Clamp (y,0, minimapImage.rectTransform.rect.height - scrollView.rect.height);

		Vector2 targetPos = new Vector2(-x,-y) + scale;

		HOTween.To (minimapImage.rectTransform, centerTweenDuration, "anchoredPosition", targetPos, false, EaseType.Linear, 0f);
	}

	#region init
	void InitMap ()
	{

		scale = new Vector2(minimapChunkPrefab.GetComponent<RectTransform>().rect.width,minimapChunkPrefab.GetComponent<RectTransform>().rect.height);

		minimapImage.rectTransform.sizeDelta = scale * (MapGenerator.Instance.MapScale + (mapScaleBuffer*2f));
//		minimapImage.rectTransform.sizeDelta = scale * (MapGenerator.Instance.MapScale);

		for (int x = 0; x <= MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y <= MapGenerator.Instance.MapScale; y++) {
				
				Coords c = new Coords (x, y);

				if (Chunk.GetChunk (c).State == ChunkState.DiscoveredIsland
					|| Chunk.GetChunk (c).State == ChunkState.VisitedIsland) {
					PlaceMapChunk (c);
				}
			}
		}
//
//
//		UpdateMinimap ();

	}

	void UpdateBoatRange ()
	{
		Debug.Log ("updating ship range");

		for (int x = -Boats.PlayerBoatInfo.ShipRange; x <= Boats.PlayerBoatInfo.ShipRange; x++) {
			for (int y = -Boats.PlayerBoatInfo.ShipRange; y <= Boats.PlayerBoatInfo.ShipRange; y++) {

				Coords c = Boats.PlayerBoatInfo.coords + new Coords (x, y);

				Chunk chunk = Chunk.GetChunk (c);

				switch (chunk.State) {
				case ChunkState.UndiscoveredSea:
					chunk.State = ChunkState.DiscoveredSea;
					break;

				case ChunkState.UndiscoveredIsland:
					chunk.State = ChunkState.DiscoveredIsland;
					PlaceMapChunk (c);
					break;

				case ChunkState.DiscoveredIsland:
					break;

				case ChunkState.VisitedIsland:
					break;

				default:
					break;

				}
			}
		}
	}
	#endregion

	#region overall
	void OpenOverallMap () {
		HOTween.To (scrollView, openOverallMapDuration , "sizeDelta" , new Vector2(Screen.width , Screen.height) );
		HOTween.To (scrollView, openOverallMapDuration , "anchoredPosition" , new Vector2(Screen.width , Screen.height)/2f );

		print ("screen x : " + Screen.width);
		print ("screen y : " + Screen.height);
	}
	#endregion

	#region minimap
	void UpdateMinimap ()
	{
		print ("oui?");
		UpdateBoatRange ();

		CenterOnBoat ();
		MoveBoatIcon ();
		UpdateBackgroundImage ();
	}

	void MoveBoatIcon () {

		Vector2 boatPos = new Vector2 ((scale.x / 2f) + Boats.PlayerBoatInfo.coords.x * scale.x, (scale.y / 2f) + Boats.PlayerBoatInfo.coords.y * scale.y);

		HOTween.To (boatRectTransform, centerTweenDuration-0.2f, "anchoredPosition", boatPos, false, EaseType.Linear, 0.2f);
//		HOTween.To (boatRectTransform, centerTweenDuration, "anchoredPosition", boatPos, false, EaseType.Linear, centerTweenDuration);

//		boatRectTransform.anchoredPosition = boatPos;

		Tween.Bounce (boatRectTransform.transform);
		//
	}

	void PlaceMapChunk(Coords c) {

		// INST
		GameObject minimapChunk = Instantiate (minimapChunkPrefab, parent.transform);

		// SCALE
		minimapChunk.transform.localScale = Vector3.one;

		// POS
		Vector3 pos;

		//Vector2 decal = scale / (Boats.PlayerBoatInfo.ShipRange * 2 + 1);
		pos = new Vector3 ( (scale.x/2) + (c.x * minimapImage.rectTransform.rect.width / MapGenerator.Instance.MapScale) , (scale.y/2) +  c.y * scale.y, 0f);

		minimapChunk.GetComponent<RectTransform>().anchoredPosition = pos;
//		minimapChunk.GetComponent<RectTransform> ().sizeDelta = new Vector2(10,10);

//		Coords worldCoords = Boats.PlayerBoatInfo.CurrentCoords + new Coords(c.x,-c.y);

		minimapChunk.GetComponent<MinimapChunk> ().UpdateChunk (c);
	}
	#endregion

	#region make background image
	public Image targetImage;
	public Color color_VisitedSea;
	public Color color_UnvisitedSea;

	void UpdateBackgroundImage () {

		Texture2D texture = new Texture2D (MapGenerator.Instance.MapScale, MapGenerator.Instance.MapScale);

		texture.filterMode = FilterMode.Point;
		texture.anisoLevel = 0;
		texture.mipMapBias = 0;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.Resize (MapGenerator.Instance.MapScale, MapGenerator.Instance.MapScale);

		for (int x = 0; x < MapGenerator.Instance.MapScale; x++) {
			for (int y = 0; y < MapGenerator.Instance.MapScale; y++) {

				Chunk chunk = Chunk.GetChunk (new Coords (x, y));

				if (chunk.State == ChunkState.DiscoveredSea
					|| chunk.State == ChunkState.VisitedIsland
					|| chunk.State == ChunkState.DiscoveredIsland) {

					texture.SetPixel (x, y, color_VisitedSea);

				} else {
					texture.SetPixel (x, y, color_UnvisitedSea);
				}
			}
		}

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, MapGenerator.Instance.MapScale,  MapGenerator.Instance.MapScale) , Vector2.one * 0.5f );

	}
	#endregion
}
