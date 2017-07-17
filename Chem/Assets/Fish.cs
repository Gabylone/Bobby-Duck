using UnityEngine;
using System.Collections;

public enum Side {
	Left,
	Right,
}

public class Fish : Ingredient {

	[SerializeField]
	private float speed = 2f;


	private Vector2 initPos;

	[SerializeField]
	private float jumpTorque = 50f;

	[SerializeField]
	private float jumpForce = 100f;

	bool jumping = false;
	[SerializeField]
	private Transform bodyTransform;

	private float currentTime = 0f;
	[SerializeField]
	private Vector2 range_TimeToJump = new Vector2(5f,10f);
	private float currentTimeToJump = 0f;

	[SerializeField]
	private Vector2 positionRange = new Vector2(-2,2);

	[SerializeField]
	Side side;

	public override void Start ()
	{
		base.Start ();

		Rigidody.isKinematic = true;

		initPos = transform.position;

		currentTime = Random.Range (range_TimeToJump.x ,range_TimeToJump.y);

		CanInteract = false;

		Side = Side.Left;
	}



	public override void Update ()
	{
		if ( !CanInteract )
			FishAround ();

	}

	public Side Side {
		get {
			return side;
		}
		set {
			side = value;

			transform.right = side == Side.Left ? Vector2.left : Vector2.right;
		}
	}

	void FishAround ()
	{
		if ( !jumping ) {

			if ( Side == Side.Left ) {
				if (transform.position.x <= initPos.x + positionRange.x) {
					Side = Side.Right;
				}
			} else {
				if (transform.position.x >= initPos.x + positionRange.y) {
					Side = Side.Left;
				}
			}

			transform.Translate (Vector2.right * speed * Time.deltaTime);

			currentTime -= Time.deltaTime;

			if ( currentTime <= 0f ) {
				jumping = true;
				Rigidody.isKinematic = false;
				Rigidody.AddForce ( Vector2.up * jumpForce );
				Rigidody.angularVelocity = Random.value < 0.5f ? jumpTorque : -jumpTorque;
			}

		} else {

			currentTime += Time.deltaTime;

			if ( transform.position.y < initPos.y && currentTime >= 0.1f ) {
				Side = Random.value < 0.5f ? Side.Left : Side.Right;
				transform.position = new Vector3 ( transform.position.x , initPos.y , 0f );

				Rigidody.velocity = Vector2.zero;
				Rigidody.angularVelocity = 0f;
				Rigidody.isKinematic = true;

				currentTime = Random.Range (range_TimeToJump.x ,range_TimeToJump.y);

				jumping = false;

			}

		}
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if ( coll.gameObject.tag == "Rock" && jumping) {

			CanInteract = true;
			Push ();
			print ("it get hit");
		}
	}

	void OnDrawGizmos () {

		Vector2 p = transform.position;
		if ( initPos != default (Vector2) )
			p = initPos;

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (p+(Vector2.right*positionRange.x),p+(Vector2.right*positionRange.y));
	}
}
