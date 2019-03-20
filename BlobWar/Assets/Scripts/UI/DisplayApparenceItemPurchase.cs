using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayApparenceItemPurchase : DisplayGroup {

    public static DisplayApparenceItemPurchase Instance;

    public Image apparenceItemImage;

    public Text uiText;

    int currentPrice = 0;
    BlobApparence.Type currentType;
    int currentID = 0;

    public Image textImage;

    public Button purchaseButton;


    private void Awake()
    {
        Instance = this;
    }

	public override void Update ()
	{
		base.Update ();
	}


    public void Display( BlobApparence.Type type , int id , int price )
    {
        //DisplayCharacterCustomization.Instance.Close(false);

        Open();

        currentID = id;
        currentPrice = price;
        currentType = type;

        apparenceItemImage.sprite = BlobApparenceManager.Instance.GetSprite(type, id);

        uiText.text = "" + price;

        if ( Inventory.Instance.diamonds < price)
        {
            purchaseButton.interactable = false;
            textImage.color = Color.red;
        }
        else
        {
            purchaseButton.interactable = true;
            textImage.color = Color.white;
        }
    }

    public void Confirm()
    {
        Inventory.Instance.RemoveDiamonds(currentPrice);

        Inventory.Instance.AddIDToApparenceItem(currentType, currentID);

        Inventory.Instance.Save();

        DisplayCharacterCustomization.Instance.UpdateGrid(currentType);

        Close(false);
    }

    public override void Close()
    {
        base.Close(false);
    }
}
