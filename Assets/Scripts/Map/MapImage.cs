﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapImage : MonoBehaviour {

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

	public void InitImage () {

		mapGenerator = GetComponent<MapGenerator> ();

		Texture2D texture = new Texture2D (textureScale, textureScale);

		for ( int x = 0; x < textureScale ; ++x ) {
			
			for (int y = 0; y < textureScale; ++y ) {


				if (revealMap) {
					
					texture.SetPixel (x, y, MapGenerator.Instance.IslandIds [x, y] > -1 ? Color.yellow : Color.blue);

				} else {

					int id = MapGenerator.Instance.IslandIds [x, y];

					Color color = undiscoveredColor;

					if (id > -2) {
						if (id == -1) {
							color = discoveredColor;
						} else {
							if (mapGenerator.IslandIds [x, y] < mapGenerator.IslandDatas.Count) {
								if (mapGenerator.IslandDatas [mapGenerator.IslandIds [x, y]].visited) {
									color = visitedIslandColor;
								} else if (mapGenerator.IslandDatas [mapGenerator.IslandIds [x, y]].seen) {
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
			if ( mapGenerator.IslandIds [x, y] > -1 ) {
				
				color = mapGenerator.IslandDatas[mapGenerator.IslandIds [x, y]].visited ? visitedIslandColor : unvisitedIslandColor;
				mapGenerator.IslandDatas [mapGenerator.IslandIds [x, y]].seen = true;

			} else if (MapGenerator.Instance.IslandIds [x, y] == -2) {

				MapGenerator.Instance.IslandIds [x, y] = -1;

			}

		}

		Texture2D texture = (Texture2D)targetImage.mainTexture;

		texture.SetPixel (x, y, color);

		texture.filterMode = FilterMode.Point;

		texture.Apply ();

		targetImage.sprite = Sprite.Create ( texture, new Rect (0, 0, textureScale , textureScale) , Vector2.one * 0.5f );

	}

	[SerializeField]
	private float maxImagePosition = 250f;

	public void UpdateImagePosition () {
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
