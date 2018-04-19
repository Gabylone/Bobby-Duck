using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldTouchFeedback : MonoBehaviour {

	NavMeshAgent agent;

	float maxSampleDistance = 1.0f;

	[SerializeField]
	private GameObject group;

	float timer = 0f;


	public float showDuration = 0.5f;

	bool visible = false;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();

		Soldier_Player.onMove += HandleOnMove;

		Hide ();

	}

	void Update () {
		if ( visible) {
			timer += Time.deltaTime;

			if (timer > showDuration) {
				Hide ();
			}
		}
	}

	void HandleOnMove (Vector3 p)
	{
		NavMeshHit hit;

		if (NavMesh.SamplePosition (p, out hit, maxSampleDistance, NavMesh.AllAreas)) {

			transform.position = hit.position;

			Show ();

		} else {
			print ("lost in world not in nav mesh");
			transform.position = Vector3.zero;
		}


	}

	void Show () {
		group.SetActive (true);

		visible = true;

		timer = 0f;

		Tween.Bounce (transform, 0.3f, 1.4f);
	}

	void Hide () {
		group.SetActive (false);

		visible = false;
	}
}
