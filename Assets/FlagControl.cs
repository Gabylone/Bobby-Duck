using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagControl : MonoBehaviour {


	[Header ("Flag")]
	[SerializeField]
	private Image flagImage;
	[SerializeField]
	private float distanceToStop = 1.1f;

	[SerializeField]
	private float distanceToTriggerIsland = 0.8f;

	[SerializeField]
	private float boatSpeed = 1.2f;

	[SerializeField]
	private Boat playerBoat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateFlagPosition ();
	}

	private void UpdateFlagPosition () {

		if (InputManager.Instance.OnInputStay ()) {
			PlaceFlagOnScreen ();
		}

		UpdateFlagToIsland ();
	}

	private void UpdateFlagToIsland () {

			// get island pos
		Vector3 islandPos = IslandManager.Instance.IslandImage.transform.position;
		islandPos.z = flagImage.transform.position.z;

		// calc distances
		float distance_BoatToFlag = Vector3.Distance (flagImage.transform.position, playerBoat.GetTransform.position);
		bool flagIsNearIsland = Vector3.Distance (flagImage.transform.position, islandPos) < distanceToTriggerIsland;

		playerBoat.TargetSpeed = (distance_BoatToFlag - distanceToStop) * boatSpeed;

		if (distance_BoatToFlag < distanceToStop) {
			playerBoat.TargetSpeed = 0f;
			if  (flagIsNearIsland )
				IslandManager.Instance.Enter ();
		}

		playerBoat.TargetDirection = (flagImage.transform.position - playerBoat.GetTransform.position).normalized;

		flagImage.color = flagIsNearIsland ? Color.red : Color.blue;
		flagImage.enabled = !(distance_BoatToFlag < distanceToStop);
	}

	private void PlaceFlagOnScreen () {
		Vector3 pos = Camera.main.ScreenToViewportPoint (InputManager.Instance.GetInputPosition ());

		flagImage.rectTransform.anchorMin = pos;
		flagImage.rectTransform.anchorMax = pos;
	}


	#region properties
	public Image FlagImage {
		get {
			return flagImage;
		}
	}
	#endregion


}
