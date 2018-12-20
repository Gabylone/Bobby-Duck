using UnityEngine;
using System.Collections;

public class EnemyBoat : Boat {

	public static EnemyBoat Instance;

	private OtherBoatInfo otherBoatInfo;

	public bool followPlayer = false;
	private bool reachedPlayer = false;
	private bool metPlayer = false;

	private bool visible = false;

	public float leavingSpeed = 20f;
	public float followPlayer_Speed= 15f;

	BoxCollider boxCollider;

    public GameObject group;

    public Directions testDir;

	void Awake (){
		Instance = this;
	}

	public override void Start ()
	{
		base.Start();

		StoryLauncher.Instance.onPlayStory += HandlePlayStoryEvent;
		StoryLauncher.Instance.onEndStory += HandleEndStoryEvent;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		boxCollider = GetComponent<BoxCollider> ();

        Hide();

    }

	void HandleChunkEvent ()
	{
        Hide();
    }

    void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.DestroyShip ) {
			
			Visible = false;

			Debug.Log ("destroying ship : boat objet");


		}
	}

	void HandlePlayStoryEvent ()
	{
		EndMovenent ();
	}

	void HandleEndStoryEvent ()
	{

		if (StoryReader.Instance.CurrentStoryHandler.storyType != StoryType.Boat)
			return;

		reachedPlayer = false;
		followPlayer = false;

		speed = leavingSpeed;

        Vector3 corner = NavigationManager.Instance.GetOppositeCornerPosition(otherBoatInfo.currentDirection);
        Vector3 p = corner + (corner - getTransform.position).normalized * 2f;

        SetTargetPos(p);
	}
	
	public override void Update ()
	{
		base.Update ();

		if  (metPlayer && StoryLauncher.Instance.PlayingStory == false ) {
			if ( Vector2.Distance (targetPos , transform.position ) < 0.5f ) {
				Hide ();
			}
		}
	}

	public void Show ( OtherBoatInfo boatInfo ) 
	{
		boxCollider.enabled = true;

		this.otherBoatInfo = boatInfo; 

		Visible = true;

		UpdatePositionOnScreen ();

        if (otherBoatInfo.storyManager.CurrentStoryHandler.Story.param == 0)
        {

            // go about
            Vector3 corner = NavigationManager.Instance.GetOppositeCornerPosition(otherBoatInfo.currentDirection);
            Vector3 p = corner + (corner - getTransform.position).normalized * 3f;

            SetTargetPos(p);

            speed = startSpeed;

        }
        else
        {

            // follow player
            SetTargetPos(PlayerBoat.Instance.getTransform.position);

            speed = followPlayer_Speed;

        }

    }

	void Hide () {
		Visible = false;
		OtherBoatInfo = null;
	}

	public override void UpdatePositionOnScreen ()
	{
		base.UpdatePositionOnScreen ();

		Vector2 getDir = NavigationManager.Instance.getDir(OtherBoatInfo.currentDirection);

        getTransform.position = NavigationManager.Instance.GetCornerPosition(otherBoatInfo.currentDirection);


		metPlayer = false;

	}

	#region world
	void OnTriggerEnter (Collider other) {

		if (metPlayer == false) {

            if (other.tag == "Player")
            {
                Enter();
            }

		}
        else
        {
            print("met player");
        }
	}
	#endregion

	#region story
	public void Enter () {

		metPlayer = true;
		reachedPlayer = true;

        boxCollider.enabled = false;

		StoryLauncher.Instance.PlayStory (OtherBoatInfo.storyManager, StoryLauncher.StorySource.boat);
	}
	#endregion

	#region properties
	public OtherBoatInfo OtherBoatInfo {
		get {
			return otherBoatInfo;
		}
		set {
			otherBoatInfo = value;
		}
	}

	public bool Visible {
		get {
			return visible;
		}
		set {

			visible = value;

			reachedPlayer = false;

            group.SetActive(value);

        }
	}
	#endregion
}
