using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

	public delegate void LoadGameData ();
	public event LoadGameData loadData;

	public delegate void SaveGameData ();
	public event SaveGameData saveData;

	void Awake () {
		Instance = this;
	}

	void Start () {
		currentData = new GameData ();

//		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	#region action
	public void LoadGame () {

		currentData = SaveTool.Instance.Load ();

		LoadEveryThing ();

	}
	public void LoadGameCoroutine() {

		currentData = SaveTool.Instance.Load ();

		StartCoroutine (LoadGameCoroutine_Coroutine ());

	}
	public void LoadEveryThing () {
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

		LoadEveryThing ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		Transitions.Instance.ScreenTransition.Fade = false;

	}
	public void SaveGame () {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		// save boats
		Boats.Instance.SaveBoats();

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.SaveIslandsData ();

		FormulaManager.Instance.SaveFormulas ();

		currentData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		currentData.currentQuests = QuestManager.Instance.CurrentQuests;
		currentData.finishedQuests = QuestManager.Instance.FinishedQuests;

		// gold
		CurrentData.playerGold = GoldManager.Instance.GoldAmount;

		Karma.Instance.SaveKarma ();

		TimeManager.Instance.SaveWeather ();

		SaveTool.Instance.Save ();

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
	// crew
	public Crew 				playerCrew;

	public Chunk[][] 			chunks;

	// islands
	public MapData 				mapData;

	public Formula[] 			formulas;

	public PlayerBoatInfo 		playerBoatInfo;
	public List<OtherBoatInfo>	otherBoatInfos;

	public StoryHandler 		storyHandler;

	public Loot 				playerLoot;

	public List<Quest> 			currentQuests;
	public List<Quest>			finishedQuests;

	public int 					playerWeight = 0;
	public int 					playerGold = 0;

		// time
	public bool 				raining = false;
	public int 					currentRain = 0;

	public bool 				night = false;
	public int 					timeOfDay = 0;

	public int 					karma = 0;
	public int 					bounty = 0;

	public GameData()
	{
		// islands ids
	}
}



//public class WorldData {
//
//	// islands
//	public MapData 				mapData;
//	public Formula[] 			formulas;
//	public List<OtherBoatInfo>	otherBoatInfos;
//
//	// time
//	public bool 				raining = false;
//	public int 					currentRain = 0;
//
//	public bool 				night = false;
//	public int 					timeOfDay = 0;
//
//	public WorldData() {
//		//
//	}
//}
//
//public class PlayerData
//{
//	public Crew 				playerCrew;
//	public PlayerBoatInfo 		playerBoatInfo;
//	public Loot 				playerLoot;
//	public List<Quest> 			currentQuests;
//	public List<Quest>			finishedQuests;
//
//	public int 					playerWeight = 0;
//	public int 					playerGold = 0;
//	public int 					karma = 0;
//	public int 					bounty = 0;
//
//	public PlayerData()
//	{
//		// islands ids
//	}
//}