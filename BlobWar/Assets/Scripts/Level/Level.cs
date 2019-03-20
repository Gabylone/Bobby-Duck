using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    static Level current;

    public static List<Level> levels = new List<Level>();

    public int id = 0;

    /* client info*/
    public int maxClientAmount = 0;
    public int clientsToRushHour = 0;
    public Enemy.Type clientType;
    public int[] clientObjectives;

    /* spawn rates */
    public float rate_RushHour = 10f;
    public float startOfDayRate = 0f;
    public float endOfDayRate = 20f;

    /* new client types */
    [HideInInspector]
    public Enemy.Type newClientType = Enemy.Type.None;
    [HideInInspector]
    public string[] newClientNames;
    [HideInInspector]
    public string[] newClientDescriptions;

    public static Level Current
    {
        get
        {
            if (Level.levels.Count == 0)
            {
                return LevelManager.Instance.defaultLevel;
            }
            else
            {
                return current;
            }
        }
    }

    public static void SetCurrent(Level level)
    {
        current = level;
    }
}

public class LevelInfo
{
    public static LevelInfo Instance;

    public int rushHour_AppearTime;
    public int rushHour_Duration;

    public int goldPerClient;

    public void Load()
    {
        TextAsset textAsset = Resources.Load( "Levels" ) as TextAsset;

        string[] rows = textAsset.text.Split('\n');


        //rushHour_AppearTime = int.Parse(rows[11].Split(';')[1]);

        //rushHour_Duration = int.Parse(rows[3].Split(';')[1]);

        goldPerClient = int.Parse(rows[10].Split(';')[1]);

        //day_Duration = int.Parse(rows[2].Split(';')[1]);


    }

}