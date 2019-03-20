using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySoldierPurchase : DisplayGroup {

    public static DisplaySoldierPurchase Instance;

    public Text titleText;

    public Image image;

    public Image goldImage;
	public Text goldText;

    public Button purchaseButton;


    SoldierInfo _soldierInfo;

	public int price = 0;

    private void Awake()
    {
        Instance = this;
    }

	public override void Update ()
	{
		base.Update ();
	}

	public override void Open ()
	{
		base.Open ();

        _soldierInfo = new SoldierInfo();
        _soldierInfo.Init();

        group.SetActive(true);

		goldText.text = "" + price;

		if ( Inventory.Instance.gold < price)
        {
            goldImage.color = Color.red;
            purchaseButton.interactable = false;
        }
        else
        {
            goldImage.color = Color.white;
            purchaseButton.interactable = true;
        }

        SoundManager.Instance.Play(SoundManager.SoundType.UI_Open);

    }

    public void Confirm()
    {
		Inventory.Instance.RemoveGold(price);

		Inventory.Instance.AddSoldier (_soldierInfo);

        Inventory.Instance.Save();

        Close();

        SoundManager.Instance.Play(SoundManager.SoundType.Star);


    }

}
