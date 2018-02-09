using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherFeedback : InfoFeedbacks {

	// Use this for initialization
	void Start () {
		TimeManager.onSetWeather += HandleOnSetWeather;

		QuestManager.onFinishQuest += HandleOnFinishQuest;
	}

	void HandleOnFinishQuest (Quest quest)
	{
		Print ("+" + quest.experience + " xp" , Color.blue);
	}

	void HandleOnSetWeather (TimeManager.WeatherType weatherType)
	{
		switch (weatherType) {
		case TimeManager.WeatherType.Day:
			Print ("Jour");
			break;
		case TimeManager.WeatherType.Night:
			Print ("Nuit");
			break;
		case TimeManager.WeatherType.Rain:
			Print ("Tempête");
			break;
		default:
			break;
		}
	}
}
