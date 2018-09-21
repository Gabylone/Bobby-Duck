using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : Movable {

    public Plate_World CurrentPlate;
    public Plate_World[] plates;


    public override void Move(Swipe.Direction direction)
    {
        Coords targetCoords = GetTargetCoords(direction);

        TurnToDirection(direction);

        if (Tile.tiles.ContainsKey(targetCoords) == false)
        {
            blocked = true;
            SoundManager.Instance.Play(audioSource, SoundManager.SoundType.UI_Close);
            return;
        }

        if (Interactable.interactables.ContainsKey(targetCoords))
        {
            blocked = true;
            //SoundManager.Instance.Play(audioSource, SoundManager.SoundType.UI_Close);
            Interactable.interactables[targetCoords].Contact(this);
            return;
        }

        base.Move(direction);
    }

}
