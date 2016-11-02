using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapImage : MonoBehaviour {

	[SerializeField]
	private Image targetImage;

	[SerializeField]
	private Color hiddenColor;

	[SerializeField]
	private int textureScale = 100;

	[SerializeField]
	private Color mapColor = Color.magenta;

	public void InitImage () {

//		Texture2D texture = targetImage.sprite.texture;
		Texture2D texture = new Texture2D (textureScale, textureScale);

		for ( int x = 0; x < textureScale ; ++x ) {
			for (int y = 0; y < textureScale; ++y ) {
//				texture.SetPixel (x, y, MapManager.Instance.CheckIsland[x,y] ? Color.yellow : Color.blue);
				texture.SetPixel (x, y, hiddenColor);
			}
		}
//
		texture.filterMode = FilterMode.Point;

		texture.Apply ();
//
		Sprite sprite = Sprite.Create (texture, new Rect (0, 0, textureScale , textureScale) , Vector2.one * 0.5f );
		targetImage.sprite = sprite;
	}

	public void UpdatePixel ( int x , int y, Color color) {

		Texture2D texture = (Texture2D)targetImage.mainTexture;

		texture.SetPixel (x, y, color);

		texture.filterMode = FilterMode.Point;

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, textureScale , textureScale) , Vector2.one * 0.5f );

	}

	public void UpdateImagePosition () {
		int x = (300 * MapManager.Instance.PosX) / textureScale;
		int y = (300 * MapManager.Instance.PosY) / textureScale;
		targetImage.transform.localPosition = new Vector2 (300 - x , (textureScale- y) );
	}

	#region properties
	public int TextureScale {
		get {
			return textureScale;
		}
		set {
			textureScale = value;
		}
	}
	#endregion
}
