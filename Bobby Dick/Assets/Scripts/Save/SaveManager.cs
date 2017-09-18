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

	public int currentIndex;

	void Awake () {
		Instance = this;
	}

	void Start () {
		currentData = new GameData ();

		NavigationManager.Instance.EnterNewChunk += HandleChunkEvent;
	}

	int saveLimit = 10;
	int saveCount = 0;
	void HandleChunkEvent ()
	{
		++saveCount;

		if (saveCount > saveLimit) {
			SaveGame (1);

			saveCount = 0;
		}
	}

	#region action
	public void LoadGame (int index) {

		currentData = SaveTool.Instance.Load (index);

		LoadEveryThing ();

	}
	public void LoadGameCoroutine (int index) {

		currentData = SaveTool.Instance.Load (index);

		StartCoroutine (LoadGameCoroutine ());

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

		TimeManager.Instance.LoadWeather ();

		NavigationManager.Instance.ChangeChunk (Directions.None);

	}
	IEnumerator LoadGameCoroutine () {

		Transitions.Instance.ScreenTransition.Fade = true;
		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);

		if (StoryLauncher.Instance.PlayingStory)
			StoryLauncher.Instance.EndStory ();

		LoadEveryThing ();

		yield return new WaitForSeconds (Transitions.Instance.ScreenTransition.Duration);
		Transitions.Instance.ScreenTransition.Fade = false;

	}
	public void SaveGame (int index) {

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

		TimeManager.Instance.SaveWeather ();

		SaveTool.Instance.Save (index);

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
	public Crew 			playerCrew;

	public Chunk[][] 		chunks;

	// islands
	public MapData 			mapData;

	public Formula[] 		formulas;

	public PlayerBoatInfo 	playerBoatInfo;
	public OtherBoatInfo[] 	otherBoatInfos;

	public StoryHandler 	storyHandler;

	public Loot 			playerLoot;

	public List<Quest> 		currentQuests;
	public List<Quest>		finishedQuests;

	public int 				playerWeight = 0;
	public int 				playerGold = 0;

		// time
	public bool 			raining = false;
	public int 				currentRain = 0;

	public bool 			night = false;
	public int 				timeOfDay = 0;

	public GameData()
	{
		// islands ids
	}
}