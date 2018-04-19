using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTouch : MonoBehaviour {

	public static bool somethingWasSelected = false;

	public enum ClickType {
		Left,
		Right,
	}

	public delegate void OnTouchWorld ( Vector3 p ,ClickType clickType );
	public static OnTouchWorld onTouchWorld;

	float timer = 0f;
	public float timeToTriggerTouch = 0.2f;

	public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < 2; i++) {
			if (Input.GetMouseButtonUp (i) ) {
				if (timer < timeToTriggerTouch) {
					TouchWorld ((ClickType)i);
				}
				timer = 0f;
			}
		}

		if (Input.GetMouseButton (0)) {
			timer += Time.deltaTime;
		}


	}

	void TouchWorld (ClickType clickType)
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;

		//				if (Physics.Raycast (ray, out hit, 100f, layerMask)) {
		if (Physics.Raycast (ray, out hit, 100f)) {

			print ("touched : " + hit.collider.name);

			Selectable selectable = hit.collider.GetComponent<Selectable> ();

			if (selectable != null ){
				selectable.Selected ();
				return;
			}

			if ( hit.collider.tag == "touchLayer" ) {

				if (onTouchWorld != null) {
					onTouchWorld (hit.point, clickType);
					return;
				}

			}


		}

		somethingWasSelected = false;
	}

}
