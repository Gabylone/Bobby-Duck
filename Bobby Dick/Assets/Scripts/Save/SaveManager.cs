using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

	public delegate void OnSave ();
	public delegate void OnLoad ();
	public static OnSave onSave;
	public static OnLoad onLoad;

	GameData gameData;

	public GameData GameData {
		get {
			return gameData;
		}
	}

	void Awake () {
		Instance = this;
	}

	void Start () {
		
		gameData = new GameData ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

	}

	void HandleChunkEvent ()
	{
		Invoke ("HandleChunkEventDelay",0.01f);
	}

	void HandleChunkEventDelay () {

		SaveGameData ();

		SaveTool.Instance.SaveSpecificChunks (Coords.current);

	}

	#region load
	public void LoadGame () {

		MapGenerator.Instance.LoadIslandsData ();

		LoadGameData ();

		if (onLoad != null)
			onLoad ();
		NavigationManager.Instance.ChangeChunk (Directions.None);

	}

	public void LoadGameData () {

		// GAME DATA
		gameData = SaveTool.Instance.LoadFromPath ("game data" , "GameData") as GameData;

		// player crew
		Crews.Instance.LoadPlayerCrew ();

		FormulaManager.Instance.LoadFormulas ();

		// player loot
		LootManager.Instance.setLoot (Crews.Side.Player, gameData.playerLoot);

		QuestManager.Instance.currentQuests = gameData.currentQuests;
		QuestManager.Instance.finishedQuests = gameData.finishedQuests;

		Member.globalID = gameData.globalID;

		// gold
		GoldManager.Instance.LoadGold();

		Karma.Instance.LoadKarma ();

		TimeManager.Instance.Load ();


	}
	#endregion

	#region save
	public void SaveOverallGame () {

		if (onSave != null)
			onSave ();

//		print ("saving overall game");

		SaveGameData ();

		SaveTool.Instance.SaveAllChunks ();

	}

	void SaveGameData () {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		FormulaManager.Instance.SaveFormulas ();

		gameData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		gameData.currentQuests = QuestManager.Instance.currentQuests;
		gameData.finishedQuests = QuestManager.Instance.finishedQuests;

		gameData.globalID = Member.globalID;

		// gold
		GameData.playerGold = GoldManager.Instance.goldAmount;

		MapGenerator.Instance.SaveImportantIslandPositions ();

		Karma.Instance.SaveKarma ();

		TimeManager.Instance.Save ();

		SaveTool.Instance.SaveToPath ("game data",gameData);

//		SaveTool.Instance.SaveGameData ();

	}
	#endregion

}


//[System.Serializable]
public class GameData
{
	// crew & loot
	public int					globalID = 0;
	public Crew 				playerCrew;
	public Loot 				playerLoot;

	public int 					playerWeight = 0;
	public int 					playerGold = 0;

	public int 					karma = 0;
	public int 					bounty = 0;

	// islands
	public MapData 				mapData;

	public Formula[] 			formulas;

	public PlayerBoatInfo 		playerBoatInfo;

	// quests
	public List<Quest> 			currentQuests;
	public List<Quest>			finishedQuests;

		// time
	public bool 				raining = false;
	public int 					currentRain = 0;

	public bool 				night = false;
	public int 					timeOfDay = 0;


	public GameData()
	{
		// islands ids
	}
}