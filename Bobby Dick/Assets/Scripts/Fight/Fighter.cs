using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fighter : MonoBehaviour {

	public enum states {
		dead,

		none,

		moveToTarget,
		moveBack,
		hit,
		getHit,
		guard,
		blocked
	}
	private states currentState = states.moveToTarget;

	public states CurrentState {
		get {
			return currentState;
		}
	}

	private states previousState;

	[SerializeField]
	private CombatFeedback combatFeedback;

	float timeInState = 0f;

	private delegate void UpdateState ();
	private UpdateState updateState;

	private int ID = 0;

	[Header ("Components")]
	[SerializeField]
	private Transform bodyTransform;
	private Animator animator;
	private CrewMember crewMember;
//	[SerializeField]
//	private MemberFeedback feedback;
	public Transform dialogueAnchor;

	private Fight_LoadSprites fightSprites;

	[Header ("Move")]
	[SerializeField]
	private float speed = 1f;
	public enum Direction {
		Right,
		Left
	}
	[SerializeField]
	private Direction direction;
	int dir = 0;

	[Header ("Hit")]
	[SerializeField]
	private float hit_Duration = 0.5f;
	public float Hit_Duration { get { return hit_Duration; } }

	[SerializeField]
	private float hit_TimeToDisableCollider = 0.6f;
	[SerializeField]
	private float hit_TimeToEnableCollider = 0.5f;
	public float Hit_TimeToEnableCollider {
		get {
			return hit_TimeToEnableCollider;
		}
	}

	[SerializeField]
	private float hitSpeed = 1f;
	[SerializeField]
	private BoxCollider2D weaponCollider;
	private BoxCollider2D bodyCollider;
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
	private Vector2 initPos = Vector2.zero;
	[SerializeField]
	private float timeToInitPos;

	[Header("Sounds")]
	[SerializeField] private AudioClip hitSound;
	[SerializeField] private AudioClip hurtSound;

	[Header ("Target")]
	private Fighter targetFighter;
	[SerializeField]
	private float stopDistance = 1f;
	[SerializeField]
	private float hitDistance = 1f; 
	[SerializeField]
	private float stopBuffer = 0.2f;
	[SerializeField]
	private GameObject chooseButton;
	bool hitting = false;

	bool reachedTarget = false;
	bool backToInitPos = false;

	[Header("Info Button")]
	[SerializeField]
	private GameObject infoButton;

	private int turnsToSkip;

	// Use this for initialization
	public virtual void Start () {

		animator = GetComponentInChildren<Animator> ();
		bodyCollider = GetComponent<BoxCollider2D> ();
		weaponCollider.enabled = false;
		Guard_Active = false;
		gameObject.SetActive (false);

		fightSprites = GetComponentInChildren<Fight_LoadSprites> ();
		fightSprites.Init ();

		dir = direction == Direction.Left ? 1 : -1;

		initPos = transform.position;

		infoButton.SetActive (false);

		chooseButton.SetActive (false);


	}

	// Update is called once per frame
	public virtual void Update () {

		if ( updateState != null )
			updateState ();

		timeInState += Time.deltaTime;

		ClampPos ();
//
//		if (Input.GetKeyDown (KeyCode.G))
//			ChangeState (states.guard);
//
//		if (Input.GetKeyDown (KeyCode.F))
//			ChangeState (states.hit);
	}

	// c'est pour que le start se fasse qu'on fois seulement pour ça c'est idiot mais bon
	bool started = false;

	#region initalization
	public void Init (CrewMember crewMember, int id )
	{
		if (!started) {
			Start ();
			started = true;
		}

		ID = id;

		gameObject.SetActive (true);

		CrewMember = crewMember;

		fightSprites.UpdateOrder (Crews.getCrew(crewMember.Side).CrewMembers.Count-id);
		fightSprites.UpdateSprites (CrewMember.MemberID);

		infoButton.SetActive (true);

		Reset ();
	}

	public void Hide ()
	{
		ChangeState (states.none);

		gameObject.SetActive (false);

		infoButton.SetActive (false);

		chooseButton.SetActive (false);

	}

	public void Reset () {
		ChangeState (states.none);
		animator.SetBool("dead",false);
	}

	public virtual void Die () {

			// self
		ChangeState (states.dead);
		Animator.SetBool ("dead", true);
		infoButton.SetActive (false);

		CombatManager.Instance.currentMember.AddXP (CrewMember.Level * 25);

			// combat flow
		CombatManager.Instance.DeleteFighter (this);

		CrewMember.Kill ();

		CombatManager.Instance.NextTurn ();

	}
	#endregion

	#region move to target
	public Fighter TargetFighter {
		get {
			return targetFighter;
		}
		set {
			targetFighter = value;
		}
	}

	public virtual void MoveToTarget_Start () {
		ChooseButton.SetActive (false);
		reachedTarget = false;
	}
	public virtual void MoveToTarget_Update () {
		
		Vector2 direction = ((Vector2)targetFighter.BodyTransform.position - (Vector2)transform.position).normalized;
		if (!ReachedTarget) {
			transform.Translate (direction * Speed * Time.deltaTime);
		}

		Animator.SetFloat ("move", ReachedTarget ? 0 : 1);

	}

	public virtual void MoveToTarget_Exit () {
		animator.SetFloat ("move", 0);
	}
	public void ClampPos () {
		Vector3 pos = transform.localPosition;
		pos.x = Mathf.Clamp (pos.x, leftAnchor.localPosition.x , rightAnchor.localPosition.x);

		transform.localPosition = pos;
	}
	public bool ReachedTarget {
		get {
			return Vector3.Distance (transform.position, targetFighter.BodyTransform.position) < stopDistance;
		}
	}
	#endregion

	#region move back
	public virtual void MoveBack_Start () {
	}

	public virtual void MoveBack_Update () {
		if (!BackToInitPos) {
			Vector2 direction = (initPos - (Vector2)transform.position).normalized;
			transform.Translate (direction * Speed * Time.deltaTime);
		}

		Animator.SetFloat ("move", 1);

		if (BackToInitPos) {
			ChangeState (states.none);
		}
	}

	public virtual void MoveBack_Exit () {
		Animator.SetFloat ("move", 0);
	}

	public bool BackToInitPos {
		get {
			return Vector2.Distance (transform.position, initPos) < stopBuffer;
		}
	}

	#endregion

	#region hit
	public virtual void hit_Start () {

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
			transform.Translate (Vector2.right * dir * hitSpeed * Time.deltaTime);
		}

		if (timeInState > hit_Duration) {
			ChangeState (states.moveBack);
		}

	}
	public virtual void hit_Exit () {

		weaponCollider.enabled = false;

		animator.speed = 1f;

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

		if (timeInState > blocked_Duration) {
			ChangeState (states.none);
		}

	}
	public void blocked_Exit () {
		animator.SetBool ( "blocked" , false);
	}
	#endregion

	#region dead
	void dead_Start ()
	{
		fightSprites.FadeSprites (1);
	}
	void dead_Update ()
	{
		
	}
	void dead_Exit ()
	{
		
	}
	#endregion

	public int TurnsToSkip {
		get {
			return turnsToSkip;
		}
		set {
			
			turnsToSkip = value;

			animator.SetBool ("uncounscious", turnsToSkip > 0);
		}
	}

	public void ShowInfo (Transform t) {
		CardManager.Instance.ShowFightingCard (CrewMember);
	}

	public virtual void OnTriggerEnter2D (Collider2D other) {
		
		if ( other.tag == hitTag) {

			if ( currentState == states.getHit ) {
				Debug.Log ("didn't touch parce que déjà en get hit");
				return;
			}

//			GetHit (other.GetComponentInParent<Fighter>());
		}
	}

	public void Speak (string txt)
	{
		DialogueManager.Instance.SetDialogueTimed (txt, dialogueAnchor);
	}

	#region get hit
	public virtual void getHit_Start () {

		weaponCollider.enabled = false;

		animator.SetTrigger ("getHit");
	}
	public virtual void getHit_Update () {

		if (timeInState > getHit_Duration)
			ChangeState (states.none);
	}
	public virtual void getHit_Exit () {
		//
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
			dam = Mathf.Round(dam/5f);
			ChangeState (states.blocked);
			impactEffect.GetComponent<SpriteRenderer> ().color = Color.black;
		} else {
			ChangeState (states.getHit);
			impactEffect.GetComponent<SpriteRenderer> ().color = Color.red;
		}

		BodyCollider.enabled = false;

		CombatFeedback.Display (dam.ToString (),impactEffect.GetComponent<SpriteRenderer> ().color);

		// collision effect
		impactEffect.SetActive (false);
		impactEffect.SetActive (true);

		impactEffect.transform.position = otherFighter.WeaponCollider.transform.position;


		crewMember.GetHit (dam);

		CardManager.Instance.ShowFightingCard (CrewMember);

		if (crewMember.Health <= 0) {
			Die ();
		}
	}
	#endregion

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

		case states.moveToTarget:
			updateState = MoveToTarget_Update;
			MoveToTarget_Start();
			break;
		case states.moveBack:
			updateState = MoveBack_Update;
			MoveBack_Start();
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
		case states.dead:
			updateState = dead_Update;
			dead_Start ();
			break;
		}
	}



	private void ExitState () {
		switch (previousState) {

		case states.none:
			//
			break;

		case states.moveToTarget:
			MoveToTarget_Exit();
			break;
		case states.moveBack:
			MoveBack_Exit();
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
		case states.dead:
			dead_Exit ();
			break;
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

	public CombatFeedback CombatFeedback {
		get {
			return combatFeedback;
		}
	}

	public BoxCollider2D BodyCollider {
		get {
			return bodyCollider;
		}
	}
}
