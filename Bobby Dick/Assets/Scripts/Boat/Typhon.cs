using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Holoville.HOTween;

public class Typhon : RandomPlacable
{
    public float delay = 1f;

    public float endDelay = 1f;

    public override void Start()
    {
        base.Start();
    }

    public override void HandleOnEnterNewChunk()
    {
        base.HandleOnEnterNewChunk();

        CheckProximityWithPlayer();

        transform.localScale = Vector3.one;
    }

    public override void Trigger()
    {
        base.Trigger();

        HOTween.To( PlayerBoat.Instance.getTransform , delay , "position" , transform.position );

        PlayerBoat.Instance.animator.SetTrigger( "Typhon" );

        Invoke("TriggerDelay", delay);

        WorldTouch.Instance.Lock();
    }

    void TriggerDelay()
    {
        foreach (var crewMember in Crews.playerCrew.CrewMembers)
        {
            crewMember.RemoveHealth(15);
            crewMember.Icon.hungerIcon.DisplayHealthAmount(15);

            if (crewMember.Health <= 0)
            {
                crewMember.Health = 2;
            }
        }

        Disappear();

        Invoke("TriggerDelayDelay" , endDelay);
    }

    void TriggerDelayDelay()
    {
        WorldTouch.Instance.Unlock();
    }
}
