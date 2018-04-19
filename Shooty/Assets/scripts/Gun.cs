using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GunInfo info;

	public Transform target = null;

	public GameObject effectPrefab;

	public Transform effectAnchor;

	public Transform testRay;

	public LineRenderer lineRenderer;

	float timer = 0f;

	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Update () {
		DrawLine ();
	}

	public void Shoot ()
	{
		float maxCone = 1f;

		float precisionFactor = 1 - (info.precision / 100f);

		float x = Random.Range ( -maxCone , maxCone ) * precisionFactor;
		float y = Random.Range ( -maxCone , maxCone ) * precisionFactor;
		Vector3 decal = new Vector3 (x,y,0f);

//		Vector3 point = target.position + target.TransformDirection (decal);
		Vector3 point = target.position + target.TransformDirection (decal);
		Vector3 dirToPoint = point - transform.position;
		RaycastHit hit;

		if ( Physics.Raycast (effectAnchor.position , dirToPoint , out hit, 10f ) )  {

			Effect.Instance.Shoot (hit.point, hit.normal);
			Effect.Instance.Shoot (effectAnchor.position, effectAnchor.forward);

			Soldier sol = hit.collider.GetComponent<Soldier> ();

			if (testRay != null) {
				testRay.position = effectAnchor.position;
				testRay.forward = dirToPoint;
				testRay.localScale = new Vector3 ( 0.1f , 0.1f , Vector3.Distance (hit.point,effectAnchor.position) );
			}
			if (sol != null ) {
				sol.Hit (this);
			}

		}

		timer = 0f;

		target = null;
	}

	public void DrawLine () {
		if (target == null) {
			lineRenderer.enabled = false;
			return;
		}

		lineRenderer.enabled = true;

		lineRenderer.SetPosition (0,effectAnchor.position);

		Vector3 dirToPoint = target.position - transform.position;

		RaycastHit hit;

		if ( Physics.Raycast (effectAnchor.position , dirToPoint , out hit, 10f ) )  {
			lineRenderer.SetPosition (1,hit.point);
		}
	}

}

[System.Serializable]
public class GunInfo {

	public int damage = 1;

	public float range = 10f;

	public float shootRate = 1f;

	[Range(0,100)]
	public float precision = 50f;

}