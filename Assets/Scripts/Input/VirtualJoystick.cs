using UnityEngine;
using System.Collections;

public class VirtualJoystick : MonoBehaviour {

	[SerializeField]
	private RectTransform backGroundTransform;

	[SerializeField]
	private RectTransform pointerTransform;

	[SerializeField]
	private float directionFactor = 0.5f;

	private bool touchingScreen = false;

	public InputManager.ScreenPart screenPart;

	[SerializeField]
	private float maxInput = 50f;

	// Use this for initialization
	void Start () {
		TouchingScreen = false;
	}
	
	// Update is called once per frame
	void Update () {

		if ( !touchingScreen ) {

			if (InputManager.Instance.OnInputDown (0,screenPart)) {
				TouchingScreen = true;
			}

		} else {

			if ( InputManager.Instance.OnInputExit (0,screenPart))  {

				TouchingScreen = false;
			}

			UdpatePointPosition ();
		}

	}

	public bool TouchingScreen {
		get {
			return touchingScreen;
		}
		set {
			touchingScreen = value;

			Vector3 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition());

			backGroundTransform.anchorMin = pos;
			backGroundTransform.anchorMax = pos;

			backGroundTransform.gameObject.SetActive (value);

			pointerTransform.position = backGroundTransform.position;

		}
	}

	private void UdpatePointPosition () {

		Vector3 dir = Camera.main.WorldToScreenPoint(backGroundTransform.position) - InputManager.Instance.GetInputPosition ();

		pointerTransform.position = backGroundTransform.position - (dir.normalized*directionFactor);

	}

	public float GetHorizontalAxis () { 
		return pointerTransform.localPosition.x / maxInput;
	}

	public float GetVerticalAxis () { 
		return pointerTransform.localPosition.y / maxInput;
	}
}
