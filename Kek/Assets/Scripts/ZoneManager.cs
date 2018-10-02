using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneManager : MonoBehaviour {

    public static ZoneManager Instance;

    public Transform soldierParent;
    public Transform barricadeParent;

	public List<Zone> zones = new List<Zone>();
    public int zoneIndex = 0;

    public TextAsset textAsset;

    public delegate void OnStartZones();
    public static OnStartZones onStartZones;

    void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Zone.onRetreat = null;
        Zone.onGoForth = null;
        Zone.onFinishZone = null;
    }

    void Start()
    {
        string[] rows = textAsset.text.Split('\n');

        int zombieAmount = 0;
        int zoneAmount = 0;

        if (Region.selected.level >= rows.Length)
        {
            Debug.Log("level is higher than rows ");
            zombieAmount = Region.selected.level * 10;
        }
        else
        {
            string[] cells = rows[Region.selected.level].Split(';');
            zombieAmount = int.Parse(cells[0]);
            zoneAmount = int.Parse(cells[1]);
        }

        float minDif = 0.1f;
        float maxDif = 0.4f;
        for (int i = 0; i < zoneAmount; i++)
        {
            Zone newZone = new Zone();

            float l = (float)i / zoneAmount;
            float f = Mathf.Lerp(minDif, maxDif, l);
            int iF = (int)(zombieAmount * f);
            if (iF < 0)
            {
                iF = 1;
            }
            newZone.initAmount = zombieAmount + iF;

            zones.Add(newZone);
        }


        zones[0].Start();

        Swipe.onSwipe += HandleOnSwipe;

        Invoke("StartDelay", 0.01f);
    }

    void StartDelay()
    {
        if (onStartZones != null)
            onStartZones();
    }

    private void HandleOnSwipe(Swipe.Direction direction)
    {
        if (Zone.Current.finished && direction == Swipe.Direction.Up)
            GoForth();
    }

    #region go forth
    private void GoForth()
    {
        Sound.Instance.PlaySound(Sound.Type.UI_Correct);
        Zone.Current.End();

        ++zoneIndex;

        if (Zone.onGoForth != null)
            Zone.onGoForth();

        if (zoneIndex >= zones.Count)
        {
            WinRegion();
            return;
        }

        zones[zoneIndex].Start();
      
    }

    private void WinRegion()
    {
        RegionOutcome.Instance.outcome = RegionOutcome.Outcome.Won;

        MessageDisplay.Instance.Display("Région conquise");

        Invoke("WinRegionDelay", 4f);
        Invoke("SetTransition", 2f);

        Sound.Instance.PlaySound(Sound.Type.Menu6);

    }
    void SetTransition()
    {
        Transition.Instance.Fade(2f);

        Invoke("SetTransitionDelay",2f);
    }
    void SetTransitionDelay()
    {
        Transition.Instance.Clear(2f);
    }
    void WinRegionDelay()
    {
        SceneManager.LoadScene("Main (map)");
    }
    #endregion

    #region retreat
    private void HandleOnRetreat()
    {
        --zoneIndex;

        if (zoneIndex == 0)
        {
            LoseRegion();
            return;
        }

        zones[zoneIndex].Start();

    }

    private void LoseRegion()
    {
        RegionOutcome.Instance.outcome = RegionOutcome.Outcome.Lost;
    }
    #endregion
}
