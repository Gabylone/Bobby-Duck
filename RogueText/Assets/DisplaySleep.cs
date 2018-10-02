using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySleep : TextTyper {

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
        if (action.type == Action.Type.Sleep)
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
        string str = "" + Player.Instance.sleep + " / " + Player.Instance.maxSleep;

        if (Player.Instance.sleep < Player.Instance.maxSleep * 0.3f)
        {
            str = "";
        }
        else if (Player.Instance.sleep < Player.Instance.maxSleep * 0.5f)
        {
            str = "légere fatigue";
        }
        else if (Player.Instance.sleep < Player.Instance.maxSleep * 0.7f)
        {
            str = "fatigue";
        }
        else if (Player.Instance.sleep < Player.Instance.maxSleep)
        {
            str = "épuisé";
        }

        Display(str);
    }
}
