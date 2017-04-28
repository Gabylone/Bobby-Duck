using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boat : MonoBehaviour {

	public static Boat Instance;

	/// <summary>
	/// Navigation system.
	/// </summary>
	public enum NavigationSystem
	{
		Wheel,
		Flag
	}

	[SerializeField]
	private NavigationSystem navigationSystem;

	[Space]
	[Header ("BoatElements")]
	[SerializeField]
	private Transform boatTransform;
	[SerializeField]
	private float boatAcceleration = 1f;
	[SerializeField]
	private Transform boatMesh;
	[SerializeField]
	private float boatRotationSpeed = 50f;
	private float boatCurrentAngle = 0f;
	private Vector3 boat_TargetDirection = Vector3.zero;
	private Vector3 boat_CurrentDirection = Vector3.zero;

	/// <summary>
	/// The boat speed.
	/// </summary>
	[SerializeField]
	private float boat_MeshRotationSpeed = 50f;
	[SerializeField]
	private float boatMaxSpeed = 5f;
	private float boatCurrentSpeed = 0f;
	private bool boat_IsMoving = false;

	[Header("Wheel")]
	[SerializeField]
	private Transform wheelTransform;
	[SerializeField]
	private float wheelRotSpeed = 10f;
	[SerializeField]
	private GameObject wheel_Arrow;
	private bool wheel_Opened = false;
	private bool wheel_Turned = false;

	[Header ("Flag")]
	[SerializeField]
	private Image flagImage;
	[SerializeField]
	private float flag_DistanceToStop = 1f;


	public float distanceToTriggerIsland = 1f;

	void Awake () {
		Instance = this;
	}

	void Start () {
		Opened = false;
	}

	void Update () {
		if (!IslandManager.Instance.OnIsland)
			UpdateBoat ();
	}

	#region boat
	private void UpdateBoat () {

		if (navigationSystem == NavigationSystem.Flag) {
			UpdateFlagPosition ();
		} else {
			UpdateWheel ();
		}

		if (boat_IsMoving) {
			UpdateBoatRotation ();

			UpdateBoatPosition ();
		}

	}
	private void UpdateBoatRotation () {

		float distanceToFlag = Vector3.Distance (flagImage.transform.position, boatTransform.position);

		float targetAngle = Vector3.Angle (boat_TargetDirection, Vector3.up);

		if (Vector3.Dot (Vector3.right, boat_TargetDirection) < 0)
			targetAngle = -targetAngle;
		
		Quaternion targetRot = Quaternion.Euler (0, targetAngle, 0);
		boatMesh.localRotation = Quaternion.RotateTowards (boatMesh.localRotation, targetRot, boat_MeshRotationSpeed * Time.deltaTime);


	}

	private void UpdateBoatPosition () {

		float distanceToFlag = Vector3.Distance (flagImage.transform.position, boatTransform.position);

		float targetSpeed = distanceToFlag - flag_DistanceToStop;
		if (distanceToFlag < flag_DistanceToStop)
			targetSpeed = 0f;

		targetSpeed = Mathf.Clamp (targetSpeed, 0f, boatMaxSpeed);
		boatCurrentSpeed = Mathf.MoveTowards ( boatCurrentSpeed, targetSpeed , boatAcceleration * Time.deltaTime );

		boat_CurrentDirection = Vector3.MoveTowards (boat_CurrentDirection, boat_TargetDirection, boatRotationSpeed * Time.deltaTime);

		boatTransform.Translate (boat_CurrentDirection * boatCurrentSpeed * Time.deltaTime, Space.World);

		flagImage.enabled = distanceToFlag > (flag_DistanceToStop*2);


	}
	#endregion

	public void OnMouseEnter() {
	}

	public void OnMouseExit() {
	}

	public void OnMouseDown() {
		Opened = !Opened;
	}

	public bool Opened {
		get {
			return wheel_Opened;
		}
		set {
			wheel_Opened = value;

			wheel_Arrow.SetActive (value);

			wheelTransform.localScale = wheel_Opened ? Vector3.one * 1.3f : Vector3.one;

		}
	}

	#region flag
	private void UpdateFlagPosition () {

		if (InputManager.Instance.OnInputStay ()) {

			boat_IsMoving = true;
			Vector3 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition ());

//			flagImage.transform.localPosition = Vector3.zero;

			flagImage.rectTransform.anchorMin = pos;
			flagImage.rectTransform.anchorMax = pos;


		}

		if (boat_IsMoving) {
			Vector3 islandPos = IslandManager.Instance.IslandImage.transform.position;
			islandPos.z = flagImage.transform.position.z;

			float distanceToFlag = Vector3.Distance (flagImage.transform.position, boatTransform.position);
			float distanceToIsland = Vector3.Distance (flagImage.transform.position, islandPos);

			flagImage.color = distanceToIsland < distanceToTriggerIsland ? Color.red : Color.blue;
			if (distanceToIsland < distanceToTriggerIsland) {

				if (distanceToFlag < flag_DistanceToStop * 2) {
					IslandManager.Instance.Enter ();
					boat_IsMoving = false;
				}
			}

			boat_TargetDirection = (flagImage.transform.position - boatMesh.position).normalized;
		}
	}
	#endregion

	#region wheel
	private void UpdateWheel () {


		if (wheel_Opened) {

			Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint (wheelTransform.position)).normalized;
			dir.z = 0f;

			if (!wheel_Turned) {
				wheelTransform.up = dir;

				wheel_Turned = true;
			} else {
				wheelTransform.up = Vector3.MoveTowards (wheelTransform.up, dir, wheelRotSpeed * Time.deltaTime);
			}
			boat_TargetDirection = wheelTransform.up;

			if ( InputManager.Instance.OnInputExit () ) {

				Opened = false;

				wheel_Turned = false;

			}


		} else {
			wheelTransform.up = Vector3.MoveTowards (wheelTransform.up, Vector3.up, boatRotationSpeed * Time.deltaTime);

			if (IslandManager.Instance.OnIsland == false) {

			}

		}
	}
	#endregion
}
