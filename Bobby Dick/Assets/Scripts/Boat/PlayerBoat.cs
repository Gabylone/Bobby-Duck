using UnityEngine;
using System.Collections;
using System;

public class PlayerBoat : Boat {

	public static PlayerBoat Instance;

	public delegate void OnEndMovement ();
	public OnEndMovement onEndMovement;

    void Awake()
    {
        Instance = this;
    }

    public override void Start ()
	{
		base.Start();


        //WorldTouch.onPointerDown += HandleOnPointerDown;
		WorldTouch.onPointerExit += HandleOnPointerExit;
        Island.onTouchIsland += HandleOnTouchIsland;

		StoryLauncher.Instance.onPlayStory += EndMovenent;
		StoryLauncher.Instance.onEndStory += EndMovenent;

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

		NavigationManager.Instance.EnterNewChunk += UpdatePositionOnScreen;

	}
    public LayerMask layerMask;
    public override void Update()
    {
        base.Update();

        /*print("cam ! " + Camera.allCameras[0].name);
        print("boat screen postion : " + Camera.allCameras[0].WorldToViewportPoint(transform.position));
        print("screen w : " + Screen.width);
        print("screen h : " + Screen.height);*/

       
    }

    void HandleChunkEvent ()
	{
        //SetTargetPos(NavigationManager.Instance.GetAnchor(Directions.None));
	}

	void HandleOnTouchIsland ()
	{
		SetTargetPos (Island.Instance.transform.position);
	}

    #region events
    void HandleOnPointerDown ()
	{

        /*Vector2 pos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetInputPosition());
        SetTargetPos(Flag.Instance.transform.position);*/
    }

    private void HandleOnPointerExit()
    {
        Tween.Bounce(getTransform);

    }
    #endregion


    public override void EndMovenent ()
	{
		base.EndMovenent ();

        WorldTouch.Instance.touching = false;

        if ( onEndMovement != null )
			onEndMovement ();
	}

	public override void UpdatePositionOnScreen ()
	{
		base.UpdatePositionOnScreen ();

        getTransform.position = NavigationManager.Instance.GetOppositeCornerPosition(Boats.playerBoatInfo.currentDirection);

	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Flag") {
			EndMovenent ();
		}
	}
}
