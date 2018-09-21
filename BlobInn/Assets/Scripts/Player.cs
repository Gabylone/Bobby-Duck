using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Player : Waiter {

    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        GetComponentInChildren<Blob_Apparence>().LoadFromInventory();

        plates = GetComponentsInChildren<Plate_World>(true);

        int plateIndex = 0;

        foreach (var plate in plates)
        {
            plate.id = plateIndex;

            plateIndex++;
        }

        Swipe.onSwipe += HandleOnSwipe;


    }

    private void HandleOnSwipe(Swipe.Direction direction)
    {
        if (sliding)
            return;

        Move(direction);
    }

    public delegate void OnMove();
    public OnMove onMove;

    public override void Move(Swipe.Direction direction)
    {
        base.Move(direction);

        if (onMove != null)
        {
            onMove();
        }

    }


}
