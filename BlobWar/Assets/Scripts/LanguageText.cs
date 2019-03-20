using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour {

    Text uiText;

    public string[] texts = new string[2];

    private void OnEnable()
    {
        if (uiText == null)
        {
            uiText = GetComponent<Text>();


        }

        if ( Inventory.currentLanguageType == Inventory.LanguageType.None)
        {
            uiText.text = "NO LANGUAGE";
            return;
        }

        uiText.text = texts[(int)Inventory.currentLanguageType];
    }
}
