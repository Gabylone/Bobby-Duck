﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconVisual : MonoBehaviour
{

    public void InitVisual(Member memberID)
    {

        // hair
        for (int i = 0; i < (int)ApparenceType.nose; i++)
        {
            ApparenceType type = (ApparenceType)i;
            Sprite spr = CrewCreator.Instance.GetApparenceItem(type, memberID.GetCharacterID(type)).GetSprite();
            if ( spr == null)
            {
                images[i].enabled = false;

            }
            else
            {
                images[i].enabled = true;
                images[i].sprite = CrewCreator.Instance.GetApparenceItem(type, memberID.GetCharacterID(type)).GetSprite();
            }
        }

        /*if (memberID.GetCharacterID(ApparenceType.hair) > -1) {
            HairImage.sprite = CrewCreator.Instance.GetApparenceItem(ApparenceType.hair, memberID.hairSpriteID).GetSprite();
            HairImage.enabled = true;
		} else {
			HairImage.enabled = false;
		}*/

        images[(int)ApparenceType.hair].color = CrewCreator.Instance.hairColors[memberID.GetCharacterID(ApparenceType.hairColor)];
        images[(int)ApparenceType.beard].color = CrewCreator.Instance.hairColors[memberID.GetCharacterID(ApparenceType.hairColor)];
        images[(int)ApparenceType.eyebrows].color = CrewCreator.Instance.hairColors[memberID.GetCharacterID(ApparenceType.hairColor)];
        
        //BodyImage.sprite = CrewCreator.Instance.BodySprites[memberID.Male ? 0:1];

        //		Debug.Log (memberID.equipedWeapon.name);
        if (memberID.equipedWeapon != null)
            UpdateWeaponSprite(memberID.equipedWeapon);

    }



    public void UpdateWeaponSprite(Item item)
    {
        weaponImage.enabled = true;
        if (item == null)
        {
            weaponImage.sprite = CrewCreator.Instance.handSprite;
            return;
        }
        weaponImage.sprite = CrewCreator.Instance.weaponSprites[item.spriteID];
    }

    [Header("BobyParts")]
    [SerializeField]
    private Image weaponImage;

    public Image[] images;


}
