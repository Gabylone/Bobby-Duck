using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Island : MonoBehaviour {

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

    public delegate void OnClickIsland();
    public static OnClickIsland onClickIsland;

    public bool targeted = false;

    public float min_RangeX = 0f;
    public float min_RangeY = 0f;

    public float max_RangeX = 0f;
    public float max_RangeY = 0f;

    #region mono
    void Awake () {


		Instance = this;
        onClickIsland = null;

    }

    void Start () {

        _transform = GetComponent<Transform>();
        image = GetComponentInChildren<Image>();

        islandTriggers = GetComponentsInChildren<IslandTrigger>(true );

        Init();
	
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
        DeactivateCollider();
	}

	void HandleOnTouchWorld ()
	{
		targeted = false;
        DeactivateCollider();
	}

	void HandleChunkEvent ()
	{
		UpdatePositionOnScreen (Boats.playerBoatInfo.coords);

	}

	void DeactivateCollider ()
	{
        Debug.Log("deactivating colliders");
        foreach (var item in islandTriggers)
        {
            item.DeactivateCollider();
        }
	}
	void ActivateCollider () {
        Debug.Log("activating colliders");
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

	

	public void OnPointerDown () {

        Debug.Log("ON POINTER ISLAND ");

		if (StoryLauncher.Instance.PlayingStory)
			return;

        if (!WorldTouch.Instance.IsEnabled())
            return;

		Tween.Bounce (transform );

		if ( onClickIsland != null ) {
			onClickIsland ();
		}

        ActivateCollider();

		targeted = true;
	}

    private void OnMouseDown()
    {
        OnPointerDown();
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
 