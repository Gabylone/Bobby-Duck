using UnityEngine;
using System.Collections;

public class Humanoid : MonoBehaviour {

	public enum states {
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
	private float limitX = 4f;

	[Header("Sounds")]
	[SerializeField] private AudioClip hitSound;
	[SerializeField] private AudioClip hurtSound;

	// Use this for initialization
	public void Init () {
		
		animator = GetComponentInChildren<Animator> ();

		ChangeState (states.move);

		weaponCollider.enabled = false;
	}

	// Update is called once per frame
	public void UpdateStateMachine () {
		updateState ();

		timeInState += Time.deltaTime;

		ClampPos ();
	}

	#region move
	public virtual void move_Start () {
		
	}
	public virtual void move_Update () {
		

	}
	public virtual void move_Exit () {
		animator.SetFloat ("move", 0);
	}
	public void ClampPos () {
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, -limitX , limitX);

		transform.position = pos;
	}
	#endregion

	#region hit
	public virtual void hit_Start () {

		animator.speed = 1 + crewMember.Dexterity / 10;

		animator.SetTrigger ( "hit" );
//		animator.SetInteger ("hitType", 1);
		animator.SetInteger ("hitType", Random.Range (0,2));
	}

	public virtual void hit_Update () {

		if (timeInState > hit_TimeToEnableCollider / 1 + crewMember.Dexterity / 10) {

			if (timeInState > (hit_TimeToDisableCollider / 1 + crewMember.Dexterity / 10)) {
				weaponCollider.enabled = false;
			} else {
				transform.Translate (Direction * hitSpeed * Time.deltaTime);
				weaponCollider.enabled = true;
			}
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
	}

	public virtual void guard_Update () {
		if (TimeInState > guard_TimeToEffective) {
			Guard_Active = true;
			guard_Feedback.SetActive (true);
		}
	}
	public virtual void guard_Exit () {
		animator.SetBool ( "guard" , false);
		Guard_Active = false;
		guard_Feedback.SetActive (false);

	}
	#endregion

	#region blocked
	public virtual void blocked_Start () {
		
		animator.SetBool ( "blocked" , true);

		weaponCollider.enabled = false;
	}

	public virtual void blocked_Update () {
		
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

		switch (currentState) {
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

		switch (previousState) {
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

			if ( currentState != states.getHit ) {

				float dam = other.GetComponentInParent<Humanoid> ().CrewMember.Attack;

				float critChance = (35f * CrewMember.Dexterity) / 10f;
				if ( Random.value * 100f < critChance ) {

					dam *= 1.6f;

					SoundManager.Instance.PlaySound (hurtSound);
					DialogueManager.Instance.SetDialogue ("Aie PUTAIN !", dialogueAnchor);

				} else {

					SoundManager.Instance.PlaySound (hitSound);
					DialogueManager.Instance.SetDialogue ("Aïe !", dialogueAnchor);

				}

				if (Guard_Active) {
					dam /= 4;
					ChangeState (states.blocked);
				} else {
					ChangeState (states.getHit);
				}

				other.GetComponentInParent<Humanoid> ().ChangeState (states.blocked);
				crewMember.GetHit (dam);

				if (crewMember.Health == 0) {
					
					CombatManager.Instance.SetTargetCrew(crewMember.Side);
					CombatManager.Instance.DefendingCrew = crewMember.Side;
					CombatManager.Instance.AttackingCrew = crewMember.Side == Crews.Side.Player ? Crews.Side.Enemy : Crews.Side.Player;
					CombatManager.Instance.ChangeState (CombatManager.States.MemberReturn);

				}

			}
		}
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

}
