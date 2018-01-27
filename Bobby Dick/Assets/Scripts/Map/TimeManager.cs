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
	private int rainRate_Min = 75;
	private int rainRate_Max = 130;
	private int rainRate = 0;
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
	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
		NavigationManager.Instance.EnterNewChunk += AdvanceTime;

		SetRainRate ();
	}



	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.ChangeTimeOfDay:
			if ( IsNight )
				StartCoroutine (SetWeatherCoroutine ("Day"));
			else
				StartCoroutine (SetWeatherCoroutine ("Night"));
			break;
		case FunctionType.SetWeather:
			StartCoroutine (SetWeatherCoroutine (cellParameters));
			break;
		case FunctionType.CheckDay:
			StoryReader.Instance.NextCell ();

			if (TimeManager.Instance.IsNight)
				StoryReader.Instance.SetDecal (1);

			StoryReader.Instance.UpdateStory ();
			break;
		}
	}

	public void Reset () {
		timeOfDay = startTime;
	}

	public bool Raining {
		get {
			return raining;
		}
		set {
			raining = value;
			currentRain = 0;
			SetRainRate ();
			rainImage.gameObject.SetActive ( value );
		}
	}
	void SetRainRate ()
	{
		rainRate = Random.Range ( rainRate_Min , rainRate_Max );
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
		SaveManager.Instance.GameData.raining = Raining;
		SaveManager.Instance.GameData.night = IsNight;
		SaveManager.Instance.GameData.timeOfDay = timeOfDay;
		SaveManager.Instance.GameData.currentRain = currentRain;
	}

	public void Load () {
		Raining = SaveManager.Instance.GameData.raining;
		IsNight = SaveManager.Instance.GameData.night;
		timeOfDay = SaveManager.Instance.GameData.timeOfDay;
		currentRain = SaveManager.Instance.GameData.currentRain;
	}

	IEnumerator SetWeatherCoroutine (string weather) {

		Transitions.Instance.FadeScreen ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		switch ( weather ) {
		case "Day":
			TimeManager.Instance.IsNight = false;
			TimeManager.Instance.Raining = false;
			break;
		case "Night":
			TimeManager.Instance.IsNight = true;
			TimeManager.Instance.Raining = false;
			break;
		case "Rain":
			TimeManager.Instance.Raining = true;
			break;
		}

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public bool IsNight {
		get {
			return isNight;
		}
		set {
			isNight = value;
			nightImage.gameObject.SetActive (value);

			if (isNight) {
				timeOfDay = nightStartTime;
			} else {
				timeOfDay = startTime;
			}
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
