using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class ClockUI : MonoBehaviour {

	[SerializeField]
	private Transform hourNeedle;
//	[SerializeField]
//	private Transform minuteNeedle;

	[SerializeField]
	private Image nightImage;

	// Use this for initialization
	void Start ()
	{
//		CrewInventory.Instance.openInventory += HandleOpenInventory;

		NavigationManager.Instance.EnterNewChunk += UpdateNeedle;
		StoryFunctions.Instance.getFunction += HandleGetFunction;

		InitClock ();
		UpdateNeedle ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.ChangeTimeOfDay:
		case FunctionType.SetWeather:
			UpdateNeedle ();
			break;
		}
	}

	void InitClock ()
	{
		float angle = (float)TimeManager.Instance.NightStartTime * 360f / (float)TimeManager.Instance.DayDuration;

		nightImage.transform.eulerAngles = new Vector3 (0,0,180-angle);

		int nightDuration = (TimeManager.Instance.DayDuration - TimeManager.Instance.NightStartTime) + TimeManager.Instance.NightEndTime;

		nightImage.fillAmount = (float)nightDuration / (float)TimeManager.Instance.DayDuration;
	}


	void UpdateNeedle ()
	{
		Invoke ("UpdateNeedleDelay", 0.01f);
	}
	void UpdateNeedleDelay ()
	{
		float angle = TimeManager.Instance.TimeOfDay * 360f / TimeManager.Instance.DayDuration;

		Vector3 targetAngles = new Vector3 (0,0,-angle);

		HOTween.To ( hourNeedle , 0.5f , "eulerAngles" , targetAngles , false , EaseType.EaseOutBounce , 0f );
	}
}
