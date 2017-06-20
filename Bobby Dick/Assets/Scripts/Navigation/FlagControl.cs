using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagControl : MonoBehaviour {

	[Header ("Flag")]
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

	[SerializeField]
	private Island island;

	public bool updatingPosition = false;

	bool targetedIsland = false;

	// Use this for initialization
	void Start () {
		flagRect = flagImage.GetComponent<RectTransform> ();
		NavigationManager.Instance.EnterNewChunk += ResetFlag;

	}
	
	// Update is called once per frame
	void Update () {
		if (updatingPosition && StoryLauncher.Instance.PlayingStory == false) {
			PlaceFlagOnScreen ();
		}

		UpdateFlagToIsland ();

	}

	public void ResetFlag () {
		Vector2 pos = Camera.main.ScreenToViewportPoint (new Vector2 (Screen.width/2 ,Screen.height/2));

		flagRect.anchorMin = pos;
		flagRect.anchorMax = pos;

	}

	private void UpdateFlagToIsland () {

			// get flat poses
		Vector2 boatPos 	= (Vector2)playerBoat.GetTransform.localPosition;
		Vector2 flagPos 	= (Vector2)flagRect.localPosition;
		Vector2 islandPos 	= (Vector2)island.getTransform.localPosition;
		Vector2 islandWorldPos = (Vector2)island.getTransform.position;

		// calc distances
		float distance_BoatToFlag = Vector2.Distance (flagPos, boatPos);
		bool flagIsNearIsland = Vector2.Distance (flagPos, islandPos) < distanceToTriggerIsland;

		flagImage.color = flagIsNearIsland ? Color.red : Color.blue;
		flagImage.enabled = !(distance_BoatToFlag < distanceToStop + 0.3f);

		if (targetedIsland == false) {
			if (updatingPosition) {

				if ( flagIsNearIsland ) {

					targetedIsland = true;

				}

			}
		} else {
			if ( updatingPosition ) {
				if (flagIsNearIsland == false) {
					targetedIsland = false;
				}
			}
		}
//

		if (targetedIsland ) {

			Vector2 pos = islandWorldPos + decalToIsland;

			if ( distance_BoatToFlag < distanceToStop ) {

				island.Enter ();

				targetedIsland = false;

				flagImage.color = Color.blue;

			}

		}
		playerBoat.TargetSpeed = boatSpeed;
		playerBoat.TargetDirection = (flagPos - boatPos).normalized;

		if (distance_BoatToFlag < distanceToStop) {
			playerBoat.TargetSpeed = 0f;
		}
	}

	private void PlaceFlagOnScreen () {
		Vector2 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition ());

		flagRect.anchorMin = pos;
		flagRect.anchorMax = pos;
	}

	public void PlaceFlagOnWorld ( Vector3 pos )
	{
		Vector2 anchor = Camera.main.WorldToViewportPoint (pos);
		flagRect.anchorMin = anchor;
		flagRect.anchorMax = anchor;
	}

	#region properties
	public Image FlagImage {
		get {
			return flagImage;
		}
	}
	#endregion


	public bool UpdatingPosition {
		get {
			return updatingPosition;
		}
		set {
			updatingPosition = value;
			PlaceFlagOnScreen ();
		}
	}
}
