using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCharacterCustomization : DisplayGroup {

    public static DisplayCharacterCustomization Instance;

    public DisplayGroup gridDisplayGroup;

    public ApparenceItemButton[] apparenceItemButtons;

    public Blob_Apparence blob_Apparence;

    public float gridBounceAmount = 1.05f;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        apparenceItemButtons = gridDisplayGroup.GetComponentsInChildren<ApparenceItemButton>(true);

        HideGrid();
    }

    public override void Open()
    {
        //DisplayUpgrades.Instance.Close(false);
        base.Open();

        blob_Apparence.LoadFromInventory();
    }

    public void ShowGrid(Blob_Apparence.Type type)
    {
        gridDisplayGroup.Open();

        UpdateGrid(type);

    }

    public override void Close(bool b)
    {
        base.Close(b);

        gridDisplayGroup.Close(false);
    }

    public void UpdateGrid(Blob_Apparence.Type type)
    {
        int a = 0;

        foreach (var apparenceItemButton in apparenceItemButtons)
        {
            apparenceItemButton.gameObject.SetActive(false);

            apparenceItemButton.id = a;

            ++a;
        }

        int l;

        if ( type == Blob_Apparence.Type.Color)
        {
            l = BlobApparenceManager.Instance.blobColors.Length;
        }
        else
        {
            l = BlobApparenceManager.Instance.sprites[(int)type].Length;
        }

        for (int spriteIndex = 0; spriteIndex < l; spriteIndex++)
        {
            apparenceItemButtons[spriteIndex].gameObject.SetActive(true);
            apparenceItemButtons[spriteIndex].UpdateSprite(type, spriteIndex);
        }
    }

    public void HideGrid()
    {
        gridDisplayGroup.Hide();
    }
}
