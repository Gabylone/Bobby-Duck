using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeatherManager : MonoBehaviour {

	public static WeatherManager Instance;

	[Header("Rain")]
	[SerializeField] private Image rainImage;

	bool raining = false;

	private int currentRain = 0;
	[SerializeField] private int rainRate = 20;
	[SerializeField] private int rainDuration = 5;

	[Header("Night")]
	[SerializeField] private Image nightImage;

	bool isNight = false;

	private int currentNight = 0;
	[SerializeField] private int nightRate = 8;
	[SerializeField] private int nightDuration = 4;

	[Header ("Sounds")]
	[SerializeField] private AudioClip rainSound;
	[SerializeField] private AudioClip daySound;
	[SerializeField] private AudioClip nightSound;

	void Awake () {
		Instance = this;
	}

	void Start () {
		PlaySound ();
	}

	public bool Raining {
		get {
			return raining;
		}
		set {
			raining = value;
			currentRain = 0;
			rainImage.gameObject.SetActive ( value );
			PlaySound ();
		}
	}

	public void UpdateWeather () {

		// rain image
		currentRain++;
		int r1 = Raining ? rainDuration : rainRate;
		if (currentRain == r1)
			Raining = !Raining;

		currentNight++;
		int r2 = IsNight ? nightDuration : nightRate;
		if (currentNight == r2)
			IsNight = !IsNight;

		BoatManager.Instance.BoatLightImage.gameObject.SetActive (Raining || IsNight);
	}
	public void PlaySound () {

		AudioClip ambiantClip;
		if (raining)
			ambiantClip = rainSound;
		else if (isNight)
			ambiantClip = nightSound;
		else
			ambiantClip = daySound;

		SoundManager.Instance.PlayAmbiance (ambiantClip);
	}

	public bool IsNight {
		get {
			return isNight;
		}
		set {
			isNight = value;
			currentNight = 0;
			nightImage.gameObject.SetActive (value);
			PlaySound ();
		}
	}
}
