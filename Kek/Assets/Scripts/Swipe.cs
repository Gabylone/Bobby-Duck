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
        onClick = null;
    }

    public delegate void OnClick ();
    public static OnClick onClick;

	Vector2 prevPoint;

	public Transform test;

	public float minimumDistance = 0.1f;

	public float minimumTime = 0.5f;

	bool swiping = false;
	bool swipped = false;

	float timer = 0f;

    public static bool canceled = false;

    public static void Cancel()
    {
        canceled = true;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start () {

        RayCatcher.onTouchRayCatcher += HandleOnInputDown;
        RayCatcher.onExitRayCatcher += HandleOnInputExit;
		//InputManager.onInputDown += HandleOnInputDown;
		//InputManager.onInputExit += HandleOnInputExit;
	}

	void HandleOnInputExit ()
	{
        if (canceled)
            return;

        Swipe_Exit ();
	}

	void HandleOnInputDown ()
	{
		Swipe_Start ();
	}
	
	// Update is called once per frame
	void Update () {

		if (swiping) {
			Swipe_Update ();
		}

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onClick();
        }

    }

	public void Swipe_Start() {

		swiping = true;
		timer = 0f;

		prevPoint = InputManager.Instance.GetInputPosition ();

	}

	void Swipe_Update() {

		float dis = Vector3.Distance ( prevPoint , InputManager.Instance.GetInputPosition() );

		if (dis > minimumDistance) {

            if (timer >= minimumTime)
            {
                Debug.Log("too slow : swipe canceled");
                return;
            }

			Vector2 dir = (Vector2)InputManager.Instance.GetInputPosition () - prevPoint;
			Direction direction = GetDirectionFromVector (dir);

//			print ("swipe : " + direction.ToString() );

			if (onSwipe != null ) {

				onSwipe (direction);

				swipped = true;

                //Debug.Log("swipe : " + direction);


            }

            Swipe_Exit ();
		}

		timer += Time.deltaTime;
	}

	public Direction GetDirectionFromVector ( Vector2 dir ) {

		for (int i = 0; i < 4; ++i ) {
			if ( Vector2.Angle ( GetDir( (Direction)i ) , dir ) < 45f ) {
				return (Direction)i;
			}
		}
		return Direction.None;
	}

	public Vector2 GetDir ( Direction dir ) {

		switch (dir) {
		case Direction.Up:
			return new Vector2 (0, 1);
		case Direction.Left:
			return new Vector2 (-1, 0);
		case Direction.Down:
			return new Vector2 (0, -1);
		case Direction.Right:
			return new Vector2 (1, 0);
		case Direction.None:
			return new Vector2 (0, 0);
		}

		return Vector2.zero;

	}

	void Swipe_Exit () {
		
		swiping = false;

        if (canceled)
            return;

        if (!swipped)
			onClick ();

		swipped = false;
	}
}
