using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSound : MonoBehaviour {

	[SerializeField]
	AudioClip[] interactClips;

	[SerializeField]
	AudioClip[] enterTriggerClips;

	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		Interactable.onEnterInteractable += HandleOnEnterInteractable;
		Interactable.onInteract += HandleOnInteract;
	}

	void HandleOnInteract ()
	{
		audioSource.clip = interactClips [Random.Range (0, interactClips.Length)];
		audioSource.Play ();
	}

	void HandleOnEnterInteractable (Transform target)
	{
		audioSource.clip = enterTriggerClips [Random.Range (0, interactClips.Length)];
		audioSource.Play ();
	}
}
