using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class Player : Soldier {

    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

		apparence.SetSpriteIDs(Inventory.Instance.soldierInfo_Player.apparenceIDs);

        Swipe.onSwipe += HandleOnSwipe;

		Swipe.onTap += HandleOnTap;

		Move (Swipe.Direction.None);
    }

	public override void Update ()
	{
		base.Update ();

		if (_shootTimer > 0 ) {
			_shootTimer -= Time.deltaTime;
		}
	}

	public override void Shoot ()
	{
		base.Shoot ();

		ShootAnim ();

	}
    
	void HandleOnTap ()
    {
		if (_shootTimer <= 0 && !shooting) {
			Shoot ();
		}
    }

    private void HandleOnSwipe(Swipe.Direction direction)
    {
		if (SoldierAI.soldierTouched)
			return;

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

		HOTween.To(_transform, moveDuration, "position", LignManager.Instance.ligns[currentLignIndex].playerAnchor.position, false, move_EaseType, 0f);

		apparence.SetRenderingOrder (CurrentLign.soldiers.Count+1);
    }


}
