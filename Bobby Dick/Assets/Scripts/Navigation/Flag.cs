using System;
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
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

        WorldTouch.onPointerDown += HandleOnTouchWorld;
        //WorldTouch.onPointerExit += HandleOnTouchWorld;

        Island.onTouchIsland += HandleOnTouchIsland;

        PlayerBoat.Instance.onEndMovement += HandleOnEndMovement;
        NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

        Swipe.onSwipe += HandleOnSwipe;

    }

    private void HandleOnSwipe(Directions direction)
    {
        Hide();
    }

    private void Update()
    {
        if (WorldTouch.Instance.touching)
        {
            PlaceFlagOnScreen(InputManager.Instance.GetInputPosition());
        }
    }

    void HandleChunkEvent ()
	{
		Vector2 p = Camera.main.WorldToScreenPoint (PlayerBoat.Instance.defaultRecTransform.position);
		PlaceFlagOnScreen (p);
	}

	void HandleOnEndMovement ()
	{
		Hide ();
	}

	

	void HandleOnTouchIsland ()
	{
		Hide ();
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

    void Show()
    {
        Tween.Bounce(transform);
        CancelInvoke();
        group.SetActive(true);
    }
    void Hide()
    {
        group.SetActive(false);
    }
}
