using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WheelControl : MonoBehaviour {

	[Header("Wheel")]
	[SerializeField]
	private Transform wheelTransform;
	[SerializeField]
	private float wheelRotSpeed = 500f;
	[SerializeField]
	private GameObject wheel_Arrow;
	private bool wheel_Opened = false;
	private bool wheel_Turned = false;

	[SerializeField]
	private Boat playerBoat;

	// Use this for initialization
	void Start () {
		Opened = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateWheel ();
	}

	public Transform WheelTransform {
		get {
			return wheelTransform;
		}
	}

	private void UpdateWheel () {

		if (wheel_Opened) {

			Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint (wheelTransform.position)).normalized;
			dir.z = 0f;

			playerBoat.TargetDirection = dir;
			wheelTransform.up = dir;

			if ( InputManager.Instance.OnInputExit () ) {

				Opened = false;

				wheel_Turned = false;

			}


		}
	}

	#region input
	public void OnMouseEnter() {
	}

	public void OnMouseExit() {
	}

	public void OnMouseDown() {
		Opened = !Opened;
	}
	#endregion

	#region properties
	public bool Opened {
		get {
			return wheel_Opened;
		}
		set {
			wheel_Opened = value;

			wheel_Arrow.SetActive (value);

			wheelTransform.localScale = wheel_Opened ? Vector3.one * 1.3f : Vector3.one;

			playerBoat.TargetSpeed = value ? playerBoat.MaxSpeed : 0f ;
		}
	}
	#endregion
}
