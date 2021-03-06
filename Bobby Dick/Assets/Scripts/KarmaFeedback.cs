﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KarmaFeedback : InfoFeedbacks {

    public static KarmaFeedback Instance;

    public Color bestColor;
    public Color goodColor;
    public Color neutralColor;
    public Color badColor;
    public Color worstColor;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start ()
	{
		base.Start ();

		Karma.onChangeKarma += HandleOnChangeKarma;
	}

	public void HandleOnChangeKarma (int previousKarma, int newKarma)
	{
        DisplayKarma();
    }

    public void DisplayKarma()
    {
        switch (Karma.Instance.karmaStep)
        {
            case Karma.KarmaStep.Best:
                KarmaFeedback.Instance.Print("Angélique", bestColor);
                break;
            case Karma.KarmaStep.Good:
                KarmaFeedback.Instance.Print("Bon : " + Karma.Instance.CurrentKarma + " / " + Karma.Instance.maxKarma, goodColor);
                break;
            case Karma.KarmaStep.Neutral:
                KarmaFeedback.Instance.Print("Neutre" , neutralColor );
                break;
            case Karma.KarmaStep.Bad:
                KarmaFeedback.Instance.Print("Mauvais : " + -Karma.Instance.CurrentKarma + " / " + Karma.Instance.maxKarma, badColor);
                break;
            case Karma.KarmaStep.Worst:
                KarmaFeedback.Instance.Print("Maléfique" , worstColor);
                break;
        }
    }
}
