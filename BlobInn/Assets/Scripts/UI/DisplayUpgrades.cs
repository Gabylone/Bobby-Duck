using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrades : DisplayGroup {

    public static DisplayUpgrades Instance;

    public GameObject tavernButton;
    public GameObject blobButton;

    public GameObject[] upgradeScrollViews;

    public Color boughtColor;
    public Color cantAffordColor;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        int i = 0;

        UpgradeButton.Type type = UpgradeButton.Type.Table;

        foreach (var item in GetComponentsInChildren<UpgradeButton>(true))
        {
            if (item.type != type)
            {
                i = 0;
            }


            item.id = i;

            type = item.type;


            ++i;
        }
    }

    void ShowDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Tables, DisplayUpgrades.Instance.upgradeScrollViews[0].transform);
    }

    public override void Open()
    {
        base.Open();
        UpdateUI();

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Open);

        Invoke("ShowDelay", 1f);
    }

    public void UpdateUI()
    {


        foreach (var item in GetComponentsInChildren<UpgradeButton>())
        {
            item.UpdateUI();
        }

    }

    public override void Close(bool b)
    {
        base.Close(b);

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Close);
    }
}
