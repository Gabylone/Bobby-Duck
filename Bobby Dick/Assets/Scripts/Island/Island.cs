using UnityEngine;
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
	Collider2D collider2D = null;

	Vector2 scale = Vector2.zero;

	public RectTransform uiBackground;

	private RectTransform rectTransform;

	[SerializeField]
	private RectTransform gameViewCenter;

	#region mono
	void Awake () {
		Instance = this;
	}

	void Start () {	

		rectTransform = GetComponent<RectTransform> ();
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

		WorldTouch.onTouchWorld += HandleOnTouchWorld;

		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);
	}

	void HandleOnSwipe (Directions direction)
	{
		collider2D.enabled = false;
	}

	void HandleOnTouchWorld ()
	{
		targeted = false;
		collider2D.enabled = false;
	}

	void HandleChunkEvent ()
	{
		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);

	}

	void DeactivateCollider ()
	{
		collider2D.enabled = false;
	}
	void ActivateCollider () {
		collider2D.enabled = true;
	}
	#endregion

	#region story
	public void Enter () {

		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager,StoryLauncher.StorySource.island);
	}
	#endregion

	#region render
	void UpdatePositionOnScreen(Coords coords) {

		Chunk chunk = Chunk.GetChunk (coords);

		IslandData islandData = chunk.IslandData;

		bool onIslandChunk = islandData != null;

		if (onIslandChunk) {

			gameObject.SetActive ( true );

			GetComponent<RectTransform> ().anchoredPosition = chunk.IslandData.positionOnScreen;

			GetComponentInChildren<Image>().sprite = sprites [islandData.SpriteID];

		} else {
			
			gameObject.SetActive ( false );

			transform.localPosition = new Vector3 (10000f, 0, 0);
		}
	}
	#endregion

	void OnCollisionStay2D ( Collision2D coll ) {
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

	public void Pointer_ClickIsland () {

		Tween.Bounce (transform );

		if ( onTouchIsland != null ) {
			onTouchIsland ();
		}

		collider2D.enabled = true;

		targeted = true;
	}

	public Vector2 GetRandomPosition () {

		if ( rectTransform == null )
			rectTransform = GetComponent<RectTransform> ();

//		float minX = uiBackground.rect.width + rectTransform.rect.width/2f;
//		float maxX = Screen.width - rectTransform.rect.width/2f;
//
//		float minY = rectTransform.rect.height/2f;
//		float maxY = Screen.height - rectTransform.rect.height/2f;
//
//		float x = Random.Range ( minX ,	maxX);
//		float y = Random.Range (minY, maxY);

		float x = Random.Range ( rectTransform.rect.width/2f , gameViewCenter.rect.width- (rectTransform.rect.width/2f) );
		float y = Random.Range ( rectTransform.rect.height/2f , gameViewCenter.rect.height - rectTransform.rect.height/2f );

		return new Vector2 (x,y);

	}
}
 