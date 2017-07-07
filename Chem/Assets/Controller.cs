using UnityEngine;
using System.Collections;

public class Controller : Touchable {

	// state machine
	public enum State {
		stop,

		// bird
		goToAnchor,

		// character
		idle,
		moving,
		shoot,
	}

	// components
	[Header("Components")]
	[SerializeField]
	private Transform bodyTransform;
	private Transform getTransform;
	private Animator animator;

	[Header("State")]
	[SerializeField]
	private State startState = State.moving;
	private State currentState;
	private State previousState;

	private float timeInState = 0f;

	private delegate void UpdateState ();
	private UpdateState updateState;

	// STOP
	private float stopDuration = 0; 

	// Use this for initialization
	void Start () {
		InitController ();
	}
	void LateUpdate () {
		ControllerUpdate();
	}

	// Update is called once per frame
	public virtual void ControllerUpdate () {
		if (updateState != null) {
			updateState ();
			timeInState += Time.deltaTime;
		}
	}

	public virtual void InitController () {


		getTransform = GetComponent<Transform>();
		animator = GetComponentInChildren<Animator> ();

		ChangeState (startState);

	}

	#region stop
	public void Stop (float duration) {
		stopDuration = duration;
		ChangeState (State.stop);
	}
	public void Stop () {
		Stop (0f);
	}
	public virtual void Stop_Start () {

	}
	public virtual void Stop_Update () {
		if (stopDuration > 0f) {
			if (TimeInState >= stopDuration) {
				ChangeState (State.moving);
			}
		}
	}
	public virtual void Stop_Exit () {
		//
	}
	#endregion

	#region moving
	public virtual void Moving_Start ()
	{

	}
	public virtual void Moving_Update ()
	{

	}
	public virtual void Moving_Exit ()
	{

	}
	#endregion

	#region shoot
	public virtual void Shoot_Start () {

	}
	public virtual void Shoot_Update () {
		
	}
	public virtual void Shoot_Exit () {
		//
	}
	#endregion

	#region shoot
	public virtual void Idle_Start () {

	}
	public virtual void Idle_Update () {

	}
	public virtual void Idle_Exit () {
		//
	}
	#endregion

	#region shoot
	public virtual void GoToAnchor_Start () {

	}
	public virtual void GoToAnchor_Update () {

	}
	public virtual void GoToAnchor_Exit () {
		//
	}
	#endregion

	#region state machine
	public void ChangeState ( State targetState ) {

		previousState = currentState;
		currentState = targetState;

		ExitPreviousState ();
		StartTargetState ();

		timeInState = 0f;

	}

	void StartTargetState ()
	{
		switch (currentState) {
		case State.moving:
			updateState = Moving_Update;
			Moving_Start();
			break;
		case State.shoot:
			updateState = Shoot_Update;
			Shoot_Start();
			break;
		case State.idle:
			updateState = Idle_Update;
			Idle_Start();
			break;
		case State.goToAnchor:
			updateState = GoToAnchor_Update;
			GoToAnchor_Start();
			break;
		case State.stop:
			updateState = Stop_Update;
			Stop_Start();
			break;
		default:
			break;
		}
	}

	void ExitPreviousState ()
	{
		switch (previousState) {
		case State.moving:
			Moving_Exit ();
			break;
		case State.shoot:
			Shoot_Exit ();
			break;
		case State.idle:
			Idle_Exit();
			break;
		case State.goToAnchor:
			GoToAnchor_Exit();
			break;
		case State.stop:
			Stop_Exit ();
			break;
		default:
			break;
		}
	}

	public State CurrentState {
		get {
			return currentState;
		}
	}

	public State PreviousState {
		get {
			return previousState;
		}
	}
	#endregion

	public Transform GetTransform {
		get {
			return getTransform;
		}
	}

	public Transform BodyTransform {
		get {
			return bodyTransform;
		}
	}

	public Animator Animator {
		get {
			return animator;
		}
	}

	public float TimeInState {
		get {
			return timeInState;
		}
		set {
			timeInState = value;
		}
	}
}
