using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Island : MonoBehaviour {

	public static Island Instance;

	public Transform getTransform;

	[SerializeField]
	private Image image;

	[SerializeField]
	private Transform boat;

	[SerializeField] private float decal = 0f;

	[SerializeField] private GameObject group;

	[SerializeField]
	private float distanceToTrigger = 1f;

	[SerializeField]
	private Vector3 flagDecal;

	[SerializeField]
	private Sprite[] sprites;

	#region mono
	void Awake () {
		Instance = this;
	}

	public void Init() {
		getTransform = GetComponent<Transform>();
		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;
		CombatManager.Instance.fightStarting += DeactivateCollider;
		CombatManager.Instance.fightEnding += ActivateCollider;
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
		StoryLauncher.Instance.PlayStory (MapGenerator.Instance.CurrentChunk.IslandData.storyManager,StoryLauncher.StorySource.island);
	}
	#endregion

	#region render
	public void UpdatePositionOnScreen() {

		bool onIslandChunk = MapGenerator.Instance.CurrentChunk.State == ChunkState.DiscoveredIsland || MapGenerator.Instance.CurrentChunk.State == ChunkState.VisitedIsland || MapGenerator.Instance.CurrentChunk.State == ChunkState.UndiscoveredIsland;
		gameObject.SetActive ( onIslandChunk );

		if (onIslandChunk) {
			getTransform.localPosition = MapGenerator.Instance.CurrentChunk.IslandData.positionOnScreen;
			GetComponentInChildren<Image> ().sprite = sprites [MapGenerator.Instance.CurrentChunk.IslandData.SpriteID];
		} else {
			getTransform.localPosition = new Vector3 (10000f, 0, 0);
		}
	}
	#endregion

	public Sprite[] Sprites {
		get {
			return sprites;
		}
	}

	void OnCollisionStay2D ( Collision2D coll ) {
		if ( coll.gameObject.tag == "Player" ) {
			if (NavigationManager.Instance.FlagControl.TargetedIsland) {
				Island.Instance.Enter ();
				NavigationManager.Instance.FlagControl.TargetedIsland = false;
			}
		}
	}

}
