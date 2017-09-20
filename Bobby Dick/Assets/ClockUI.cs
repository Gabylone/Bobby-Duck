using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		CrewInventory.Instance.openInventory += HandleOpenInventory;

		InitClock ();
	}

	void HandleOpenInventory (CrewMember member)
	{
		UpdateNeedle ();
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
		float angle = TimeManager.Instance.TimeOfDay * 360f / TimeManager.Instance.DayDuration;

		hourNeedle.eulerAngles = new Vector3 (0,0,-angle);
	}
}
