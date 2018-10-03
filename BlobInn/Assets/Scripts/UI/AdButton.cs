using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdButton : MonoBehaviour {

    public enum Type
    {
        Lifes,
        Gold,
        MultiplyGold
    }

    public Type type;

    public int lifeReward = 10;
    public int goldReward = 100;

    public void OnPointerClick()
    {
        Tween.Bounce(transform);

        Transition.Instance.Fade(1f);

        Invoke("OnPointerClickDelay", 1f);

    }

#if UNITY_ANDROID
    void OnPointerClickDelay()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions showOptions = new ShowOptions
            {
                resultCallback = HandleShowResult
            };

            Advertisement.Show(showOptions);

            Debug.Log("app is ready");

        }
        else
        {
            Debug.Log("app isnt ready");
        }
    }

    private void HandleShowResult(ShowResult obj)
    {
        switch (obj)
        {
            case ShowResult.Failed:
                Debug.Log("failed");
                break;
            case ShowResult.Skipped:
                Debug.Log("skipped");
                break;
            case ShowResult.Finished:
                Debug.Log("ad watched");
                RewardPlayer();
                break;
            default:
                break;
        }

        Transition.Instance.Clear(1f);

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
            case Type.MultiplyGold:
                Inventory.multiplyGold = true;
                DisplayLevel.Instance.UpdateUI();
                break;
            default:
                break;
        }

        Inventory.Instance.Save();

    }
#endif
}
