using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Holoville.HOTween;
using UnityEngine.UI;

public class Humanoid : Placable {

	// STATES
	public enum State {
		Idle,
		Moving,
		Shooting,
		Attacking,

		None,
	}

    public State state = State.Idle;
	public State prevState;
	public delegate void OnUpdateStateMachine();
	public OnUpdateStateMachine onUpdateStateMachine;
	public float timeInState = 0f;

    public Color initColor;
    public Image image;

    public float moveDuration = 0f;

    public float speedBetweenLigns = 1f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        image = GetComponentInChildren<Image> ();

		ChangeState (state);

        Invoke("StartDelay",0.01f);
	}

    public virtual void StartDelay()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (onUpdateStateMachine != null) {
			onUpdateStateMachine ();
			timeInState += Time.deltaTime;
		}
	}

	#region idle
	public virtual void Idle_Start () {
//		agent.stop
	}
	public virtual void Idle_Update () {
		//
	}
	public virtual void Idle_Exit () {
		//
	}
	#endregion

	#region moving
	public virtual void Moving_Start () {
        //transform.localScale = Vector3.one * 0.8f;
        //HOTween.To( transform , moveDuration , "localScale" , Vector3.one );
    }
	public virtual void Moving_Update () {
        if (timeInState > moveDuration)
            ChangeState(State.Idle);
	}
	public virtual void Moving_Exit () {
		//transform.up = Vector2.up;
    }

    public virtual void ChangeLign (int newLineIndex)
	{
        currentLignIndex = newLineIndex;
	}
	public virtual void ChangeLignDelay () {


	}

    public virtual void Move(Vector3 v, float t)
    {
        moveDuration = t;

        HOTween.To(rectTransform, t, "position", v, false , EaseType.Linear , 0f );

        //transform.forward = (v - transform.position).normalized;

        CancelInvoke("MoveDelay");
        Invoke("MoveDelay", t);

        ChangeState(State.Moving);
    }
    public virtual void MoveDelay()
    {
		//ChangeState (State.Idle);
    }
    #endregion

    #region shooting
    public virtual void Shooting_Start () 
	{
		//
	}
	public virtual void Shooting_Update () 
	{

	}
	public virtual void Shooting_Exit () {
		//
	}
	#endregion

	#region attacking
	public virtual void Attacking_Start () 
	{
		//
	}
	public virtual void Attacking_Update () 
	{

	}
	public virtual void Attacking_Exit () {
		//
	}
	#endregion

	#region state machine
	public delegate void OnChangeState ( State newState , State prevState );
	public OnChangeState onChangeState;
	public virtual void ChangeState ( State targetState ) {

		prevState = state;
		state = targetState;

		timeInState = 0f;

		EnterNewState ();
		ExitPreviousState ();

		if ( onChangeState != null )
			onChangeState (state, prevState);
	}

	void EnterNewState ()
	{
		switch (state) {
		case State.Idle:
			onUpdateStateMachine = Idle_Update;
			Idle_Start ();
			break;
		case State.Moving:
			onUpdateStateMachine = Moving_Update;
			Moving_Start ();
			break;
		case State.Shooting:
			onUpdateStateMachine = Shooting_Update;
			Shooting_Start ();
			break;
		case State.Attacking:
			onUpdateStateMachine = Attacking_Update;
			Attacking_Start ();
			break;

		case State.None:
			onUpdateStateMachine = null;
			break;
		default:
			break;
		}
	}

	void ExitPreviousState ()
	{
		switch (prevState) {
		case State.Idle:
			Idle_Exit ();
			break;
		case State.Moving:
			Moving_Exit ();
			break;
		case State.Shooting:
			Shooting_Exit ();
			break;
		case State.Attacking:
			Attacking_Exit ();
			break;
		default:
			break;
		}
	}
	#endregion

	public virtual void Kill ()
	{
		ChangeState (State.None);

		float dur = 0.8f;

		HOTween.To (rectTransform, dur, "up", -Vector3.forward);
        HOTween.To(image, dur, "color", Color.clear);

		Tween.Bounce (transform);

		Destroy (gameObject, dur);
	}
}
