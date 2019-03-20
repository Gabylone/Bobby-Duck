using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarricadeInventoryButton : InventoryButton {

    public static BarricadeInventoryButton Instance;

	public Transform barricadeParent;

    public GameObject prefab;

    public Text text;

    public bool upgrade = false;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        UpdateDisplay();
    }

    public void UpdateDisplay ()
	{
		text.text = "" + Inventory.Instance.barricadeAmount;

		if (Inventory.Instance.barricadeAmount == 0)
			Hide ();
	}

    public override void Activate()
    {
        base.Activate();

        Tween.Bounce(transform);

        if ( upgrade)
        {
            DisplayPurchase_Barricade.Instance.Open();
        }
        else
        {
            GameObject barricade = Instantiate(prefab, barricadeParent) as GameObject;
            barricade.GetComponent<Barricade>().PlaceStart();

            Inventory.Instance.RemoveBarricade();

            UpdateDisplay();

            DisplayCharge.Instance.ResetCharge();
        }

		
	}
}
