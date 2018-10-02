using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInvitation : DisplayGroup {

    public static DisplayInvitation Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Open()
    {
        base.Open();

        Inventory.Instance.displayedInvitation = true;

        Inventory.Instance.Save();
    }

    public void RateGame()
    {
        Application.OpenURL("market://details?id=" + Application.productName);

        Close();
    }

    public override void Close(bool _showBottomBar)
    {
        base.Close(_showBottomBar);

        DisplayLevel.Instance.HandleOnPointerClick(10);

    }


}
