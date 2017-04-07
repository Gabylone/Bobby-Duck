using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wheel : MonoBehaviour {

	[SerializeField]
	private float rotSpeed = 10f;

	bool opened = false;

	[SerializeField]
	private Transform boatTransform;

	[SerializeField]
	private float boatSpeed = 0.3f;
	[SerializeField]
	private GameObject arrowObj;

	Vector3 initPos = Vector3.zero;

	[SerializeField]
	Vector3 targetPos = Vector3.zero;

	Vector3 currentDirection = Vector3.zero;

	bool turned = false;


	void Start () {
		initPos = transform.localPosition;	

		Opened = false;
	}

	void Update () {
		
		if (opened) {
			Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position));
			dir.z = 0f;

			if (!turned) {
				transform.up = dir;

				turned = true;
			} else {
				transform.up = Vector3.MoveTowards (transform.up, dir, rotSpeed * Time.deltaTime);
			}

			currentDirection = transform.up;

			if ( Input.GetMouseButtonUp (0) ) {
				
				Opened = false;

				Directions targetDirection = NavigationManager.Instance.getDirectionFromVector (dir);
				NavigationManager.Instance.Move (targetDirection);

				turned = false;

			}

			boatTransform.Translate (currentDirection * boatSpeed * 2 * Time.deltaTime, Space.World);

		} else {
			transform.up = Vector3.MoveTowards (transform.up, Vector3.up, rotSpeed * Time.deltaTime);

			if (IslandManager.Instance.OnIsland == false) {
				boatTransform.Translate (currentDirection * boatSpeed * Time.deltaTime, Space.World);
			}

		}
//
	}

	public void OnMouseEnter() {
	}

	public void OnMouseExit() {
	}

	public void OnMouseDown() {
		Opened = !Opened;
	}

	public bool Opened {
		get {
			return opened;
		}
		set {
			opened = value;

			arrowObj.SetActive (value);

			transform.localScale = opened ? Vector3.one * 1.3f : Vector3.one;

//			transform.localPosition = opened ? targetPos : initPos;
		}
	}
}
