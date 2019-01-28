using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseDiamondButton : MonoBehaviour {

    public enum Type
    {
        Five,
        Fifty,
        Hundred,

    }

    public Type type;

    public void OnPointerClick()
    {
        Tween.Bounce(transform);

        switch (type)
        {
            case Type.Five:

                Purchaser.Instance.BuyProductID(Purchaser.fiveDiamondsID);
                break;
            case Type.Fifty:

                Purchaser.Instance.BuyProductID(Purchaser.fiftyDiamondsID);

                break;
            case Type.Hundred:
                Purchaser.Instance.BuyProductID( Purchaser.hundredDiamondsID );
                break;
            default:
                break;
        }


    }
}
