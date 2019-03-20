using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayGold : MonoBehaviour {

    public Text uiText;

    public RectTransform layoutGroup;

	// Use this for initialization
	void Start () {

        Inventory.Instance.onChangeGold += HandleOnChangeGold;

        UpdateUI();

	}

    private void HandleOnChangeGold()
    {
        Tween.Bounce(transform);
        UpdateUI();
    }

    void UpdateUI () {

        uiText.text = "" + Inventory.Instance.gold;

        if ( layoutGroup != null)
        {

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
        }


    }
}
