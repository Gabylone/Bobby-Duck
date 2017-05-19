using UnityEngine;
using System.Collections;

public class BoatLight : MonoBehaviour {

	[SerializeField]
	private Transform boatTransform;
	
	// Update is called once per frame
	void Update () {
		transform.position = boatTransform.position;
	}
}
