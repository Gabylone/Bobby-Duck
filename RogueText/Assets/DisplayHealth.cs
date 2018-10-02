using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHealth : TextTyper {

    // Use this for initialization
    public override void Start()
    {
        base.Start();

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
        string str = "" + Player.Instance.hunger + " / " + Player.Instance.maxHunger;

        if (Player.Instance.hunger < Player.Instance.maxHealth * 0.25f)
        {
            str = "";
        }
        else if (Player.Instance.hunger < Player.Instance.maxHealth * 0.5f)
        {
            str = "faiblissant";
        }
        else if (Player.Instance.hunger < Player.Instance.maxHealth * 0.75f)
        {
            str = "faible";
        }
        else if (Player.Instance.hunger < Player.Instance.maxHealth)
        {
            str = "mourrant";
        }

        Display(str);
    }
}
