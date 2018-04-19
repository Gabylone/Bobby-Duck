// Just add this script to your camera. It doesn't need any configuration.

using UnityEngine;

public class TouchCamera : MonoBehaviour {
	Vector2?[] oldTouchPositions = {
		null,
		null
	};
	Vector2 oldTouchVector;
	float oldTouchDistance;

	public Transform target;

	Camera camera;

	void Start () {
		camera = GetComponent<Camera> ();
	}

	int touchCount {
		get {
			if ( Application.isMobilePlatform ) {
				return Input.touchCount;
			}

			if ( Input.GetMouseButton(0) ) {
				if ( Input.GetMouseButton(1) ) {
					return 2;
				}
				return 1;
			}

			return 0;
		}
	}

	Vector2 touchPos (int i) {
		
		if ( Application.isMobilePlatform ) {
			return Input.GetTouch(i).position;
		}

		if ( i == 1 ) {
			return Vector2.zero;
		}

		return Input.mousePosition;
	}


	void Update() {

		if (touchCount == 0) {

			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		else if (touchCount == 1) {

			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null) {
				oldTouchPositions[0] = touchPos(0);
				oldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = touchPos(0);

				Vector3 dir = transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * camera.orthographicSize / camera.pixelHeight * 2f));
				dir.z = 0f;
				transform.position += dir;

				oldTouchPositions[0] = newTouchPosition;
			}
		}
		else {
			if (oldTouchPositions[1] == null) {
				oldTouchPositions[0] = touchPos(0);
				oldTouchPositions[1] = touchPos(1);
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else {
				Vector2 screen = new Vector2(camera.pixelWidth, camera.pixelHeight);
				
				Vector2[] newTouchPositions = {
					touchPos(0),
					touchPos(1)
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * camera.orthographicSize / screen.y));
				float f = Mathf.Asin (Mathf.Clamp ((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f;
				transform.rotation *= Quaternion.Euler(new Vector3(0, f, 0));
				camera.orthographicSize *= oldTouchDistance / newTouchDistance;
				transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * camera.orthographicSize / screen.y);

				oldTouchPositions[0] = newTouchPositions[0];
				oldTouchPositions[1] = newTouchPositions[1];
				oldTouchVector = newTouchVector;
				oldTouchDistance = newTouchDistance;
			}
		}
	}
}
