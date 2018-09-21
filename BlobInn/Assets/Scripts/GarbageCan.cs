using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Interactable {

    public override void Start()
    {
        base.Start();

        Interactable.interactables.Add(coords, this);

    }

    public override void Contact(Waiter waiter)
    {
        base.Contact(waiter);

        if (Player.Instance.CurrentPlate != null)
        {
            Player.Instance.CurrentPlate.Clear();
        }

        Tween.Bounce(transform);
    }
}
