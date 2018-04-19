using UnityEngine;
using System.Collections;

public class Ingredient : Interactable {

	public bool harvested = false;

	[SerializeField]
	private float timeToEnablePickUp = 1f;
	private float timer = 0f;

	[SerializeField]
	private float startForce = 100f;

	// COMPONENTS
	private Rigidbody2D rigidody;
	private Collider2D trigger;
	private Collider2D collider;

	// Use this for initialization
	public virtual void Start () {
		
		rigidody = GetComponent<Rigidbody2D> ();
		if ( rigidody == null ) {
			Debug.LogError ("pas de rigidbody sur ingredient : " + name);
		}

		trigger = GetComponent<Collider2D> ();
		if ( trigger == null ) {
			Debug.LogError ("pas de trigger sur ingredient : " + name);
		}

		collider = GetComponentsInChildren<Collider2D> () [1];
		if ( collider == null ) {
			Debug.LogError ("pas de collider sur ingredient : " + name);
		}

	}

	public override void Interact ()
	{
		base.Interact ();

		Character.Instance.animator.SetTrigger ("pickUp");

		IngredientsSpiral.Instance.AddItem (transform);

		EnterInventory ();
	}

	void EnterInventory() {

		DisablePhysics ();

		Trigger.enabled = false;
		canInteract = false;

	}

	public void ExitInventory( ){

		EnablePhysics ();

		Trigger.enabled = true;

		canInteract = true;

		transform.SetParent (null);
	}

	public void EnablePhysics () {
		Rigidody.isKinematic = false;
		Collider.enabled = true;
	}

	public void DisablePhysics () {
		Rigidody.isKinematic = true;
		Collider.enabled = false;
	}

	public Rigidbody2D Rigidody {
		get {
			return rigidody;
		}
	}

	public Collider2D Trigger {
		get {
			return trigger;
		}
	}
	public void Push () {
		rigidody.velocity = Vector2.zero;
		Rigidody.AddForce ( Vector2.one * startForce );

	}

	public Collider2D Collider {
		get {
			return collider;
		}
	}

	void OnCollisionEnter2D ( Collision2D col ) {
		if ( col.collider.GetComponent<Tile>() != null )
		GetComponent<AudioSource> ().Play ();
	}

}
