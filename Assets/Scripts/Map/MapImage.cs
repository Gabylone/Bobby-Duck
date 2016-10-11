using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapImage : MonoBehaviour {

	[SerializeField]
	private Image targetImage;

	[SerializeField]
	private int textureWidth = 100;

	[SerializeField]
	private int textureHeight = 100;

	[SerializeField]
	private Color mapColor = Color.magenta;

	public void InitImage () {

		Texture2D texture = new Texture2D (textureWidth, textureHeight);

		for ( int x = 0; x < textureWidth ; ++x ) {
			for (int y = 0; y < textureHeight; ++y ) {
				texture.SetPixel ( x , y , mapColor );
			}
		}

		texture.filterMode = FilterMode.Point;

		Sprite sprite = Sprite.Create (texture, new Rect (0, 0, textureWidth , textureHeight) , Vector2.one * 0.5f );
		targetImage.sprite = sprite;
	}

	public void UpdateCurrentPixel (Color color) {

		Texture2D texture = (Texture2D)targetImage.mainTexture;

		texture.SetPixel (MapManager.Instance.PosX, MapManager.Instance.PosY, color);

		texture.filterMode = FilterMode.Point;

//		UpdateImagePosition ();

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, textureWidth , textureHeight) , Vector2.one * 0.5f );

	}

	public void UpdateImagePosition () {
		int x = (300 * MapManager.Instance.PosX) / textureWidth;
		int y = (300 * MapManager.Instance.PosY) / textureHeight;
		targetImage.transform.localPosition = new Vector2 (300 - x , (textureHeight- y) );
	}

	#region properties
	public int TextureWidth {
		get {
			return textureWidth;
		}
		set {
			textureWidth = value;
		}
	}

	public int TextureHeight {
		get {
			return textureHeight;
		}
		set {
			textureHeight = value;
		}
	}
	#endregion
}
