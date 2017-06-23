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
		GetComponentInChildren<BoxCollider2D> ().enabled = false;
	}
	void ActivateCollider () {
		GetComponentInChildren<BoxCollider2D> ().enabled = true;
	}
	#endregion

	#region story
	public void Enter () {
		StoryLauncher.Instance.PlayStory (MapData.Instance.currentChunk.IslandData.storyManager,StoryLauncher.StorySource.island);
	}
	#endregion

	#region render
	public void UpdatePositionOnScreen() {

		bool onIslandChunk = MapData.Instance.currentChunk.State == ChunkState.DiscoveredIsland || MapData.Instance.currentChunk.State == ChunkState.VisitedIsland || MapData.Instance.currentChunk.State == ChunkState.UndiscoveredIsland;
		gameObject.SetActive ( onIslandChunk );

		if (onIslandChunk) {
			getTransform.localPosition = MapData.Instance.currentChunk.IslandData.positionOnScreen;
			GetComponentInChildren<Image> ().sprite = sprites [MapData.Instance.currentChunk.IslandData.SpriteID];
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
}
