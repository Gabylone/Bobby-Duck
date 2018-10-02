using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager Instance;

	public enum ScreenPart {
		Any,
		Left,
		Right
	}

	public delegate void OnInputDown ();
	public static OnInputDown onInputDown;

	public delegate void OnInputExit ();
	public static OnInputExit onInputExit;

	bool mobileTest = false;

	float timer = 0f;

	public float clickBuffer = 0.1f;

	void Awake () {
		Instance = this;
	}
    private void OnDestroy()
    {
        onInputDown = null;
        onInputExit = null;
    }

    void Update () {

		if (InputDown ()) {

			timer = 0f;

			if (onInputDown != null)
				onInputDown ();
		}

		if (InputExit ()) {

			if (onInputExit != null)
				onInputExit ();
		}

		timer += Time.deltaTime;

	}

	#region get touch & click
	/// <summary>
	/// Raises the input down event.
	/// </summary>
	bool InputDown () {
		return InputDown (0,ScreenPart.Any);
	}
	bool InputDown (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile) {
			
			if (Input.touches.Length <= 0)
				return false;
			
			return Input.GetTouch (id).phase == TouchPhase.Began && rightSideOfScreen;
		}
		else
			return Input.GetMouseButtonDown (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Raises the input stay event.
	/// </summary>
	bool InputStay () {
		return InputStay (0,ScreenPart.Any);
	}
	bool InputStay (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile) {
			if (Input.touches.Length <= 0)
				return false;

			return (Input.GetTouch (id).phase == TouchPhase.Stationary || Input.GetTouch (id).phase == TouchPhase.Moved) && rightSideOfScreen;
		} else
			return Input.GetMouseButton (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Raises the input exit event.
	/// </summary>
	bool InputExit () {
		return InputExit (0,ScreenPart.Any);
	}
	bool InputExit (int id, ScreenPart screenPart) {

		bool rightSideOfScreen = GetInputPosition ().x > 0;
		if (screenPart == ScreenPart.Left)
			rightSideOfScreen = GetInputPosition ().x <= Screen.width / 2;
		if (screenPart == ScreenPart.Right)
			rightSideOfScreen = GetInputPosition ().x > Screen.width / 2;

		if (OnMobile) {
			
			if (Input.touches.Length <= 0)
				return false;

			return (Input.GetTouch (id).phase == TouchPhase.Ended) && rightSideOfScreen;
		}
		else
			return Input.GetMouseButtonUp (id) && rightSideOfScreen;
	}

	/// <summary>
	/// Gets the input position.
	/// </summary>
	/// <returns>The input position.</returns>
	public Vector3 GetInputPosition () {
		return GetInputPosition (0);
	}
	public Vector3 GetInputPosition (int id) {
		if (OnMobile) {
			if ( Input.touches.Length <=0 ) {
				return Vector3.zero;
			}
			return Input.GetTouch (id).position;
		} else {
			return Input.mousePosition;
		}
	}
	#endregion


	bool OnMobile {
		get {
			return Application.isMobilePlatform;
		}
	}
}