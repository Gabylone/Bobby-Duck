using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class CamBehavior : MonoBehaviour
{
    public Vector3 decal;

    public float distanceToIsland = 10f;

    public float zoomDuration = 1f;

    Vector3 initPos;
    Vector3 initRot;

    // Use this for initialization
    void Start()
    {
        initPos = transform.position;
        initRot = transform.forward;

        StoryLauncher.Instance.onPlayStory += Zoom;
        StoryLauncher.Instance.onEndStory += UnZoom;
    }

    void Zoom()
    {
        //Vector3 dirFromIsland = Island.Instance.transform.position - decal;

        if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.other)
            return;

        Vector3 targetPos = Island.Instance.transform.position;

        if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.boat)
        {
            targetPos = EnemyBoat.Instance.getTransform.position;
        }

        Vector3 p = targetPos + decal;


        HOTween.To(transform, zoomDuration, "position", p , false , EaseType.EaseInOutCubic , 0f );
        HOTween.To(transform, zoomDuration, "forward", (targetPos-p).normalized , false, EaseType.EaseInOutCubic , 0f );
    }

    void UnZoom()
    {
        if (StoryLauncher.Instance.CurrentStorySource == StoryLauncher.StorySource.other)
            return;

        HOTween.To(transform, zoomDuration, "position", initPos, false, EaseType.EaseInOutCubic, 0f);
        HOTween.To(transform, zoomDuration, "forward", initRot, false, EaseType.EaseInOutCubic, 0f);

    }
}
