using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

	public enum Direction {
		Left,
		Right,
		Up,
		Down,

		None,
	}

	public delegate void OnSwipe (Direction direction);
	public static OnSwipe onSwipe;

    public static Swipe Instance;

    private void OnDestroy()
    {
        onSwipe = null;
    }

	Vector3 prevPoint;
    Vector3 m_pressedPosition;

    public Transform test;

	public float minimumDistance = 0.1f;
    public float minMagnitude = 50f;

    public float minimumTime = 0.5f;

	bool swiping = false;

	float timer = 0f;


    private void Awake()
    {
        Instance = this;
    }

    void Start () {
		InputManager.onInputDown += Swipe_Start;
		InputManager.onInputExit += HandleOnInputExit;
        onSwipe += HandleOnSwipe;
	}

    void Update () {

		if (swiping) {
			Swipe_Update ();
		}

        DebugSwipe();

    }

    #region swipe state
    public void Swipe_Start() {

		swiping = true;
		timer = 0f;

        m_pressedPosition = InputManager.Instance.GetInputPosition();
        prevPoint = InputManager.Instance.GetInputPosition ();

	}


	void Swipe_Update() {

        Vector3 upPosition = Input.mousePosition;
        Vector3 diff = upPosition - m_pressedPosition;

        if (diff.magnitude > minMagnitude)
        {

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                if (diff.x > 0)
                {
                    onSwipe(Direction.Right);
                }
                else
                {
                    onSwipe(Direction.Left);

                }
            }
            else
            {
                if (diff.y > 0)
                {
                    onSwipe(Direction.Up);

                }
                else
                {
                    onSwipe(Direction.Down);

                }
            }
        }
    }

    void HandleOnInputExit()
    {
        Swipe_Exit();
    }
    private void HandleOnSwipe(Direction direction)
    {
        Swipe_Exit();
    }

    void Swipe_Exit()
    {
        swiping = false;
    }
    #endregion

    public Direction GetDirectionFromVector ( Vector3 dir ) {

		for (int i = 0; i < 4; ++i ) {
			if ( Vector3.Angle ( GetDir( (Direction)i ) , dir ) < 45f ) {
				return (Direction)i;
			}
		}
		return Direction.None;
	}

	public Vector3 GetDir ( Direction dir ) {

		switch (dir) {
		case Direction.Up:
			return new Vector3 (0, 1);
		case Direction.Left:
			return new Vector3 (-1, 0);
		case Direction.Down:
			return new Vector3 (0, -1);
		case Direction.Right:
			return new Vector3 (1, 0);
		case Direction.None:
			return new Vector3 (0, 0);
		}

		return Vector3.zero;

	}

	void DebugSwipe()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            onSwipe(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            onSwipe(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            onSwipe(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            onSwipe(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            onSwipe(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            onSwipe(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            onSwipe(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onSwipe(Direction.Up);
        }
    }
}
