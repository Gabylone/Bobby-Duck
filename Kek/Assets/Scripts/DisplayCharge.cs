using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharge : MonoBehaviour {

    public float chargeReloadDuration = 5f;

    public GameObject lockGroup;

    float currentLoad = 0f;

    public GameObject rayblockerGroup;

    public RectTransform backGroundRectTransform;

    public RectTransform fillRectTransform;
    public Image fillImage;

    public Color color1;
    public Color color2;

    public int killCount = 0;
    public int killsToReload = 3;

    bool filling = false;

	// Use this for initialization
	void Start () {

        BarricadeInventoryButton.onClickBarricadeInventoryButton += HandleOnClickBarricadeInventoryButton;
        SoldierInventoryButton.onClickSoldierInventoryButton+= HandleOnClickSoldierInventoryButton;

        Zombie.onZombieKill += HandleOnZombieKill;

        UpdateImage();

    }
    private void OnDestroy()
    {
        Zombie.onZombieKill -= HandleOnZombieKill;
    }

    private void HandleOnZombieKill()
    {
        if (killCount == killsToReload)
            return;

        ++killCount;

        UpdateImage();
    }

    private void HandleOnClickSoldierInventoryButton(SoldierInfo soldierInfo)
    {
        ResetCharge();
    }
    private void HandleOnClickBarricadeInventoryButton()
    {
        ResetCharge();
    }

    private void ResetCharge()
    {
        currentLoad = 0f;
        killCount = 0;
        rayblockerGroup.SetActive(true);

        UpdateImage();
    }

    private void UpdateImage()
    {

        if (killCount == killsToReload)
        {
            Tween.Bounce(transform);
            rayblockerGroup.SetActive(false);

        }

        float w = backGroundRectTransform.rect.width;

        //float l = currentLoad / chargeReloadDuration;
        float l = (float)killCount / killsToReload;

        fillImage.color = Color.Lerp(color1, color2, l);
        
        fillRectTransform.sizeDelta = new Vector2( - w + l * w , fillRectTransform.sizeDelta.y );
    }
}
