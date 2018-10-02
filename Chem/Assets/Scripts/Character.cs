using UnityEngine;
using System.Collections;

public class Character : Touchable {

    // state machine
    public enum State
    {
        none,

        moving,
        shooting,
        swimming,
    }

    // components
    [Header("Components")]
    public Transform bodyTransform;
    public Transform getTransform;
    public Animator animator;

    [Header("State")]
    public State startState = State.moving;
    public State currentState;
    public State previousState;

    public float timeInState = 0f;

    delegate void UpdateState();
    UpdateState updateState;

    // STOP
    float stopDuration = 0;

    public static Character Instance;

	// COMPONENT
	private Rigidbody2D rigidbody;
	private Thrower thrower;

	// MOVEMENT
	[Header("Moving")]
	public float movingSpeed = 5f;
	public float acceleration = 4f;
	public float decceleration = 10f;
	public float currentSpeed = 0f;

	private float targetSpeed;

	// JUMP
	[Header("Jump")]
	public float jumpForce = 150f;
	public bool doubleJumped = false;

    public float sideDecal = 1f;
    public float sideDistance = 1f;

	// SHOOT
	[Header("Shoot")]
	[SerializeField]
	private float shootRecoilSpeed = 1f;
	[SerializeField]
	private float shootRecoilDecc = 1f;
	private float shootCurrentRecoil = 1f;
	[SerializeField]
	private float shootDuration;

	// CROUCH
	[Header("Crouch")]
	[SerializeField]
	private float crouchSpeed = 2f;
	public bool crouching = false;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	public void Start ()
	{
        getTransform = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();

        rigidbody = GetComponent<Rigidbody2D> ();
		thrower = GetComponent <Thrower>();

		IngredientsSpiral.Instance.onOpenInventory += HandleOnOpenInventory;
		IngredientsSpiral.Instance.onCloseInventory += HandleOnCloseInventory;

        ChangeState(startState);

    }

    public void Update ()
	{
        if (updateState != null)
        {
            updateState();
            timeInState += Time.deltaTime;
        }

        UpdateGrounded();

        UpdateAnimation ();
	}

    public bool FacingWall()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + ((Vector2)bodyTransform.right * sideDistance) + Vector2.up * sideDecal, bodyTransform.right, 0.1f, tileLayerMask);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

	#region moving
    void Moving_Start()
    {

    }
	public void Moving_Update ()
	{
		targetSpeed = movingSpeed;
		if (crouching)
			targetSpeed = crouchSpeed;
        
       

		float acc = pressingInput () ? acceleration : decceleration;
		currentSpeed = Mathf.MoveTowards ( currentSpeed , pressingInput () ? targetSpeed : 0f , acc * Time.deltaTime );

       /* if (FacingWall())
        {
            currentSpeed = 0f;
        }*/

        if ( pressingInput () ) {
			Straighten ();
			if (Input.GetAxis ("Horizontal") < 0) {
                SetDirection(Direction.Left);
			} else {
                SetDirection(Direction.Right);
			}
		}
        
        if ( FacingWall () == false )
        {
            getTransform.Translate(bodyTransform.right * currentSpeed * Time.deltaTime);
        }

        if ( Input.GetButtonDown("Jump") ) {
			Jump ();
		}


		if (crouching) {
			if (!pressingCrouch ()) {
				crouching = false;
				animator.SetBool ("crouching", false);
			}
		} else {
			if (pressingCrouch ()) {
				crouching = true;
				animator.SetBool ("crouching", true);
			}
		}


	}
    void Moving_Exit()
    {

    }
	#endregion

	#region shoot
	public void Shooting_Start () {
		
		Throw ();

		animator.SetTrigger ("shoot");

		shootCurrentRecoil = shootRecoilSpeed;

		currentSpeed = 0f;
	}
	public void Shooting_Update () {

		float l = timeInState * shootDuration;

		getTransform.Translate ( -bodyTransform.right * (shootDuration - l) * Time.deltaTime );

		if ( timeInState >= shootDuration ) {
			ChangeState (State.moving);
		}
	}
	public void Shooting_Exit () {
		//
	}
	#endregion

	#region physics
	void DisablePhysics ()
	{
		rigidbody.bodyType = RigidbodyType2D.Kinematic;
	}
	void EnablePhysics () {
		rigidbody.bodyType = RigidbodyType2D.Dynamic;
	}
	#endregion

	#region jump
	public bool moving {
		get {
			return Input.GetAxis ("Horizontal") > 0 || Input.GetAxis ("Horizontal") < 0;
		}
	}

	public delegate void OnJump();
	public OnJump onJump;
	void Jump ()
	{
		if (grounded == false) {
			if (doubleJumped)
				return;

			animator.SetTrigger ("doubleJump");

			doubleJumped = true;
		} else {
			doubleJumped = false;
		}

		Straighten ();
		rigidbody.velocity = Vector2.zero;
		rigidbody.AddForce ( Vector2.up * jumpForce );

		if (onJump != null)
			onJump ();

	}
	void Straighten () {
		getTransform.up = Vector3.up;
	}
	void OnDrawGizmos () {

        bool gr = grounded;

		Gizmos.color = gr ? Color.green : Color.red;

		Gizmos.DrawCube ((Vector2)transform.position + (Vector2.up * (groundedDecal)), new Vector2(boxWidth , 0.1f));


        bool gr2 = FacingWall();

        Gizmos.color = gr2 ? Color.green : Color.red;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + (Vector2.right * sideDistance) + Vector2.up * sideDecal, Vector2.right);

        Gizmos.DrawRay((Vector2)transform.position + ((Vector2)bodyTransform.right * (sideDistance)) + Vector2.up * sideDecal,  bodyTransform.right * 0.1f);


    }
    #endregion

    void Throw()
    {
        thrower.Throw();
    }

    void UpdateAnimation ()
	{
		animator.SetFloat ("move", currentSpeed / targetSpeed);
		animator.SetBool ("jumping", !grounded);
		animator.SetBool ("crouching", pressingCrouch() );
	}

	void HandleOnCloseInventory ()
	{
		ChangeState (State.moving);
	}

	void HandleOnOpenInventory ()
	{
		ChangeState (State.none);
		print ("stoping");
	}

	#region touch effects
	public override void Touch ()
	{
		if (!base.AlreadyTouched ())
			return;

		base.Touch ();

		gameObject.AddComponent<FloatComponent_Living> ();
	}
	#endregion

	void OnTriggerEnter2D ( Collider2D col ) {
		if ( col.tag == "Water" ) {
			ChangeState (State.swimming);
			print ("enter swimming");
		}
	}

	void OnTriggerExit2D ( Collider2D col ) {
		if ( col.tag == "Water" ) {
			ChangeState (State.moving);
			print ("leave swimming");
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
            case State.moving:
                updateState = Moving_Update;
                Moving_Start();
                break;

            case State.shooting:
                updateState = Shooting_Update;
                Shooting_Start();
                break;

            default:
                break;
        }
    }

    void ExitPreviousState()
    {
        switch (previousState)
        {
            case State.moving:
                Moving_Exit();
                break;

            case State.shooting:
                Shooting_Exit();
                break;

            default:
                break;
        }
    }
    #endregion

    #region direction
    public Direction direction;
    public void SetDirection(Direction dir)
    {

        if ( direction == dir)
        {
            return;
        }

        direction = dir;

        currentSpeed = 0f;

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

    #region grounded
    public float groundedDistance = 0.2f;
    public float groundedDecal = 0.2f;
    public float boxWidth = 0.7f;
    public float boxHeight = 0.7f;
    public bool grounded = false;
    public delegate void OnTouchGround();
    public OnTouchGround onTouchGround;
    public LayerMask tileLayerMask;

    void UpdateGrounded()
    {

        Vector2 origin = (Vector2)getTransform.position + (Vector2.up * groundedDecal);
        bool groundedRayCast = Physics2D.BoxCast(origin, new Vector2(boxWidth, groundedDistance), 0f, -Vector2.up, 0f);

        if (grounded == groundedRayCast)
            return;

        if (groundedRayCast == true)
        {

            grounded = true;

            if (onTouchGround != null)
                onTouchGround();
        }

        if (groundedRayCast == false)
        {
            grounded = false;
        }
    }
    #endregion

    private bool pressingInput()
    {
        return Input.GetAxis("Horizontal") >= 0.3f || Input.GetAxis("Horizontal") <= -0.3f;
    }
    private bool pressingCrouch()
    {
        return Input.GetAxis("Vertical") <= -0.3f;
    }
}
