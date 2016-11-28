using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public static MapManager Instance;

	private MapImage mapImage;

	private UIButton mapButton;

	private MapGenerator mapGenerator;

	private int posX = 0;
	private int posY = 0;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	public void Init () {

		mapImage = GetComponent<MapImage> ();
		mapGenerator = GetComponent<MapGenerator> ();
		mapButton = GetComponent<UIButton> ();

			// init boat pos
		posX = Random.Range (0, (int)(mapImage.TextureScale));
		posY = (int)(mapImage.TextureScale / 6);

		mapGenerator.GenerateIslands ();

	}

	public void SetNewPos ( Vector2 v ) {

		mapImage.UpdatePixel (posX, posY);

		PosX += (int)v.x;
		PosY += (int)v.y;

		UpdateImage ();
	}

	public void UpdateImage () {


		int shipRange = NavigationManager.Instance.ShipRange;

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				if (x == 0 && y == 0) {
					mapImage.UpdatePixel (posX + x, posY + y, Color.red);
				} else {

					if (posX + x < mapImage.TextureScale && posX + x >= 0 &&
					    posY + y < mapImage.TextureScale && posY + y >= 0) {

						mapImage.UpdatePixel (posX + x, posY + y);
					}
				}

			}

		}

		mapImage.TargetImage.transform.localPosition = new Vector2 (200 - (posX* 2) , -(posY* 2));


	}

	#region properties
	public int PosX {
		get {
			return posX;
		}
		set {
			posX = Mathf.Clamp (value, 0 , mapImage.TextureScale-1);
		}
	}
	public int PosY {
		get {
			return posY;
		}
		set {
			posY = Mathf.Clamp (value, 0 , mapImage.TextureScale-1);
		}
	}
	public int Middle {
		get {
			return mapImage.TextureScale/2;
		}
	}
	public bool NearIsland {
		get {
			return MapGenerator.Instance.IslandIds [posX, posY] > -1;
		}
	}
	public int IslandID {
		get {
			return MapGenerator.Instance.IslandIds [posX, posY]; 
		}
	}
	public IslandData CurrentIsland {
		get {
			return MapGenerator.Instance.IslandDatas [IslandID];
		}
	}
	#endregion

	public UIButton MapButton {
		get {
			return mapButton;
		}
	}
}
