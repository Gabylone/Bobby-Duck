using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCreationButton_Apparence : MemberCreatorButton {

    public bool raycastOnStart = false;

    public override void Start()
    {
        base.Start();

        GetComponent<Image>().raycastTarget = raycastOnStart;

    }

    public void OnPointerEnter()
    {
        Select();
    }

    public override void OnPointerUp()
    {
        base.OnPointerUp();


        transform.SetAsFirstSibling();
    }

    public override void Select()
    {
        base.Select();

        if (apparenceItem.locked)
            return;

        Crews.playerCrew.captain.MemberID.SetCharacterID(apparenceType, id);

        SoundManager.Instance.PlaySound(SoundManager.Sound.Select_Small);

        Crews.playerCrew.captain.Icon.InitVisual(Crews.playerCrew.captain.MemberID);

    }


}
