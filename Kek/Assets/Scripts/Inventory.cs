using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{

    public static Inventory Instance;

    public Inventory()
    {

    }

    /// <summary>
    /// data
    /// </summary>
    public int barricadeCount = 0;
    public int gold = 0;
    //public int gold = 1000;
    public int health = 3;

    public SoldierInfo playerSoldierInfo;
    public List<SoldierInfo> soldierInfos = new List<SoldierInfo>();

    /// <summary>
    /// events
    /// </summary>
    public delegate void OnAddBarricade();
    public static OnAddBarricade onAddBarricade;

    /*public delegate void OnRemoveBarricade();
    public static OnRemoveBarricade onRemoveBarricade;*/

    public delegate void OnAddSoldier();
    public static OnAddSoldier onAddSoldier;

    internal void AddBarricade()
    {
        ++barricadeCount;

        if (onAddBarricade != null)
            onAddBarricade();
    }

    internal void RemoveBarricade()
    {
        --barricadeCount;

        /*if (onRemoveBarricade != null)
            onRemoveBarricade();*/
    }

    internal void AddSoldier(SoldierInfo newSoldierInfo)
    {
        soldierInfos.Add(newSoldierInfo);

        if (onAddSoldier != null)
            onAddSoldier();
    }
}