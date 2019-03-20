using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Linq;

public class WorldMapScrollView : MonoBehaviour {

    public static WorldMapScrollView Instance;

    public RectTransform contentRectTransform;
    public RectTransform scrollRectTransform;

    public float centerViewDuration = 1f;

	public RectTransform target;

	public void SnapTo(RectTransform target)
	{
		Canvas.ForceUpdateCanvases();

		Vector2 v = (Vector2)scrollRectTransform.transform.InverseTransformPoint (contentRectTransform.position) - (Vector2)scrollRectTransform.transform.InverseTransformPoint (target.position);

//		contentRectTransform.anchoredPosition = v;
		HOTween.To (contentRectTransform , centerViewDuration , "anchoredPosition" , v);
	}

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        HandleRegionOutcome();
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

		SnapTo(regionButton.rectTransform);
	}

	private void CenterOnRegion(Coords c)
	{

		RegionButton regionButton = RegionButton.regionButtons[c];

		Vector2 p = -regionButton.rectTransform.anchoredPosition + (scrollRectTransform.rect.size / 2f);

		HOTween.To( contentRectTransform , centerViewDuration, "anchoredPosition" , p);

	}

	#region defeat
    private void HandleDefeat()
    {
		SnapTo(RegionButton.GetSelected.rectTransform);
        Invoke("HandleDefeatDelay", centerViewDuration);
    }

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

			SnapTo(RegionButton.GetSelected.rectTransform);
        }

        RegionManager.Instance.SaveRegions();

    }
	#endregion

	#region victory
    void HandleVictory()
    {
		SnapTo(RegionButton.GetSelected.rectTransform);
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
	#endregion

  
}
