using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier_StateMachine : Selectable {

	delegate void UpdateState();
	UpdateState updateState;

	public State startState;

	public State prevState;
	public State currentState;

	public float timeInState = 0f;

	// Use this for initialization
	public virtual void Start () {
		ChangeState (startState);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		updateState ();

		timeInState += Time.deltaTime;
	}

	#region state1
	public virtual void Idle_Start() {
		//
	}
	public virtual void Idle_Update() {
		//
	}
	public virtual void Idle_Exit() {
		//
	}
	#endregion

	#region state2
	public virtual void Moving_Start() {
		//
	}
	public virtual void Moving_Update() {
		//
	}
	public virtual void Moving_Exit() {
		//
	}
	#endregion

	#region state3
	public virtual void InCover_Start() {
		//
	}
	public virtual void InCover_Update() {
		//
	}
	public virtual void InCover_Exit() {
		//
	}
	#endregion

	#region state4
	public virtual void Aiming_Start() {
		//
	}
	public virtual void Aiming_Update() {
		//
	}
	public virtual void Aiming_Exit() {
		//
	}
	#endregion

	#region hit
	public virtual void Hit_Start() {
		//
	}
	public virtual void Hit_Update() {
		//
	}
	public virtual void Hit_Exit() {
		//
	}
	#endregion

	#region shoot
	public virtual void Shoot_Start() {
		//
	}
	public virtual void Shoot_Update() {
		//
	}
	public virtual void Shoot_Exit() {
		//
	}
	#endregion

	#region state
	public enum State {
		idle,
		move,
		inCover,
		aiming,
		hit,
		shoot
	}
	public void ChangeState ( State targetState ) {

		timeInState = 0f;

		prevState = currentState;
		currentState = targetState;

		ExitState (prevState);
		NewState (currentState);

	}

	void NewState (State targetState)
	{
		switch (targetState) {
		case State.idle:
			updateState = Idle_Update;
			Idle_Start ();
			break;
		case State.move:
			updateState = Moving_Update;
			Moving_Start ();
			break;
		case State.inCover:
			updateState = InCover_Update;
			InCover_Start ();
			break;
		case State.aiming:
			updateState = Aiming_Update;
			Aiming_Start ();
			break;
		case State.hit:
			updateState = Hit_Update;
			Hit_Start ();
			break;
		case State.shoot:
			updateState = Shoot_Update;
			Shoot_Start ();
			break;
		default:
			break;
		}
	}

	void ExitState (State targetState)
	{
		switch (targetState) {
		case State.idle:
			Idle_Exit ();
			break;
		case State.move:
			Moving_Exit ();
			break;
		case State.inCover:
			InCover_Exit ();
			break;
		case State.aiming:
			Aiming_Exit ();
			break;
		case State.hit:
			Hit_Exit ();
			break;
		case State.shoot:
			Shoot_Exit ();
			break;
		default:
			break;
		}
	}
	#endregion

}
