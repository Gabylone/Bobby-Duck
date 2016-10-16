using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public static MapManager Instance;
	private MapImage mapImage;

	private int posX = 0;
	private int posY = 0;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		mapImage = GetComponent<MapImage> ();

			// init boat pos
		posX = (int)(mapImage.TextureWidth / 2);
		posY = (int)(mapImage.TextureHeight / 2);

		mapImage.InitImage ();

		mapImage.UpdateCurrentPixel (Color.red);

	}

	public void SetNewPos ( Vector2 v ) {

		mapImage.UpdateCurrentPixel (Color.grey);

		posX += (int)v.x;
		posY += (int)v.y;

		mapImage.UpdateCurrentPixel (Color.red);


	}
	
	// Update is called once per frame
	void Update () {
	
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
	#endregion
}
