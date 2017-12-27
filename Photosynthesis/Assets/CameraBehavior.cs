using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	float delta = 0f;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.RightArrow) ) {
			transform.RotateAround (Vector3.zero , Vector3.up , speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftArrow) ) {
			transform.RotateAround (Vector3.zero , Vector3.up , -speed * Time.deltaTime);
		}

	}
}
