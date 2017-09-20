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

	[SerializeField]
	private Vector3 flagDecal;



	#region mono
	void Start () {


		Init ();
	}

	public void Init () {
		sprites = Resources.LoadAll<Sprite> ("Graph/IslandSprites");

		CombatManager.Instance.fightStarting += DeactivateCollider;
		CombatManager.Instance.fightEnding += ActivateCollider;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		image = GetComponentInChildren<Image> ();
	}

	void HandleChunkEvent ()
	{
		UpdatePositionOnScreen (Boats.PlayerBoatInfo.currentCoords);
	}

	void DeactivateCollider ()
	{
		GetComponentInChildren<PolygonCollider2D> ().enabled = false;
	}
	void ActivateCollider () {
		GetComponentInChildren<PolygonCollider2D> ().enabled = true;
	}
	#endregion

	#region story
	public void Enter () {
		StoryLauncher.Instance.PlayStory (Chunk.currentChunk.IslandData.storyManager,StoryLauncher.StorySource.island);
	}
	#endregion

	#region render
	public void UpdatePositionOnScreen(Coords coords) {

		Chunk chunk = Chunk.GetChunk (coords);

		IslandData islandData = chunk.IslandData;

		bool onIslandChunk = islandData != null;

		if (onIslandChunk) {

			print ("spotted island");

			gameObject.SetActive ( true );

//			transform.localPosition = chunk.IslandData.positionOnScreen;
			transform.localPosition = Vector2.zero;
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
}
