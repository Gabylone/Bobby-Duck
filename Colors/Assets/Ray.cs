using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour {

	LineRenderer lineRenderer;

	public Direction direction;
	Vector3 dir = Vector3.up;

	// Use this for initialization
	void Start () {

		rayPrefab = Resources.Load ("Prefabs/Ray") as GameObject;

		lineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		lineRenderer.SetPosition (0, transform.position);

		RaycastHit hit;

		if ( Physics.Raycast(transform.position , Grid.GetVectorDir (direction) , out hit) ) {
			

			CubeSide cubeSide = hit.collider.GetComponent<CubeSide> ();

			lineRenderer.SetPosition (1, hit.point );

			if ( cubeSide != null ) {
				switch (cubeSide.type) {
				case CubeSide.Type.Block:
					break;
				case CubeSide.Type.Deviate:
					cubeSide.DeviateRay ();
					break;
				default:
					break;
				}
			}

		} else {
			
			lineRenderer.SetPosition (1, Grid.GetLimit(direction,transform.position) );

		}



	}

	// ray management
	public static GameObject rayPrefab;

	public static void NewRay ( Vector3 pos , Direction direction ) {
		
		GameObject rayObj = Instantiate (rayPrefab, pos, Quaternion.identity);

		Ray ray = rayObj.GetComponent<Ray> ();

		ray.SetDir (direction);

	}

	public void SetDir (Direction direction)
	{
		this.direction = direction;
	}
}
