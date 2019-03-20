using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayConfirm : DisplayGroup {

	public static DisplayConfirm Instance;

	public delegate void OnConfirm ();
	public OnConfirm onConfirm;

	void Awake () {
		Instance = this;
	}

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Return ()
	{
		base.Return ();
	}

	public override void Open ()
	{
		base.Open ();

		CancelInvoke ("StopTime");
		MainMenu.Instance.CancelInvoke ("StopTime");
	}

	public override void Close (bool b)
	{
		base.Close (b);

		onConfirm = null;

		if ( stopTime ) {
			CancelInvoke ("StopTime");
			Invoke ("StopTime", tweenDuration*2f);
		}
	}

	void StopTime () {
		Time.timeScale = 0f;
	}

	public void Confirm(){

		if (onConfirm != null) {
			onConfirm ();
		}

		//Close ();

	}

}
