using UnityEngine;
using Holoville.HOTween;
using System.Collections;

public class FloatComponent : MonoBehaviour {

	private float duration = 2f;

	private float timer = 0f;

	private bool hadRigibody = true;
	private Vector2 minimumVelocity = Vector2.one;

	private Rigidbody2D rigidbody2D;

	bool floating = false;

	// Use this for initialization
	public virtual void Start () {
		Float_Start ();
	}

	// Update is called once per frame
	public virtual void Update () {
		Float_Update ();
	}


	bool isIdle ()
	{
		return rigidbody2D.velocity.x <= minimumVelocity.x && rigidbody2D.velocity.x >= -minimumVelocity.x && rigidbody2D.velocity.y <= minimumVelocity.y && rigidbody2D.velocity.y >= -minimumVelocity.y;
	}

	public virtual void Float_Start () {

		floating = true;

		if (GetComponent<Rigidbody2D> () == null) { 
			gameObject.AddComponent<Rigidbody2D> ();
			hadRigibody = false;
		}

		rigidbody2D = GetComponent<Rigidbody2D> ();

		foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>()) {
			rend.color = Color.cyan;
		}

		rigidbody2D.gravityScale = -0.05f;

	}

	public void Float_Update () {

		if ( floating ) {

			if ( timer >= duration ) {
				Float_Exit ();
				timer = 0f;
			}

		} else {

			if (isIdle ()) {
				if ( timer >= duration )
					Kill ();
			} else {
				timer = 0f;
			}

		}

		timer += Time.deltaTime;
	}

	public virtual void Float_Exit () {
		
		floating = false;

		foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>()) {
			rend.color = Color.grey;
		}

		rigidbody2D.gravityScale = 1;
	}

	public virtual void Kill (){

		if (hadRigibody == false) {
			Destroy (rigidbody2D);
		}

		foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>()) {
			rend.color = Color.white;
		}


		Destroy (this);

	}
}
