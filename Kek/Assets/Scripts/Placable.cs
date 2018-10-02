using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour {

    public bool placing = false;

    public RectTransform rectTransform;

    public int currentLignIndex = 0;

    public Lign CurrentLign
    {
        get
        {
            return Lign.ligns[currentLignIndex];
        }
    }

    public LayerMask layerMask;

    // Use this for initialization
    public virtual void Start () {
		InputManager.onInputExit += HandleOnInputExit;

        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void PlaceStart ()
    {
        Sound.Instance.PlaySound(Sound.Type.Fall2);
        placing = true;

        DisplayInventory.Instance.Hide();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (placing)
            PlaceUpdate();
    }

    void PlaceUpdate()
    {
        Vector2 p = Input.mousePosition;

        float f = (p.x * 4 / Screen.width);
        currentLignIndex = (int)f;

        //transform.position = new Vector3(-4.5f + currentLignIndex * 3, p.y, 0f);
        rectTransform.anchoredPosition = new Vector2( 200f / 2f + (currentLignIndex*200) , p.y );

    }

    public virtual void PlaceExit()
    {
        Tween.Bounce(transform);

        placing = false;

        Sound.Instance.PlaySound(Sound.Type.Fall1);

        DisplayInventory.Instance.Show();

    }

    void HandleOnInputExit()
    {
        if (placing)
        {
            PlaceExit();
        }
    }
}
