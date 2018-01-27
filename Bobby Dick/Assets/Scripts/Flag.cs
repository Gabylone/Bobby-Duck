using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	public static Flag Instance;

	public RectTransform rectTransform;

	[SerializeField]
	private GameObject group = null;


	[SerializeField]
	private RectTransform defaultRectTransform = null;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		rectTransform = GetComponent<RectTransform> ();

		WorldTouch.onTouchWorld+=HandleOnTouchWorld;

		Island.onTouchIsland += HandleOnTouchIsland;

		PlayerBoat.Instance.onEndMovement += HandleOnEndMovement;
		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

	}

	void HandleChunkEvent ()
	{
//		ResetFlag ();

		Vector2 p = Camera.main.WorldToScreenPoint (PlayerBoat.Instance.defaultRecTransform.position);
//		p = Camera.main.inst
		PlaceFlagOnScreen (p);
	}

	void HandleOnEndMovement ()
	{
//		Tween.Bounce (transform);
		Hide ();
	}
	void Hide() {
		group.SetActive (false);
	}

	void HandleOnTouchIsland ()
	{
		Hide ();
	}
	void Show () {
		Tween.Bounce (transform);
		CancelInvoke ();
		group.SetActive (true);
	}
	void HandleOnTouchWorld ()
	{
		PlaceFlagOnScreen (InputManager.Instance.GetInputPosition ());
	}

	void ResetFlag ()
	{
		Show ();

		rectTransform.anchorMin = Vector2.one * 0.5f;
		rectTransform.anchorMax = Vector2.one * 0.5f;

		rectTransform.localPosition = defaultRectTransform.localPosition;	
	}

	void PlaceFlagOnScreen ( Vector2 p ) {

		Show ();

		rectTransform.anchoredPosition = Vector2.zero;	

		Vector2 pos = Camera.main.ScreenToViewportPoint (p);

		rectTransform.anchorMin = pos;
		rectTransform.anchorMax = pos;
	}
}
