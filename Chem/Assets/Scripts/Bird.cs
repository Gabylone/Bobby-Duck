using UnityEngine;
using System.Collections;

public class Bird : Controller {

	[SerializeField]
	private Transform anchor;
	[SerializeField]
	private float Anchor_DistanceToIdle = 1f;

	[SerializeField]
	private float flySpeed = 1f;

	[SerializeField]
	private Vector2 timeToTurnRange = new Vector2(5f,10f);
	private float currentTimeToTurn = 0f;

	[SerializeField]
	private float distanceToFlyAway = 1f;

	[SerializeField]
	private float flying_TimeToStop = 3f;

	[SerializeField]
	private float bufferTimeAfterTurn = 1f;

	[SerializeField]
	private GameObject featherPrefab;
	[SerializeField]
	private float deathForce = 100f;
	[SerializeField]
	private float featherForce = 200f;

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	#region idle
	public override void State1_Start ()
	{
		animator.SetBool ("flying", false);
		currentTimeToTurn = Random.Range ( timeToTurnRange.x , timeToTurnRange.y );
	}
	public override void State1_Update ()
	{
		if (timeInState >= currentTimeToTurn) {

			if (direction == Direction.Left)
				direction = Direction.Right;
			else
				direction = Direction.Left;
			
			timeInState = 0f;
		}

		if ( Vector3.Distance (Character.Instance.getTransform.position, getTransform.localPosition ) < distanceToFlyAway ) {

			if (Character.Instance.crouching == false) {
				ChangeState (State.state2);
			}

			if ( direction != Character.Instance.direction ) {

				if ( Character.Instance.crouching == false && timeInState >= bufferTimeAfterTurn ) {
					ChangeState (State.state2);
				}

			}

		}
	}
	bool caughtPlayer {
		get {
			bool playerIsInRange = Vector3.Distance (Character.Instance.getTransform.position, getTransform.localPosition) < distanceToFlyAway;
			bool playerIsCrouching = Character.Instance.crouching;
			bool facingPlayer = direction != Character.Instance.direction;
			bool waitedForBuffer = timeInState >= bufferTimeAfterTurn;
			bool playerIsMoving = Character.Instance.moving;

			if ( playerIsInRange ) {

				if (playerIsCrouching == false) {
					print ("flew because player is not crouching");
					return true;
				}

				if ( facingPlayer ) {
					if (playerIsMoving) {
						print ("flew because player is moving");
						return true;
					}
				}

			}

			return false;
		}
	}
	#endregion

	#region moving
	public override void State2_Start ()
	{
		animator.SetBool ("flying", true);
	}
	public override void State2_Update ()
	{
		base.State2_Update ();

		if (timeInState <= flying_TimeToStop) {
			getTransform.Translate (Vector3.up * flySpeed * Time.deltaTime);
		}

		if ( Vector3.Distance (Character.Instance.getTransform.position, getTransform.localPosition ) > distanceToFlyAway ) {

			if ( timeInState >= 5f ) {
				ChangeState (State.state3);
			}

		}
	}
	#endregion

	#region go to anchor
	public override void State3_Start ()
	{
		base.State3_Start ();
	}
	public override void State3_Update ()
	{
		Vector3 dir = (anchor.position - getTransform.position).normalized;

		getTransform.Translate (dir * distanceToFlyAway * Time.deltaTime);
		bodyTransform.right = dir;

		if (Vector3.Distance (getTransform.position, anchor.position) < Anchor_DistanceToIdle) {
			ChangeState (State.state1);
		}

	}
	public override void State3_Exit ()
	{
		bodyTransform.up = Vector2.up;
		getTransform.position = anchor.position;
	}
	#endregion

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, distanceToFlyAway);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, Anchor_DistanceToIdle);
	
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (anchor.position, 0.2f);
	}

	public void Die ()
	{
		Stop ();
		animator.SetBool ("dead", true);

		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Rigidbody2D> ().AddForce ( ((Vector2)Character.Instance.bodyTransform.right + Vector2.up) * deathForce );
		GetComponent<Rigidbody2D> ().AddTorque ( deathForce );

		for (int i = 0; i < 2; i++) {
			GameObject g = Instantiate (featherPrefab);
			g.transform.position = getTransform.position;
			g.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * featherForce);
		}


	}

	void OnCollisionEnter2D (Collision2D coll) {
		if ( coll.gameObject.tag == "Rock" ) {

			ChangeState (State.state2);
		}
	}
}