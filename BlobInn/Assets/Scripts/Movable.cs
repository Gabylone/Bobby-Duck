using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Movable : Tilable
{
    public Animator animator;

    public bool sliding = false;
    public bool moving = false;
    public float moveDuration = 0.3f;
    public float moveRate = 0.2f;

    public AudioSource audioSource;

    public AudioSource movementAudioSource;

    public Transform bodyTransform;
    public Transform _transform;

    public bool blocked = false;

    public override void Start()
    {
        _transform = GetComponent<Transform>();

        base.Start();

        animator = GetComponentInChildren<Animator>();

    }
    public EaseType move_EaseType = EaseType.EaseInOutCirc;

    public void TurnToDirection(Swipe.Direction direction)
    {
        if (direction == Swipe.Direction.Left)
        {
            bodyTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Swipe.Direction.Right)
        {
            bodyTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public virtual void Move(Swipe.Direction direction)
    {
        TurnToDirection(direction);

        SoundManager.Instance.Play(movementAudioSource, SoundManager.SoundType.Slide);

        coords = GetTargetCoords(direction);

        animator.SetTrigger("move");

        Vector3 dir = GetTargetDirection(direction);

        HOTween.To(_transform, moveDuration, "position", _transform.position + dir, false, move_EaseType, 0f);

        sliding = true;

        Invoke("MoveDelay", moveDuration);

        
    }

    public virtual void MoveDelay()
    {
        sliding = false;
    }

    public Vector3 GetTargetDirection(Swipe.Direction direction)
    {
        Vector3 dir = Vector3.zero;

        switch (direction)
        {
            case Swipe.Direction.Left:
                dir = -Vector3.right;
                break;
            case Swipe.Direction.Right:
                dir = Vector3.right;
                break;
            case Swipe.Direction.Up:
                dir = Vector3.up;
                break;
            case Swipe.Direction.Down:
                dir = -Vector3.up;
                break;
            default:
                break;
        }

        return dir;
    }


    public Coords GetTargetCoords(Swipe.Direction direction)
    {
        Vector3 dir = GetTargetDirection(direction);

        return new Coords(coords.x + (int)dir.x, coords.y + (int)dir.y);
    }

    #region movement
    public IEnumerator GoToCoordsCoroutine(List<Coords> targetCoords)
    {
        if (moving)
        {
            yield break;
        }

        moving = true;


        foreach (var targetCoord in targetCoords)
        {
            Swipe.Direction horizontalDirection = targetCoord.x > coords.x ? Swipe.Direction.Right : Swipe.Direction.Left;

            while (coords.x != targetCoord.x)
            {
                Move(horizontalDirection);

                if (blocked)
                {
                    blocked = false;
                    break;
                }

                yield return new WaitForSeconds(moveDuration + moveRate);
            }

            if (blocked)
                break;

            Swipe.Direction verticalDirection = targetCoord.y > coords.y ? Swipe.Direction.Up : Swipe.Direction.Down;

            while (coords.y != targetCoord.y)
            {
                Move(verticalDirection);
                if ( blocked )
                {
                    blocked = false;
                    break;
                }

                yield return new WaitForSeconds(moveDuration + moveRate);

            }

        }

        yield return new WaitForSeconds(moveRate);

        ReachTargetCoords();
    }
    public virtual void ReachTargetCoords()
    {
        moving = false;
    }

    public void GoToCoords ( Coords targetCoords)
    {
        List<Coords> tmpCoords = new List<Coords>();
        tmpCoords.Add(targetCoords);

        StartCoroutine(GoToCoordsCoroutine(tmpCoords));
    }
    public void GoToCoords ( Coords c1, Coords c2)
    {
        List<Coords> tmpCoords = new List<Coords>();
        tmpCoords.Add(c1);
        tmpCoords.Add(c2);

        StartCoroutine(GoToCoordsCoroutine(tmpCoords));
    }
    public void GoToCoords (List<Coords> targetCoords)
    {
        StartCoroutine(GoToCoordsCoroutine(targetCoords));
    }
    
    #endregion
}
