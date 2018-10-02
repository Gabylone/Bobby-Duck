using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimator : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		GetComponentInParent<Soldier> ().onChangeState += HandleOnChangeState;
		animator = GetComponentInChildren<Animator> ();
	}

	void HandleOnChangeState (Humanoid.State newState, Humanoid.State prevState)
	{
		switch (newState) {
		case Humanoid.State.Idle:
			animator.SetBool ("Aiming", false);
			break;
		case Humanoid.State.Moving:
			animator.SetFloat ("Move", 1f);
			break;
		case Humanoid.State.Shooting:

			if (prevState == Humanoid.State.Shooting) {
			}

			break;
		default:
			break;
		}
	}
}
