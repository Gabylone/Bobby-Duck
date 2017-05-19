using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance;

	[SerializeField]
	private AudioSource[] soundSource;

	[SerializeField]
	private AudioSource ambianceSource;

	void Awake () {
		Instance = this;
	}

	public void PlayRandomSound (AudioClip[] clips) {
		PlaySound (clips [Random.Range (0, clips.Length)]);
	}

	public void PlaySound ( AudioClip clip ) {

		if ( clip == null ) {
			Debug.LogError ("unassigned clip");
			return;
		}

		int sourceIndex = 0;

		if (soundSource [sourceIndex].isPlaying)
			sourceIndex++;

		if (sourceIndex < soundSource.Length) {
			soundSource[sourceIndex].clip = clip;
			soundSource[sourceIndex].Play ();
		} else {
			Debug.Log ("too mush sound");
		}
	}

	public void PlayAmbiance ( AudioClip clip ) {
		ambianceSource.clip = clip;
		ambianceSource.Play ();
	}

	public AudioSource AmbianceSource {
		get {
			return ambianceSource;
		}
	}
}
