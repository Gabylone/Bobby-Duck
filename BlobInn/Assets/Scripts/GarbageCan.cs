using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Interactable {

    public bool opened = false;

    Animator animator;

    bool contacted = false;

   /* public float timeToClose = 2f;
    float timer = 0f;

    private void Update()
    {
        if (opened)
        {
            timer += Time.deltaTime;

            if (timer >= timeToClose)
            {
                Close();
            }
        }
    }*/

    public override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

        Interactable.interactables.Add(coords, this);

        Player.Instance.onMove += HandleOnMove;

    }

    private void HandleOnMove()
    {
        if ( contacted)
        {
            contacted = false;
            return;
        }

        if (opened)
        {
            Close();
        }
    }

    public override void Contact(Waiter waiter)
    {
        base.Contact(waiter);

        if ( opened)
        {
            if (Player.Instance.CurrentPlate != null)
            {
                Player.Instance.CurrentPlate.Clear();
            }

            Close();
        }
        else
        {
            Open();
        }

        contacted = true;
    }

    void Open()
    {
        opened = true;
        Tween.Bounce(transform);
        animator.SetBool("opened", true);
    }

    void Close()
    {
        opened = false;
        Tween.Bounce(transform);
        animator.SetBool("opened", false);

    }
}
