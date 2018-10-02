using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Zone {
    
    /// <summary>
    /// static
    /// </summary>
    public static Zone Current;

    public delegate void OnFinishZone();
    public static OnFinishZone onFinishZone;

    public delegate void OnGoForth();
    public static OnGoForth onGoForth;

    public delegate void OnRetreat();

    public static OnRetreat onRetreat;


    /// <summary>
    /// instance
    /// </summary>
    public int initAmount = 0;
    public int currAmount = 0;

    public bool finished = false;

    public void Start()
    {
        Current = this;

        currAmount = initAmount;

        Zombie.onZombieKill += HandleOnZombieKill;

    }
    public void End()
    {
        Zombie.onZombieKill -= HandleOnZombieKill;
    }

    private void HandleOnZombieKill()
    {
        --currAmount;

        if(currAmount == 0)
        {
            FinishZone();
        }
    }

    private void FinishZone()
    {
        finished = true;

        if (onFinishZone != null)
            onFinishZone();
    }
}
