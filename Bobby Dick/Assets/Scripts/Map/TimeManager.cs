using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public static TimeManager Instance;

	[Range(0,24)]
	[SerializeField]
	private int startTime = 6;
	private int timeOfDay = 0;
	private int dayDuration = 24;

	private int currentRain = 0;
	[SerializeField]
	private int rainRate = 55;
	[SerializeField]
	private int rainDuration = 10;

	[Header("Night")]
	[SerializeField]
	private Image nightImage;

	private bool isNight = false;

	[SerializeField]
	private int nightStartTime = 21;
	[SerializeField]
	private int nightEndTime = 4;

	[Header("Rain")]
	[SerializeField] private Image rainImage;
	private bool raining = false;

	void Awake () {
		Instance = this;
	}

	public void Init () {
		timeOfDay = startTime;
		NavigationManager.Instance.EnterNewChunk += AdvanceTime;
	}

	public bool Raining {
		get {
			return raining;
		}
		set {
			raining = value;
			currentRain = 0;
			rainImage.gameObject.SetActive ( value );
		}
	}

	public void AdvanceTime () {

		++timeOfDay;
		if (timeOfDay == dayDuration)
			timeOfDay = 0;

		if (IsNight == false) {
			if (TimeOfDay >= NightStartTime) {
				IsNight = true;
			} else if (TimeOfDay < nightEndTime) {
				IsNight = true;
			}
		} else {
			if (timeOfDay < 12 && timeOfDay >= nightEndTime) {
				IsNight = false;
			}
		}

		// rain image
		currentRain++;
		int r1 = Raining ? rainDuration : rainRate;
		if (currentRain == r1)
			Raining = !Raining;


	}

	public void SaveWeather () {
		SaveManager.Instance.CurrentData.raining = Raining;
		SaveManager.Instance.CurrentData.night = IsNight;
		SaveManager.Instance.CurrentData.timeOfDay = timeOfDay;
		SaveManager.Instance.CurrentData.currentRain = currentRain;
	}

	public void LoadWeather () {
		Raining = SaveManager.Instance.CurrentData.raining;
		IsNight = SaveManager.Instance.CurrentData.night;
		timeOfDay = SaveManager.Instance.CurrentData.timeOfDay;
		currentRain = SaveManager.Instance.CurrentData.currentRain;
	}

	public bool IsNight {
		get {
			return isNight;
		}
		set {
			isNight = value;
			nightImage.gameObject.SetActive (value);
		}
	}

	public int TimeOfDay {
		get {
			return timeOfDay;
		}
	}

	public int DayDuration {
		get {
			return dayDuration;
		}
	}

	public int NightStartTime {
		get {
			return nightStartTime;
		}
	}

	public int NightEndTime {
		get {
			return nightEndTime;
		}
	}
}
