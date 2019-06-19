using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Island : RandomPlacable {

	public static Island Instance;

	public static Sprite[] sprites;
	public static Sprite[] minimapSprites;

    public GameObject[] islandMeshes;

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

    private IslandTrigger[] islandTriggers;

    public float min_RangeX = 0f;
    public float min_RangeY = 0f;

    public float max_RangeX = 0f;
    public float max_RangeY = 0f;

    public bool targeted = false;

    #region mono
    void Awake () {
		Instance = this;
    }

    public override void Start()
    {
        base.Start();

        _transform = GetComponent<Transform>();
        image = GetComponentInChildren<Image>();

        islandTriggers = GetComponentsInChildren<IslandTrigger>(true);

        Init();
    }

    void Init () {

		sprites = Resources.LoadAll<Sprite> ("Graph/IslandSprites");
		minimapSprites = Resources.LoadAll<Sprite> ("Graph/IslandMinimapSprites");

		Swipe.onSwipe += HandleOnSwipe;

		WorldTouch.onPointerExit += HandleOnTouchWorld;

		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);
	}

	void HandleOnSwipe (Directions direction)
	{
        DeactivateCollider();
	}

	void HandleOnTouchWorld ()
	{
		targeted = false;
        DeactivateCollider();
	}

    public override void HandleOnEnterNewChunk()
    {
        base.HandleOnEnterNewChunk();

        UpdatePositionOnScreen (Boats.playerBoatInfo.coords);

	}

	void DeactivateCollider ()
	{
        foreach (var item in islandTriggers)
        {
            item.DeactivateCollider();
        }
	}
	void ActivateCollider () {
        foreach (var item in islandTriggers)
        {
            item.ActivateCollider();
        }
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
            transform.localPosition = new Vector3( chunk.IslandData.worldPosition.x  , 0f , chunk.IslandData.worldPosition.y);
            transform.rotation = Quaternion.EulerAngles(0,chunk.IslandData.worldRotation,0);

            //GetComponentInChildren<Image>().sprite = sprites [islandData.storyManager.storyHandlers [0].Story.param];

            foreach (var item in islandMeshes)
            {
                item.SetActive(false);
            }
            islandMeshes[islandData.storyManager.storyHandlers[0].Story.param].SetActive(true);

		} else {
			
			gameObject.SetActive ( false );

			transform.localPosition = new Vector3 (10000f, 0, 0);
		}
	}
    #endregion

    public override void OnMouseDown()
    {
        base.OnMouseDown();

        ActivateCollider();

        targeted = true;
    }

    public Vector2 GetRandomPosition () {

		if ( _transform == null )
			_transform= GetComponent<RectTransform> ();

        return new Vector2 (Random.Range(-min_RangeX,max_RangeX) , Random.Range(-min_RangeY,max_RangeY) );
	}

    public void CollideWithPlayer()
    {
        if (targeted)
        {
            Enter();
            targeted = false;
        }
    }
}
 