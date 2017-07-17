using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Array : MonoBehaviour {

	public static Array Instance;

	[SerializeField]
	private GameObject tileOverall;
	[SerializeField]
	private Sprite[] sprites;
	private Dictionary<TileCoord,SpriteRenderer> tileSpriteRenderers = new Dictionary<TileCoord, SpriteRenderer>();

	[Header("Loading")]
	[SerializeField]
	private GameObject loadingGroup;
	[SerializeField]
	private Image loadingImage;
	[SerializeField]
	private int loadingLimit = 50;
	private int currentLoad = 0;

	#region load
	void Awake () {
		Instance = this;
	}
	void Start () {
//		LoadTiles ();
	}
	public void LoadTiles () {

		if (tileOverall == null) {
			Debug.Log ("tile overall not assigned");
			return;
		}

		tileSpriteRenderers.Clear ();
	
		BoxCollider2D[] tileRenderers = tileOverall.GetComponentsInChildren<BoxCollider2D> ();

		for (int i = 0; i < tileRenderers.Length; i++) {

			TileCoord newTile = new TileCoord ((int)tileRenderers [i].transform.position.x, (int)tileRenderers [i].transform.position.y);

			if ( tileSpriteRenderers.ContainsKey (newTile) ) {

				continue;

			}
			tileSpriteRenderers.Add (newTile, tileRenderers [i].GetComponent<SpriteRenderer>());

		}

		foreach ( TileCoord tileCoord in tileSpriteRenderers.Keys ) {
			UpdateTile (tileCoord);
		}

	}
//	IEnumerator LoadTiles_Coroutine () {
//
//		BoxCollider2D[] tileRenderers = tileOverall.GetComponentsInChildren<BoxCollider2D> ();
//
//		loadingGroup.SetActive (true);
//
//		int max = tileRenderers.Length * 2;
//		int current = 0;
//
//		for (int i = 0; i < tileRenderers.Length; i++) {
//
//			TileCoord newTile = new TileCoord ((int)tileRenderers [i].transform.position.x, (int)tileRenderers [i].transform.position.y);
//
//			tileSpriteRenderers.Add (newTile, tileRenderers [i].GetComponent<SpriteRenderer>());
//
//			++current;
//
//			if ( current >= loadingLimit ) {
//
//				current = 0;
//
//				loadingImage.fillAmount = i / max;
//
//				yield return new WaitForEndOfFrame ();
//			}
//
//		}
//
//		loadingGroup.SetActive (false);
//
//
//	}
	#endregion

	#region check tile surrounding
	private TileCoord[] tilesToCheck = new TileCoord[8] {
		new TileCoord (0, 1),
		new TileCoord (1, 1),
		new TileCoord (1, 0),
		new TileCoord (1, -1),
		new TileCoord (0, -1),
		new TileCoord (-1, -1),
		new TileCoord (-1, 0),
		new TileCoord (-1, 1)
	};
	public struct TileSurrounding {

		public bool[] full;

		public bool Empty ( Surrounding surr ) {
			return !full [(int)surr];
		}
	}

	public void AddTile ( TileCoord tileToAdd ,SpriteRenderer rend) {
		if (tileSpriteRenderers.ContainsKey (tileToAdd)) {
			Debug.Log ("ADD TILE : a tile is already there ");
			return;
		}
		tileSpriteRenderers.Add (tileToAdd, rend);
		UpdateSurroundingTile (tileToAdd);
		UpdateTile (tileToAdd);
	}

	public void RemoveTile (TileCoord tileToRemove) {
		tileSpriteRenderers [tileToRemove].sprite = sprites [(int)TileType.NoConnections];
		tileSpriteRenderers.Remove (tileToRemove);
		UpdateSurroundingTile (tileToRemove);
	}

	public void UpdateSurroundingTile ( TileCoord centerTile ) {
		
		for (int i = 0; i < tilesToCheck.Length; i++) {

			TileCoord surroundingTile = new TileCoord (centerTile.x + tilesToCheck [i].x, centerTile.y + tilesToCheck [i].y);

			if (tileSpriteRenderers.ContainsKey (surroundingTile)) {
				UpdateTile (surroundingTile);
			}

		}
	}

	public void UpdateTile ( TileCoord tile ) {
		tileSpriteRenderers [tile].sprite = sprites [(int)GetTileType (tile)];
	}

	public TileSurrounding CheckTileSurrounding (TileCoord tileToCheck) {

		TileSurrounding tileSurr = new TileSurrounding ();
		tileSurr.full = new bool[8];

		for (int i = 0; i < tilesToCheck.Length; i++) {

			TileCoord newTile = new TileCoord (
				tileToCheck.x + tilesToCheck [i].x,
				tileToCheck.y + tilesToCheck [i].y);

			tileSurr.full [i] = tileSpriteRenderers.ContainsKey (newTile);


		}

		return tileSurr;
	}
	#endregion

	#region props

	[Header("Ornements")]
	[SerializeField]
	private GameObject[] ornementsPrefabs;

	[SerializeField]
	private float ornementsRange;

	[SerializeField]
	private int maxOrnementsAmount = 2;

	public void GenerateProps () {

		ClearProps ();

		LoadTiles ();

		SpriteRenderer[] tileRenderers = tileOverall.GetComponentsInChildren<SpriteRenderer> ();

		for (int i = 0; i < tileRenderers.Length; i++) {

			TileCoord newTile = new TileCoord ((int)tileRenderers [i].transform.position.x, (int)tileRenderers [i].transform.position.y);

			TileSurrounding tileSurr = CheckTileSurrounding (newTile);

			if ( tileSurr.Empty (Surrounding.Top) )
				GeneratePropsOnTile (tileRenderers [i].transform, Surrounding.Top);
			if ( tileSurr.Empty (Surrounding.Right) )
				GeneratePropsOnTile (tileRenderers [i].transform, Surrounding.Right);
			if ( tileSurr.Empty (Surrounding.Bottom) )
				GeneratePropsOnTile (tileRenderers [i].transform, Surrounding.Bottom);
			if ( tileSurr.Empty (Surrounding.Left) )
				GeneratePropsOnTile (tileRenderers [i].transform, Surrounding.Left);

		}

	}
	void GeneratePropsOnTile (Transform t, Surrounding surr) {

		Vector3 p = Vector3.zero;

		Vector3 ornementDirectionFromTile = Vector3.zero;

		Vector3 ornementSpreadDirection = Vector2.zero;

		switch (surr) {
		case Surrounding.Top:
			p = t.position + Vector3.up * (ornementsRange / 2f);
			ornementDirectionFromTile = Vector3.up;
			ornementSpreadDirection = Vector3.right;
			break;
		case Surrounding.Right:
			p = t.position + Vector3.right * (ornementsRange / 2f);
			ornementDirectionFromTile = Vector3.right;
			ornementSpreadDirection = Vector3.up;
			break;
		case Surrounding.Bottom:
			p = t.position - Vector3.up * (ornementsRange / 2f);
			ornementDirectionFromTile = Vector3.down;
			ornementSpreadDirection = Vector3.right;
			break;
		case Surrounding.Left:
			p = t.position - Vector3.right * (ornementsRange / 2f);
			ornementDirectionFromTile = Vector3.left;
			ornementSpreadDirection = Vector3.up;
			break;
		}

		int amount = Random.Range ( 0,maxOrnementsAmount );

		int ornementIndex = Random.Range ( 0 , ornementsPrefabs.Length);

		for (int i = 0; i < amount; i++) {

			GameObject ornement = Instantiate (ornementsPrefabs [ornementIndex]) as GameObject;

			ornement.transform.position = t.transform.position + (ornementDirectionFromTile * ornementsRange / 2f);
			ornement.transform.up = (ornement.transform.position - t.position).normalized;

//			ornement.transform.position = p + (ornementSpreadDirection * Random.Range (-ornementsRange/2f,ornementsRange/2f));

			ornement.transform.SetParent (t);

		}

	}
	public void ClearProps () {
		
		BoxCollider2D[] tileColliders = tileOverall.GetComponentsInChildren<BoxCollider2D> ();

		for (int i = 0; i < tileColliders.Length; i++) {

			SpriteRenderer[] props = tileColliders [i].GetComponentsInChildren<SpriteRenderer> ();
			if (props.Length == 1)
				continue;

			for (int a = 1; a < props.Length; a++) {
				DestroyImmediate(props[a].gameObject);
			}

		}
	}
	#endregion

	#region tools
	public static Vector2 RoundToUnit ( Vector2 p ) {
		p.x = (float)Mathf.RoundToInt (p.x);
		p.y = (float)Mathf.RoundToInt (p.y);

		return p;
	}
	public static TileCoord GetTileFromPos ( Vector2 p ) {
		return new TileCoord (Mathf.RoundToInt (p.x),Mathf.RoundToInt (p.y));
	}
	#endregion

	public TileType GetTileType (TileCoord tile) { 

		TileSurrounding surr = CheckTileSurrounding (tile);


		if ( surr.Empty(Surrounding.Top) ) {

			if ( surr.Empty(Surrounding.Right) ) {
				
				if ( surr.Empty(Surrounding.Left) ) {

					if ( surr.Empty(Surrounding.Bottom) ) {
						return TileType.NoConnections;
					}

					return TileType.Floor_LeftRightTop;
				}

				if ( surr.Empty(Surrounding.Bottom) ) {
					return TileType.Floor_TopBottomRight;
				}

				if ( surr.Empty(Surrounding.BottomLeft) ) {
					return TileType.Floor_TopRight_Corner;
				}

				return TileType.Floor_TopRight;


			}

			if ( surr.Empty(Surrounding.Left) ) {

				if ( surr.Empty(Surrounding.Bottom) ) {
					return TileType.Floor_TopBottomLeft;
				}

				if ( surr.Empty(Surrounding.BottomRight) ) {
					return TileType.Floor_TopLeft_Corner;
				}
				return TileType.Floor_TopLeft;
			}

			if ( surr.Empty(Surrounding.Bottom) ) {

				if ( surr.Empty(Surrounding.Left) ) {
					return TileType.Floor_TopBottomLeft;
				}

				if ( surr.Empty(Surrounding.Right) ) {
					return TileType.Floor_TopBottomRight;
				}

				return TileType.Floor_TopBottom;

			}

			if (surr.Empty (Surrounding.BottomLeft)) {
				if (surr.Empty (Surrounding.BottomRight)) {
					return TileType.Floor_Top_TwoCorners;
				}
				return TileType.Floor_Top_CornerLeft;
			}

			if (surr.Empty (Surrounding.BottomRight)) {
				return TileType.Floor_Top_CornerRight;
			}

			return TileType.Floor_Top;

		}

		if ( surr.Empty (Surrounding.Bottom) ) {

			if ( surr.Empty (Surrounding.Right) ) {

				if ( surr.Empty (Surrounding.Left) ) {
					return TileType.Floor_LeftRightBottom;
				}

				if ( surr.Empty (Surrounding.TopLeft) ) {
					return TileType.Floor_BottomRight_Corner;
				}

				return TileType.Floor_BottomRight;
			}

			if ( surr.Empty (Surrounding.Left) ) {
				
				if ( surr.Empty (Surrounding.TopRight) ) {
					return TileType.Floor_BottomLeft_Corner;
				}

				return TileType.Floor_BottomLeft;
			}

			if ( surr.Empty (Surrounding.TopLeft) ) {
				if ( surr.Empty (Surrounding.TopRight) ) {
					return TileType.Floor_Bottom_TwoCorners;
				}
				return TileType.Floor_Bottom_CornerLeft;
			}

			if ( surr.Empty (Surrounding.TopRight) ) {
				return TileType.Floor_Bottom_CornerRight;
			}

			return TileType.Floor_Bottom;

		}

		if ( surr.Empty (Surrounding.Left) ) {

			if ( surr.Empty (Surrounding.Right) ) {
				return TileType.Floor_LeftRight;
			}

			if ( surr.Empty (Surrounding.TopRight) ) {
				if ( surr.Empty (Surrounding.BottomRight) ) {
					return TileType.Floor_Left_TwoCorners;
				}
				return TileType.Floor_Left_CornerTop;
			}

			if ( surr.Empty (Surrounding.BottomRight) ) {
				return TileType.Floor_Left_CornerBottom;
			}
			return TileType.Floor_Left;
		}

		if ( surr.Empty (Surrounding.Right) ) {
			if ( surr.Empty (Surrounding.TopLeft) ) {
				if ( surr.Empty (Surrounding.BottomLeft) ) {
					return TileType.Floor_Right_TwoCorners;
				}
				return TileType.Floor_Right_CornerTop;
			}
			if ( surr.Empty (Surrounding.BottomLeft) ) {
				return TileType.Floor_Right_CornerBottom;
			}
			return TileType.Floor_Right;
		}


		if ( surr.Empty (Surrounding.BottomLeft) ) {

			if ( surr.Empty (Surrounding.TopLeft) ) {
				if ( surr.Empty (Surrounding.TopRight) ) {
					if ( surr.Empty (Surrounding.TopRight) ) {
						return TileType.Corner_BottomLeft_TopLeft_TopRight_BottomRight;
					}
					return TileType.Corner_BottomLeft_TopLeft_TopRight;
				}
				return TileType.Corner_BottomLeft_TopLeft;
			}

			if ( surr.Empty (Surrounding.BottomRight) ) {
				if ( surr.Empty (Surrounding.TopRight) ) {
					return TileType.Corner_TopRight_BottomRight_BottomLeft;
				}
				if ( surr.Empty (Surrounding.TopLeft) ) {
					return TileType.Corner_BottomRight_BottomLeft_TopLeft;
				}
				return TileType.Corner_BottomRight_BottomLeft;
			}

			if ( surr.Empty (Surrounding.TopRight) ) {
				return TileType.Corner_TopRight_BottomLeft;
			}

			return TileType.Corner_BottomLeft;
		}

		if ( surr.Empty (Surrounding.TopLeft) ) {
			if ( surr.Empty (Surrounding.TopRight) ) {
				if ( surr.Empty (Surrounding.BottomRight) ) {
					return TileType.Corner_TopLeft_TopRight_BottomRight;
				}
				return TileType.Corner_TopLeft_TopRight;
			}
			if ( surr.Empty (Surrounding.BottomRight) ) {
				return TileType.Corner_TopLeft_BottomRight;
			}

			return TileType.Corner_TopLeft;
		}

		if ( surr.Empty (Surrounding.TopRight) ) {

			if ( surr.Empty (Surrounding.BottomRight) ) {
				return TileType.Corner_TopRight_BottomRight;
			}

			return TileType.Corner_TopRight;
		}

		if ( surr.Empty (Surrounding.BottomRight) ) {
			return TileType.Corner_BottomRight;
		}

		return TileType.Floor_Full;

	}

	public GameObject TileOverall {
		get {
			return tileOverall;
		}
	}
}


public struct TileCoord {
	public int x;
	public int y;

	public TileCoord ( int x , int y ) {
		this.x = x;
		this.y = y;
	}
}

public enum Surrounding {
	Top,
	TopRight,
	Right,
	BottomRight,
	Bottom,
	BottomLeft,
	Left,
	TopLeft,
}

public enum TileType {
	
	Corner_TopRight,
	Corner_BottomRight,
	Corner_BottomLeft,
	Corner_TopLeft,

	Floor_TopRight,
	Floor_BottomRight,
	Floor_BottomLeft,
	Floor_TopLeft,

	Floor_Top,
	Floor_Right,
	Floor_Bottom,
	Floor_Left,

	Floor_Full,

	Floor_LeftRight,
	Floor_TopBottom,

	Floor_LeftRightBottom,
	Floor_TopBottomLeft,
	Floor_LeftRightTop,
	Floor_TopBottomRight,

	Floor_BottomRight_Corner,
	Floor_TopRight_Corner,
	Floor_TopLeft_Corner,
	Floor_BottomLeft_Corner,

	Floor_Bottom_CornerLeft,
	Floor_Bottom_CornerRight,

	Floor_Right_CornerTop,
	Floor_Right_CornerBottom,

	Floor_Top_CornerLeft,
	Floor_Top_CornerRight,

	Floor_Left_CornerTop,
	Floor_Left_CornerBottom,

	RockyTop,
	RockyLeft,
	RockyRight,

	NoConnections,

	Corner_TopLeft_TopRight,
	Corner_TopRight_BottomRight,
	Corner_BottomRight_BottomLeft,
	Corner_BottomLeft_TopLeft,

	Corner_TopLeft_TopRight_BottomRight,
	Corner_TopRight_BottomRight_BottomLeft,
	Corner_BottomRight_BottomLeft_TopLeft,
	Corner_BottomLeft_TopLeft_TopRight,

	Corner_BottomLeft_TopLeft_TopRight_BottomRight,

	Floor_Top_TwoCorners,
	Floor_Right_TwoCorners,
	Floor_Bottom_TwoCorners,
	Floor_Left_TwoCorners,

	Corner_TopLeft_BottomRight,
	Corner_TopRight_BottomLeft,



}
