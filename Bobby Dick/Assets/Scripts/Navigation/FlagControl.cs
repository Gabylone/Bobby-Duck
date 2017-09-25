using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagControl : MonoBehaviour {

	[Header ("Flag UI")]
	[SerializeField]
	private Image flagImage;
	private RectTransform flagRect;
	[SerializeField]
	private float distanceToStop = 1.1f;

	[SerializeField]
	private float distanceToTriggerIsland = 0.8f;

	[SerializeField]
	private float boatSpeed = 1.2f;

	[SerializeField]
	private Boat playerBoat;

	[SerializeField]
	private Vector2 decalToIsland = Vector2.zero;

	public bool updatingPosition = false;

	private bool targetedIsland = false;

	// Use this for initialization
	void Start () {
		flagRect = flagImage.GetComponent<RectTransform> ();
		NavigationManager.Instance.EnterNewChunk += ResetFlag;

		Island.onTouchIsland += HandleOnTouchIsland;

	}

	void HandleOnTouchIsland ()
	{
		TargetedIsland = true;

		PlaceFlagOnScreen ();
	}
	
	// Update is called once per frame
	void Update () {

		if (UpdatingPosition) {
			PlaceFlagOnScreen ();
		}
		
		UpdateBoat ();

	}

	public void ResetFlag () {
		Vector2 pos = Camera.main.ScreenToViewportPoint (new Vector2 (Screen.width/2 ,Screen.height/2));

		flagRect.anchorMin = pos;
		flagRect.anchorMax = pos;

	}

	private void UpdateBoat () {

			// get flat poses
		Vector2 boatPos 	= (Vector2)playerBoat.GetTransform.localPosition;
		Vector2 flagPos 	= (Vector2)flagRect.localPosition;

		// calc distances
		float distance_BoatToFlag = Vector2.Distance (flagPos, boatPos);

		playerBoat.TargetSpeed = boatSpeed;
		playerBoat.TargetDirection = (flagPos - boatPos).normalized;

		if (!TargetedIsland) {
			flagImage.enabled = distance_BoatToFlag > distanceToStop;

			if (distance_BoatToFlag < distanceToStop) {
				playerBoat.TargetSpeed = 0f;
			}
		}
	}

	public void PlaceFlagOnScreen () {

		flagRect.anchoredPosition = Vector2.zero;	

		Vector2 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition ());

		flagRect.anchorMin = pos;
		flagRect.anchorMax = pos;
	}

	public void PlaceFlagOnWorld ( Vector3 pos )
	{
		pos.z = 0f;
		flagRect.transform.position = pos;

//		Vector2 anchor = Camera.main.WorldToViewportPoint (pos);
//		flagRect.anchorMin = anchor;
//		flagRect.anchorMax = anchor;
	}

	#region properties
	public Image FlagImage {
		get {
			return flagImage;
		}
	}
	#endregion

	#region island events

	#endregion

	public void OnPointerDown () {
		UpdatingPosition = true;
	}

	public void OnPointerUp () {
		UpdatingPosition = false;
	}

	public bool UpdatingPosition {
		get {
			return updatingPosition;
		}
		set {

			if (updatingPosition == value)
				return;

			updatingPosition = value;

			if (value) {
				Tween.Bounce (FlagImage.transform);
				PlaceFlagOnScreen ();
				TargetedIsland = false;
			}

		}
	}

	public bool TargetedIsland {
		get {
			return targetedIsland;
		}
		set {

			if (targetedIsland == value)
				return;

			if (value) {
				flagImage.enabled = false;
			}
			
			targetedIsland = value;

			UpdateBoat ();

		}
	}
}
