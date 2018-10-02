using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {

	public delegate void OnTouchGround ( Vector3 pos );
	public static OnTouchGround onTouchGround;

	public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		
	}
    private void OnDestroy()
    {
        onTouchGround = null;
    }

    // Update is called once per frame
    void Update () {

		if ( Input.GetMouseButtonDown(0) ) {
			CheckRay ();
		}

	}

	void CheckRay () {
		RaycastHit hit;

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if ( Physics.Raycast ( ray , out hit , 999f ) ) {
//		if ( Physics.Raycast ( ray , out hit , 999f , layerMask ) ) {

			if ( onTouchGround != null && hit.collider.name == "Floor") {
				onTouchGround (hit.point);
			}

		}
	}
}
