using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RegionManager : MonoBehaviour {

    public static RegionManager Instance;

    public Transform regionButtonParent;

    public Coords startCoords;

    public int maxLevel = 0;

    public int conqueredRegionCount = 0;

    public delegate void OnStartMap();
    public static OnStartMap onStartMap;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        RegionButton.regionButtons.Clear();

        foreach (var item in regionButtonParent.GetComponentsInChildren<RegionButton>())
        {
            RegionButton.regionButtons.Add(item.coords, item);
        }

        if (Region.regions.Count == 0)
            InitRegions();

        if (onStartMap != null)
        {
            onStartMap();
        }
    }


    void SetRegionLevels()
    {
        int range = 1;

        int level = 0;

        Region startRegion = Region.regions[startCoords];

        while (true)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {

                    Coords c = new Coords(startCoords.x + x, startCoords.y + y);

                    if (Region.regions.ContainsKey(c) == false)
                    {
                        continue;
                    }

                    if (Region.regions[c].level > 0)
                        continue;

                    if (Region.regions[c].state == Region.State.Conquered)
                        continue;

                    Region r = Region.regions[c];

                    r.level = level;

                    ++level;


                    RegionButton.regionButtons[c].UpdateDisplay();
                }
            }

            ++range;

            if (level >= Region.regions.Count-1)
            {
                maxLevel = level;
                break;
            }
        }

    }

    private void SetStartRegion()
    {
        int id = Random.Range(0, RegionButton.regionButtons.Count);

        RegionButton startRegionButton = RegionButton.regionButtons.ElementAt(id).Value;

        startCoords = startRegionButton.coords;

        startRegionButton.region.SetState(Region.State.Conquered);

        SetRegionLevels();
        
    }

    private void InitRegions()
    {
        if (SaveTool.Instance.FileExists("regions"))
        {
            LoadRegions();
        }
        else
        {
            CreateRegions();
        }
    }

    private void CreateRegions()
    {
        Region.regions.Clear();

        foreach (var item in RegionButton.regionButtons.Values )
        {
            Region newRegion = new Region();

            newRegion.SetState(Region.State.Invaded);

            newRegion.coords = item.coords;

            Region.regions.Add(item.coords, newRegion);

        }

        SetStartRegion();

        SaveRegions();
    }

    public void SaveRegions()
    {
        RegionList regionList = new RegionList();

        regionList.regions = Region.regions.Values.ToList();

        SaveTool.Instance.SaveToPath("regions", regionList);
    }

    private void LoadRegions()
    {

        RegionList regionList = SaveTool.Instance.LoadFromPath("regions", "RegionList") as RegionList;

        conqueredRegionCount = regionList.conqueredRegionCount;

        foreach (var item in regionList.regions)
        {
            Region.regions.Add( item.coords , item );
        }
    }

    

}

[System.Serializable]
public class RegionList
{
    public int conqueredRegionCount = 0;
    public List<Region> regions = new List<Region>();
}