using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager Instance;

	public enum ScreenPart {
		Any,
		Left,
		Right
	}

	[SerializeField]
	private VirtualJoystick virtualJoystick;

	void Awake () {
		Instance = this;
	}

	#region get touch & click
	/// <summary>
	/// Raises the input down event.
	/// </summary>
	public bool OnInputDown () {
		return OnInputDown (0,ScreenPart.Any);
	}
	public bool OnInputDown (int id, ScreenPart screenPart) {
		if (Application.isMobilePlatform) {
			if ( screenPart == ScreenPart.Any ) {
				print ("pas sensé petre ici");
				return Input.GetTouch (id).phase == TouchPhase.Began;

				//
			} else {
				if (screenPart == ScreenPart.Left) {

					if (GetInputPosition ().x < Screen.width / 2) {
						print ("POS : " + GetInputPosition ().x);
						print ("SCREEN : " + Screen.width);
						return Input.GetTouch (id).phase == TouchPhase.Began;
					}

					return false;
				}
				if (screenPart == ScreenPart.Right) {

					if (GetInputPosition ().x >= Screen.width / 2) {
						return Input.GetTouch (id).phase == TouchPhase.Began;
					}
					return false;

				}
				return false;

			}
		}
		else
			return Input.GetMouseButtonDown (id);
	}

	/// <summary>
	/// Raises the input stay event.
	/// </summary>
	public bool OnInputStay () {
		return OnInputStay (0,ScreenPart.Any);
	}
	public bool OnInputStay (int id, ScreenPart screenPart) {
		if (Application.isMobilePlatform) {
			if ( screenPart == ScreenPart.Any ) {
				return Input.GetTouch (id).phase == TouchPhase.Moved || Input.GetTouch (id).phase == TouchPhase.Stationary;
				//
			} else {
				if (screenPart == ScreenPart.Left) {

					if (GetInputPosition ().x < Screen.width / 2) {
						return Input.GetTouch (id).phase == TouchPhase.Moved || Input.GetTouch (id).phase == TouchPhase.Stationary;
					}

					return false;
				}
				if (screenPart == ScreenPart.Right) {

					if (GetInputPosition ().x >= Screen.width / 2) {
						return Input.GetTouch (id).phase == TouchPhase.Moved || Input.GetTouch (id).phase == TouchPhase.Stationary;
					}
					return false;

				}
				return false;

			}
		}
		else
			return Input.GetMouseButton (id);
	}

	/// <summary>
	/// Raises the input exit event.
	/// </summary>
	public bool OnInputExit () {
		return OnInputExit (0,ScreenPart.Any);
	}
	public bool OnInputExit (int id, ScreenPart screenPart) {
		if (Application.isMobilePlatform) {
			if ( screenPart == ScreenPart.Any ) {
				return Input.GetTouch (id).phase == TouchPhase.Ended;
				//
			} else {
				if (screenPart == ScreenPart.Left) {

					if (GetInputPosition ().x < Screen.width / 2) {
						return Input.GetTouch (id).phase == TouchPhase.Ended;
					}

					return false;
				}
				if (screenPart == ScreenPart.Right) {

					if (GetInputPosition ().x >= Screen.width / 2) {
						return Input.GetTouch (id).phase == TouchPhase.Ended;
					}
					return false;

				}
				return false;

			}
		}
		else
			return Input.GetMouseButtonUp (id);
	}

	/// <summary>
	/// Gets the input position.
	/// </summary>
	/// <returns>The input position.</returns>
	public Vector3 GetInputPosition () {
		return GetInputPosition (0);
	}
	public Vector3 GetInputPosition (int id) {
		if (Application.isMobilePlatform)
			return Input.GetTouch (id).position;
		else
			return Input.mousePosition;
	}
	#endregion


	#region get axis
	/// <summary>
	/// Gets the horizontal axis.
	/// </summary>
	/// <returns>The horizontal axis.</returns>
	public float GetHorizontalAxis () {

		if (Application.isMobilePlatform) {
			return virtualJoystick.GetHorizontalAxis ();
		} else {
			return Input.GetAxis ("Horizontal");
		}

	}

	/// <summary>
	/// Gets the vertical axis.
	/// </summary>
	/// <returns>The vertical axis.</returns>
	public float GetVerticalAxis () {

		if (Application.isMobilePlatform) {
			return virtualJoystick.GetVerticalAxis();
		} else {
			return Input.GetAxis ("Vertical");
		}

	}
	#endregion
}
