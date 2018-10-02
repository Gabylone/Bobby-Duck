using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDays : TextTyper {

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        TimeManager.Instance.onNextDay += HandleOnNextDay;

        UpdateUI();
    }

    private void HandleOnNextDay()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        Display("j:" + TimeManager.Instance.daysPasted);
    }
}
