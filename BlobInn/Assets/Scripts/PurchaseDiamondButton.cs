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

        Purchaser.Instance.BuyProductID( type.ToString() );
    }
}
