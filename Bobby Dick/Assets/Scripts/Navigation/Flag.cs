using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	public static Flag Instance;

	public RectTransform rectTransform;

    [SerializeField]
    private GameObject group = null;

    public Camera cam;

    bool visible = false;

	[SerializeField]
	private RectTransform defaultRectTransform = null;

    public LayerMask layerMask;

	void Awake () {
		Instance = this;
	}

    // Use this for initialization
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

        //WorldTouch.onPointerDown += HandleOnTouchWorld;
        WorldTouch.onPointerExit += HandleOnPointerExit;

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
        if ( WorldTouch.Instance.touching && Swipe.Instance.timer > Swipe.Instance.minimumTime )
        {
            if (!visible)
                Show();

            UpdateFlagPos();
        }
    }

    private void UpdateFlagPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            transform.position = hit.point;
            PlayerBoat.Instance.SetTargetPos(hit.point);
        }

    }

    void HandleChunkEvent ()
	{
        Show();
        transform.localPosition = Vector3.zero;
        PlayerBoat.Instance.SetTargetPos(transform.position);
    }

    void HandleOnEndMovement ()
	{
		Hide ();
	}

	void HandleOnTouchIsland ()
	{
		Hide ();
	}
	
    void HandleOnPointerExit ()
	{
        Show();
        UpdateFlagPos();
    }

	void ResetFlag ()
	{
		rectTransform.anchorMin = Vector2.one * 0.5f;
		rectTransform.anchorMax = Vector2.one * 0.5f;

		rectTransform.localPosition = defaultRectTransform.localPosition;	
	}

    void Show()
    {
        if (visible)
            return;

        visible = true;

        Tween.Bounce(transform);
        CancelInvoke();
        group.SetActive(true);
    }

    void Hide()
    {
        visible = false;

        group.SetActive(false);
    }
}
