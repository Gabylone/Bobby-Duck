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
	private float distanceToFlyAway_Turned = 1f;

	[SerializeField]
	private float distanceToFlyAway_Front = 1f;


	[SerializeField]
	private float flying_TimeToStop = 3f;

	[SerializeField]
	private float bufferTimeAfterTurn = 1f;

	public override void InitController ()
	{
		base.InitController ();
	}

	public override void ControllerUpdate ()
	{
		base.ControllerUpdate ();
	}

	#region idle
	public override void Idle_Start ()
	{
		Animator.SetBool ("flying", false);
		currentTimeToTurn = Random.Range ( timeToTurnRange.x , timeToTurnRange.y );
	}
	public override void Idle_Update ()
	{
		if ( TimeInState >= currentTimeToTurn ) {
			BodyTransform.right = -BodyTransform.right;
			TimeInState = 0f;
		}

		float distanceToPlayer = Vector3.Distance (Character.Instance.GetTransform.position, GetTransform.localPosition);

		if ( distanceToPlayer < distanceToFlyAway_Front ) {

			Vector2 dirToPlayer = (Character.Instance.GetTransform.position - GetTransform.position).normalized;

				// is facing player
			if ( Vector2.Dot ( BodyTransform.right , dirToPlayer ) > 0 ) {

				if ( Character.Instance.Crouching == false && TimeInState >= bufferTimeAfterTurn ) {
					ChangeState (State.moving);
				}

			} else {

				if (distanceToPlayer < distanceToFlyAway_Front && Character.Instance.Crouching == false) {
					ChangeState (State.moving);
				}

			}

		}
	}
	#endregion

	#region moving
	public override void Moving_Start ()
	{
		Animator.SetBool ("flying", true);
	}
	public override void Moving_Update ()
	{
		base.Moving_Update ();

		if (TimeInState <= flying_TimeToStop) {
			GetTransform.Translate (Vector3.up * flySpeed * Time.deltaTime);
		}

		if ( Vector3.Distance (Character.Instance.GetTransform.position, GetTransform.localPosition ) > distanceToFlyAway_Turned ) {

			if ( TimeInState >= 5f ) {
				ChangeState (State.goToAnchor);
			}

		}
	}
	#endregion

	#region go to anchor
	public override void GoToAnchor_Start ()
	{
		base.GoToAnchor_Start ();
	}
	public override void GoToAnchor_Update ()
	{

		Vector3 dir = (anchor.position - GetTransform.position).normalized;

		GetTransform.Translate (dir * distanceToFlyAway_Turned * Time.deltaTime);
		BodyTransform.right = dir;

		if (Vector3.Distance (GetTransform.position, anchor.position) < Anchor_DistanceToIdle) {
			ChangeState (State.idle);
		}

	}
	public override void GoToAnchor_Exit ()
	{
		BodyTransform.up = Vector2.up;
		GetTransform.position = anchor.position;
	}
	#endregion

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, distanceToFlyAway_Turned);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, Anchor_DistanceToIdle);
	}


	[SerializeField]
	private GameObject featherPrefab;
	[SerializeField]
	private float deathForce = 100f;
	[SerializeField]
	private float featherForce = 200f;

	public void Die ()
	{
		Stop ();
		Animator.SetBool ("dead", true);

		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Rigidbody2D> ().AddForce ( ((Vector2)Character.Instance.BodyTransform.right + Vector2.up) * deathForce );

		for (int i = 0; i < 2; i++) {
			GameObject g = Instantiate (featherPrefab);
			g.transform.position = GetTransform.position;
			g.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * featherForce);
		}


	}

	void OnCollisionEnter2D (Collision2D coll) {
		if ( coll.gameObject.tag == "Rock" ) {

			ChangeState (State.moving);
		}
	}
}
 

