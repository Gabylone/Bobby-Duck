using UnityEngine;
using System.Collections;

public class Thrower : MonoBehaviour {

	private Character character;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private Transform anchor;

	[SerializeField]
	private float delay = 0.2f;

	float timer = 0f;

	void Start () {
		character = GetComponent<Character> ();
	}

	void Update () {
	}

	private bool pressingInput () {
		return Input.GetAxis ("Horizontal") >= 0.3f || Input.GetAxis ("Horizontal") <= -0.3f
			|| Input.GetAxis ("Vertical") >= 0.3f || Input.GetAxis ("Vertical") <= -0.3f;
	}

	public void Throw () {
		Invoke ("InstantiateObject", delay);
	}

	private void InstantiateObject () {

		GameObject rockObj = Instantiate (prefab) as GameObject;
//		Vector3 inputDir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		rockObj.transform.position = anchor.position;

		Vector3 inputDir = new Vector3(Input.GetAxis ("Horizontal") ,Input.GetAxis ("Vertical") ,0f);
		if ( pressingInput () == false )
			inputDir = character.bodyTransform.right;

		rockObj.transform.right = inputDir;
	}
}
