using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NavigationManager.Instance.EnterNewChunk += UpdateBackgroundImage;
		UpdateBackgroundImage ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
}
