using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{

    public RectTransform bgRectTransform;

    public Image fillImage;

    public RectTransform[] rushHour_Feedbacks;

    public void Start()
    {
        ResetFeedbacks();
    }

    private void ResetFeedbacks()
    {
        foreach (var item in rushHour_Feedbacks)
        {
            //item.anchoredPosition = 
        }
    }
}
