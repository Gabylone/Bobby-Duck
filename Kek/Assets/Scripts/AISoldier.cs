using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISoldier : Soldier
{
    public static bool soldierSelected = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ChangeLign(int i)
    {
        CurrentLign.RemoveSoldier(this);

        base.ChangeLign(i);

        CurrentLign.AddSoldier(this);

        Sound.Instance.PlaySound(Sound.Type.Runaway);


    }

    public override void PlaceStart()
    {
        base.PlaceStart();

        ChangeState(State.None);
    }

    public override void PlaceExit()
    {
        base.PlaceExit();

        ChangeLign(currentLignIndex);
    }

    #region idle
    public override void Idle_Update()
    {
        base.Idle_Update();

        if (timeInState > shootRate && !soldierSelected)
        {
            timeInState = 0f;
            Shoot();
        }

    }
    #endregion

    public override void HandleOnSwipe(Swipe.Direction direction)
    {
        base.HandleOnSwipe(direction);

        Swipe.onSwipe -= HandleOnSwipe;

        Deselect();
    }

    public override void MoveDelay()
    {
        base.MoveDelay();

        rectTransform.SetAsLastSibling();

    }

    public void OnPointerDown()
    {
        if (state != State.Idle)
            return;

        Swipe.Instance.Swipe_Start();
        Swipe.onSwipe += HandleOnSwipe;
        InputManager.onInputExit += HandleOnInputExit;

        Select();
    }

    

    private void HandleOnInputExit()
    {
        Swipe.onSwipe -= HandleOnSwipe;
        Deselect();
    }

    void Select()
    {
        image.color = Color.red;

        soldierSelected = true;

        Tween.Bounce(transform);
    }

    void Deselect()
    {
        image.color = initColor;

        soldierSelected = false;
    }

    public override void Shoot()
    {
        if (CurrentLign.zombies.Count == 0)
        {
            return;
        }

        Zombie targetZombie = CurrentLign.ClosestZombie();

        float dis = Vector3.Distance(targetZombie.rectTransform.position, rectTransform.position);

        if (dis > minimumDistanceKill)
        {
            return;
        }

        targetZombie.Kill();

        base.Shoot();
    }
}
