using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTouch : MonoBehaviour {

    public static WorldTouch Instance;

	public delegate void OnTouchWorld ();
	public static OnTouchWorld onPointerExit;

	public delegate void OnPointerDownEvent ();
	public static OnPointerDownEvent onPointerDown;

    public bool touching = false;

    float timer = 0f;
	float timeToTouch = 0.25f;

    public bool isEnabled = false;

    bool invoking = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

		Swipe.onSwipe += HandleOnSwipe;

        NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

        StoryLauncher.Instance.onPlayStory += Disable;
        StoryLauncher.Instance.onEndStory += Enable;

    }

    private void Enable()
    {
        isEnabled = true;
        invoking = false;
    }

    private void Disable()
    {
        isEnabled = false;
    }

	void HandleChunkEvent ()
	{
		
	}

	void HandleOnSwipe (Directions direction)
	{
		touching = false;
        swipped = true;
	}
	
	// Update is called once per frame
	void Update () {
		
        if ( isEnabled == false )
        {
            if ( !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && !invoking )
            {
                invoking = true;
                Invoke("Enable", 0.58f);
            }
        }
        else
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() )
            {
                Disable();
            }
        }

        /*if (touching)
        {
            if ( Swipe.Instance.timer > Swipe.Instance.minimumTime)
            {
                
            }
        }*/

	}

    //public void OnPointerDown () {
    public void OnMouseDown()
    {
        Debug.Log("touch down");

        if (!isEnabled)
        {
            Debug.Log("is disabled");
            return;
        }

        touching = true;

        if (onPointerDown != null)
        {
            onPointerDown();
        }

    }

    public bool swipped = false;

    //public void OnPointerUp () {
    private void OnMouseUp()
    {
        if (!touching)
            return;

        Debug.Log("touch up");
        if (!isEnabled)
        {
        Debug.Log("is disabled");
            return;
        }

        touching = false;


        if (onPointerExit != null) {
            onPointerExit();
		}
	}
}
