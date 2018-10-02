using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySoldierButton : MonoBehaviour {

    public delegate void OnClickBuySoldierButton();
    public static OnClickBuySoldierButton onClickBuySoldierButton;

    private void OnDestroy()
    {
        onClickBuySoldierButton = null;
    }

    private void Start()
    {
        
    }

    public void OnClick()
    {
        if (onClickBuySoldierButton != null)
            onClickBuySoldierButton();

        Sound.Instance.PlaySound(Sound.Type.Menu1);

        Tween.Bounce(transform);
    }
}
