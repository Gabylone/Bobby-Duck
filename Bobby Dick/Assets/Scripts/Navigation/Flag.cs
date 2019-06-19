using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Holoville.HOTween;

public class Flag : MonoBehaviour {

	public static Flag Instance;

    [SerializeField]
    private GameObject group = null;

    public Camera cam;

    bool visible = false;

    SpriteRenderer spriteRenderer;

    public LayerMask layerMask;

	void Awake () {
		Instance = this;
	}

    // Use this for initialization
    void Start()
    {
        WorldTouch.onPointerExit += HandleOnPointerExit;

        PlayerBoat.Instance.onEndMovement += HandleOnEndMovement;
        NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

        Swipe.onSwipe += HandleOnSwipe;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
        else
        {
            Debug.Log("failed to update flag position");
        }

    }

    void HandleChunkEvent ()
	{
        Show();
        transform.localPosition = Vector3.zero;
        PlayerBoat.Instance.SetTargetPos(transform.position);
    }

    void HandleOnEndMovement()
    {
        Tween.Bounce(transform);

        HOTween.Kill(spriteRenderer);
        HOTween.To( spriteRenderer , Tween.defaultDuration , "color" , Color.clear );

        CancelInvoke("Hide");
        Invoke("Hide", Tween.defaultDuration);
    }

	void HandleOnTouchIsland ()
	{
		Hide ();
	}


    void HandleOnPointerExit()
    {
        Show();
        UpdateFlagPos();

        Tween.Bounce(transform);

    }

    void Show()
    {
        HOTween.Kill(spriteRenderer);
        CancelInvoke("Hide");
        spriteRenderer.color = Color.white;

        if (visible)
            return;


        visible = true;

        CancelInvoke();

        group.SetActive(true);
    }

    public void Hide()
    {
        visible = false;

        group.SetActive(false);
    }
}
