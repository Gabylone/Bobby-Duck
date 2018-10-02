using UnityEngine;
using System.Collections;

public class Bird : Touchable {

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
    public float characterSpeedToFly = 1f;

	[SerializeField]
	private GameObject featherPrefab;
	[SerializeField]
	private float deathForce = 100f;
	[SerializeField]
	private float featherForce = 200f;

    #region states vars
    // state machine
    public enum State
    {
        none,

        idle,
        moving,
        goToAnchor,
    }

    // components
    [Header("Components")]
    public Transform bodyTransform;
    public Transform getTransform;
    public Animator animator;

    [Header("State")]
    public State startState = State.idle;
    public State currentState;
    public State previousState;

    public float timeInState = 0f;

    delegate void UpdateState();
    UpdateState updateState;
    #endregion


    // Use this for initialization
    public void Start()
    {
        getTransform = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();

        ChangeState(startState);

    }

    public void Update()
    {
        if (updateState != null)
        {
            updateState();
            timeInState += Time.deltaTime;
        }

    }

    #region idle
    public void Idle_Start ()
	{
		animator.SetBool ("flying", false);
		currentTimeToTurn = Random.Range ( timeToTurnRange.x , timeToTurnRange.y );
	}
	public void Idle_Update ()
	{
		if (timeInState >= currentTimeToTurn) {

            timeInState = 0f;

            if (direction == Direction.Left)
                SetDirection(Direction.Right);
			else
                SetDirection(Direction.Left);

		}

		if ( Vector3.Distance (Character.Instance.getTransform.position, getTransform.localPosition ) < distanceToFlyAway ) {

			if (!Character.Instance.crouching )
            {
                ChangeState(State.moving);
                return;
            }


            if ( direction != Character.Instance.direction ) {

				if ( Character.Instance.crouching && timeInState > bufferTimeAfterTurn && Character.Instance.currentSpeed > characterSpeedToFly) {

                    if (timeInState > bufferTimeAfterTurn)
                    {
                        Debug.Log("time in state : " + timeInState);
                    }
					ChangeState (State.moving);
				}

			}

		}
	}
    void Idle_Exit()
    {

    }
	#endregion

	#region moving
	public void Moving_Start ()
	{
		animator.SetBool ("flying", true);
	}
	public void Moving_Update ()
	{
		if (timeInState <= flying_TimeToStop) {
			getTransform.Translate (Vector3.up * flySpeed * Time.deltaTime);
		}

		if ( Vector3.Distance (Character.Instance.getTransform.position, anchor.position) > distanceToFlyAway ) {

			if ( timeInState >= 5f ) {
				ChangeState (State.goToAnchor);
			}

		}
	}
    void Moving_Exit()
    {

    }
	#endregion

	#region go to anchor
	public void GoToAnchor_Start ()
	{
		
	}
	public void GoToAnchor_Update ()
	{
		Vector3 dir = (anchor.position - getTransform.position).normalized;

		getTransform.Translate (dir * distanceToFlyAway * Time.deltaTime);
		bodyTransform.right = dir;

		if (Vector3.Distance (getTransform.position, anchor.position) < Anchor_DistanceToIdle) {
			ChangeState (State.idle);
		}

	}
	public void GoToAnchor_Exit ()
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
		animator.SetBool ("dead", true);

		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Rigidbody2D> ().AddForce ( ((Vector2)Character.Instance.bodyTransform.right + Vector2.up) * deathForce );
		GetComponent<Rigidbody2D> ().AddTorque ( deathForce );




        for (int i = 0; i < 2; i++) {
			GameObject g = Instantiate (featherPrefab);
			g.transform.position = getTransform.position;
			g.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * featherForce);
		}

        Destroy(this);

    }

    void OnCollisionEnter2D (Collision2D coll) {
		if ( coll.gameObject.tag == "Rock" ) {

			ChangeState (State.moving);
		}
	}

    #region state machine
    public delegate void OnChangeState();
    public OnChangeState onChangeState;
    public void ChangeState(State targetState)
    {

        previousState = currentState;
        currentState = targetState;

        ExitPreviousState();
        StartTargetState();

        timeInState = 0f;

        if (onChangeState != null)
            onChangeState();

    }

    void StartTargetState()
    {
        switch (currentState)
        {
            case State.idle:
                updateState = Idle_Update;
                Idle_Start();
                break;

            case State.moving:
                updateState = Moving_Update;
                Moving_Start();
                break;

            case State.goToAnchor:
                updateState = GoToAnchor_Update;
                GoToAnchor_Start();
                break;
            default:
                break;
        }
    }

    void ExitPreviousState()
    {
        switch (previousState)
        {
            case State.idle:
                Idle_Exit();
                break;

            case State.moving:
                Moving_Exit();
                break;

            case State.goToAnchor:
                GoToAnchor_Exit();
                break;

            default:
                break;
        }
    }
    #endregion

    #region direction
    public Direction direction;
    public delegate void OnChangeDirection();
    public OnChangeDirection onChangeDirection;
    public void SetDirection ( Direction dir )
    {
        direction = dir;

        if (dir == Direction.Left)
        {
            bodyTransform.right = -Vector3.right;
        }
        else
        {
            bodyTransform.right = Vector3.right;
        }
    }
    #endregion
}


public enum Direction
{
    Right,
    Left
}