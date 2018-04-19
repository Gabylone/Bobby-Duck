using UnityEngine;
using System.Collections;

public class Character : Controller {

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
	public override void Start ()
	{
		base.Start ();

		rigidbody = GetComponent<Rigidbody2D> ();
		thrower = GetComponent <Thrower>();

		IngredientsSpiral.Instance.onOpenInventory += HandleOnOpenInventory;
		IngredientsSpiral.Instance.onCloseInventory += HandleOnCloseInventory;
	}
	
	public override void Update ()
	{
		base.Update ();
		UpdateAnimation ();
	}

	#region stop
	public override void None_Start ()
	{
		base.None_Start ();
		currentSpeed = 0f;
	}
	#endregion

	#region moving
	public override void State2_Update ()
	{
		base.State2_Update ();

		targetSpeed = movingSpeed;
		if (crouching)
			targetSpeed = crouchSpeed;

		float acc = pressingInput () ? acceleration : decceleration;
		currentSpeed = Mathf.MoveTowards ( currentSpeed , pressingInput () ? targetSpeed : 0f , acc * Time.deltaTime );

		if ( pressingInput () ) {
			Straighten ();
			if (Input.GetAxis ("Horizontal") < 0) {
				direction = Direction.Left;
			} else {
				direction = Direction.Right;
			}
		}

		getTransform.Translate ( bodyTransform.right * currentSpeed * Time.deltaTime );

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
	bool pressingInput () {
		return Input.GetAxis ("Horizontal") >= 0.3f || Input.GetAxis ("Horizontal") <= -0.3f;
	}
	bool pressingCrouch () {
		return Input.GetAxis ("Vertical") <= -0.3f;
	}
	void Throw ()
	{
		thrower.Throw ();	
	}
	#endregion

	#region shoot
	public override void State3_Start () {
		
		Throw ();

		animator.SetTrigger ("shoot");

		shootCurrentRecoil = shootRecoilSpeed;

		currentSpeed = 0f;
	}
	public override void State3_Update () {

		float l = timeInState * shootDuration;

		getTransform.Translate ( -bodyTransform.right * (shootDuration - l) * Time.deltaTime );

		if ( timeInState >= shootDuration ) {
			ChangeState (State.state2);
		}
	}
	public override void State3_Exit () {
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

	#region swimming
	public override void State4_Start ()
	{
		base.State4_Start ();

		DisablePhysics ();
	}


	public override void State4_Update ()
	{
		base.State4_Update ();

		if ( Input.GetButtonDown("Jump") ) {
			Jump ();
		}
	}
	public override void State4_Exit ()
	{
		base.State4_Exit ();

//		EnablePhysics ();
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

		bool gr = Physics2D.BoxCast ((Vector2)transform.position + (Vector2.up*groundedDecal), new Vector2(boxScale , groundedDistance), 0f, -Vector2.up, 0f);

		Gizmos.color = gr ? Color.green : Color.red;

		Gizmos.DrawCube ((Vector2)transform.position + (Vector2.up * (groundedDecal)), new Vector2(boxScale , groundedDistance));

	}
	#endregion

	void UpdateAnimation ()
	{
		animator.SetFloat ("move", currentSpeed / targetSpeed);
		animator.SetBool ("jumping", !grounded);
		animator.SetBool ("crouching", pressingCrouch() );
	}

	void HandleOnCloseInventory ()
	{
		ChangeState (State.state2);
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
			ChangeState (State.state4);
			print ("enter swimming");
		}
	}

	void OnTriggerExit2D ( Collider2D col ) {
		if ( col.tag == "Water" ) {
			ChangeState (State.state2);
			print ("leave swimming");
		}
	}

}
