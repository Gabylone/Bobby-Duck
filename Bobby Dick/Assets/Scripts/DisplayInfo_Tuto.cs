using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInfo_Tuto : DisplayInfo {

	public override void Start ()
	{
		base.Start ();

		if (KeepOnLoad.displayTuto) {

			Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
			Tutorial.onWaitForConfirm += HandleOnWaitForConfirm;
			Tutorial.onHideTutorial += HandleOnHideTutorial;

		}
	}

	void HandleOnHideTutorial ()
	{
		Fade ();
	}

	void HandleOnWaitForConfirm ()
	{
		confirmGroup.SetActive (true);
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		Display (tutoStep.title, NameGeneration.CheckForKeyWords(tutoStep.description));

		Move (tutoStep.corner);

	}

	public override void Confirm ()
	{
		base.Confirm ();

		if (Tutorial.onHideTutorial != null) {
			Tutorial.onHideTutorial ();
		}

	}
}
