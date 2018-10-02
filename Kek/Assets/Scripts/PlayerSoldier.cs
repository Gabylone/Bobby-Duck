using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class PlayerSoldier : Soldier {

    public override void Start()
    {
        base.Start();
        Swipe.onClick += HandleOnClick;
        Swipe.onSwipe += HandleOnSwipe;

        SetInfo(Inventory.Instance.playerSoldierInfo);

    }

    public override void StartDelay()
    {
        base.StartDelay();

        ChangeLign(currentLignIndex);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void HandleOnSwipe(Swipe.Direction direction)
    {
        if (AISoldier.soldierSelected)
            return;

        base.HandleOnSwipe(direction);
    }

    public override void ChangeLign(int i)
    {
        base.ChangeLign(i);

        Move(CurrentLign.playerAnchor.position, speedBetweenLigns);

        Sound.Instance.PlaySound(Sound.Type.Runaway);

    }

    public override void Shoot()
    {
        if (CurrentLign.zombies.Count > 0)
        {
            Zombie targetZombie = CurrentLign.ClosestZombie();

            float dis = Vector3.Distance(targetZombie.transform.position, transform.position);

            if (dis < minimumDistanceKill)
            {
                targetZombie.Kill();
            }
        }

        base.Shoot();
    }

    void HandleOnClick()
    {
        if (state == State.Idle)
            Shoot();
    }

    
}
