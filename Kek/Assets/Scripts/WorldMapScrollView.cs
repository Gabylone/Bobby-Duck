using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Linq;

public class WorldMapScrollView : MonoBehaviour {

    public static WorldMapScrollView Instance;

    public RectTransform content;
    public RectTransform rectTransform;

    public float centerViewDuration = 1f;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        HandleRegionOutcome();
	}

    private void CenterOnStartRegion()
    {
        RegionButton regionButton = null;

        foreach (var item in RegionButton.regionButtons)
        {
            if (item.Value.region.state == Region.State.Conquered)
            {
                regionButton = item.Value;
            }
        }

        CenterOnRegion(regionButton.region.coords);
    }

    private void HandleRegionOutcome()
    {
        switch (RegionOutcome.Instance.outcome)
        {
            case RegionOutcome.Outcome.None:
                CenterOnStartRegion();
                break;
            case RegionOutcome.Outcome.Lost:
                HandleDefeat();
                break;
            case RegionOutcome.Outcome.Won:
                HandleVictory();
                break;
            default:
                break;
        }
    }

    private void HandleDefeat()
    {
        CenterOnRegion(Region.selected.coords);
        Invoke("HandleDefeatDelay", centerViewDuration);
    }

    public delegate void OnCounterAttack();
    public OnCounterAttack onCounterAttack;

    private void HandleDefeatDelay()
    {
        List<RegionButton> regionButtons = RegionButton.regionButtons.Values.ToList().FindAll(x => x.region.state == Region.State.Conquered);
        RegionButton firstRegionButton = regionButtons.Find(x => x.region.level == 0);

        regionButtons.Remove(firstRegionButton);

        if ( regionButtons.Count > 0)
        {
            RegionButton regionButton = regionButtons[Random.Range(0, regionButtons.Count)];

            regionButton.region.SetState(Region.State.Attacked);

            Tween.Bounce(regionButton.transform);

            regionButton.UpdateDisplay();

            CenterOnRegion(Region.selected.coords);

            if (onCounterAttack != null)
                onCounterAttack();
        }

        RegionManager.Instance.SaveRegions();

    }

    void HandleVictory()
    {
        CenterOnRegion(Region.selected.coords);
        Invoke("HandleVictoryDelay", centerViewDuration);
    }
    
    void HandleVictoryDelay()
    {
        RegionButton regionButton = RegionButton.regionButtons[Region.selected.coords];

        Tween.Bounce(regionButton.transform);

        regionButton.region.SetState(Region.State.Conquered);

        RegionManager.Instance.SaveRegions();

        regionButton.UpdateDisplay();
    }

    private void CenterOnRegion(Coords c)
    {

        RegionButton regionButton = RegionButton.regionButtons[c];

        Vector2 p = -regionButton.rectTransform.anchoredPosition + (rectTransform.rect.size / 2f);

        HOTween.To( content , centerViewDuration, "anchoredPosition" , p);


    }
}
