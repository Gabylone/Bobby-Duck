using UnityEngine;
using System.Collections;

public class FightIA : Humanoid {

	[SerializeField]
	private Transform target;

	[SerializeField]
	private float stopDistance = 1f;

	[SerializeField]
	private float hitDistance = 1f;

	[SerializeField]
	private Vector2 hitRateRange = new Vector2 (1.5f , 4f);
	private float currentHitRange = 2f;

	int hitCount = 0;

	[SerializeField]
	private float chanceOfGuarding = 0.4f;
	[SerializeField]
	private Vector2 guardingDurationRange = new Vector2 ( 1.5f , 3f );

	[SerializeField]
	private float stopBuffer = 0.2f;

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateStateMachine ();
	}

	#region move
	public override void move_Start ()
	{
		base.move_Start ();

		currentHitRange = Random.Range (hitRateRange.x, hitRateRange.y) - (hitCount/3);
	}
	public override void move_Update ()
	{
		base.move_Update ();

		if (transform.position.x < target.position.x) {

			transform.Translate (-Direction * Speed * 1.5f * Time.deltaTime);
			Animator.SetFloat ("move", 1);
		} else {

			if (Vector3.Distance (target.position, transform.position) > stopDistance + stopBuffer) {
			
				transform.Translate (Direction * Speed * Time.deltaTime);
				Animator.SetFloat ("move", 1);

			} else {

				if (TimeInState > currentHitRange) {

					if (Vector3.Distance (target.position, transform.position) > hitDistance) {

						transform.Translate (Direction * Speed * Time.deltaTime);
						Animator.SetFloat ("move", 1);

					} else {

						if (Random.value < 0.15f)
							ChangeState (states.guard);
						else
							ChangeState (states.hit);
					}

				} else {
				
					if (Vector3.Distance (target.position, transform.position) < stopDistance - stopBuffer) {
					
						transform.Translate (-Direction * Speed * Time.deltaTime);
						Animator.SetFloat ("move", 1);

					} else {
						Animator.SetFloat ("move", 0);
					}
				}

			}
		}

	}
	#endregion

	#region hit
	public override void hit_Start()
	{
		base.hit_Start();
		currentHitRange = Random.Range (hitRateRange.x, hitRateRange.y);

		hitCount = 0;

	}
	public override void hit_Exit ()
	{
		base.hit_Exit ();
//
//		if (Random.value < 0.4f)
//			ChangeState (states.hit);
	}
	#endregion

	#region get hit
	public override void getHit_Start ()
	{
		base.getHit_Start ();

		++hitCount;
	}
	public override void getHit_Exit ()
	{
		base.getHit_Exit ();

		float r = Random.value;

		if ( r < 0.45f )
			ChangeState (states.hit);

		if ( r >= 0.45f && r < 0.65f )
			ChangeState (states.guard);
	}
	#endregion

	#region guard
	public override void guard_Start ()
	{
		base.guard_Start ();

		currentHitRange = Random.Range ( guardingDurationRange.x , guardingDurationRange.y );
	}
	public override void guard_Update ()
	{
		base.guard_Update ();

		if ( TimeInState > currentHitRange ) {
			ChangeState (states.move);
		}
	}
	public override void guard_Exit ()
	{
		base.guard_Exit ();

		if (Random.value < 0.4f)
			ChangeState (states.hit);
	}
	#endregion
}
