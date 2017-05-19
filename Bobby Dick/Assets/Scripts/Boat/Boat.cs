using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boat : MonoBehaviour {

	[SerializeField]
	private BoatInfo boatInfo;

	[SerializeField]
	private Vector2 boatBounds = new Vector2(290f , 125f);

	[Space]
	[Header ("Boat Elements")]
	[SerializeField]
	private Transform boatMesh;
	private Transform getTransform;

	[Space]
	[Header ("Boat Position Parameters")]

	[SerializeField]
	private float acceleration = 1f;
	[SerializeField]
	private float decceleration = 1f;
	[SerializeField]
	private float maxSpeed = 5f;
	private float currentSpeed = 0f;
	private float targetSpeed = 0f;

	private bool boat_IsMoving = false;

	[Space]
	[Header ("Boat Rotation Parameters")]

	[SerializeField]
	private float boatRotationSpeed = 0.7f;
	private Vector3 targetDirection = Vector3.up;
	private Vector3 currentDirection = Vector3.up;

	[Space]
	[Header ("Mesh Rotation Parameters")]
	[SerializeField]
	private float boat_MeshRotationSpeed = 50f;

	public virtual void Start () {
		getTransform = GetComponent<Transform> ();


	}

	public virtual void Update () {
		UpdateBoatRotation ();
		UpdateBoatPosition ();
	}

	#region boat transform

	private void UpdateBoatRotation () {

		float targetAngle = Vector3.Angle (currentDirection, Vector3.up);
//
		if (Vector3.Dot (Vector3.right, currentDirection) < 0)
			targetAngle = -targetAngle;
		
		Quaternion targetRot = Quaternion.Euler (0, targetAngle, 0);
		boatMesh.localRotation = targetRot;

	}
	private void UpdateBoatPosition () {

			// smooth speed
		currentSpeed = Mathf.MoveTowards ( currentSpeed, targetSpeed , ( (TargetSpeed<0.1f) ? decceleration  : acceleration ) * Time.deltaTime );

		// clamp speed
		currentSpeed =  Mathf.Clamp (currentSpeed, 0f, maxSpeed);

			// set boat direction
		currentDirection = Vector3.MoveTowards (currentDirection, targetDirection, boatRotationSpeed * Time.deltaTime);

			// translate boat
		getTransform.Translate (currentDirection * currentSpeed * Time.deltaTime, Space.World);

	}
	#endregion

	#region map position 
	public void UpdatePositionOnScreen () {
		Vector2 getDir = NavigationManager.Instance.getDir(boatInfo.currentDirection);
		transform.localPosition = new Vector2(-getDir.x * boatBounds.x, -getDir.y * boatBounds.y);
	}

	#endregion

	#region properties
	public Transform GetTransform {
		get {
			return getTransform;
		}
	}

	public Vector3 TargetDirection {
		get {
			return targetDirection;
		}
		set {
			targetDirection = value;
		}
	}

	public float TargetSpeed {
		get {
			return targetSpeed;
		}
		set {
			targetSpeed = value;
		}
	}

	public float MaxSpeed {
		get {
			return maxSpeed;
		}
	}

	public BoatInfo BoatInfo {
		get {
			return boatInfo;
		}
		set {
			boatInfo = value;
		}
	}
	#endregion
}
