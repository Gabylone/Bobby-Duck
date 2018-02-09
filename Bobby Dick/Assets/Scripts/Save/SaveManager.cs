using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

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

	}

	public void LoadGameData () {

		gameData = SaveTool.Instance.LoadGameData ();

		// player crew
		Crews.Instance.LoadPlayerCrew ();

		// boat position
		Boats.Instance.LoadBoats ();

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

		NavigationManager.Instance.ChangeChunk (Directions.None);

	}
	#endregion

	#region save
	public void SaveOverallGame () {

//		print ("saving overall game");

		SaveGameData ();

		SaveTool.Instance.SaveAllChunks ();

	}

	void SaveGameData () {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		// save boats
		Boats.Instance.SaveBoats();

		FormulaManager.Instance.SaveFormulas ();

		gameData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		gameData.currentQuests = QuestManager.Instance.currentQuests;
		gameData.finishedQuests = QuestManager.Instance.finishedQuests;

		gameData.globalID = Member.globalID;

		// gold
		GameData.playerGold = GoldManager.Instance.GoldAmount;

		MapGenerator.Instance.SaveImportantIslandPositions ();

		Karma.Instance.SaveKarma ();

		TimeManager.Instance.SaveWeather ();

		SaveTool.Instance.SaveGameData ();

	}
	#endregion

	GameData gameData;

	public GameData GameData {
		get {
			return gameData;
		}
	}

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
	public List<OtherBoatInfo>	otherBoatInfos;

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