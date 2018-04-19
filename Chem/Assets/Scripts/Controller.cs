using UnityEngine;
using System.Collections;

public class Controller : Touchable {



	// state machine
	public enum State {
		none,

		state1,
		state2,
		state3,
		state4,
	}

	// components
	[Header("Components")]
	public Transform bodyTransform;
	public Transform getTransform;
	public Animator animator;

	[Header("State")]
	public State startState = State.state2;
	public State currentState;
	public State previousState;

	public float timeInState = 0f;

	delegate void UpdateState ();
	UpdateState updateState;

	// STOP
	float stopDuration = 0; 

	public virtual void Start () {

		getTransform = GetComponent<Transform>();
		animator = GetComponentInChildren<Animator> ();

		ChangeState (startState);

	}

	// Update is called once per frame
	public virtual void Update () {
		if (updateState != null) {
			updateState ();
			timeInState += Time.deltaTime;
		}

		UpdateGrounded ();

	}

	#region stop
	public void Stop (float duration) {
		stopDuration = duration;
		ChangeState (State.none);
	}
	public void Stop () {
		Stop (0f);
	}
	public virtual void None_Start () {

	}
	public virtual void None_Update () {
		if (stopDuration > 0f) {
			if (timeInState >= stopDuration) {
				ChangeState (State.state2);
			}
		}
	}
	public virtual void None_Exit () {
		//
	}
	#endregion

	#region shoot
	public virtual void State1_Start () {

	}
	public virtual void State1_Update () {

	}
	public virtual void State1_Exit () {
		//
	}
	#endregion

	#region moving
	public virtual void State2_Start ()
	{

	}
	public virtual void State2_Update()
	{

	}
	public virtual void State2_Exit ()
	{

	}
	#endregion

	#region shoot
	public virtual void State3_Start () {

	}
	public virtual void State3_Update () {

	}
	public virtual void State3_Exit () {
		//
	}
	#endregion

	#region water
	public virtual void State4_Start () {

	}
	public virtual void State4_Update () {

	}
	public virtual void State4_Exit () {
		//
	}
	#endregion

	#region state machine
	public delegate void OnChangeState ();
	public OnChangeState onChangeState;
	public void ChangeState ( State targetState ) {

		previousState = currentState;
		currentState = targetState;

		ExitPreviousState ();
		StartTargetState ();

		timeInState = 0f;

		if (onChangeState != null)
			onChangeState ();

	}

	void StartTargetState ()
	{
		switch (currentState) {
		case State.state1:
			updateState = State1_Update;
			State1_Start();
			break;

		case State.state2:
			updateState = State2_Update;
			State2_Start();
			break;

		case State.state3:
			updateState = State3_Update;
			State3_Start();
			break;

		case State.none:
			updateState = None_Update;
			None_Start();
			break;

		default:
			break;
		}
	}

	void ExitPreviousState ()
	{
		switch (previousState) {
		case State.state1:
			State1_Exit();
			break;

		case State.state2:
			State2_Exit ();
			break;

		case State.state3:
			State3_Exit();
			break;

		case State.none:
			None_Exit ();
			break;

		default:
			break;
		}
	}
	#endregion

	#region grounded

	public float groundedDistance = 0.2f;
	public float groundedDecal = 0.2f;
	public float boxScale = 0.7f;
	public bool grounded = false;
	public delegate void OnTouchGround();
	public OnTouchGround onTouchGround;
	void UpdateGrounded () {

		Vector2 origin = (Vector2)getTransform.position + (Vector2.up * groundedDecal);
		bool groundedRayCast = Physics2D.BoxCast (origin, new Vector2 (boxScale, groundedDistance), 0f, -Vector2.up, 0f);

		if (grounded == groundedRayCast)
			return;

		if ( groundedRayCast == true ) {
			grounded = true;
			if ( onTouchGround != null )
				onTouchGround ();
		}

		if ( groundedRayCast == false ) {
			grounded = false;
		}
	}
	#endregion

	public enum Direction {
		Right,
		Left
	}
	Direction _direction;
	public delegate void OnChangeDirection ();
	public OnChangeDirection onChangeDirection;
	public Direction direction {
		get {
			return _direction;
		}
		set {
			
			_direction = value;

			if (value == Direction.Left) {
				bodyTransform.right = -Vector3.right;
			} else {
				bodyTransform.right = Vector3.right;
			}
		}
	}
}
