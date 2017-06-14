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

	public override void UpdatePositionOnScreen ()
	{
		base.UpdatePositionOnScreen ();


	}

	#region world
	private void FollowPlayer () {
		Vector2 boatPos = (Vector2)GetTransform.position;
		Vector2 playerBoatPos = (Vector2)PlayerBoat.Instance.GetTransform.position;

		TargetSpeed = boatSpeed;
		TargetDirection = (playerBoatPos - boatPos).normalized;


		Vector2 getDir = NavigationManager.Instance.getDir(BoatInfo.currentDirection);
		GetTransform.position = NavigationManager.Instance.OtherAnchors[(int)BoatInfo.currentDirection].position;
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
	#endregion

	#region story
	public void Enter () {
		
		reachedPlayer = true;

		TargetSpeed = 0f;

		OtherBoatInfo.metPlayer = true;

		StoryLauncher.Instance.PlayStory (OtherBoatInfo.StoryHandlers, StoryLauncher.StorySource.boat);

		NavigationManager.Instance.FlagControl.PlaceFlagOnWorld (GetTransform.position);
	}

	public void Leave () {
		reachedPlayer = false;
		followPlayer = false;
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
