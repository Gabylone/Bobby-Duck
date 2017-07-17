using UnityEngine;
using System.Collections;

public class Character : Controller {

	public static Character Instance;

	private Rigidbody2D rigidbody;
	private Thrower thrower;

	[Header("Moving")]
	[SerializeField]
	private float movingSpeed = 5f;
	[SerializeField]
	private float acceleration = 4f;
	[SerializeField]
	private float decceleration = 10f;
	private float currentSpeed = 0f;
	private Vector2 deltaPoint;

	float targetSpeed;

	[Header("Jump")]
	[SerializeField]
	private float jumpForce = 150f;
	[SerializeField]
	private float groundedDistance = 0.2f;
	[SerializeField]
	private float groundedDecal = 0.2f;
	private bool doubleJumped = false;
	[SerializeField]
	private float boxScale = 0.7f;


	[Header("Shoot")]
	[SerializeField]
	private float shootRecoilSpeed = 1f;
	[SerializeField]
	private float shootRecoilDecc = 1f;
	private float shootCurrentRecoil = 1f;
	[SerializeField]
	private float shootDuration;

	[Header("Crouch")]
	[SerializeField]
	private float crouchSpeed = 2f;
	private bool crouching = false;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	public override void InitController ()
	{
		base.InitController ();

		rigidbody = GetComponent<Rigidbody2D> ();
		thrower = GetComponent <Thrower>();

	}
	
	public override void ControllerUpdate ()
	{
		base.ControllerUpdate ();
		UpdateAnimation ();
	}

	#region stop
	public override void Stop_Start ()
	{
		base.Stop_Start ();
		currentSpeed = 0f;
	}
	#endregion

	#region moving
	public override void Moving_Update ()
	{
		base.Moving_Update ();

		targetSpeed = movingSpeed;
		if (Crouching)
			targetSpeed = crouchSpeed;

		float acc = pressingInput () ? acceleration : decceleration;
		currentSpeed = Mathf.MoveTowards ( currentSpeed , pressingInput () ? targetSpeed : 0f , acc * Time.deltaTime );

		if ( pressingInput () ) {
			Straighten ();
			BodyTransform.right = Input.GetAxis ("Horizontal") > 0 ? Vector3.right : Vector3.left;
		}

		GetTransform.Translate ( BodyTransform.right * currentSpeed * Time.deltaTime );

		if ( Input.GetButtonDown("Jump") ) {
			Jump ();
		}

		if ( Input.GetButtonDown ("Fire1") ) {
			ChangeState (State.shoot);
		}

		if ( Input.GetButtonDown ("ShowIngredients") ) {
			IngredientsSpiral.Instance.Opened = !IngredientsSpiral.Instance.Opened;
		}

		if ( Input.GetButtonDown ("ShowPotions") ) {
			ChangeState (State.shoot);
		}

		if (Crouching) {
			if (!pressingCrouch ()) {
				crouching = false;
				Animator.SetBool ("crouching", false);
			}
		} else {
			if (pressingCrouch ()) {
				crouching = true;
				Animator.SetBool ("crouching", true);
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
	public override void Shoot_Start () {
		
		Throw ();

		Animator.SetTrigger ("shoot");

		shootCurrentRecoil = shootRecoilSpeed;

		currentSpeed = 0f;
	}
	public override void Shoot_Update () {

		float l = TimeInState * shootDuration;

		GetTransform.Translate ( -BodyTransform.right * (shootDuration - l) * Time.deltaTime );

		if ( TimeInState >= shootDuration ) {
			ChangeState (State.moving);
		}

		if ( !grounded() ) {
			
		}
	}
	public override void Shoot_Exit () {
		//
	}
	#endregion

	#region jump
	bool grounded () {

		Vector2 origin = (Vector2)GetTransform.position + (Vector2.up * groundedDecal);
		return Physics2D.BoxCast (origin, new Vector2(boxScale , groundedDistance), 0f, -Vector2.up, 0f);

		return Physics2D.Raycast ((Vector2)GetTransform.position + (Vector2.up * groundedDecal), -Vector2.up, groundedDistance);
	}
	void Jump ()
	{
		if (grounded ()== false) {
			if (doubleJumped)
				return;

			Animator.SetTrigger ("doubleJump");

			doubleJumped = true;
		} else {
			doubleJumped = false;
		}

		Straighten ();
		rigidbody.velocity = Vector2.zero;
		rigidbody.AddForce ( Vector2.up * jumpForce );
	}
	void Straighten () {
		GetTransform.up = Vector3.up;
	}
	void OnDrawGizmos () {

		bool gr = Physics2D.BoxCast ((Vector2)transform.position + (Vector2.up*groundedDecal), new Vector2(boxScale , groundedDistance), 0f, -Vector2.up, 0f);

		Gizmos.color = gr ? Color.green : Color.red;

		Gizmos.DrawCube ((Vector2)transform.position + (Vector2.up * (groundedDecal)), new Vector2(boxScale , groundedDistance));

	}
	#endregion

	void UpdateAnimation ()
	{
		Animator.SetFloat ("move", currentSpeed / targetSpeed);
		Animator.SetBool ("jumping", !grounded ());
		Animator.SetBool ("crouching", pressingCrouch() );
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

	public float CurrentSpeed {
		get {
			return currentSpeed;
		}
	}

	public bool Crouching {
		get {
			return crouching;
		}
	}
}
