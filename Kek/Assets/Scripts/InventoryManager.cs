using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour {

	public static InventoryManager Instance;

    public bool debugInventory = false;

    public Color[] soldierColors = new Color[5]
    {
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.red
    };

    /// <summary>
    /// serializable
    /// </summary>
    Inventory inventory;

    public int maxSoldierAmount = 10;

	void Awake () {

		Instance = this;
        
    }

    void Start()
    {
        RegionManager.onStartMap += HandleOnStartMap;
        Zombie.onZombieKill += HandleOnZombieKill;
    }

    private void HandleOnStartMap()
    {
        InitInventory();
        if (debugInventory)
            DebugInv();
    }

    private void InitInventory()
    {
        if (SaveTool.Instance.FileExists("inventory"))
        {
            inventory = SaveTool.Instance.LoadFromPath("inventory", "Inventory") as Inventory;
            Inventory.Instance = inventory;
        }
        else
        {
            inventory = new Inventory();
            Inventory.Instance = inventory;

            inventory.playerSoldierInfo = new SoldierInfo();
            inventory.playerSoldierInfo.name = "Joueur";
            inventory.playerSoldierInfo.colorID = Random.Range(0, soldierColors.Length);

            SaveInventory();

        }


    }

    private void SaveInventory()
    {
        SaveTool.Instance.SaveToPath("inventory", inventory);
    }

    private void HandleOnZombieKill()
    {
        AddGold(1);
    }

    private void DebugInv()
    {

        inventory.gold = 2000;


        /*inventory.barricadeCount = 5;

        for (int i = 0; i < 5; i++)
        {
            SoldierInfo newSoldierInfo = new SoldierInfo();

            newSoldierInfo.shootRate = 0.5f;

            inventory.soldierInfos.Add(newSoldierInfo);

        }*/
    }

    public delegate void OnAddGold();
    public static OnAddGold onAddGold;
    public delegate void OnRemoveGold();
    public static OnRemoveGold onRemoveGold;
    public void AddGold(int i)
    {
        inventory.gold += i;

        SaveInventory();

        if (onAddGold != null)
            onAddGold();
    }
    
    public void RemoveGold(int i)
    {
        inventory.gold -= i;

        SaveInventory();

        if (onRemoveGold != null)
            onRemoveGold();
    }


}
