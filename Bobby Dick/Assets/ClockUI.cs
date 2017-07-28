using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : UiIcon {

	[SerializeField]
	private Transform hourNeedle;
//	[SerializeField]
//	private Transform minuteNeedle;

	[SerializeField]
	private Image nightImage;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		float angle = TimeManager.Instance.NightStartTime * 360f / TimeManager.Instance.DayDuration;

		nightImage.transform.eulerAngles = new Vector3 (0,0,180-angle);

		int nightDuration = (TimeManager.Instance.DayDuration - TimeManager.Instance.NightStartTime) + TimeManager.Instance.NightEndTime;

		nightImage.fillAmount = (float)nightDuration / (float)TimeManager.Instance.DayDuration;
	}

	public override void HandleChunkEvent ()
	{
		base.HandleChunkEvent ();

		UpdateUI ();
	}

	public override void UpdateUI ()
	{
		base.UpdateUI ();

		float angle = TimeManager.Instance.TimeOfDay * 360f / TimeManager.Instance.DayDuration;

		hourNeedle.eulerAngles = new Vector3 (0,0,-angle);
	}
}
