using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wheel : MonoBehaviour {

	bool opened = false;

	[Header("Wheel")]
	[SerializeField]
	private Transform wheelTransform;
	[SerializeField]
	private float wheelRotSpeed = 10f;

	[SerializeField]
	private float boatSpeed = 0.3f;
	[SerializeField]
	private GameObject arrowObj;

	Vector3 initPos = Vector3.zero;

	[SerializeField]
	Vector3 targetPos = Vector3.zero;

	Vector3 currentDirection = Vector3.zero;

	bool turned = false;

	[Header("Boat Mesh")]
	[SerializeField]
	private Transform boatMesh;
	private float boatCurrentAngle = 0f;
	[SerializeField]
	private float boatRotationSpeed = 50f;

	void Start () {
		initPos = wheelTransform.localPosition;	

		Opened = false;
	}

	Vector3 GetDir() {

		Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint (wheelTransform.position)).normalized;
		dir.z = 0f;

		return dir;
	}

	void Update () {

		if (opened) {

			Vector3 dir = GetDir ();

			if (!turned) {
				wheelTransform.up = dir;

				turned = true;
			} else {
				wheelTransform.up = Vector3.MoveTowards (wheelTransform.up, dir, wheelRotSpeed * Time.deltaTime);
			}

			currentDirection = wheelTransform.up;

			UpdateBoatRotation ();

			if ( Input.GetMouseButtonUp (0) ) {
				
				Opened = false;

				turned = false;

			}

			BoatManager.Instance.BoatTransform.Translate (currentDirection * boatSpeed * 2 * Time.deltaTime, Space.World);

		} else {
			wheelTransform.up = Vector3.MoveTowards (wheelTransform.up, Vector3.up, boatRotationSpeed * Time.deltaTime);

			if (IslandManager.Instance.OnIsland == false) {
				BoatManager.Instance.BoatTransform.Translate (currentDirection * boatSpeed * Time.deltaTime, Space.World);
			}

		}
//
	}

	private void UpdateBoatRotation () {
		// BOAT ROTATION
		float targetAngle = Vector3.Angle (currentDirection, Vector3.up);

		if (Vector3.Dot (Vector3.right, currentDirection) < 0)
			targetAngle = -targetAngle;
		
		boatCurrentAngle = Mathf.MoveTowards (boatCurrentAngle, targetAngle, boatRotationSpeed * Time.deltaTime);

		boatMesh.localRotation = Quaternion.Euler (0, targetAngle, 0);
	}

	float previousAngle;

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

			wheelTransform.localScale = opened ? Vector3.one * 1.3f : Vector3.one;

//			wheelTransform.localPosition = opened ? targetPos : initPos;
		}
	}
}
