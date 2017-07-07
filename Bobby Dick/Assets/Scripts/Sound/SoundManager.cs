using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public enum Sound {
		Select_Small,
		Select_Big,
	}

	public static SoundManager Instance;

	[SerializeField]
	private AudioSource soundSource;

	[SerializeField]
	private AudioSource ambianceSource;

	[SerializeField]
	private AudioClip[] clips;

	bool enableSound = true;

	void Start () {
		EnableSound = true;
	}

	void Awake () {
		Instance = this;
	}

	public void PlayRandomSound (AudioClip[] clips) {
		PlaySound (clips [Random.Range (0, clips.Length)]);
	}

	public void PlaySound ( Sound sound ) {
		PlaySound (clips [(int)sound]);
	}

	public void PlaySound ( AudioClip clip ) {

		if ( clip == null ) {
			Debug.LogError ("unassigned clip");
			return;
		}

		soundSource.clip = clip;
		soundSource.Play ();
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

	[SerializeField]
	private Image soundImage;
	[SerializeField]
	private Sprite sprite_SoundOn;

	[SerializeField]
	private Sprite sprite_SoundOff;

	public void SwitchEnableSound () {
		EnableSound = !EnableSound;
	}

	public bool EnableSound {
		get {
			return enableSound;
		}
		set {
			enableSound = value;

			soundSource.enabled = value;
			ambianceSource.enabled = value;
			if ( value ) {
				ambianceSource.Play ();
			}

			soundImage.sprite = value ? sprite_SoundOn : sprite_SoundOff;
		}
	}
}
