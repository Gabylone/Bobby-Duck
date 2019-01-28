using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory Instance;

    public bool loadOnStart = true;

    public int gold = 0;
    public int diamonds = 0;
    public int lifes = 3;
    public int progress = 1;
    public int[] starAmounts;
    public int plateAmount = 1;
    public int tableAmount = 1;
    public int waiterAmount = 0;
    public List<Ingredient.Type> ingredientTypes = new List<Ingredient.Type>();

    public bool showedMultGold = false;

    public int highscore = 0;

    public int goldAquiredInLevel = 0;

    public enum LanguageType
    {
        French,
        English,

        None,
    }

    public static LanguageType currentLanguageType = LanguageType.None;

    public static bool multiplyGold = false;

    public ApparenceItem[] aquiredApparenceItems;
    public int[] apparenceIDs = new int[3];

    public delegate void OnChangeGold();
    public OnChangeGold onChangeGold;

    public delegate void OnChangeDiamonds();
    public OnChangeDiamonds onChangeDiamonds;

    public delegate void OnChanceLife();
    public OnChanceLife onChanceLife;
	public bool portrait = false;

    public bool displayedInvitation = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Screen.SetResolution(900, 1600, true);

        if (Level.levels.Count == 0)
        {
            starAmounts = new int[50];
        }
        else
        {
            starAmounts = new int[Level.levels.Count];
        }

        for (int i = 0; i < starAmounts.Length; i++)
        {
            starAmounts[i] = -1;
        }

        InitAquiredItems();

        Screen.orientation = ScreenOrientation.Portrait;

		if (loadOnStart)
			Load();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold(55);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AddDiamonds(7);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.SetInt("diamonds", diamonds);
        PlayerPrefs.SetInt("progress", progress);
        PlayerPrefs.SetInt("tableAmount", tableAmount);
        PlayerPrefs.SetInt("plateAmount", plateAmount);
        PlayerPrefs.SetInt("waiterAmount", waiterAmount);

        PlayerPrefs.SetInt("languageType", (int)currentLanguageType);

        PlayerPrefs.SetInt("highscore", highscore);

        PlayerPrefs.SetInt("displayedInvitation", displayedInvitation ? 1 : 0);
        PlayerPrefs.SetInt("enableSound", SoundManager.Instance.playSounds ? 1 : 0);
        PlayerPrefs.SetInt("showedMultGold", showedMultGold ? 1 : 0);

		PlayerPrefs.SetInt("portrait", portrait ? 1 : 0);

        SaveLifes();

        int ingredientIndex = 0;
        foreach (var ingredientType in ingredientTypes)
        {
            PlayerPrefs.SetInt("ingredientType" + ingredientIndex, (int)ingredientType);

            ++ingredientIndex;
        }

        int levelIndex = 0;
        foreach (var goldAmount in starAmounts)
        {
            if (goldAmount >= 0)
            {
                PlayerPrefs.SetInt(levelIndex.ToString(), goldAmount);
            }

            ++levelIndex;
        }

    }

    public void SaveLifes()
    {
        PlayerPrefs.SetInt("lifes", lifes);
    }

    public void Load()
    {
        gold = PlayerPrefs.GetInt("gold");
        diamonds = PlayerPrefs.GetInt("diamonds");
        lifes = PlayerPrefs.GetInt("lifes",5);
        progress = PlayerPrefs.GetInt("progress",1);
        plateAmount = PlayerPrefs.GetInt("plateAmount",1);
        tableAmount = PlayerPrefs.GetInt("tableAmount",2);
        waiterAmount = PlayerPrefs.GetInt("waiterAmount", 0);
        
        highscore = PlayerPrefs.GetInt("highscore", 0);

		LoadLanguage ();

        displayedInvitation = PlayerPrefs.GetInt("displayedInvitation", 0) == 1;
        bool playSounds = PlayerPrefs.GetInt("enableSound", 1) == 1;
        SoundManager.Instance.playSounds = playSounds;
        Music.Instance.source.enabled = playSounds;
        showedMultGold = PlayerPrefs.GetInt("showedMultGold", 0) == 1;

		// screen orientation
		portrait = PlayerPrefs.GetInt("portrait", 1) == 1;
		UpdateScreenOrientation ();

        // ingredients //
        ingredientTypes.Clear();

        int ingredientIndex = 0;
        while (true)
        {
            string s = "ingredientType" + ingredientIndex;

            int a = PlayerPrefs.GetInt("ingredientType" + ingredientIndex, -1);

            if ( a < 0)
            {
                break;
            }

            ingredientTypes.Add((Ingredient.Type)a);

            ++ingredientIndex;
        }

        if ( ingredientIndex == 0 && ingredientTypes.Count == 0)
        {
            ingredientTypes.Add(Ingredient.Type.Tomato);
            ingredientTypes.Add(Ingredient.Type.Chicken);
            ingredientTypes.Add(Ingredient.Type.Cheese);
        }
        // //

        // level //
        int levelIndex = 0;
        while (levelIndex < Level.levels.Count)
        {
            int starAmount = PlayerPrefs.GetInt(levelIndex.ToString(), -1);

            if ( levelIndex < starAmounts.Length )
                starAmounts[levelIndex] = starAmount;

            ++levelIndex;
            
        }
        //

        // blob apparence
        LoadBlobApparence();
        //
       
    }

	public void UpdateScreenOrientation ()
	{
		if ( portrait ) {
			Screen.orientation = ScreenOrientation.Portrait;
		} else {
			Screen.orientation = ScreenOrientation.Landscape;
		}
	}

	void LoadLanguage ()
	{
		currentLanguageType =  (LanguageType)PlayerPrefs.GetInt("languageType", (int)LanguageType.None);

		if (currentLanguageType == LanguageType.None && DisplayLanguage.Instance != null)
		{
			DisplayLanguage.Instance.Open();

		}
	}

    public void SetStarAmount ( int levelID , int starAmount)
    {
        if ( starAmount > starAmounts[levelID])
        {
            starAmounts[levelID] = starAmount;
        }
    }

    #region blob apparence
    private void InitAquiredItems()
    {
        aquiredApparenceItems = new ApparenceItem[(int)Blob_Apparence.Type.None];
        for (int i = 0; i < aquiredApparenceItems.Length; i++)
        {
            aquiredApparenceItems[i] = new ApparenceItem();
            aquiredApparenceItems[i].type = (Blob_Apparence.Type)i;
        }   
    }
    public void SaveBlobApparence()
    {
        // CURRENT BLOB ITEMS
        int a = 0;
        foreach (var id in DisplayCharacterCustomization.Instance.blob_Apparence.ids)
        {
            Blob_Apparence.Type type = (Blob_Apparence.Type)a;

            PlayerPrefs.SetInt("current_" + type.ToString(), id);

            ++a;
        }

        // AQUIRED ITEMS
        foreach (var aquiredApparenceItem in aquiredApparenceItems)
        {
            if (aquiredApparenceItem.ids.Count == 0)
            {
                PlayerPrefs.SetString("aquired_" + aquiredApparenceItem.type.ToString(), "null");
            }
            else
            {
                string str = "" + aquiredApparenceItem.ids[0];
                for (int i = 1; i < aquiredApparenceItem.ids.Count; i++)
                {
                    str += "|" + aquiredApparenceItem.ids[i].ToString();
                }

                PlayerPrefs.SetString("aquired_" + aquiredApparenceItem.type.ToString(), str);
            }

        }
    }

    public void AddIDToApparenceItem(Blob_Apparence.Type type, int id)
    {
        if (aquiredApparenceItems[(int)type].ids.Contains(id))
        {
            return;
        }

        aquiredApparenceItems[(int)type].ids.Add(id);
    }

    void LoadBlobApparence()
    {
        // CURRENT BLOB ITEMS
        apparenceIDs = new int[(int)Blob_Apparence.Type.None];

        for (int typeIndex = 0; typeIndex < (int)Blob_Apparence.Type.None; typeIndex++)
        {
            Blob_Apparence.Type type = (Blob_Apparence.Type)typeIndex;

            int id = PlayerPrefs.GetInt("current_" + type.ToString(), 0);

            apparenceIDs[typeIndex] = id;

        }

        bool loadedSomething = false;

        // AQUIRED ITEMS
        for (int typeIndex = 0; typeIndex < (int)Blob_Apparence.Type.None; typeIndex++)
        {
            Blob_Apparence.Type type = (Blob_Apparence.Type)typeIndex;

            string content = PlayerPrefs.GetString("aquired_" + aquiredApparenceItems[typeIndex].type.ToString(), "null");

            if (content == "null")
            {
                continue;
            }

            loadedSomething = true;

            string[] parts = content.Split('|');

            foreach (var part in parts)
            {

                int id = int.Parse(part);

                aquiredApparenceItems[(int)type].ids.Add(id);
            }
        }

        if (!loadedSomething)
        {
            aquiredApparenceItems[(int)Blob_Apparence.Type.Head].ids.Add(0);
            aquiredApparenceItems[(int)Blob_Apparence.Type.Head].ids.Add(1);

            aquiredApparenceItems[(int)Blob_Apparence.Type.Eyes].ids.Add(0);
            aquiredApparenceItems[(int)Blob_Apparence.Type.Eyes].ids.Add(1);

            aquiredApparenceItems[(int)Blob_Apparence.Type.EyesAccessories].ids.Add(0);
            aquiredApparenceItems[(int)Blob_Apparence.Type.EyesAccessories].ids.Add(1);

            for (int i = 0; i < BlobApparenceManager.Instance.blobColors.Length; i++)
            {
                aquiredApparenceItems[(int)Blob_Apparence.Type.Color].ids.Add(i);
            }

        }
    }
    #endregion

    public void AddGold(int i)
    {
        gold += i;

        goldAquiredInLevel += i;

        if (onChangeGold != null)
            onChangeGold();
    }

    public void RemoveGold(int i)
    {
        gold -= i;

        gold = Mathf.Clamp( gold , 0 , gold );

        if (onChangeGold != null)
            onChangeGold();
    }

    public void AddDiamonds(int i)
    {
        diamonds += i;

        if (onChangeDiamonds != null)
            onChangeDiamonds();
    }

    public void RemoveDiamonds(int i)
    {
        diamonds -= i;

        if (onChangeDiamonds != null)
            onChangeDiamonds();
    }

    public void AddLifes(int i)
    {
        lifes += i;

        lifes = Mathf.Clamp(lifes, 0, 10);

        if (onChanceLife != null)
            onChanceLife();
    }

    public void RemoveLife(int i)
    {
        lifes -= i;

        lifes = Mathf.Clamp(lifes, 0, lifes);

        if (onChanceLife != null)
            onChanceLife();
    }
}
