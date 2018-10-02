using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldDisplay : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {

        InventoryManager.onAddGold += UpdateDisplay;
        InventoryManager.onRemoveGold += UpdateDisplay;

        UpdateDisplay();
	}
    private void OnDestroy()
    {
        InventoryManager.onAddGold -= UpdateDisplay;
        InventoryManager.onRemoveGold -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        Tween.Bounce(transform);
        
        text.text = "" + Inventory.Instance.gold;
    }
}
