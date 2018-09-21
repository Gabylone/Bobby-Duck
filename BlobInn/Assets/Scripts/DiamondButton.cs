using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondButton : MonoBehaviour {

    public enum Type
    {
        Lifes,
        Gold,
    }

    public int price = 10;

    public Text uiText;

    public Type type;

    public int lifeReward = 10;
    public int goldReward = 100;

    public Image diamondImage;

    private void OnEnable()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        uiText.text = "" + price;

        if (Inventory.Instance.diamonds < price)
        {
            diamondImage.color = Color.red;
            uiText.color = Color.white;

        }
        else
        {
            diamondImage.color = Color.white;
            uiText.color = Color.black;
        }
    }

    public void OnPointerClick()
    {
        if ( Inventory.Instance.diamonds < price)
        {
            Tween.Bounce(uiText.transform);

            return;
        }

        Tween.Bounce(transform);

        Inventory.Instance.RemoveDiamonds( price );
        RewardPlayer();
    }

    private void RewardPlayer()
    {
        switch (type)
        {
            case Type.Lifes:
                Inventory.Instance.AddLifes(lifeReward);
                break;
            case Type.Gold:
                Inventory.Instance.AddGold(goldReward);
                break;
            default:
                break;
        }

        UpdateUI();

        Inventory.Instance.Save();
    }
}
