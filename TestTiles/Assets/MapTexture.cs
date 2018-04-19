using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTexture : MonoBehaviour {

	public static MapTexture Instance;

	public Image targetImage;
	public Texture2D texture;

	public int range = 1;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		Player.onPlayerMove += HandleOnPlayerMove;
	}

	void Update () 
	{
		if ( Input.GetMouseButtonDown(0) ) {

			Vector2 mousePos = Input.mousePosition;

			float pixelWidth = Screen.width / WorldGeneration.mapScale;
			float pixelHeight = Screen.height/ WorldGeneration.mapScale;

			Debug.Log (pixelWidth);

			float xF = (mousePos.x * WorldGeneration.mapScale / Screen.width) - (0.5f);
			float yF = (mousePos.y * WorldGeneration.mapScale / Screen.height) - (0.5f);

			int x = Mathf.RoundToInt (xF);
			int y = Mathf.RoundToInt (yF);

			Coords origin = new Coords (x,y);

			List<Coords> coords = new List<Coords> ();

			for (int cX = -range; cX <= range; cX++) {
				for (int cY = -range; cY <= range; cY++) {

					Coords c = origin + new Coords (cX, cY);
					if ( c != origin )
						coords.Add (c);
				}
			}

			Paint (origin, Color.magenta);
			foreach (var item in coords) {
				Paint (item, Color.magenta);
			}
			RefreshTexture ();

		}
	}

	void HandleOnPlayerMove (Coords previousCoords, Coords newCoords)
	{
		Paint (previousCoords, Tile.GetTile(previousCoords).type );
		Paint (newCoords, Color.red);
		RefreshTexture ();
	}

	int scale = 0;
	
	#region texture
	public void InitTexture (int scale)
	{
		this.scale = scale;

		texture = new Texture2D (scale, scale);

		texture.filterMode = FilterMode.Point;
		texture.anisoLevel = 0;
		texture.mipMapBias = 0;
		texture.wrapMode = TextureWrapMode.Clamp;

		texture.Resize (scale,scale);
	}
//
//	void Update () {
//		if ( Input.GetKeyDown(KeyCode.R) )
//			ResetTexture ();
//	}
	public void ResetTexture () {
		Color[] colors = new Color[scale*scale];
		for (int i = 0; i < colors.Length; i++) {
			colors[i] = Color.black;
		}
		texture.SetPixels(colors);
	}
	public void RefreshTexture() {
		texture.Apply ();
		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, texture.width,texture.height) , Vector2.one * 0.5f );
	}
	public void Paint ( Coords coords , Color c ) {
		texture.SetPixel (coords.x, coords.y, c);
	}
	public void Paint ( Coords c , Tile.Type tileType ) {
		Paint (c, GetTileColor (tileType));
	}
	#endregion

	#region colors
	public Color[] tileColors;
	public Color GetTileColor ( Tile.Type type ) {
		return tileColors [(int)type];
	}
	#endregion
}
