﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo_Tuto : DisplayInfo {

    public static DisplayInfo_Tuto Instance;

    public Corner corner = Corner.BottomLeft;

    public GameObject handObj;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start ()
	{
		base.Start ();

		if (KeepOnLoad.displayTuto) {

			Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
			Tutorial.onWaitForConfirm += HandleOnWaitForConfirm;
			Tutorial.onHideTutorial += HandleOnHideTutorial;

		}

        handObj.SetActive(false);
	}

    void HandleOnHideTutorial ()
	{
		Fade ();
	}

	void HandleOnWaitForConfirm ()
	{
        StoryInput.Instance.Lock();

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

        StoryInput.Instance.Unlock();

        Tutorial.Instance.ExitCurrent();

        //StoryInput.Instance.CancelInvoke("Unlock");
        


    }
}
