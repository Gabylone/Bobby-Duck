using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	enum Directions {
		Up,
		Down,
		Left,
		Right,
		Front,
		Back
	}
	Vector3[] directions = new Vector3[6] { 
		-Vector3.forward,
		Vector3.down,
		Vector3.left,
		Vector3.right,
		Vector3.forward,
		Vector3.back,

	};

	[SerializeField] private float minForce = 500f;
	[SerializeField] private float minTorque = 100f;
	[SerializeField] private float maxForce = 700f;
	[SerializeField] private float maxTorque = 200f;

	private float timer = 0f;

	private int throwDirection = 1;

	private bool settling = false;
	private Quaternion initRot = Quaternion.identity;
	private Quaternion targetRot = Quaternion.identity;
	private float settleDuration = 0.5f;

	private Vector3 initPos;
	private bool thrown = false;

	// Use this for initialization
	public void Init () {

		initPos = transform.position;

		settleDuration = DiceManager.Instance.settlingDuration;
	}

	// Update is called once per frame
	void Update () {
		if (settling) {
			Settling ();
			timer += Time.deltaTime;
		}
	}

	public void Reset () {

		transform.up = directions [Random.Range (0,directions.Length)];

		throwDirection = DiceManager.Instance.ThrowDirection;

		Vector3 pos = initPos;
		pos.x *= throwDirection;

		transform.position = pos;

		transform.localScale = Vector3.one;

		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	public void Throw () {

		thrown = true;

		GetComponent<BoxCollider> ().enabled = true;
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<Rigidbody> ().AddForce ( Vector3.right * throwDirection *  Random.Range (minForce , maxForce) );
		GetComponent<Rigidbody> ().AddTorque ( Vector3.right * throwDirection * Random.Range (minTorque,maxTorque) );

	}

	#region settle
	public void Settle () {
		settling = true;
		timer = 0f;
	}

	private void Settling () {
		float l = timer / settleDuration;

		transform.localScale = Vector3.Lerp (Vector3.one, Vector3.one * 1.4f, l);

		if (l >= 1) 
			settling = false;
	}
	#endregion

	#region properties
	public int result {
		get {
			Vector3[] dirs = new Vector3[6] {
				transform.up,
				transform.right,
				transform.forward,
				-transform.forward,
				-transform.right,
				-transform.up,
			};

			int i = 1;

			foreach ( Vector3 d in dirs ) {
				if (Vector3.Dot (d, -Vector3.forward) > 0.5f) {
					return i;
				}

				++i;
			}

			Debug.LogError ("Coudn't find value of die\nreturning 0");
			return 0;
		}
	}

	public int ThrowDirection {
		get {
			return throwDirection;
		}
		set {
			throwDirection = value;
		}
	}
	#endregion

	#region dice color
	public void Paint ( DiceTypes type ) {
		foreach ( SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>() ) {
			rend.color = DiceManager.Instance.DiceColors (type);
		}
	}
	public void Fade () {
		
	}
	#endregion
}
