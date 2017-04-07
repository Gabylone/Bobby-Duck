using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapImage : MonoBehaviour {

	public static MapImage Instance;

	private MapGenerator mapGenerator;

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
	private int textureScale = 100;

	[SerializeField]
	private float maxImagePosition = 250f;

	void Awake() {
		Instance = this;
	}

	public void InitImage () {

		mapGenerator = GetComponent<MapGenerator> ();

		Texture2D texture = new Texture2D (textureScale, textureScale);

		for ( int x = 0; x < textureScale ; ++x ) {
			
			for (int y = 0; y < textureScale; ++y ) {


				if (revealMap) {
					
					texture.SetPixel (x, y, IslandManager.Instance.IslandIds [x, y] > -1 ? Color.yellow : Color.blue);

				} else {

					int id = IslandManager.Instance.IslandIds [x, y];

					Color color = undiscoveredColor;

					if (id > -2) {
						if (id == -1) {
							color = discoveredColor;
						} else {
							if (IslandManager.Instance.IslandIds [x, y] < IslandManager.Instance.IslandDatas.Count) {
								if (IslandManager.Instance.IslandDatas [IslandManager.Instance.IslandIds [x, y]].visited) {
									color = visitedIslandColor;
								} else if (IslandManager.Instance.IslandDatas [IslandManager.Instance.IslandIds [x, y]].seen) {
									color = unvisitedIslandColor;
								}
							}
						}
					}

					texture.SetPixel (x, y, color);
				}
			}
		}
//
		texture.filterMode = FilterMode.Point;

		texture.Apply ();
//
		Sprite sprite = Sprite.Create (texture, new Rect (0, 0, textureScale , textureScale) , Vector2.one * 0.5f );
		targetImage.sprite = sprite;
	}

	public void UpdatePixel ( int x , int y, Color color = default(Color)) {

		if ( color == default(Color) ) {
			color = discoveredColor;
			if ( IslandManager.Instance.IslandIds [x, y] > -1 ) {
				
				color = IslandManager.Instance.IslandDatas[IslandManager.Instance.IslandIds [x, y]].visited ? visitedIslandColor : unvisitedIslandColor;
				IslandManager.Instance.IslandDatas [IslandManager.Instance.IslandIds [x, y]].seen = true;

			} else if (IslandManager.Instance.IslandIds [x, y] == -2) {

				IslandManager.Instance.IslandIds [x, y] = -1;

			}

		}

		Texture2D texture = (Texture2D)targetImage.mainTexture;

		texture.SetPixel (x, y, color);

		texture.filterMode = FilterMode.Point;

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, textureScale , textureScale) , Vector2.one * 0.5f );

	}

	public void UpdateImagePosition () {
		return;
		float x = (maxImagePosition * MapManager.Instance.PosX) / textureScale;
		float y = (maxImagePosition * MapManager.Instance.PosY) / textureScale;
		targetImage.transform.localPosition = new Vector2 (-x , (maxImagePosition - y) );
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

	public Image TargetImage {
		get {
			return targetImage;
		}
	}
	#endregion
}
