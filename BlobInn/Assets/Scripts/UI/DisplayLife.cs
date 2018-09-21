using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLife : MonoBehaviour {

    public Text uiText;

    public RectTransform layoutGroup;

    // Use this for initialization
    void Start()
    {
        Inventory.Instance.onChanceLife += HandleOnChangeLife;

        UpdateUI();

    }

    private void HandleOnChangeLife()
    {
        UpdateUI();
        Tween.Bounce(transform);
    }

    void UpdateUI()
    {

        uiText.text = "" + Inventory.Instance.lifes;

        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);

    }
}
