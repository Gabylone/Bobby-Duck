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

	BoxCollider2D boxCollider;

	void Awake (){
		Instance = this;
	}

	public override void Start ()
	{
		base.Start();

		StoryLauncher.Instance.onStartStory += HandlePlayStoryEvent;
		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		boxCollider = GetComponent<BoxCollider2D> ();

	}

	void HandleChunkEvent ()
	{
		Hide ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.DestroyShip ) {
			
			Visible = false;


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

		RectTransform rectTrans = NavigationManager.Instance.GetOppositeAnchor (otherBoatInfo.currentDirection).GetComponent<RectTransform>();
		SetTargetPos (rectTrans);
	}
	
	public override void Update ()
	{
		base.Update ();

		if  (metPlayer && StoryLauncher.Instance.PlayingStory == false ) {
			if ( Vector2.Distance ( targetRectTransform.position , transform.position ) < 0.5f ) {
				Hide ();
			}
		}
	}

	public void Show ( OtherBoatInfo boatInfo ) 
	{
		print ("showing boat");

		boxCollider.enabled = true;

		this.otherBoatInfo = boatInfo; 

		Visible = true;

		UpdatePositionOnScreen ();
		
		if (otherBoatInfo.storyManager.CurrentStoryHandler.Story.param == 0) {

			// go about
			RectTransform rectTrans = NavigationManager.Instance.GetOppositeAnchor (otherBoatInfo.currentDirection).GetComponent<RectTransform> ();
			SetTargetPos (rectTrans);

			speed = startSpeed;
		} else {

			speed = followPlayer_Speed;

			// follow player
			RectTransform boatRectTransform = PlayerBoat.Instance.GetComponent<RectTransform>();
			SetTargetPos (boatRectTransform);
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

		getTransform.position = NavigationManager.Instance.OtherAnchors[(int)otherBoatInfo.currentDirection].position;


		metPlayer = false;

	}

	#region world
	void OnCollisionEnter2D (Collision2D collider) {
		if (metPlayer == false) {
			if (collider.gameObject.tag == "Player") {
				Enter ();
			}
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

			gameObject.SetActive (value);

			reachedPlayer = false;
		}
	}
	#endregion
}
