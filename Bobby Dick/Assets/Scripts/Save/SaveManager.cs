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
		
		currentData = new GameData ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;

	}

	void HandleChunkEvent ()
	{
		SaveOverallGame ();
	}

	#region load
	public void LoadGame () {

		currentData = SaveTool.Instance.Load ();

		LoadOverallGame ();

	}
	public void LoadGameCoroutine() {

		currentData = SaveTool.Instance.Load ();

		StartCoroutine (LoadGameCoroutine_Coroutine ());

	}
	public void LoadOverallGame () {
		// player crew
		Crews.Instance.LoadPlayerCrew ();

		// boat position
		Boats.Instance.LoadBoats ();

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.LoadIslandsData ();

		FormulaManager.Instance.LoadFormulas ();

		// player loot
		LootManager.Instance.setLoot (Crews.Side.Player, currentData.playerLoot);

		QuestManager.Instance.CurrentQuests = currentData.currentQuests;
		QuestManager.Instance.FinishedQuests = currentData.finishedQuests;

		FormulaManager.Instance.LoadFormulas ();


		// gold
		GoldManager.Instance.LoadGold();

		Karma.Instance.LoadKarma ();

		TimeManager.Instance.LoadWeather ();

		NavigationManager.Instance.ChangeChunk (Directions.None);

	}
	IEnumerator LoadGameCoroutine_Coroutine () {

		Transitions.Instance.ScreenTransition.Fade = true;
		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		if (StoryLauncher.Instance.PlayingStory)
			StoryLauncher.Instance.EndStory ();

		LoadOverallGame ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		Transitions.Instance.ScreenTransition.Fade = false;

	}
	#endregion

	#region save
	public void SaveOverallGame () {

		SaveGameData ();

		SaveAllChunks ();

		SaveTool.Instance.Save ();

	}

	public void SaveGameData () {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		// save boats
		Boats.Instance.SaveBoats();

		FormulaManager.Instance.SaveFormulas ();

		currentData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		currentData.currentQuests = QuestManager.Instance.CurrentQuests;
		currentData.finishedQuests = QuestManager.Instance.FinishedQuests;

		// gold
		CurrentData.playerGold = GoldManager.Instance.GoldAmount;

		Karma.Instance.SaveKarma ();

		TimeManager.Instance.SaveWeather ();

	}

	public void SaveAllChunks () {

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.SaveIslandsData ();

	}
	#endregion

	GameData currentData;

	public GameData CurrentData {
		get {
			return currentData;
		}
	}

}


//[System.Serializable]
public class GameData
{
	// crew & loot
	public Crew 				playerCrew;
	public Loot 				playerLoot;

	public int 					playerWeight = 0;
	public int 					playerGold = 0;

	public Chunk[][] 			chunks;

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