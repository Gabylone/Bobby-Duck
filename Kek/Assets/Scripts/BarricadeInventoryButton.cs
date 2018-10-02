using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarricadeInventoryButton : InventoryButton {

    public GameObject prefab;

	public Text text;

    public delegate void OnClickBarricadeInventoryButton();
    public static OnClickBarricadeInventoryButton onClickBarricadeInventoryButton;

    // Use this for initialization
    void Start () {
		UpdateDisplay ();
	}

    private void OnDestroy()
    {
        onClickBarricadeInventoryButton = null;
    }

    void UpdateDisplay ()
	{
		text.text = "" + Inventory.Instance.barricadeCount;

		if (Inventory.Instance.barricadeCount == 0)
			Hide ();
	}

    public override void Activate()
    {
        base.Activate();

        GameObject barricade = Instantiate (prefab, ZoneManager.Instance.barricadeParent) as GameObject;

        Inventory.Instance.RemoveBarricade();

        if (onClickBarricadeInventoryButton != null)
        {
            onClickBarricadeInventoryButton();
        }

        UpdateDisplay ();
	}
}
