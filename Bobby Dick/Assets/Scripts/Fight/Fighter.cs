using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fighter : MonoBehaviour {

	public enum states {
		none,

		move,
		hit,
		getHit,
		guard,
		blocked
	}
	states currentState = states.move;
	states previousState;

	float timeInState = 0f;

	private delegate void UpdateState ();
	private UpdateState updateState;

	[Header ("Components")]
	[SerializeField]
	private Transform bodyTransform;
	private Animator animator;
	private CrewMember crewMember;
//	[SerializeField]
//	private MemberFeedback feedback;
	[SerializeField]
	private Transform dialogueAnchor;

	[Header ("Move")]
	[SerializeField]
	private float speed = 1f;
	[SerializeField]
	private Vector2 direction;

	[Header ("Hit")]
	[SerializeField]
	private float hit_Duration = 0.5f;
	[SerializeField]
	private float hit_TimeToDisableCollider = 0.6f;
	[SerializeField]
	private float hit_TimeToEnableCollider = 0.5f;
	[SerializeField]
	private float hitSpeed = 1f;
	[SerializeField]
	private BoxCollider2D weaponCollider;
	[SerializeField]
	private GameObject impactEffect;

	[SerializeField]
	private string hitTag = "Weapon";

	[Header ("Get Hit")]
	[SerializeField]
	private float getHit_Speed = 2.5f;
	[SerializeField]
	private float getHit_Duration = 0.3f;
	[SerializeField]
	private float getHit_TimetoStop = 0.2f;

	[Header ("Blocked")]
	[SerializeField]
	private float blocked_Duration = 0.5f;

	[Header ("Guard")]
	[SerializeField]
	private float guard_TimeToEffective = 0.2f;
	private bool guard_Active = false;
	[SerializeField]
	private GameObject guard_Feedback;
	[SerializeField]
	private Image guard_Image;

	[SerializeField]
	private float guard_RechargeSpeed = 1f;

	bool guard_Tired = false;
	[SerializeField]
	private float guard_ChargeToRest = 0.3f;

	[SerializeField]
	private float guard_DecreaseSpeed = 2f;

	private float guard_CurrentCharge = 1f;

	[Header("Bounds")]
	[SerializeField]
	private float limitX = 4f;
	[SerializeField]
	private Transform leftAnchor;
	[SerializeField]
	private Transform rightAnchor;

	[Header("Sounds")]
	[SerializeField] private AudioClip hitSound;
	[SerializeField] private AudioClip hurtSound;

	[Header ("Target")]
	private Transform target;
	[SerializeField]
	private float stopDistance = 1f;
	[SerializeField]
	private float hitDistance = 1f; 
	[SerializeField]
	private float stopBuffer = 0.2f;
	[SerializeField]
	private GameObject chooseButton;

	// Use this for initialization
	public virtual void Start () {
		animator = GetComponentInChildren<Animator> ();
		weaponCollider.enabled = false;
		Guard_Active = false;
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	public virtual void Update () {

		if ( updateState != null )
			updateState ();

		timeInState += Time.deltaTime;

		ClampPos ();

		if (Input.GetKeyDown (KeyCode.H))
			ChangeState (states.none);

		if (Input.GetKeyDown (KeyCode.G))
			ChangeState (states.guard);

		if (Input.GetKeyDown (KeyCode.F))
			ChangeState (states.hit);
	}

	#region move
	public virtual void move_Start () {
		
	}
	public virtual void move_Update () {
		if (Vector3.Distance (target.position, transform.position) > stopDistance + stopBuffer) {

			transform.Translate (Direction * Speed * Time.deltaTime);
			Animator.SetFloat ("move", 1);

		} else if (Vector3.Distance (target.position, transform.position) < stopDistance - stopBuffer) {

			transform.Translate (-Direction * Speed * Time.deltaTime);
			Animator.SetFloat ("move", 1);

		} else {
			Animator.SetFloat ("move", 0);
		}
	}
	public virtual void move_Exit () {
		animator.SetFloat ("move", 0);
	}
	public void ClampPos () {
		Vector3 pos = transform.localPosition;
		pos.x = Mathf.Clamp (pos.x, leftAnchor.localPosition.x , rightAnchor.localPosition.x);

		transform.localPosition = pos;
	}
	#endregion

	#region hit
	bool hitting = false;
	public virtual void hit_Start () {

		animator.speed = 1 + crewMember.Dexterity / 10;

		animator.SetTrigger ( "hit" );
		animator.SetInteger ("hitType", Random.Range (0,2));

		hitting = false;

		weaponCollider.enabled = false;
	}


	public virtual void hit_Update () {

//		float f = hit_TimeToEnableCollider / 1 + crewMember.Dexterity / 10;
		float f = hit_TimeToEnableCollider;

		if ( timeInState > f ) {
			hitting = true;
			weaponCollider.enabled = true;
		}

		if ( hitting ) {
			transform.Translate (Direction * hitSpeed * Time.deltaTime);
		}

		if ( timeInState > hit_Duration / 1 + crewMember.Dexterity / 10)
			ChangeState (states.move);

	}
	public virtual void hit_Exit () {

		weaponCollider.enabled = false;

		animator.speed = 1f;

	}
	#endregion

	#region get hit
	public virtual void getHit_Start () {

		weaponCollider.enabled = false;

		animator.SetTrigger ("getHit");
	}
	public virtual void getHit_Update () {

		if (timeInState < getHit_TimetoStop)
			transform.Translate (-Direction * getHit_Speed * Time.deltaTime);

		if (timeInState > getHit_Duration)
			ChangeState (states.move);
	}
	public virtual void getHit_Exit () {
		//
	}
	#endregion

	#region guard
	public virtual void guard_Start () {
		animator.SetBool ( "guard" , true);
		Guard_Active = true;
	}

	public virtual void guard_Update () {

	}
	public virtual void guard_Exit () {
		animator.SetBool ( "guard" , false);
		Guard_Active = false;
	}
	#endregion

	#region blocked
	public virtual void blocked_Start () {
		
		animator.SetBool ( "blocked" , true);

		weaponCollider.enabled = false;
	}

	public virtual void blocked_Update () {

		if ( timeInState > 1f ) {
			animator.speed = 1f;
		}

		if (timeInState < getHit_TimetoStop)
			transform.Translate (-Direction * speed * Time.deltaTime);

		if (timeInState > blocked_Duration) {
			ChangeState (states.move);
		}

	}
	public void blocked_Exit () {
		animator.SetBool ( "blocked" , false);
	}
	#endregion

	#region state machine
	public void ChangeState ( states targetState ) {

		previousState = currentState;
		currentState = targetState;

		timeInState = 0f;

		ExitState ();
		EnterState ();
	}
	private void EnterState () {
		switch (currentState) {

		case states.none:
			updateState = null;
			break;

		case states.move:
			updateState = move_Update;
			move_Start();
			break;
		case states.hit:
			updateState = hit_Update;
			hit_Start();
			break;
		case states.getHit:
			updateState = getHit_Update;
			getHit_Start ();
			break;
		case states.guard:
			updateState = guard_Update;
			guard_Start ();
			break;
		case states.blocked:
			updateState = blocked_Update;
			blocked_Start ();
			break;
		}
	}
	private void ExitState () {
		switch (previousState) {

		case states.none:
				//
			break;

		case states.move:
			move_Exit ();
			break;
		case states.hit:
			hit_Exit ();
			break;
		case states.getHit:
			getHit_Exit ();
			break;
		case states.guard:
			guard_Exit ();
			break;
		case states.blocked:
			blocked_Exit ();
			break;
		}
	}
	#endregion

	public virtual void OnTriggerEnter2D (Collider2D other) {
		
		if ( other.tag == hitTag) {

			if ( currentState == states.getHit ) {
				Debug.Log ("didn't touch parce que déjà en get hit");
				return;
			}

			GetHit (other.GetComponentInParent<Fighter>());
		}
	}

	public void GetHit (Fighter otherFighter) {
		
		float dam = otherFighter.CrewMember.CurrentAttack;

		if ( DiceManager.Instance.HighestResult == 6 ) {
			SoundManager.Instance.PlaySound (hurtSound);
			impactEffect.transform.localScale = Vector3.one * 2f;
		} else {
			SoundManager.Instance.PlaySound (hitSound);
			impactEffect.transform.localScale = Vector3.one * 0.75f;
		}

		if (Guard_Active) {
			dam /= 4;
			ChangeState (states.blocked);
			impactEffect.GetComponent<SpriteRenderer> ().color = Color.grey;
		} else {
			ChangeState (states.getHit);
			impactEffect.GetComponent<SpriteRenderer> ().color = Color.red;
		}

			// collision effect
		impactEffect.SetActive (false);
		impactEffect.SetActive (true);

		impactEffect.transform.position = otherFighter.WeaponCollider.transform.position;


			// set other state
		otherFighter.ChangeState (states.blocked);
		crewMember.GetHit (dam);

		if (crewMember.Health <= 0) {
			Die ();
		}
	}

	public virtual void Die () {
		CombatManager.Instance.DeleteFighter (this);

		Animator.SetBool ("dead", true);
		ChangeState (states.none);

		CombatManager.Instance.NextTurn ();

		CombatManager.Instance.currentMember.AddXP (CrewMember.Level * 25);

		CrewMember.Kill ();
	}

	#region getters
	public Animator Animator {
		get {
			return animator;
		}
	}
	public float Speed {
		get { return speed; }
	}
	public Transform BodyTransform {
		get {
			return bodyTransform;
		}
	}
	public Vector2 Direction {
		get {
			return direction;
		}
		set {
			direction = value;
		}
	}
	#endregion

	public float TimeInState {
		get {
			return timeInState;
		}
		set {
			timeInState = value;
		}
	}

	public bool Guard_Active {
		get {
			return guard_Active;
		}
		set {
			guard_Active = value;

			guard_Feedback.SetActive (value && !guard_Tired);
		}
	}

	public CrewMember CrewMember {
		get {
			return crewMember;
		}
		set {
			crewMember = value;
		}
	}

	public BoxCollider2D WeaponCollider {
		get {
			return weaponCollider;
		}
	}

	public GameObject ChooseButton {
		get {
			return chooseButton;
		}
	}

	public Transform Target {
		get {
			return target;
		}
		set {
			target = value;
		}
	}

}
