using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFeedback : MonoBehaviour {

    public GameObject group;

    public Animator animator;

	// Use this for initialization
	void Start () {

        Zone.onFinishZone += HandleOnFinishZone;

        Hide();
	}

    private void HandleOnFinishZone()
    {
        if ( ZoneManager.Instance.zoneIndex == ZoneManager.Instance.zones.Count)
        {
            return;
        }

        if ( Inventory.Instance.health <= 0)
        {
            Debug.Log("not showing hand because loxst");
            return;
        }

        animator.SetTrigger("Go Forth");

        Show();

        Swipe.onSwipe += HandleOnSwipe;
    }

    private void HandleOnSwipe(Swipe.Direction direction)
    {
        if ( direction == Swipe.Direction.Up)
        {
            Hide();
            Swipe.onSwipe -= HandleOnSwipe;
        }

    }

    private void Show()
    {
        group.SetActive(true);
    }

    private void Hide()
    {
        group.SetActive(false);
    }
}
