using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boat : MonoBehaviour {

	[SerializeField]
	private BoatInfo boatInfo;

	[SerializeField]
	private Vector2 boatBounds = new Vector2(290f , 125f);
	[SerializeField]
	private Vector2 boundsBuffer = new Vector2(30f, 30f);

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

	public virtual void Init () {
		getTransform = GetComponent<Transform> ();

		CombatManager.Instance.fightStarting += DeactivateCollider;
		CombatManager.Instance.fightEnding += ActivateCollider;

		StoryLauncher.Instance.playStoryEvent += HandlePlayStoryEvent;
	}

	void HandlePlayStoryEvent ()
	{
		TargetSpeed = 0f;
		currentSpeed = 0f;
	}

	public virtual void Update () {
		UpdateBoatRotation ();
		UpdateBoatPosition ();
	}

	void DeactivateCollider ()
	{
		GetComponentInChildren<BoxCollider2D> ().enabled = false;
	}
	void ActivateCollider () {
		GetComponentInChildren<BoxCollider2D> ().enabled = true;
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
	public virtual void UpdatePositionOnScreen () {

		foreach (TrailRenderer renderer in GetComponentsInChildren<TrailRenderer>())
			renderer.Clear ();

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
