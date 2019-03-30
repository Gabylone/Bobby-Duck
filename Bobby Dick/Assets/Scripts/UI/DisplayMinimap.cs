using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System;

public class DisplayMinimap : MonoBehaviour {

	RectTransform rectTransform;

	public static DisplayMinimap Instance;

	// minimap chunks
	public GameObject minimapChunkPrefab;
	public GameObject minimapChunkParent;

	// minimap
	public RectTransform overallRectTranfsorm;
	public RectTransform scrollViewRectTransform;
	public Mask viewPortMask;

	// boat feedback
	public RectTransform boatRectTransform;
	public float centerTweenDuration = 0.5f;

	public Vector2 minimapChunkScale;

	public GameObject enemyBoatIconPrefab;
	public List<RectTransform> enemyBoatIcons = new List<RectTransform>();

	public Image rayBlockerImage;

	public float hiddenPos = 0f;
	public float hideDuration = 0.3f;

	/// <summary>
	/// zoom
	/// </summary>
	public float zoomDuration = 0.8f;

	RectTransform previousParent;
	public RectTransform zoomParent;
    public float initPosY = 0f;
    public float initPosX = 0f;
	public float initScaleY = 0f;
	public float initScaleX = 0f;

    public GameObject zoomBackground;

	public Image outlineImage;

	public GameObject mapCloseButton;

	public Dictionary<Coords,MinimapChunk> minimapChunks = new Dictionary<Coords, MinimapChunk>();

	void Awake () {
		Instance = this;

        onZoom = null;
	}

	// Use this for initialization
	void Start () {

		rectTransform = GetComponent<RectTransform> ();
		minimapChunkScale = new Vector2(minimapChunkPrefab.GetComponent<RectTransform> ().rect.width,minimapChunkPrefab.GetComponent<RectTransform> ().rect.height);

		// subscribe
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
		Quest.showQuestOnMap += HandleShowQuestOnMap;

		CombatManager.Instance.onFightStart += FadeOut;
		CombatManager.Instance.onFightEnd += FadeIn;

		StoryFunctions.Instance.getFunction += HandleOnGetFunction;

		// quest feedback
		Quest.onSetTargetCoords += HandleOnSetTargetCoords;

		QuestManager.onFinishQuest += HandleOnFinishQuest;
		QuestManager.onGiveUpQuest += HandleOnGiveUpQuest;

        StoryLauncher.Instance.onPlayStory += HandlePlayStoryEvent;

        CrewInventory.Instance.onOpenInventory += HandleOnOpenInventory;
        CrewInventory.Instance.onCloseInventory += HandleOnCloseInventory;

		initScaleX = rectTransform.sizeDelta.x;
        initScaleY = rectTransform.sizeDelta.y;

		Show ();
		HideCloseButton ();

		ClampScrollView ();
	}

    private void HandleOnCloseInventory()
    {
        FadeIn();
    }

    private void HandleOnOpenInventory(CrewMember member)
    {
        FadeOut();
    }

    public void Init () {

		InitMap ();

		InitBoatIcons ();

		HandleChunkEvent ();

	}

	void HandleOnGetFunction (FunctionType func, string cellParameters)
	{
		if (func == FunctionType.ObserveHorizon) {

			UpdateRange (4);

			StoryReader.Instance.NextCell ();

			StoryReader.Instance.UpdateStory ();

			Tween.Bounce (minimapChunkParent.transform);

		}
	}

	#region update minimap chunks
	void HandleOnSetTargetCoords (Quest quest)
	{
		if (quest.targetCoords == quest.originCoords) {
			if ( minimapChunks.ContainsKey (quest.previousCoords) )
				minimapChunks [quest.previousCoords].HideQuestFeedback ();
		}

		if ( minimapChunks.ContainsKey(quest.targetCoords) )
			minimapChunks [quest.targetCoords].ShowQuestFeedback ();

	}

	void HandleOnGiveUpQuest (Quest quest)
	{
		if ( minimapChunks.ContainsKey(quest.targetCoords) )
		minimapChunks [quest.targetCoords].HideQuestFeedback ();
	}

	void HandleOnFinishQuest (Quest quest)
	{
		if ( minimapChunks.ContainsKey(quest.targetCoords) )
		minimapChunks [quest.targetCoords].HideQuestFeedback ();
	}

    void HandlePlayStoryEvent()
    {
        if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.island)
        {
            minimapChunks[Coords.current].SetVisited();
        }
    }

	void HandleChunkEvent ()
	{

		UpdateRange (currentShipRange);

		CenterOnBoat ();
		MovePlayerIcon ();

		CheckForOtherBoats ();
	}
    #endregion
        
    void InitMap ()
	{

		// get minimap chunk scale
		//		minimapChunkScale = minimapChunkPrefab.GetComponent<RectTransform> ().rect.size;

		overallRectTranfsorm.sizeDelta = minimapChunkScale * (MapGenerator.Instance.MapScale);

		for (int x = 0; x <= MapGenerator.Instance.MapScale-1; x++) {

			for (int y = 0; y <= MapGenerator.Instance.MapScale-1; y++) {

				Coords chunk = new Coords (x, y);

				if (Chunk.GetChunk (chunk).state == ChunkState.DiscoveredIsland
					|| Chunk.GetChunk (chunk).state == ChunkState.VisitedIsland) {

					PlaceMapChunk (chunk);


				}
			}
		}

//		foreach (var item in QuestManager.Instance.currentQuests) {
//			minimapChunks [item.targetCoords].ShowQuestFeedback ();
//		}

	}

	// QUEST
	#region quest
	void HandleShowQuestOnMap (Quest quest)
	{
        StartCoroutine(HandleShowQuestOnMapCoroutine(quest));
		
	}
    public float showOnMapDuration = 4f;
    IEnumerator HandleShowQuestOnMapCoroutine(Quest quest)
    {
        FadeIn();

        yield return new WaitForSeconds(hideDuration);

        CenterMap(quest.targetCoords);

        yield return new WaitForSeconds(showOnMapDuration);

        FadeOut();
    }
    void HandleShowQuestOnMapDelay()
    {

    }
    #endregion

    #region center
    void CenterOnBoat() {
		CenterMap (Boats.playerBoatInfo.coords);
	}
	void ClampScrollView() {
		int buffer = 0;

		Coords coords = Boats.playerBoatInfo.coords;

		float x = overallRectTranfsorm.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
		x -= scrollViewRectTransform.rect.width / 2f - (minimapChunkScale.x/2f);
		//		x = Mathf.Clamp (x,0, overallRectTranfsorm.rect.width- scrollViewRectTransform.rect.widt );
		x = Mathf.Clamp (x,0-buffer, (overallRectTranfsorm.rect.width - scrollViewRectTransform.rect.width) +buffer);

		float y = overallRectTranfsorm.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
		y -= scrollViewRectTransform.rect.height / 2f  - (minimapChunkScale.y/2f);
		//		y = Mathf.Clamp (y,0, overallRectTranfsorm.rect.height - (scrollViewRectTransform.rect.height/2f));
		y = Mathf.Clamp (y,0-buffer, overallRectTranfsorm.rect.height - scrollViewRectTransform.rect.height + buffer);

		//		float x = overallRectTranfsorm.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
		//		x -= minimapChunkScale.x/2f;
		//		x = Mathf.Clamp (x,0, overallRectTranfsorm.rect.width - scrollViewRectTransform.rect.width);
		//
		//		float y = overallRectTranfsorm.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
		//		y -= minimapChunkScale.y/2f;
		//		y = Mathf.Clamp (y,0, overallRectTranfsorm.rect.height - scrollViewRectTransform.rect.height);

		Vector2 targetPos = new Vector2(-x,-y);
		overallRectTranfsorm.anchoredPosition = targetPos;
	}
	public void CenterMap (Coords coords)
	{
		int buffer = 5;

		float x = overallRectTranfsorm.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
		x -= scrollViewRectTransform.rect.width / 2f - (minimapChunkScale.x/2f);
		//		x = Mathf.Clamp (x,0, overallRectTranfsorm.rect.width- scrollViewRectTransform.rect.widt );
		x = Mathf.Clamp (x,0-buffer, (overallRectTranfsorm.rect.width - scrollViewRectTransform.rect.width) +buffer);

		float y = overallRectTranfsorm.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
		y -= scrollViewRectTransform.rect.height / 2f  - (minimapChunkScale.y/2f);
		//		y = Mathf.Clamp (y,0, overallRectTranfsorm.rect.height - (scrollViewRectTransform.rect.height/2f));
		y = Mathf.Clamp (y,0-buffer, overallRectTranfsorm.rect.height - scrollViewRectTransform.rect.height + buffer);

//		float x = overallRectTranfsorm.rect.width * (float)coords.x / MapGenerator.Instance.MapScale;
//		x -= minimapChunkScale.x/2f;
//		x = Mathf.Clamp (x,0, overallRectTranfsorm.rect.width - scrollViewRectTransform.rect.width);
//
//		float y = overallRectTranfsorm.rect.height * (float)coords.y / MapGenerator.Instance.MapScale;
//		y -= minimapChunkScale.y/2f;
//		y = Mathf.Clamp (y,0, overallRectTranfsorm.rect.height - scrollViewRectTransform.rect.height);

		Vector2 targetPos = new Vector2(-x,-y);

		HOTween.To (overallRectTranfsorm, centerTweenDuration, "anchoredPosition", targetPos, false, EaseType.Linear, 0f);
	}
	#endregion

	#region map range
	void UpdateRange (int range)
	{
		for (int x = -range; x <= range; x++) {
			
			for (int y = -range; y <= range; y++) {

				Coords c = Boats.playerBoatInfo.coords + new Coords (x, y);

				if (c.OutOfMap ())
					continue;

				Chunk chunk = Chunk.GetChunk (c);

				switch (chunk.state) {
				case ChunkState.UndiscoveredSea:
					chunk.state = ChunkState.DiscoveredSea;
					MapGenerator.Instance.discoveredCoords.coords.Add (c);
					break;

				case ChunkState.UndiscoveredIsland:
					chunk.state = ChunkState.DiscoveredIsland;
					chunk.Save (c);
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

			if (TimeManager.Instance.raining)
				range--;
			
			if (TimeManager.Instance.dayState == TimeManager.DayState.Night)
				range--;

			return range;
		}
	}

	public void CheckForOtherBoats ()
	{
		int boatIndexInRange = 0;

		int boatRange = currentShipRange;

        foreach (var item in enemyBoatIcons)
        {
            item.gameObject.SetActive(false);
        }

		foreach ( OtherBoatInfo boatInfo in Boats.Instance.getBoats ) {

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
        enemyBoatIcons [boatIndexInRange].GetComponentInChildren<Image>().color = boatInfo.color;

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

	#region zoom / unzoom
	public delegate void OnZoom ();
	public static OnZoom onZoom;
	public void Zoom ()
	{
		Transitions.Instance.ScreenTransition.FadeIn (zoomDuration/2f);

		Invoke ("ShowCloseButton",zoomDuration);
		Invoke ("ZoomDelay",zoomDuration/2f);
	}
	void ZoomDelay () {

		Vector2 scale = new Vector2(0f,0f);

        rectTransform.anchorMin = new Vector2 ( 0,0 );
        rectTransform.anchorMax = new Vector2 ( 1,1 );

		rectTransform.offsetMin = scale;
		rectTransform.offsetMax = scale;

//		HOTween.To (outlineImage, zoomDuration /2f , "color" , Color.clear );
		outlineImage.gameObject.SetActive(false);

        //zoomBackground.SetActive(true);


        rayBlockerImage.gameObject.SetActive(true);
		rayBlockerImage.color = Color.black;

		viewPortMask.enabled = false;
//		viewPortRectTransform.
//		HOTween.To (rayBlockerImage, zoomDuration, "color", c, false , EaseType.Linear , zoomDuration);

		Transitions.Instance.ScreenTransition.FadeOut (zoomDuration/2f);

		ClampScrollView ();

		if (onZoom != null)
			onZoom ();
	}

	bool unzooming = false;

	public void UnZoom ()
	{
		if (unzooming)
			return;

		Tween.Bounce (mapCloseButton.transform, 0.2f , 1.1f);

		Transitions.Instance.ScreenTransition.FadeIn (zoomDuration/2f);

		unzooming = true;


		Invoke ("HideCloseButton",0.2f);
		Invoke ("UnZoomDelay", zoomDuration/2f);
	}
	void UnZoomDelay () {

		rayBlockerImage.gameObject.SetActive(false);

        rectTransform.anchorMin = new Vector2( 0, 0);
        rectTransform.anchorMax = new Vector2( 0, 0);

        rectTransform.sizeDelta = new Vector2( initScaleX , initScaleY );

        rectTransform.anchoredPosition = Vector2.zero;

        //zoomBackground.SetActive(false);

        viewPortMask.enabled = true;

		ClampScrollView ();

		outlineImage.gameObject.SetActive(true);
		Transitions.Instance.ScreenTransition.FadeOut (zoomDuration/2f);
	}

	void ShowCloseButton ()
	{
		Tween.Bounce (mapCloseButton.transform, 0.2f , 1.1f);
		mapCloseButton.SetActive (true);
	}
	void HideCloseButton ()
	{
		unzooming = false;
		mapCloseButton.SetActive (false);
	}
	void FadeOut ()
	{
		HOTween.To ( rectTransform  , hideDuration , "anchoredPosition" , new Vector2 ( hiddenPos , 0f ) );
	}
	void FadeIn ()
	{
		HOTween.To ( rectTransform  , hideDuration , "anchoredPosition" , new Vector2 ( initPosX , initPosY ) );
	}
	#endregion
}
