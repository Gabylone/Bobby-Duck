using UnityEngine;
using System.Collections;

public class Ingredient : Interactable {

	[SerializeField]
	private float timeToEnablePickUp = 1f;
	private float timer = 0f;

	[SerializeField]
	private float startForce = 100f;

	private Rigidbody2D rigidody;
	private Collider2D trigger;
	private Collider2D collider;

	// Use this for initialization
	public virtual void Start () {
		rigidody = GetComponent<Rigidbody2D> ();
		trigger = GetComponent<Collider2D> ();
		collider = GetComponentsInChildren<Collider2D> () [1];
	}
	
	// Update is called once per frame
	public virtual void Update () {
		CanInteract = timer >= timeToEnablePickUp;
		timer += Time.deltaTime;
	}

	public override void Interact ()
	{
		base.Interact ();

		Character.Instance.Animator.SetTrigger ("pickUp");
		IngredientsSpiral.Instance.AddItem (transform);

		Collider.enabled = false;
		Trigger.enabled = false;
		Rigidody.isKinematic = true;
		CanInteract = false;
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
		Rigidody.AddForce ( ((Vector2)Character.Instance.BodyTransform.right+Vector2.up) * startForce );

	}

	public Collider2D Collider {
		get {
			return collider;
		}
	}
}
