using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour {

	private Rigidbody2D rigidbody;

	[SerializeField]
	private float torque = -100f;

	[SerializeField]
	private float force = 100f;

	[SerializeField]
	private GameObject effectPrefab;

	public virtual void Start () {

		rigidbody = GetComponent<Rigidbody2D> ();

		Throw ();
	}

	public virtual void Throw () {
		rigidbody.AddForce ( transform.right * force );
		rigidbody.AddTorque ( torque );
	}

	void OnCollisionEnter2D ( Collision2D col ) {
		Collide (col.gameObject);
	}

	public virtual void Collide (GameObject obj){

		GameObject g = (GameObject)Instantiate (effectPrefab, transform.position, Quaternion.identity);
		g.transform.up = transform.position -  obj.transform.position;
		Destroy (g,0.5f);
		Destroy (gameObject);

	}
}
