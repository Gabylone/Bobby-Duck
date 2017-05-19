using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Island : MonoBehaviour {

	private Transform transform;

	private Image image;

	[SerializeField]
	private Transform boat;

	[SerializeField]
	private float decal = 0f;

	private Canvas canvas;

	[SerializeField]
	private float distanceToTrigger = 1f;

	[SerializeField]
	private Vector3 flagDecal;


	#region mono
	void Start () {
		transform = GetComponent<Transform>();
		canvas = GetComponentInParent<Canvas> ();

		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;

	}
	
	void LateUpdate () {
		UpdateRenderOrder ();
	}
	#endregion

	#region render
	public void UpdatePositionOnScreen () {
		
		bool onIslandChunk = MapData.Instance.currentChunk.state == State.DiscoveredIsland || MapData.Instance.currentChunk.state == State.VisitedIsland;

		gameObject.SetActive ( onIslandChunk );
		if (onIslandChunk)
			transform.localPosition = MapData.Instance.currentChunk.IslandData.PositionOnScreen;
		else
			transform.localPosition = new Vector3 ( 10000f,0,0 );
	}

	private void UpdateRenderOrder () {
		canvas.planeDistance = (boat.localPosition.y + decal < transform.localPosition.y ? 12 : 8);
	}
	#endregion

	#region input events
	public void OnMouseEnter () {

		if ( Vector3.Distance ( boat.position, transform.position ) < distanceToTrigger && IslandManager.Instance.OnIsland == false){
			transform.localScale = Vector3.one * 1.2f;
		}
	}

	public void OnMouseExit() {
		transform.localScale = Vector3.one;
	}

	public void OnMouseDown () {
		
		if (NavigationManager.Instance.CurrentNavigationSystem == NavigationManager.NavigationSystem.Flag) {

			Vector3 pos = Camera.main.WorldToViewportPoint (transform.position + flagDecal);

			NavigationManager.Instance.FlagControl.FlagImage.rectTransform.anchorMin = pos;
			NavigationManager.Instance.FlagControl.FlagImage.rectTransform.anchorMax = pos;
		} else {
			if (Vector3.Distance (boat.position, transform.position) < distanceToTrigger) {
				transform.localScale = Vector3.one;

				IslandManager.Instance.Enter ();

			}

		}
	}
	#endregion
}
