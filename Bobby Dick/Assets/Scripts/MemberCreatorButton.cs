using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberCreatorButton : MonoBehaviour {

    public static MemberCreatorButton lastSelected;

    public GameObject lockGroup;

    public Text pearlPriceUIText;

    public ApparenceItem apparenceItem;

    public Transform initParent;

    public Image image;

    public bool selected = false;

    public float scaleAmount = 2f;

    public virtual void Start()
    {
        UpdateImage();

        if (Crews.playerCrew.captain.MemberID.GetCharacterID(apparenceItem.apparenceType) == apparenceItem.id)
        {
            Select();
        }
    }

    #region select & deselect
    public void OnPointerDown () {

        if (selected)
        {
            Deselect();
        }
        else
        {
            Select();
        }

    }

    public virtual void OnPointerUp()
    {
        initParent = transform.parent;

        if (apparenceItem.locked)
        {
            DisplayPurchase.Instance.Display(apparenceItem, transform);
            return;
        }

    }

    public void Deselect()
    {
        selected = false;
        Tween.Scale(transform, 0.2f, 1f);
    }

    public virtual void Select()
    {
        if ( lastSelected != null)
        {
            if ( lastSelected.apparenceItem.apparenceType == apparenceItem.apparenceType)
            {
                lastSelected.Deselect();
            }
        }

        Tween.Scale(transform, 0.2f, scaleAmount);

        selected = true;
        lastSelected = this;
    }
    #endregion

    #region image
    public virtual void UpdateImage() {

		Member member = Crews.playerCrew.captain.MemberID;

        apparenceItem = CrewCreator.Instance.GetApparenceItem(apparenceItem.apparenceType, apparenceItem.id);
        if (apparenceItem.apparenceType == ApparenceType.hairColor)
        {
            image.enabled = false;
            GetComponent<Image>().color = CrewCreator.Instance.hairColors[apparenceItem.id];
        }
        else
        {
            if (apparenceItem.GetSprite() == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = apparenceItem.GetSprite();
                image.enabled = true;
            }
        }

        if (apparenceItem.locked)
        {
            lockGroup.SetActive(true);

            pearlPriceUIText.text = "" + apparenceItem.price;
        }
        else
        {
            lockGroup.SetActive(false);
        }
    }
    #endregion

    #region enable disable
    void Enable () {
		gameObject.SetActive (true);
	}

    void Disable()
    {
        gameObject.SetActive(false);
    }   
    #endregion
}
