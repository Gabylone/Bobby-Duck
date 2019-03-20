using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour {

    public bool placing = false;

	public Transform _transform;

    public int currentLignIndex = 0;

	public float battlefieldLenght = 10f;

    public Lign CurrentLign
    {
        get
        {
			return LignManager.Instance.ligns[currentLignIndex];
        }
    }

    // Use this for initialization
    public virtual void Start () {

		_transform = GetComponent<Transform>();
    }

    public virtual void PlaceStart ()
    {
		InputManager.onInputExit += HandleOnInputExit;

		SoundManager.Instance.Play (SoundManager.SoundType.Door_Close);
        placing = true;

		BottomBar.Instance.Down ();

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

		float y = battlefieldLenght * Input.mousePosition.y / Screen.height;

        float f = (p.x * 4 / Screen.width);
        currentLignIndex = (int)f;

		transform.position = new Vector3(CurrentLign.transform.position.x, battlefieldLenght - y, 0f);

    }

    public virtual void PlaceExit()
    {
        Tween.Bounce(transform);

        placing = false;

		SoundManager.Instance.Play (SoundManager.SoundType.Door_Close);

		BottomBar.Instance.Up ();

		InputManager.onInputExit -= HandleOnInputExit;



    }

    void HandleOnInputExit()
    {
        if (placing)
        {
            PlaceExit();
        }
    }
}
