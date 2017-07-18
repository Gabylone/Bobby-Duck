using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public static TimeManager Instance;

	[Header("Rain")]
	[SerializeField] private Image rainImage;

	bool raining = false;

	int timeOfDay = 0;
	int dayDuration = 24;

	private int currentRain = 0;
	[SerializeField] private int rainRate = 20;
	[SerializeField] private int rainDuration = 5;

	[Header("Night")]
	[SerializeField] private Image nightImage;

	bool isNight = false;

	[SerializeField] private int nightStartTime = 21;
	[SerializeField] private int nightEndTime = 4;

	void Awake () {
		Instance = this;
	}

	void Start () {
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
			if (timeOfDay == nightStartTime) {
				IsNight = true;
			}
		} else {
			if (timeOfDay == nightEndTime) {
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
}
