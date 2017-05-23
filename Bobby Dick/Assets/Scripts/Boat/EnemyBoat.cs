using UnityEngine;
using System.Collections;

public class EnemyBoat : Boat {

	private OtherBoatInfo otherBoatInfo;

	[Space]
	[Header("Enemy Boat")]
	[SerializeField]
	private float distanceToStop = 1f;

	[SerializeField]
	private float boatSpeed = 1f;

	public bool followPlayer = false;
	private bool reachedPlayer = false;

	private bool visible = false;

	public override void Start ()
	{
		base.Start ();
	}
	
	public override void Update ()
	{
		if (reachedPlayer == false) {
			
			if (followPlayer) {
				FollowPlayer ();
			} else {
				GoAbout ();
			}
		}

		base.Update ();
	}

	private void FollowPlayer () {
		Vector2 boatPos = (Vector2)GetTransform.position;
		Vector2 playerBoatPos = (Vector2)PlayerBoat.Instance.GetTransform.position;

		TargetSpeed = boatSpeed;
		TargetDirection = (playerBoatPos - boatPos).normalized;
	}

	private void GoAbout () {
		Vector2 dir = NavigationManager.Instance.getDir (otherBoatInfo.currentDirection);

		TargetDirection = dir;

		TargetSpeed = boatSpeed;
	}

	void OnCollisionEnter2D (Collision2D collider) {
		if ( collider.gameObject.tag == "Player") {
			Enter ();
		}
	}

	public void Enter () {
		reachedPlayer = true;

		TargetSpeed = 0f;

		OtherBoatInfo.metPlayer = true;

		StoryReader.Instance.CurrentStoryHandler = OtherBoatInfo.StoryHandler;
		StoryLauncher.Instance.CurrentStorySource = StoryLauncher.StorySource.boat;
		StoryLauncher.Instance.PlayingStory = true;

		NavigationManager.Instance.FlagControl.PlaceFlagOnWorld (GetTransform.position);
	}

	public void Leave () {
		reachedPlayer = false;
		followPlayer = false;
	}

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
}
