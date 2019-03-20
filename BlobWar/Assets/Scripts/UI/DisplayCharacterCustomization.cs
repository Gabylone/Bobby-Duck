using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCharacterCustomization : DisplayGroup {

    public static DisplayCharacterCustomization Instance;

    public DisplayGroup gridDisplayGroup;

    public ApparenceItemButton[] apparenceItemButtons;

    public BlobApparence blob_Apparence;

    public SoldierInfo _soldierInfo;

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

	public override void Update ()
	{
		base.Update ();
	}

    public override void Open()
    {
        base.Open();


        _soldierInfo = DisplayUpgradeSoldier.Instance.currentSoldierInfo;

        blob_Apparence.SetSpriteIDs(_soldierInfo.apparenceIDs);

        DisplayUpgradeSoldier.Instance.Close(false);

    }

    public void ShowGrid(BlobApparence.Type type)
    {
        gridDisplayGroup.Open();

        UpdateGrid(type);

    }

    public override void Close(bool b)
    {
        base.Close(b);

        gridDisplayGroup.Close(false);

        DisplaySoldiers.Instance.UpdateDisplay();
    }

    public void UpdateGrid(BlobApparence.Type type)
    {
        int a = 0;

        foreach (var apparenceItemButton in apparenceItemButtons)
        {
            apparenceItemButton.gameObject.SetActive(false);

            apparenceItemButton.id = a;

            ++a;
        }

        int l;

        if ( type == BlobApparence.Type.Body)
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
