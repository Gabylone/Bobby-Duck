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

	public override void Start ()
	{
		base.Start ();
	}
	
	public override void Update ()
	{

		if (followPlayer && !reachedPlayer) {
			FollowPlayer ();
		} else {
			GoAbout ();
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

			reachedPlayer = true;

			TargetSpeed = 0f;

			StoryLauncher.Instance.PlayingStory = true;

		}
	}

	public OtherBoatInfo OtherBoatInfo {
		get {
			return otherBoatInfo;
		}
		set {
			otherBoatInfo = value;
		}
	}
}
