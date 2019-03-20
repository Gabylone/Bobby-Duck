using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory Instance;

    public bool loadOnStart = true;

	[Header("General")]
    public int gold = 0;
    public int diamonds = 0;
    public int lifes = 3;
    public int progress = 1;
    public int[] starAmounts;

	[Header("Items")]
	public int barricadeAmount = 1;
	public int maxSoldierAmount = 10;

	[Header("Info")]
    public bool showedMultGold = false;
    public int highscore = 0;
    public int goldAquiredInLevel = 0;
	public static bool multiplyGold = false;
	public bool portrait = false;
    public bool displayedInvitation = false;


	[Header("Soldiers")]
	public SoldierInfo soldierInfo_Player;
	public List<SoldierInfo> soldierInfos = new List<SoldierInfo>();

	/// <summary>
	/// Language.
	/// </summary>
    public enum LanguageType
    {
        French,
        English,

        None,
    }
    public static LanguageType currentLanguageType = LanguageType.None;


	/// <summary>
	/// The aquired apparence items.
	/// </summary>
	[Header("Apparence Items")]
    public ApparenceItem[] aquiredApparenceItems;
    public int[] apparenceIDs = new int[4];

	/// <summary>
	/// events
	/// </summary>
    public delegate void OnChangeGold();
    public OnChangeGold onChangeGold;

    public delegate void OnChangeDiamonds();
    public OnChangeDiamonds onChangeDiamonds;

    public delegate void OnChanceLife();
    public OnChanceLife onChanceLife;

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
        PlayerPrefs.SetInt("barricadeAmount", barricadeAmount);
        PlayerPrefs.SetInt("languageType", (int)currentLanguageType);
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.SetInt("displayedInvitation", displayedInvitation ? 1 : 0);
        PlayerPrefs.SetInt("enableSound", SoundManager.Instance.playSounds ? 1 : 0);
        PlayerPrefs.SetInt("showedMultGold", showedMultGold ? 1 : 0);
		PlayerPrefs.SetInt("portrait", portrait ? 1 : 0);

        SaveLifes();
		SaveSoldiers ();
        SaveBlobApparence();

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

    public void Load()
    {
        gold = PlayerPrefs.GetInt("gold");
        diamonds = PlayerPrefs.GetInt("diamonds");
        lifes = PlayerPrefs.GetInt("lifes",5);
        progress = PlayerPrefs.GetInt("progress",1);
        barricadeAmount = PlayerPrefs.GetInt("barricadeAmount",1);
        highscore = PlayerPrefs.GetInt("highscore", 0);
        displayedInvitation = PlayerPrefs.GetInt("displayedInvitation", 0) == 1;
		showedMultGold = PlayerPrefs.GetInt("showedMultGold", 0) == 1;

		// sounds
        bool playSounds = PlayerPrefs.GetInt("enableSound", 1) == 1;
        SoundManager.Instance.playSounds = playSounds;
        Music.Instance.source.enabled = playSounds;


		// screen orientation
		portrait = PlayerPrefs.GetInt("portrait", 1) == 1;
		UpdateScreenOrientation ();

		LoadLanguage ();
		LoadSoldiers ();
		LoadBlobApparence();

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



    public void SetStarAmount ( int levelID , int starAmount)
    {
        if ( starAmount > starAmounts[levelID])
        {
            starAmounts[levelID] = starAmount;
        }
    }

	#region language
	void LoadLanguage ()
	{
		currentLanguageType =  (LanguageType)PlayerPrefs.GetInt("languageType", (int)LanguageType.None);

		Invoke ("LoadLanguageDelay", 0.1f);
	}
	void LoadLanguageDelay () {

		if (currentLanguageType == LanguageType.None && DisplayLanguage.Instance != null)
		{
			DisplayLanguage.Instance.Open();
		}

	}
	#endregion

    #region blob apparence
    private void InitAquiredItems()
    {
        aquiredApparenceItems = new ApparenceItem[(int)BlobApparence.Type.None];
        for (int i = 0; i < aquiredApparenceItems.Length; i++)
        {
            aquiredApparenceItems[i] = new ApparenceItem();
            aquiredApparenceItems[i].type = (BlobApparence.Type)i;
        }   
    }
    public void SaveBlobApparence()
    {
        // CURRENT BLOB ITEMS
        int a = 0;
        foreach (var id in DisplayCharacterCustomization.Instance.blob_Apparence.ids)
        {
            BlobApparence.Type type = (BlobApparence.Type)a;

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

    public void AddIDToApparenceItem(BlobApparence.Type type, int id)
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
        apparenceIDs = new int[(int)BlobApparence.Type.None];

        for (int typeIndex = 0; typeIndex < (int)BlobApparence.Type.None; typeIndex++)
        {
            BlobApparence.Type type = (BlobApparence.Type)typeIndex;

            int id = PlayerPrefs.GetInt("current_" + type.ToString(), 0);

            apparenceIDs[typeIndex] = id;

        }

        bool loadedSomething = false;

        // AQUIRED ITEMS
        for (int typeIndex = 0; typeIndex < (int)BlobApparence.Type.None; typeIndex++)
        {
            BlobApparence.Type type = (BlobApparence.Type)typeIndex;

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
            aquiredApparenceItems[(int)BlobApparence.Type.Head].ids.Add(0);
            aquiredApparenceItems[(int)BlobApparence.Type.Head].ids.Add(1);

            aquiredApparenceItems[(int)BlobApparence.Type.Eyes].ids.Add(0);
            aquiredApparenceItems[(int)BlobApparence.Type.Eyes].ids.Add(1);

            aquiredApparenceItems[(int)BlobApparence.Type.EyesAccessories].ids.Add(0);
            aquiredApparenceItems[(int)BlobApparence.Type.EyesAccessories].ids.Add(1);

            for (int i = 0; i < BlobApparenceManager.Instance.blobColors.Length; i++)
            {
                aquiredApparenceItems[(int)BlobApparence.Type.Body].ids.Add(i);
            }

        }
    }
    #endregion

	#region barricade
    public void AddBarricade ()
    {
        ++barricadeAmount;
    }
	public void RemoveBarricade ()
	{
		--barricadeAmount;
	}
	#endregion

	#region soldiers
	public void AddSoldier (SoldierInfo soldierInfo) {

		soldierInfos.Add ( soldierInfo );

		DisplaySoldiers.Instance.UpdateDisplay ();

	}
	public void RemoveSoldier() {
		//
	}

	void SaveSoldiers() {

        Debug.Log("saving");

		PlayerPrefs.SetString ("soldierInfo_Player", soldierInfo_Player.Serialize() );

		for (int i = 0; i < soldierInfos.Count; i++) {

			PlayerPrefs.SetString ("soldierInfo_" + i , soldierInfos[i].Serialize() );

		}

	}
	void LoadSoldiers () {


		string playerInfo_Str = PlayerPrefs.GetString ("soldierInfo_Player", "null");

		if (playerInfo_Str != "null") {
			soldierInfo_Player = SoldierInfo.Deserialize (playerInfo_Str);
		} else {
            soldierInfo_Player = new SoldierInfo();
            soldierInfo_Player.Init();
		}

		// soldiers
		string str = "";

		int i = 0;

		while (true) {

			str = PlayerPrefs.GetString ("soldierInfo_" + i , "null" );

			if (str == "null") {
				break;
			} else {

				SoldierInfo newSoldierInfo = SoldierInfo.Deserialize (str);
				soldierInfos.Add(newSoldierInfo);


				++i;
			}
		}
	}
	#endregion

	#region gold
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
	#endregion

	#region diamonds
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
	#endregion

	#region lives
	public void SaveLifes()
	{
		PlayerPrefs.SetInt("lifes", lifes);
	}
    public void AddLives(int i)
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
	#endregion
}
