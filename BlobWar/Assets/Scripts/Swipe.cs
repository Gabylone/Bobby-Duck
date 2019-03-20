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

	public delegate void OnTap ();
	public static OnTap onTap;

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

	bool canTap = false;

	bool swiping = false;

	float timer = 0f;

	public float tapMaxTime = 0.15f;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
		
		InputManager.onInputDown += Swipe_Start;
		InputManager.onInputExit += HandleOnInputExit;

	}

    void Update () {

		if (swiping) {
			Swipe_Update ();
		}

		if (Application.isMobilePlatform) {

			if (Input.touchCount > 0) {

				if (Input.GetTouch (0).phase == TouchPhase.Began  ) {

					canTap = true;

					timer = 0f;

				}

				if (Input.GetTouch (0).phase == TouchPhase.Canceled || Input.GetTouch (0).phase == TouchPhase.Ended ) {

					TryTap ();
				}

			}

		} else {

			if ( Input.GetMouseButtonDown(0)  ) {

				canTap = true;

				timer = 0f;

			}

			if ( Input.GetMouseButtonUp(0)  ) {

				TryTap ();
			}

			timer += Time.deltaTime;

		}

        DebugSwipe();

    }

	void TryTap () {
		if (timer < tapMaxTime && canTap && !SoldierAI.soldierTouched) {

			if (onTap != null)
				onTap ();

		}

		SoldierAI.soldierTouched = false;
	}

    #region swipe state
    public void Swipe_Start() {

		swiping = true;

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
					SwipeToDir(Direction.Right);
                }
                else
                {
					SwipeToDir(Direction.Left);

                }
            }
            else
            {
                if (diff.y > 0)
                {
					SwipeToDir(Direction.Up);

                }
                else
                {
					SwipeToDir(Direction.Down);

                }
            }
        }
    }

	void SwipeToDir ( Direction dir ) {



		onSwipe(dir);

		canTap = false;

		Swipe_Exit ();

	}

    void HandleOnInputExit()
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
			SwipeToDir(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
			SwipeToDir(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			SwipeToDir(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			SwipeToDir(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
			SwipeToDir(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
			SwipeToDir(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
			SwipeToDir(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
			SwipeToDir(Direction.Up);
        }
    }
}
