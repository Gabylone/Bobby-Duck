using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLifePurchase : DisplayGroup {

    
    public GameObject maxGroup;

    public override void Start()
    {
        base.Start();

        Inventory.Instance.onChanceLife += UpdateUI;
    }

	public override void Update ()
	{
		base.Update ();
	}


    public override void Open()
    {
        base.Open();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (Inventory.Instance.lifes >= 10)
        {
            maxGroup.SetActive(true);
        }
        else
        {
            maxGroup.SetActive(false);
        }
    }

}
