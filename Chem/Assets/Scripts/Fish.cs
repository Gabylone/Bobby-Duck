using UnityEngine;
using System.Collections;
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
	private float maxX = 0f;
	[SerializeField]
	private float minX = 0f;

	[SerializeField]
	private AudioSource audioSource;

	public override void Start ()
	{
		base.Start ();

		Rigidody.isKinematic = true;

		initPos = transform.position;

		currentTime = Random.Range (range_TimeToJump.x ,range_TimeToJump.y);

		canInteract = false;

        CameraBehavior.Instance.onCamMove += HandleOnCamMove;
	}

    private void HandleOnCamMove(Coords newCoords)
    {
        
    }

    void Update ()
	{
		if ( !harvested )
			FishAround ();

	}

	void FishAround ()
	{
		if ( !jumping ) {

			if (transform.position.x >= initPos.x + maxX) {
				transform.right = Vector2.left;
			}

			if (transform.position.x <= initPos.x + minX) {
				transform.right = Vector2.right;
			}

			transform.Translate (Vector2.right * speed * Time.deltaTime);

			currentTime -= Time.deltaTime;

			if ( currentTime <= 0f ) {
				jumping = true;
				Rigidody.isKinematic = false;
				Rigidody.AddForce ( Vector2.up * jumpForce );
				Rigidody.angularVelocity = Random.value < 0.5f ? jumpTorque : -jumpTorque;
				audioSource.Play ();
			}

		} else {

			currentTime += Time.deltaTime;

			if ( transform.position.y < initPos.y && currentTime >= 0.1f ) {

				Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;

				transform.right = dir;

				transform.position = new Vector3 ( transform.position.x , initPos.y , 0f );

				Rigidody.velocity = Vector2.zero;
				Rigidody.angularVelocity = 0f;
				Rigidody.isKinematic = true;

				currentTime = Random.Range (range_TimeToJump.x ,range_TimeToJump.y);

				jumping = false;

				audioSource.Play ();
			}

		}
	}

	void OnTriggerEnter2D (Collider2D coll) {

        if (coll.tag == "Player" && jumping)
        {

            harvested = true;
            canInteract = true;

            Interact();
            
            //Push ();
        }

	}

	void OnDrawGizmos () {

		Vector2 p = transform.position;
		if ( initPos != default (Vector2) )
			p = initPos;

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine (p+(Vector2.right*minX),p+(Vector2.right*maxX));
	}
}
