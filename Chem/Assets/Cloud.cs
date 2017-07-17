using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	Transform transform;

	public float speed = 1f;

	public float max = 28f;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector2.right * speed * Time.deltaTime);

		if ( transform.localPosition.x >= max ) {
			transform.localPosition = new Vector3 (-max, transform.localPosition.y, 0f);
		}
	}
}
