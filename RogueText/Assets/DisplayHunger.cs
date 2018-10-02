using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHunger : TextTyper {

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
        if (action.type == Action.Type.Eat)
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
        Invoke("UpdateUIDelay", 0.0001f);
    }

    void UpdateUIDelay()
    {
        string str = "";

        if (Player.Instance.hunger == 0)
        {
            str = "";
        }
        else if (Player.Instance.hunger == 1)
        {
            str = "lègere faim";
        }
        else if (Player.Instance.hunger == 2)
        {
            str = "faim";
        }
        else{
            str = "affamé";
        }

        Display(str);
    }
}
