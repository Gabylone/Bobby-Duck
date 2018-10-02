using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyBarricadeButton : MonoBehaviour {

    public Text text;

    public delegate void OnClickBuyBarricadeButton();
    public static OnClickBuyBarricadeButton onClickBuyBarricadeButton;
    private void OnDestroy()
    {
        onClickBuyBarricadeButton = null;
        Inventory.onAddBarricade -= UpdateDisplay;
    }

    private void Start()
    {
		UpdateDisplay ();

        Inventory.onAddBarricade += UpdateDisplay;
    }
    

    public void OnClick()
    {
        if (onClickBuyBarricadeButton != null)
            onClickBuyBarricadeButton();

        Sound.Instance.PlaySound(Sound.Type.Menu1);

        Tween.Bounce( transform );
    }

    void UpdateDisplay()
    {
        text.text = "" + Inventory.Instance.barricadeCount;
    }
}
