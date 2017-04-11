using UnityEngine;
using System.Collections;

public class Island : MonoBehaviour {

	private Transform island;

	[SerializeField]
	private Transform boat;

	[SerializeField]
	private float decal = 0f;

	Canvas canvas;

	// Use this for initialization
	void Start () {
		island = transform;

		canvas = GetComponentInParent<Canvas> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		canvas.planeDistance = (boat.localPosition.y + decal< island.localPosition.y ? 12 : 8);
	}
}
