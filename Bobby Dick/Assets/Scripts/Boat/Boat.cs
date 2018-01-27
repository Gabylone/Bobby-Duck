using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Boat : MonoBehaviour {

	public bool moving = false;

	[Space]
	[Header ("Boat Elements")]
	[SerializeField]
	private Transform boatMesh;
	public Transform getTransform;

	[Space]
	[Header ("Boat Position Parameters")]
	public float speed = 5f;
	public float startSpeed = 5f;

	public RectTransform targetRectTransform;
	private Vector2 targetDir;

	public Camera cam;

//	public delegate void OnLeaveScreen ();
//	public OnLeaveScreen onLeaveScreen;

	public virtual void Start () {
		
		getTransform = GetComponent<Transform> ();

		cam = Camera.main;

		CombatManager.Instance.onFightStart += DeactivateCollider;
		CombatManager.Instance.onFightEnd += ActivateCollider;

	}

	public virtual void Update () {
		if ( moving ) {
			UpdateBoatPosition ();
			SetBoatRotation ();
		}
	}

	void CheckForBounds ()
	{
		Vector2 p = (Vector2)getTransform.position;

		Vector2 viewportPos = cam.WorldToViewportPoint (p);
	}

	private void SetBoatRotation () {

		Vector2 targetDir = ((Vector2)targetRectTransform.localPosition - (Vector2)getTransform.localPosition).normalized;

		float targetAngle = Vector2.Angle (targetDir, Vector2.up);
		if (Vector2.Dot (Vector2.right, targetDir) < 0)
			targetAngle = -targetAngle;

		Quaternion targetRot = Quaternion.Euler (0, targetAngle, 0);

		boatMesh.localRotation = targetRot;

	}
	#region moving
	public virtual void SetTargetPos (RectTransform rectTransform ) {

		Tween.Bounce (getTransform);

		targetRectTransform = rectTransform;

		moving = true;

	}
	private void UpdateBoatPosition () {

		Vector2 targetDir = (Vector2)(targetRectTransform.localPosition - getTransform.localPosition).normalized;

		// translate boat
		getTransform.Translate (boatMesh.forward * speed * Time.deltaTime, Space.World);

	}

	public virtual void EndMovenent() {
		moving = false;
	}
	void DeactivateCollider ()
	{
		GetComponentInChildren<BoxCollider2D> ().enabled = false;
	}
	void ActivateCollider ()
	{
		GetComponentInChildren<BoxCollider2D> ().enabled = true;
	}
	#endregion

	#region map position 
	public virtual void UpdatePositionOnScreen () {
		foreach (TrailRenderer renderer in GetComponentsInChildren<TrailRenderer>())
			renderer.Clear ();
	}
	#endregion

}
