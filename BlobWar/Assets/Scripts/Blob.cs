using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Blob : Placable
{
    public Animator animator;

    public bool sliding = false;
    public bool moving = false;
    public float moveDuration = 0.3f;
    public float moveRate = 0.2f;

    public AudioSource audioSource;

    public AudioSource movementAudioSource;

    public Transform bodyTransform;

    public bool blocked = false;

	public EaseType move_EaseType = EaseType.EaseInOutCirc;

	public BlobApparence_SpriteRenderer apparence;

	public delegate void UpdateState();
	public UpdateState updateState;

	public float timeInState = 0f;

    public override void Start ()
	{
		base.Start ();

		animator = GetComponentInChildren<Animator>();

		apparence = GetComponentInChildren<BlobApparence_SpriteRenderer> ();

	}

	public override void Update ()
	{
		base.Update ();

		if (updateState != null)
		{
			updateState();

			timeInState += Time.deltaTime;
		}
	}

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
		TurnToDirection (direction);

		SoundManager.Instance.Play (movementAudioSource, SoundManager.SoundType.Slide);

		animator.SetTrigger ("move");

		if (direction == Swipe.Direction.Left)
			currentLignIndex--;
		else if (direction == Swipe.Direction.Right)
			currentLignIndex++;

		currentLignIndex = Mathf.Clamp (currentLignIndex, 0, LignManager.Instance.ligns.Length-1);

        sliding = true;

        Invoke("MoveDelay", moveDuration);
        
    }

    public virtual void MoveDelay()
    {
        sliding = false;
    }

}
