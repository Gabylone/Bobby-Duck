using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Target : MonoBehaviour {

	RectTransform rectTransform;

	[SerializeField]
	private Transform target;

	[SerializeField]
	private Camera cam;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos = cam.WorldToViewportPoint (target.position);

		rectTransform.anchorMin = pos;
		rectTransform.anchorMax = pos;
	}
}
