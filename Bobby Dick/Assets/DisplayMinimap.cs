using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class DisplayMinimap : MonoBehaviour {

	// minimap chunks
	public GameObject minimapChunkPrefab;
	Dictionary<Coords,MinimapChunk> minimapChunks = new Dictionary<Coords, MinimapChunk>();
	public GameObject minimapChunkParent;

	// minimap
	public RectTransform overallRectTranfsorm;
	public RectTransform scrollViewRectTransform;

	// boat feedback
	public RectTransform boatRectTransform;
	public float centerTweenDuration = 0.5f;

	private Vector2 minimapChunkScale;

	public GameObject enemyBoatIconPrefab;
	public List<RectTransform> enemyBoatIcons = new List<RectTransform>();

	// Use this for initialization
	void Start () {

		InitMap ();

		InitBoatIcons ();
		// subscribe
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;;
		Quest.showQuestOnMap += HandleShowQuestOnMap;

		HandleChunkEvent ();
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

		print (Boats.PlayerBoatInfo.coords.ToString());

		// get minimap chunk scale
		//		minimapChunkScale = minimapChunkPrefab.GetComponent<RectTransform> ().rect.size;
		minimapChunkScale = new Vector2(minimapChunkPrefab.GetComponent<RectTransform> ().rect.width,minimapChunkPrefab.GetComponent<RectTransform> ().rect.height);

		overallRectTranfsorm.sizeDelta = minimapChunkScale * (MapGenerator.Instance.MapScale);

		for (int x = 0; x <= MapGenerator.Instance.MapScale; x++) {

			for (int y = 0; y <= MapGenerator.Instance.MapScale; y++) {

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
		UpdateSingleChunk (quest.targetCoords);
	}
	#endregion

	#region center
	// CENTER
	void CenterOnBoat() {
		CenterMap (Boats.PlayerBoatInfo.coords);
	}
	void CenterMap (Coords coords)
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
		int boatRange = Boats.PlayerBoatInfo.ShipRange;
		print ("boat range : " + boatRange);
//		int boatRange = Boats.PlayerBoatInfo.ShipRange;

		for (int x = -boatRange; x <= boatRange; x++) {
			
			for (int y = -boatRange; y <= boatRange; y++) {

				Coords c = Boats.PlayerBoatInfo.coords + new Coords (x, y);

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

	void UpdateSingleChunk (Coords coords) {

		Chunk chunk = Chunk.GetChunk (coords);

		switch (chunk.State) {
		case ChunkState.UndiscoveredIsland:
			print ("it is undiscoved island");
			chunk.State = ChunkState.DiscoveredIsland;
			PlaceMapChunk (coords);
			break;
		default:
			break;

		}

	}
	#endregion


	#region map chunk
	void PlaceMapChunk(Coords c) {

		if ( minimapChunks.ContainsKey(c) ) {
			Debug.LogError ("WHOOPS ! trying to place a minimap chunk where there's already one !");
			return;
		}

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

		minimapChunks.Add (c, minimapChunk.GetComponent<MinimapChunk> ());

	}
	#endregion

	#region boatIcons
	void InitBoatIcons() {
		boatRectTransform.sizeDelta = minimapChunkScale;
	}
	Vector2 getPosFromCoords (Coords coords) {

		return new Vector2 ((minimapChunkScale.x / 2f) + coords.x * minimapChunkScale.x, (minimapChunkScale.y / 2f) + coords.y * minimapChunkScale.y);

	}
	void MovePlayerIcon () {

		Vector2 boatPos = getPosFromCoords (Boats.PlayerBoatInfo.coords);
		HOTween.To (boatRectTransform, centerTweenDuration-0.2f, "anchoredPosition", boatPos, false, EaseType.Linear, 0.2f);

		Tween.Bounce (boatRectTransform.transform);
	}


	void CheckForOtherBoats ()
	{

		foreach (var item in enemyBoatIcons) {
			item.gameObject.SetActive (false);
		}

		int boatIndexInRange = 0;

		int boatRange = Boats.PlayerBoatInfo.ShipRange;

		foreach ( OtherBoatInfo boatInfo in Boats.Instance.OtherBoatInfos ) {


			if ( boatInfo.coords <= Boats.PlayerBoatInfo.coords + boatRange&& boatInfo.coords >= Boats.PlayerBoatInfo.coords - boatRange ) {

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


}
