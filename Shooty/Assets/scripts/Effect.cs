using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	public static Effect Instance;

	[SerializeField]
	private GameObject shootEffect;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}

	public void Shoot (Vector3 p , Vector3 d) {
		GameObject go = Instantiate (shootEffect) as GameObject;
		go.transform.position = p;
		go.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward , d);
		Destroy (go, 0.1f);
	}
}
