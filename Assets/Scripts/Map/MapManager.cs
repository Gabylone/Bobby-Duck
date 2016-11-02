using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public static MapManager Instance;

	private MapImage mapImage;

	[SerializeField]
	private Color islandColor;
	[SerializeField]
	private Color discoveredColor;

	private MapGenerator mapGenerator;
	bool[,] 	checkIsland;
	Vector2[,] 	islandPositions;
	Loot[,] islandLoots;

	private int posX = 0;
	private int posY = 0;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		mapImage = GetComponent<MapImage> ();
		mapGenerator = GetComponent<MapGenerator> ();

			// init boat pos
		posX = (int)(mapImage.TextureScale / 2);
		posY = (int)(mapImage.TextureScale / 6);

		mapGenerator.GenerateIslands ();

	}

	public void SetNewPos ( Vector2 v ) {

		posX += (int)v.x;
		posY += (int)v.y;

		int shipRange = NavigationManager.Instance.ShipRange;

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				if (x == 0 && y == 0) {
					mapImage.UpdatePixel (posX + x, posY + y, Color.red);
				} else {
					mapImage.UpdatePixel (posX + x, posY + y, checkIsland [posX + x, posY + y] ? islandColor : discoveredColor);
				}


			}

		}

	}

	#region properties
	public int PosX {
		get {
			return posX;
		}
		set {
			posX = value;
		}
	}
	public int PosY {
		get {
			return posY;
		}
		set {
			posY = value;
		}
	}
	public int Middle {
		get {
			return mapImage.TextureScale/2;
		}
	}
	#endregion

	public bool[,] CheckIsland {
		get {
			return checkIsland;
		}
		set {
			checkIsland = value;
		}
	}

	public bool CheckCurrentPosIsland {
		get {
			return checkIsland [posX, posY];
		}
	}

	public Vector2[,] IslandPositions {
		get {
			return islandPositions;
		}
		set {
			islandPositions = value;
		}
	}

	public Vector2 CurrentIslandPosition {
		get {
			return islandPositions[posX,posY];
		}
	}

	public Loot[,] IslandLoots {
		get {
			return islandLoots;
		}
		set {
			islandLoots = value;
		}
	}

	public Loot CurrentIslandLoot {
		get {
			return islandLoots [posX, posY];
		}
		set {
			islandLoots [posX, posY] = value;
		}
	}
}
