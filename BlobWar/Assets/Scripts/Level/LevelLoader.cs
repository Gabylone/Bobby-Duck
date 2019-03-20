using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader Instance;

    public int startRow = 19;

    public Color color_Locked;
    public Color color_Done;
    public Color color_Completed;
    public Color color_YetToBeDone;

    public float rushHourPercentAppear = 70f;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        LevelInfo.Instance = new LevelInfo();
        LevelInfo.Instance.Load();

        TextAsset textAsset = Resources.Load("Levels") as TextAsset;

		Enemy.infos = new Enemy.Info[(int)Enemy.Type.None];

        string[] rows = textAsset.text.Split('\n');

        int levelIndex = 0;

        Enemy.Type currentClientType = Enemy.Type.Regular;

        for (int rowIndex = startRow; rowIndex < rows.Length -1; rowIndex++)
        {
            Level newLevel = new Level();

            string[] cells = rows[rowIndex].Split(';');

            newLevel.id = levelIndex;

            int cellIndex = 2;
            
            /// MAX CLIENT ///
            int clientAmount = 0;
            if (!int.TryParse(cells[cellIndex], out clientAmount))
            {
                Debug.LogError(" client amount did not parse : " + cells[cellIndex]);
            }

            newLevel.maxClientAmount = clientAmount;

            ++cellIndex;

            /// START OF DAY RATE ///
            float startOfDayRate = 0.0f;

            if (!float.TryParse(cells[cellIndex].Replace(',', '.'), out startOfDayRate))
            {
                Debug.LogError(" normal rate did not parse : " + cells[cellIndex]);
            }
            newLevel.startOfDayRate = (float)startOfDayRate;
            ++cellIndex;

            /// END OF DAY RATE ///
            float endOfdayRate = 0.0f;
            
            if (!float.TryParse(cells[cellIndex].Replace(',','.'), out endOfdayRate))
            {
                Debug.LogError(" normal rate did not parse : " + cells[cellIndex]);
            }
            newLevel.endOfDayRate = (float)endOfdayRate;
            ++cellIndex;



            /// RUSH HOUR RATE ///
            float rate_Rush = 0f;
            if (!float.TryParse(cells[cellIndex].Replace(',', '.'), out rate_Rush))
            {
                Debug.LogError(" rush hour rate did not parse : " + cells[cellIndex]);
            }
            newLevel.rate_RushHour = rate_Rush;
            ++cellIndex;

            /// RUSH HOUR CLIENT INDEX ///
            newLevel.clientsToRushHour = Mathf.RoundToInt( (float)newLevel.maxClientAmount * rushHourPercentAppear / 100f);

            /// CLIENT OBJECTIVES ///
            newLevel.clientObjectives = new int[3];

            newLevel.clientObjectives[0] = (newLevel.maxClientAmount - 4);
            newLevel.clientObjectives[1] = (newLevel.maxClientAmount - 2);
            newLevel.clientObjectives[2] = (newLevel.maxClientAmount);

            // CLIENT TYPE
            if (cells[cellIndex].Length > 1)
            {
                ++currentClientType;

                newLevel.newClientType = currentClientType;


				Enemy.infos [(int)currentClientType] = new Enemy.Info ();
				Enemy.infos [(int)currentClientType].names = new string[(int)Inventory.LanguageType.None];
                //newLevel.newClientNames = new string[(int)Inventory.LanguageType.None];

				Enemy.infos [(int)currentClientType].names[(int)Inventory.LanguageType.French] = cells[cellIndex];
				Enemy.infos [(int)currentClientType].names[(int)Inventory.LanguageType.English] = cells[cellIndex + 2];
//                newLevel.newClientNames[(int)Inventory.LanguageType.French] = cells[cellIndex];
//                newLevel.newClientNames[(int)Inventory.LanguageType.English] = cells[cellIndex + 2];


                ++cellIndex;

                /*newLevel.newClientDescriptions = new string[(int)Inventory.LanguageType.None];

                newLevel.newClientDescriptions[(int)Inventory.LanguageType.French] = cells[cellIndex];
                newLevel.newClientDescriptions[(int)Inventory.LanguageType.English] = cells[cellIndex + 2];*/
            }

            newLevel.clientType = currentClientType;

            Level.levels.Add(newLevel);

           /* Debug.Log("////");
            Debug.Log("LEVEL N " + newLevel.id);
            Debug.Log("client amount " + newLevel.maxClient);
            Debug.Log("start of day rate " + newLevel.startOfDayRate);
            Debug.Log("end of day rate " + newLevel.endOfDayRate);
            Debug.Log("rush rate " + newLevel.rate_RushHour);
            Debug.Log("golf objective 1 " + newLevel.goldObjectives[0]);
            Debug.Log("golf objective 2 " + newLevel.goldObjectives[1]);
            Debug.Log("golf objective 3 " + newLevel.goldObjectives[2]);
            Debug.Log("////");*/

            ++levelIndex;
        }

	}
	
}
