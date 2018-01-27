using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayMinimap : MonoBehaviour {

	public static DisplayMinimap Instance;

	// minimap chunks
	public GameObject minimapChunkPrefab;
//	Dictionary<Coords,MinimapChunk> minimapChunks = new Dictionary<Coords, MinimapChunk>();
	public GameObject minimapChunkParent;

	// minimap
	public RectTransform overallRectTranfsorm;
	public RectTransform scrollViewRectTransform;

	// boat feedback
	public RectTransform boatRectTransform;
	public float centerTweenDuration = 0.5f;

	public Vector2 minimapChunkScale;

	public GameObject enemyBoatIconPrefab;
	public List<RectTransform> enemyBoatIcons = new List<RectTransform>();

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		InitMap ();

		InitBoatIcons ();
		// subscribe
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		Quest.showQuestOnMap += HandleShowQuestOnMap;

		HandleChunkEvent ();

		Show ();

		CombatManager.Instance.onFightStart += Hide;
		CombatManager.Instance.onFightEnd += Show;
	}

	void HandleChunkEvent ()
	{
		UpdateBoatRange ();

		CenterOnBoat ();
		MovePlayerIcon ();

		CheckForOtherBoats ();
	}

	void InitMap ()
	{

		// get minimap chunk scale
		//		minimapChunkScale = minimapChunkPrefab.GetComponent<RectTransform> ().rect.size;
		minimapChunkScale = new Vector2(minimapChunkPrefab.GetComponent<RectTransform> ().rect.width,minimapChunkPrefab.GetComponent<RectTransform> ().rect.height);

		overallRectTranfsorm.sizeDelta = minimapChunkScale * (MapGenerator.Instance.MapScale);

		for (int x = 0; x <= MapGenerator.Instance.MapScale-1	; x++) {

			for (int y = 0; y <= MapGenerator.Instance.MapScale-1; y++) {

				Coords chunk = new Coords (x, y);

				if (Chunk.GetChunk (chunk).State == ChunkState.DiscoveredIsland
					|| Chunk.GetChunk (chunk).State == ChunkState.VisitedIsland) {

					PlaceMapChunk (chunk);
				}
			}
		}

	}

	// QUEST
	#region quest
	void HandleShowQuestOnMap (Quest quest)
	{
		CenterMap (quest.targetCoords);
	}
	#endregion

	#region center
	void CenterOnBoat() {
		CenterMap (Boats.playerBoatInfo.coords);
	}
	public void CenterMap (Coords coords)
	{
		float x = overallRectTranfsorm.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
		x -= minimapChunkScale.x/2f;
		x = Mathf.Clamp (x,0, overallRectTranfsorm.rect.width - scrollViewRectTransform.rect.width);

		float y = overallRectTranfsorm.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
		y -= minimapChunkScale.y/2f;
		y = Mathf.Clamp (y,0, overallRectTranfsorm.rect.height - scrollViewRectTransform.rect.height);

		Vector2 targetPos = new Vector2(-x,-y) + minimapChunkScale;

		HOTween.To (overallRectTranfsorm, centerTweenDuration, "anchoredPosition", targetPos, false, EaseType.Linear, 0f);
	}
	#endregion

	#region map range
	void UpdateBoatRange ()
	{
		int boatRange = currentShipRange;

		for (int x = -boatRange; x <= boatRange; x++) {
			
			for (int y = -boatRange; y <= boatRange; y++) {

				Coords c = Boats.playerBoatInfo.coords + new Coords (x, y);

				Chunk chunk = Chunk.GetChunk (c);

				switch (chunk.State) {
				case ChunkState.UndiscoveredSea:
					chunk.State = ChunkState.DiscoveredSea;
					break;

				case ChunkState.UndiscoveredIsland:
					chunk.State = ChunkState.DiscoveredIsland;
					PlaceMapChunk (c);
					break;

				case ChunkState.DiscoveredIsland:
					break;

				case ChunkState.VisitedIsland:
					break;

				default:
					break;

				}
			}

		}
	}
	#endregion

	#region map chunk
	void PlaceMapChunk(Coords c) {

//		if ( minimapChunks.ContainsKey(c) ) {
//			Debug.LogError ("WHOOPS ! trying to place a minimap chunk where there's already one !");
//			return;
//		}

		// INST
		GameObject minimapChunk = Instantiate (minimapChunkPrefab, minimapChunkParent.transform);

		// SCALE
		minimapChunk.transform.localScale = Vector3.one;

		// POS
		//Vector2 decal = scale / (Boats.PlayerBoatInfo.ShipRange * 2 + 1);
		float x = (minimapChunkScale.x/2) 	+ (c.x * overallRectTranfsorm.rect.width / MapGenerator.Instance.MapScale);
		float y = (minimapChunkScale.y / 2) + c.y * minimapChunkScale.y;
		Vector2 pos = new Vector2 (x,y);

		//		minimapChunk.GetComponent<RectTransform>().anchoredPosition = getPosFromCoords (c);
		minimapChunk.GetComponent<RectTransform>().anchoredPosition = pos;

		minimapChunk.GetComponent<MinimapChunk> ().InitChunk (c);

//		minimapChunks.Add (c, minimapChunk.GetComponent<MinimapChunk> ());

	}
	#endregion

	#region boatIcons
	void InitBoatIcons() {
		boatRectTransform.sizeDelta = minimapChunkScale;
	}
	public Vector2 getPosFromCoords (Coords coords) {

		return new Vector2 ((minimapChunkScale.x / 2f) + coords.x * minimapChunkScale.x, (minimapChunkScale.y / 2f) + coords.y * minimapChunkScale.y);

	}
	void MovePlayerIcon () {

		Vector2 boatPos = getPosFromCoords (Boats.playerBoatInfo.coords);
		HOTween.To (boatRectTransform, centerTweenDuration-0.2f, "anchoredPosition", boatPos, false, EaseType.Linear, 0.2f);

		Tween.Bounce (boatRectTransform.transform);
	}

	int currentShipRange {
		get {
			int range = Boats.playerBoatInfo.shipRange;

			if (TimeManager.Instance.Raining)
				range--;
			
			if (TimeManager.Instance.IsNight)
				range--;

			return range;
		}
	}

	void CheckForOtherBoats ()
	{

		foreach (var item in enemyBoatIcons) {
			item.gameObject.SetActive (false);
		}

		int boatIndexInRange = 0;

		int boatRange = currentShipRange;

		foreach ( OtherBoatInfo boatInfo in Boats.Instance.OtherBoatInfos ) {

			if ( boatInfo.coords <= Boats.playerBoatInfo.coords + boatRange&& boatInfo.coords >= Boats.playerBoatInfo.coords - boatRange ) {

				PlaceOtherBoatIcon (boatInfo,boatIndexInRange);

				++boatIndexInRange;

			}

		}
	}


	void PlaceOtherBoatIcon (OtherBoatInfo boatInfo, int boatIndexInRange ) {

		if ( boatIndexInRange >= enemyBoatIcons.Count ) {
			CreateEnemyIcon ();
		}

		enemyBoatIcons [boatIndexInRange].gameObject.SetActive (true);
		enemyBoatIcons [boatIndexInRange].anchoredPosition = getPosFromCoords (boatInfo.coords);

	}
	void CreateEnemyIcon() {

		GameObject boatIcon = Instantiate (enemyBoatIconPrefab, minimapChunkParent.transform);

		boatIcon.transform.localScale = Vector3.one;

		boatIcon.GetComponent<RectTransform> ().sizeDelta = minimapChunkScale;

		enemyBoatIcons.Add (boatIcon.GetComponent<RectTransform>());
	}
	#endregion

	void Show () {
		scrollViewRectTransform.gameObject.SetActive (true);
	}

	void Hide () {
		scrollViewRectTransform.gameObject.SetActive (false);
	}
}
