﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Island : MonoBehaviour {

	public static Island Instance;

	public static Sprite[] sprites;
	public static Sprite[] minimapSprites;

	private Image image;

	[SerializeField]
	private float decal = 0f;

	[SerializeField]
	private GameObject group;

	[SerializeField]
	Collider _collider = null;

	Vector2 scale = Vector2.zero;

	public RectTransform uiBackground;

	private Transform _transform;

	[SerializeField]
	private RectTransform gameViewCenter;

	#region mono
	void Awake () {
		Instance = this;
	}

	void Start () {

        _transform = GetComponent<Transform>();
		image = GetComponentInChildren<Image> ();

		Init ();
	
	}

	void Init () {

		sprites = Resources.LoadAll<Sprite> ("Graph/IslandSprites");
		minimapSprites = Resources.LoadAll<Sprite> ("Graph/IslandMinimapSprites");

		CombatManager.Instance.onFightStart 	+= DeactivateCollider;
		CombatManager.Instance.onFightEnd 		+= ActivateCollider;

		Swipe.onSwipe += HandleOnSwipe;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		WorldTouch.onPointerExit += HandleOnTouchWorld;

		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);
	}

	void HandleOnSwipe (Directions direction)
	{
		_collider.enabled = false;
	}

	void HandleOnTouchWorld ()
	{
		targeted = false;
		_collider.enabled = false;
	}

	void HandleChunkEvent ()
	{
		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);

	}

	void DeactivateCollider ()
	{
		_collider.enabled = false;
	}
	void ActivateCollider () {
		_collider.enabled = true;
	}
	#endregion

	#region story
	public void Enter () {

        StoryLauncher.Instance.PlayStory(Chunk.currentChunk.IslandData.storyManager, StoryLauncher.StorySource.island);
	}
    #endregion

    #region render
    void UpdatePositionOnScreen(Coords coords) {

		Chunk chunk = Chunk.GetChunk (coords);

		IslandData islandData = chunk.IslandData;

		bool onIslandChunk = islandData != null;

		if (onIslandChunk) {

			gameObject.SetActive ( true );

            //GetComponent<RectTransform> ().anchoredPosition = chunk.IslandData.positionOnScreen;
            transform.localPosition = new Vector3( chunk.IslandData.positionOnScreen.x  , 0f , chunk.IslandData.positionOnScreen.y);

			//GetComponentInChildren<Image>().sprite = sprites [islandData.storyManager.storyHandlers [0].Story.param];

		} else {
			
			gameObject.SetActive ( false );

			transform.localPosition = new Vector3 (10000f, 0, 0);
		}
	}
	#endregion

	void OnCollisionStay ( Collision coll ) {
		if ( coll.gameObject.tag == "Player" ) {
			if (targeted) {
				Enter ();
				targeted = false;
			}
		}
	}

	public delegate void OnTouchIsland ();
	public static OnTouchIsland onTouchIsland;

	public bool targeted = false;

	public void OnPointerDown () {

		if (StoryLauncher.Instance.PlayingStory)
			return;

        if (!WorldTouch.Instance.isEnabled)
            return;

		Tween.Bounce (transform );

		if ( onTouchIsland != null ) {
			onTouchIsland ();
		}

		_collider.enabled = true;

		targeted = true;
	}

    public float min_RangeX = 0f;
    public float min_RangeY = 0f;

    public float max_RangeX = 0f;
    public float max_RangeY = 0f;

    private void OnMouseDown()
    {
        OnPointerDown();
    }
    public Vector2 GetRandomPosition () {

		if ( _transform == null )
			_transform= GetComponent<RectTransform> ();
        return new Vector2 (Random.Range(-min_RangeX,max_RangeX) , Random.Range(-min_RangeY,max_RangeY) );

	}
}
 