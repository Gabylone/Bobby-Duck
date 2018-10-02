using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayThirst : TextTyper {

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        Player.onPlayerMove += HandleOnPlayerMove;

        UpdateUI();

        ActionManager.onAction += HandleOnAction;

    }

    private void HandleOnAction(Action action)
    {
        if ( action.type == Action.Type.Drink)
        {
            UpdateUI();
        }
    }

    private void HandleOnPlayerMove(Coords prevCoords, Coords newCoords)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        Invoke("UpdateUIDelay",0.0001f);
    }

    void UpdateUIDelay() {
        string str = "";

        if (Player.Instance.thirst == 0)
        {
            str = "";
        }
        else if (Player.Instance.thirst == 1)
        {
            str = "lègere soif";
        }
        else if (Player.Instance.thirst == 2)
        {
            str = "soif";
        }
        else if (Player.Instance.thirst == 3)
        {
            str = "assoifé";
        }

        Display(str);
    }
}
