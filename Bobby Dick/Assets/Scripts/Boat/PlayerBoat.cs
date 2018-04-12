using UnityEngine;
using System.Collections;
using System;

public class PlayerBoat : Boat {

	public static PlayerBoat Instance;

	public delegate void OnEndMovement ();
	public OnEndMovement onEndMovement;

	public RectTransform defaultRecTransform;

    bool followingPointer = false;

	public override void Start ()
	{
		base.Start();

		WorldTouch.onPointerDown += HandleOnPointerDown;
		WorldTouch.onPointerExit += HandleOnPointerExit;
        Island.onTouchIsland += HandleOnTouchIsland;

		StoryLauncher.Instance.onStartStory += EndMovenent;
		StoryLauncher.Instance.endStoryEvent += EndMovenent;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;

	}


    void Awake () {
		Instance = this;
	}

	void HandleChunkEvent ()
	{

		SetTargetPos (defaultRecTransform);
	}

	void HandleOnTouchIsland ()
	{
		SetTargetPos (Island.Instance.GetComponent<RectTransform>());
	}

    #region events
    void HandleOnPointerDown ()
	{
        followingPointer = true;

        Vector2 pos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetInputPosition());
        SetTargetPos(Flag.Instance.rectTransform);
    }

    private void HandleOnPointerExit()
    {
        
    }
    #endregion

    public override void Update ()
	{
		base.Update ();
	}

	public override void EndMovenent ()
	{
		base.EndMovenent ();

        followingPointer = false;

        if ( onEndMovement != null )
			onEndMovement ();
	}

	public override void UpdatePositionOnScreen ()
	{
		base.UpdatePositionOnScreen ();

		getTransform.position = NavigationManager.Instance.Anchors[(int)Boats.playerBoatInfo.currentDirection].position;

	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Flag") {
			EndMovenent ();
		}
	}
}
