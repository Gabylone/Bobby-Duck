using UnityEngine;
using System.Collections;

public class Island : MonoBehaviour {

	private Transform island;

	[SerializeField]
	private Transform boat;

	[SerializeField]
	private float decal = 0f;

	private Canvas canvas;

	[SerializeField]
	private float distanceToTrigger = 1f;

	[SerializeField]
	private Vector3 flagDecal;

	// Use this for initialization
	void Start () {
		island = transform;
		canvas = GetComponentInParent<Canvas> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		canvas.planeDistance = (boat.localPosition.y + decal< island.localPosition.y ? 12 : 8);
	}

	public void OnMouseEnter () {

		if ( Vector3.Distance ( boat.position, transform.position ) < distanceToTrigger && IslandManager.Instance.OnIsland == false){
			transform.localScale = Vector3.one * 1.2f;
		}
	}

	public void OnMouseExit() {
		transform.localScale = Vector3.one;
	}

	public void OnMouseDown () {
		
	
		if (NavigationManager.Instance.CurrentNavigationSystem == NavigationManager.NavigationSystem.Flag) {

			Vector3 pos = Camera.main.WorldToViewportPoint (transform.position + flagDecal);

			NavigationManager.Instance.FlagControl.FlagImage.rectTransform.anchorMin = pos;
			NavigationManager.Instance.FlagControl.FlagImage.rectTransform.anchorMax = pos;
		} else {
			if (Vector3.Distance (boat.position, transform.position) < distanceToTrigger) {
				transform.localScale = Vector3.one;

				IslandManager.Instance.Enter ();

			}

		}
	}


}
