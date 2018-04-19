using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

	public AudioSource audioSource_fx;
	public AudioSource audioSource_Steps;
	[SerializeField] AudioClip landClip;
	[SerializeField] AudioClip jumpClip;
	[SerializeField] AudioClip doubleJumpClip;
	[SerializeField] AudioClip[] stepClips;
	[SerializeField] AudioClip pickUpClip;

	Character character;

	// Use this for initialization
	void Start () {
		
		character = GetComponentInParent<Character> ();

		character.onJump += HandleOnJump;

		character.onTouchGround += HandleOnTouchGround;

	}

	void Update () {
		//
	}

	void HandleOnTouchGround ()
	{
		audioSource_fx.clip = landClip;
		audioSource_fx.Play ();
	}

	void HandleOnJump ()
	{
		audioSource_fx.clip = jumpClip;
		audioSource_fx.Play ();
	}

	public void Step () {
		audioSource_Steps.clip = stepClips [Random.Range (0, stepClips.Length)];
		audioSource_Steps.Play();
	}

	public void PickUp() {
		audioSource_fx.clip = pickUpClip;
		audioSource_fx.Play ();
	}
}
