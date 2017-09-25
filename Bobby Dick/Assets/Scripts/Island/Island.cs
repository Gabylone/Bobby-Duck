using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Island : MonoBehaviour {

	public static Island Instance;

	public static Sprite[] sprites;

	private Image image;

	[SerializeField]
	private Transform boat;

	[SerializeField] private float decal = 0f;

	[SerializeField] private GameObject group;

	[SerializeField]
	private float distanceToTrigger = 1f;

	Vector2 scale = Vector2.zero;

	public RectTransform anchor;

	#region mono

	void Awake () {
		Instance = this;
	}

	void Start () {	
		Init ();
	}

	public void Init () {
		sprites = Resources.LoadAll<Sprite> ("Graph/IslandSprites");

		CombatManager.Instance.fightStarting 	+= DeactivateCollider;
		CombatManager.Instance.fightEnding 		+= ActivateCollider;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		image = GetComponentInChildren<Image> ();

		UpdatePositionOnScreen (Boats.PlayerBoatInfo.coords);
	}

	void HandleChunkEvent ()
	{
		UpdatePositionOnScreen (Boats.PlayerBoatInfo.currentCoords);

	}

	void DeactivateCollider ()
	{
		GetComponentInChildren<BoxCollider2D> ().enabled = false;
	}
	void ActivateCollider () {
		GetComponentInChildren<BoxCollider2D> ().enabled = true;
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
			if (NavigationManager.Instance.FlagControl.TargetedIsland) {
				Enter ();
				NavigationManager.Instance.FlagControl.TargetedIsland = false;
			}
		}
	}

	public delegate void OnTouchIsland ();
	public static OnTouchIsland onTouchIsland;
	public void Pointer_ClickIsland () {

		Tween.Bounce (transform );

		if ( onTouchIsland != null ) {
			onTouchIsland ();
		}



	}

	public Vector2 GetRandomPosition () {
		float x = Random.Range (GetComponent<RectTransform> ().rect.width/2,anchor.rect.width-GetComponent<RectTransform> ().rect.width);
		float y = Random.Range (GetComponent<RectTransform> ().rect.height/2,anchor.rect.height-GetComponent<RectTransform> ().rect.height);

		return new Vector2 (x,y);
	}
}
