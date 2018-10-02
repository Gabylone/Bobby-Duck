using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Level defaultLevel;

    float timer = 0f;
    public bool rushHour = false;
    public bool dayEnded = false;

    public delegate void OnLevelStart();
    public static OnLevelStart onLevelStart;

    public static bool endless = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if (LevelInfo.Instance == null)
        {
            LevelInfo.Instance = new LevelInfo();
            LevelInfo.Instance.Load();
        }

        // endless
        if (endless)
        {
            SetCurrentLevel(10);
        }

        Invoke("InitTiles", 0.001f);
        Invoke("StartDelay", 1.5f);
    }

    void StartDelay()
    {
        Tutorial.Instance.Show(TutorialStep.Movement);
    }

    public void StartRushHour()
    {
        rushHour = true;

        Music.Instance.PlayRushHour();

        ClientManager.Instance.ResetTimer();

        RushHourFeedback.Instance.Show();

        switch (Inventory.currentLanguageType)
        {
            case Inventory.LanguageType.French:
                DisplayInfo.Instance.Show("HEURE DE POINTE !");
                break;
            case Inventory.LanguageType.English:
                DisplayInfo.Instance.Show("RUSH HOUR !");
                break;
            case Inventory.LanguageType.None:
                DisplayInfo.Instance.Show("heure d epoi !");
                break;
            default:
                break;
        }
    }

    public void EndDay()
    {
        RushHourFeedback.Instance.Hide();
        MoonFeedback.Instance.Show();

        if ( endless)
        {
            Endless_PaceUp();
            return;
        }

        dayEnded = true;

        switch (Inventory.currentLanguageType)
        {
            case Inventory.LanguageType.French:
                DisplayInfo.Instance.Show("FIN DE JOURNEE !");
                break;
            case Inventory.LanguageType.English:
                DisplayInfo.Instance.Show("END OF DAY !");
                break;
            case Inventory.LanguageType.None:
                break;
            default:
                break;
        }
    }

#region endless
    private void Endless_PaceUp()
    {
        if ( Level.Current.id < Level.levels.Count - 2 )
        {
            switch (Inventory.currentLanguageType)
            {
                case Inventory.LanguageType.French:
                    DisplayInfo.Instance.Show("PLUS VITE !");
                    break;
                case Inventory.LanguageType.English:
                    DisplayInfo.Instance.Show("PACE UP !");
                    break;
                case Inventory.LanguageType.None:
                    DisplayInfo.Instance.Show("ULALA SA VA PLUS VITE");
                    break;
                default:
                    break;
            }

            rushHour = false;
            ClientManager.Instance.currentClientAmount = 0;

            int i = Mathf.Clamp(Level.Current.id + 3 , 0 , Level.levels.Count - 3);

            SetCurrentLevel(i);

            Debug.Log("augmenter rhygtme...");
        }

    }

    void SetCurrentLevel(int newID)
    {
        Level.SetCurrent(Level.levels[newID]);
        Level.Current.clientType = Client.Type.Brunch;
        Level.Current.maxClientAmount = Mathf.RoundToInt((float)Level.Current.maxClientAmount / 2f);
    }

#endregion

    void InitTiles()
    {
        int tmpMaxY = 0;
        while (true)
        {
            if (Tile.tiles.ContainsKey(new Coords(0, tmpMaxY + 1)) == false)
            {
                Tile.maxY = tmpMaxY;
                break;
            }

            ++tmpMaxY;
        }


        int tmpMinY = 0;
        while (true)
        {
            if (Tile.tiles.ContainsKey(new Coords(0, tmpMinY - 1)) == false)
            {
                Tile.minY = tmpMinY;
                break;
            }

            --tmpMinY;
        }


        int tmpMaxX = 0;
        while (true)
        {
            if (Tile.tiles.ContainsKey(new Coords(tmpMaxX + 1, 0)) == false)
            {
                Tile.maxX = tmpMaxX;
                break;
            }

            ++tmpMaxX;
        }

        int tmpMinX = 0;
        while (true)
        {
            if (Tile.tiles.ContainsKey(new Coords(tmpMinX - 1, 0)) == false)
            {
                Tile.minX = tmpMinX;
                break;
            }

            --tmpMinX;
        }

    }
}