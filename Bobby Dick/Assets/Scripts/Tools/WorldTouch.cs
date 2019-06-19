using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldTouch : MonoBehaviour
{

    public static WorldTouch Instance;

    public delegate void OnTouchWorld();
    public static OnTouchWorld onPointerExit;

    public delegate void OnPointerDownEvent();
    public static OnPointerDownEvent onPointerDown;

    public bool touching = false;

    public bool swipped = false;

    float timer = 0f;
    float timeToTouch = 0.25f;

	public Image testimage;

    public bool isEnabled = false;

    public bool locked = false;

    bool invoking = false;

    private void Awake()
    {
        Instance = this;

        onPointerDown = null;
        onPointerExit = null;
    }

    // Use this for initialization
    void Start()
    {
        Swipe.onSwipe += HandleOnSwipe;
    }

    private void Enable()
    {
        isEnabled = true;
        invoking = false;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    void HandleOnSwipe(Directions direction)
    {
        touching = false;
        swipped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled == false)
        {
            if (!IsPointerOverUIObject() && !invoking)
            {
                invoking = true;

                CancelInvoke("Enable");
                Invoke("Enable", 0.01f);
            }
        }
        else
        {
            if (IsPointerOverUIObject())
            {
                Disable();
            }
        }

    }

    public bool IsEnabled ()
    {
        //return isEnabled && IsPointerOverUIObject() == false;
        return isEnabled;
    }

    public void OnMouseDown()
    {
        if (locked)
            return;

        if (!IsEnabled())
        {
            return;
        }

        touching = true;

        if (onPointerDown != null)
        {
            onPointerDown();
        }

    }

    private void OnMouseUp()
    {
        if (locked)
            return;

        if (!IsEnabled())
        {
            return;
        }

        if (!touching)
        {
            return;
        }

        touching = false;

        if (onPointerExit != null)
        {
            onPointerExit();
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        /*if ( results.Count > 0)
        {
            Debug.LogError("results : " + results[0].gameObject.name);
        }
        else
        {
            Debug.LogError("not touching anything");
        }*/

        return results.Count > 0;
    }

}
