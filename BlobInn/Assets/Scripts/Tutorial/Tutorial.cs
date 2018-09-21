using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class Tutorial : MonoBehaviour {

    public static Tutorial Instance;

    public static bool show = false;

    public static int count = 0;

    public TextAsset tutoData;

    public bool debugTutorial = true;

    public delegate void OnDisplayTutorial(TutoInfo tutoStep);
    public static OnDisplayTutorial onDisplayTutorial;

    public delegate void OnHideTutorial();
    public static OnHideTutorial onHideTutorial;

    public delegate void OnWaitForConfirm();
    public static OnWaitForConfirm onWaitForConfirm;

    public static List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutoInfo[] tutoInfos;

    private void Awake()
    {
        Instance = this;
    }

    void Start() {
        if (debugTutorial)
        {
            show = true;
        }

        InitTutorials();
    }

    void LoadData()
    {
        string[] rows = tutoData.text.Split('&');

        int tutoIndex = 0;

        for (int i = 0; i < rows.Length - 1; i++) {


            string row = rows[i].TrimStart('\r', '\n');

            string[] cells = row.Split(';');

            tutoInfos[tutoIndex].title = cells[0];
            tutoInfos[tutoIndex].description = cells[1];

            ++tutoIndex;
        }
    }

    public void Show(TutorialStep tutorialStep, Transform focusTransform) {

        if (show == false)
        {
            return;
        }

        if (tutorialSteps.Contains(tutorialStep))
        {
            return;
        }

        tutorialSteps.Add(tutorialStep);

        if ( focusTransform != null)
        {
            DisplayInfo_Tuto.Instance.SetFocus(focusTransform);
        }

        DisplayInfo_Tuto.Instance.Display( tutoInfos[(int)tutorialStep] );

    }
    public void Show(TutorialStep tutorialStep)
    {
        Show(tutorialStep, null);
    }

	void InitTutorials ()
	{
		int stepCount = System.Enum.GetValues (typeof(TutorialStep)).Length;

		tutoInfos = new TutoInfo[stepCount];

        for (int i = 0; i < stepCount; i++)
        {
            TutoInfo newTutoStep = new TutoInfo();

            newTutoStep.Init();
            newTutoStep.step = (TutorialStep)i;

            tutoInfos[i] = newTutoStep;
        }

		LoadData ();

	}

}

public enum TutorialStep {

	Movement,
    Client,
    Order,
    Ingredients,
    EmptyPlate,
    Service,
    Gold,
    Patience1,
    Patience2,
    Map,
    Tables,
    Plates,
    NewIngredients,
    Waiters,
    NewClients

}

public class TutoInfo {

	public TutorialStep step;

	public string title = "";

	public string description = "";

	public void Display () {
		//
		Tutorial.onDisplayTutorial ( this );

        Tutorial.count++;

        Debug.Log("displaying : " + title);

	}

	public virtual void Init () {

	}

	public void WaitForConfirm () {
		Tutorial.onWaitForConfirm ();


    }
}