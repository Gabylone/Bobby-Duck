using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCreationButton_Apparence : MemberCreatorButton {

    public override void Start()
    {
        base.Start();

        GetComponent<Image>().raycastTarget = false;


    }
    Transform parent;

    public void OnPointerEnter()
    {
        Select();
    }

    public void OnPointerUp()
    {
        Debug.Log("wow " + name);

        parent = transform.parent;

        GetComponent<RectTransform>().SetParent(null);

        Invoke("wai", 1f);
    }

    void wai()
    {
        transform.SetParent(parent);

    }


}
