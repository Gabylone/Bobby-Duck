using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSound : MonoBehaviour {

	public AudioSource audioSource_fx;
	[SerializeField] AudioClip dieClip;
	[SerializeField] AudioClip kekClip;
	[SerializeField] AudioClip[] flapClips;

	public void Flap() {
		audioSource_fx.clip = flapClips [Random.Range (0, flapClips.Length)];
		audioSource_fx.Play ();
	}

	public void Kek () {
		audioSource_fx.clip = kekClip;
		audioSource_fx.Play ();
	}

	public void Die () {
		audioSource_fx.clip = dieClip;
		audioSource_fx.Play ();
	}
}
