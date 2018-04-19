using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cover : MonoBehaviour {

	public static List<Cover> covers = new List<Cover>();

	float decal = 0.7f;

	Vector3 mamille = Vector3.zero;

	// Use this for initialization
	void Start () {
		covers.Add (this);
	}

	public static Cover getRandomCover {
		get {
			return covers [Random.Range (0, covers.Count)];
		}
	}

	public Vector3 GetClosestPosition (Transform otherTarget) {

		Vector3 dirToOther = otherTarget.position - transform.position;
		dirToOther = dirToOther.normalized;

		Vector3 desiredPoint = transform.position + (dirToOther * decal);

		mamille = desiredPoint;

		return desiredPoint;

	}

	void OnDrawGizmos () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (mamille,0.1f);
	}
}
