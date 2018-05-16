using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPearls : MonoBehaviour {

    public Text uiText;

    public GameObject group;

    // Use this for initialization
    void Start () {

        UpdateUI();

        CrewInventory.Instance.openInventory += HandleOnOpenInventory;
        CrewInventory.Instance.closeInventory += HandleOnCloseInventory;

        PlayerInfo.onChangePearlAmount += UpdateUI;

	}

    private void HandleOnCloseInventory()
    {
        Hide();
    }

    private void HandleOnOpenInventory(CrewMember member)
    {
        Show();
    }

    private void UpdateUI()
    {
        uiText.text = "" + PlayerInfo.Instance.pearlAmount;
    }

    public void Show()
    {
        group.SetActive(true);
    }

    public void Hide()
    {
        group.SetActive(false);
    }
}
