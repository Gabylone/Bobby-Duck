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

	void HandleOnPlayerMove (Coords previousCoords, Coords newCoords)
	{
		Paint (previousCoords, TileSet.current.GetTile(previousCoords).type );
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
