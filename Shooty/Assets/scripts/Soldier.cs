using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Holoville.HOTween;
public class Soldier : Soldier_StateMachine {

	public static List<Soldier> soldiers = new List<Soldier>();

	// HEALTH
	[Header("health")]
	public int health = 100;

	// MOVE
	[Header("move")]
	public float disToStop = 0.2f;
	public float moveSpeed = 0f;
	public Vector3 targetPoint;

	[Header("components")]
	public Transform headAnchor;
	[SerializeField]
	public SkinnedMeshRenderer renderer;
	public Gun gun;
	Rigidbody rigidbody;
	NavMeshAgent agent;
	Animator animator;
	ChestRotation chestRotation;

	// AIM & SHOOT
	public float timeToAim = 1f;
	public float timeToShoot = 1f;
	public float hitForce = 200f;
	public float hitTorque = 10f;

	// Use this for initialization
	public override void Start () {

		soldiers.Add (this);

		agent = GetComponent<NavMeshAgent> ();
		agent.stoppingDistance = disToStop - 0.05f;
		animator = GetComponentInChildren<Animator> ();
		gun = GetComponentInChildren<Gun> ();
		rigidbody = GetComponent<Rigidbody> ();
		chestRotation = GetComponentInChildren<ChestRotation> ();

		// init targetpoint
		targetPoint = transform.position;

		base.Start ();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if ( Input.GetKeyDown(KeyCode.L)) {
			ChangeState (State.hit);
		}
	}

	#region idle
	public override void Idle_Start ()
	{
		animator.SetFloat ("move", 0f);
	}
	public override void Idle_Update ()
	{
		
	}
	public override void Idle_Exit ()
	{

	}
	#endregion

	#region move
	public virtual void Move (Vector3 v) {
		targetPoint = v;
		ChangeState (State.move);

	}
	public override void Moving_Start() {

		agent.isStopped = false;

		agent.speed = moveSpeed;

		agent.SetDestination (targetPoint);
		//
	}
	public override void Moving_Update() {
		if (Vector3.Distance (transform.position, targetPoint) < disToStop) {
			
			ChangeState (State.idle);
		} else {
			animator.SetFloat ("move", 1f);
		}

	}
	public override void Moving_Exit() {
		agent.isStopped = true;
	}
	#endregion

	#region cover
	public override void InCover_Start() {
		animator.SetBool ("cover", true);
	}
	public override void InCover_Update() {
		
	}
	public override void InCover_Exit() {
		animator.SetBool ("cover", false);
	}

	void OnTriggerStay( Collider coll ) {
		switch (currentState) {
		case State.idle:
			if ( coll.tag == "cover" ) {
				ChangeState (State.inCover);
			}
			return;
		default:
			break;
		}
	}

	void OnTriggerExit ( Collider coll ) {
		if (coll.tag == "cover") {
//			ChangeState(State.
		}
	}
	#endregion

	#region aiming
	public void AimAtTarget ( Transform t ) {
		gun.target = t;

		ChangeState (State.aiming);
	}
	public override void Aiming_Start ()
	{
		Aiming_TurnToTarget ();

		animator.SetBool ("aiming", true);
	}

	void Aiming_TurnToTarget ()
	{
		Vector3 dir = gun.target.position - transform.position;
		dir = dir.normalized;
		dir.y = 0f;

		transform.forward = dir;

		chestRotation.TurnToTarget (gun.target);
	}

	public override void Aiming_Update ()
	{
		if  ( timeInState > timeToAim ) {
			ChangeState (State.shoot);
		}
	}
	public override void Aiming_Exit ()
	{
		animator.SetBool ("aiming", false);
		chestRotation.Stop ();

	}
	#endregion

	#region shoot
	public override void Shoot_Start ()
	{
		animator.SetTrigger ("shoot");

		gun.Shoot ();

		Tween.Bounce (transform);
	}
	public override void Shoot_Update ()
	{
		base.Shoot_Update ();

		if (timeInState > timeToShoot) {
			ChangeState (State.idle);
		}
	}
	public override void Shoot_Exit ()
	{
		base.Shoot_Exit ();
	}
	#endregion

	#region hit
	bool gettingUp = false;
	public delegate void OnHit (Gun gun);
	public OnHit onHit;
	public void Hit (Gun gun)
	{
		ChangeState (State.hit);

		transform.forward = (gun.transform.position - transform.position);

		Tween.Bounce (transform);

		health -= gun.info.damage;

		if (onHit!=null)
			onHit (gun);

		if (health <= 0)
			Die ();
	}
	public override void Hit_Start ()
	{
	}
	public override void Hit_Update ()
	{
		if (gettingUp) {
			if (timeInState > 1f) {
				ChangeState (State.move);
			}
		} else {
			if ( timeInState > 1 ) {
				gettingUp = true;
			}
		}
	}
	public override void Hit_Exit ()
	{
		rigidbody.isKinematic = true;
		agent.enabled = true;
		gettingUp = false;
	}
	#endregion

	#region select
	public override void Selected ()
	{
		base.Selected ();

		if (selected) {

			Deselect ();

		} else {

			Select ();

		}
	}

	public bool selected = false;

	public delegate void OnSelect ();
	public OnSelect onSelect;
	public virtual void Select ()
	{
		if (selected)
			return;

		selected = true;

	}

	public delegate void OnDeselect();
	public OnDeselect onDeselect;
	public virtual void Deselect ()
	{
		if (!selected)
			return;

		selected = false;

	}
	#endregion

	#region death
	void Die () {
		rigidbody.isKinematic = false;

		agent.enabled = false;

		GetComponent<BoxCollider> ().enabled = false;

		rigidbody.AddForce(Vector3.up* hitForce);
		rigidbody.AddForce(-transform.forward* hitForce);
		rigidbody.AddTorque (-transform.forward* hitTorque);

		animator.SetTrigger ("death");
//		animator.enabled = false;

		Destroy (this);
	}
	#endregion

	Vector3 dirToPoint {
		get {
			Vector3 dir = (targetPoint - transform.position).normalized;
			dir.y = 0f;

			return dir;
		}
	}

	void OnDestroy () {
		soldiers.Remove (this);
	}
}
