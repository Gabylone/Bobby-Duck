using UnityEngine;

[System.Serializable]
public class SoldierInfo
{
    public string name = "";

    public int colorID = 0;

    public int[] stats_Curr = new int[2] { 0, 0 };
    public int[] stats_Max = new int[2] { 10, 10 };

    public int[] apparenceIDs;

    public SoldierInfo()
    {

        
    }

    public void Init ()
    {
        name = NameGeneration.Instance.randomWord;

        stats_Curr = new int[2] { 0, 0 };
        stats_Max = new int[2] { 10, 10 };

        apparenceIDs = BlobApparence.GetRandomIDs();
    }

    public enum Stat
    {
        ShootRate,
        SpeedBetweenLigns,
    }

    #region params
    public int GetPrice(Stat stat)
    {
        return stats_Curr[(int)stat] * 100;
    }

    public int GetStat ( Stat stat) {
        return stats_Curr[(int)stat];
    }

    public int GetStatMax ( Stat stat) {
        return stats_Max[(int)stat];
    }

    public void SetStat(Stat stat, int i)
    {
        stats_Curr[(int)stat] = i;
    }

    public void SetStatMax(Stat stat, int i)
    {
        stats_Max[(int)stat] = i;
    }

    public Color GetColor
    {
        get
        {
            return Color.white;
            //return InventoryManager.Instance.soldierColors[colorID];
        }
    }
    #endregion

    #region serialization
    public string Serialize()
    {
        string ids_str = "";

        for (int i = 0; i < apparenceIDs.Length; i++)
        {
            ids_str += apparenceIDs[i].ToString();

            if (i < apparenceIDs.Length - 1)
                ids_str += ",";

        }

        string str = name + ","
            + colorID + ","
            + GetStat(SoldierInfo.Stat.ShootRate) + ","
            + GetStat(Stat.SpeedBetweenLigns) + ","
            + ids_str;

        Debug.Log(str);

        return str;
    }

    public static SoldierInfo Deserialize(string str)
    {
        SoldierInfo soldierInfo = new SoldierInfo();
        soldierInfo.Init();

        string[] parts = str.Split(',');

        soldierInfo.name = parts[0];
        soldierInfo.colorID = int.Parse(parts[1]);

        int shootRate = int.Parse(parts[2]);
        int speedBetweenLigns = int.Parse(parts[3]);

        for (int i = 0; i < 4; i++)
        {
            int id = int.Parse(parts[4 + i]);
            soldierInfo.apparenceIDs[i] = id;
        }

        soldierInfo.SetStat(SoldierInfo.Stat.ShootRate,shootRate);
        soldierInfo.SetStat(Stat.SpeedBetweenLigns,speedBetweenLigns);

        return soldierInfo;
    }
    #endregion
}