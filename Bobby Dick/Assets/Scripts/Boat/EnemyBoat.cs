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

	void Awake (){
		Instance = this;
	}

	public override void Start ()
	{
		base.Start();

		StoryLauncher.Instance.playStoryEvent += HandlePlayStoryEvent;
		StoryLauncher.Instance.endStoryEvent += HandleEndStoryEvent;

		StoryFunctions.Instance.getFunction += HandleGetFunction;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

	}

	void HandleChunkEvent ()
	{
		Hide ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		if ( func == FunctionType.DestroyShip ) {
			
			Visible = false;

			Boats.Instance.otherBoatInfos.Remove (otherBoatInfo);

			StoryReader.Instance.NextCell ();
			StoryReader.Instance.UpdateStory ();
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
	}

	public void Show ( OtherBoatInfo boatInfo ) 
	{
		this.otherBoatInfo = boatInfo; 

		Visible = true;

		UpdatePositionOnScreen ();


		if (otherBoatInfo.StoryHandlers.CurrentStoryHandler.Story.param == 0) {

			// go about
			RectTransform rectTrans = NavigationManager.Instance.GetOppositeAnchor (otherBoatInfo.currentDirection).GetComponent<RectTransform> ();
			SetTargetPos (rectTrans);
		} else {
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
		speed = startSpeed;

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

		StoryLauncher.Instance.PlayStory (OtherBoatInfo.StoryHandlers, StoryLauncher.StorySource.boat);
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
