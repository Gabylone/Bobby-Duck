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
	}

	#region action
	public void LoadGame (int index) {

		currentData = SaveTool.Instance.Load (index);

		StartCoroutine (LoadGameCoroutine ());
	}

	IEnumerator LoadGameCoroutine () {

		Transitions.Instance.ScreenTransition.Switch ();

		yield return new WaitForEndOfFrame ();

		// player crew
		Crews.Instance.LoadPlayerCrew ();

		yield return new WaitForEndOfFrame ();

		// boat position
		MapManager.Instance.LoadBoatPosition ();

		yield return new WaitForEndOfFrame ();

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.LoadIslandsData ();

		yield return new WaitForEndOfFrame ();

		// player loot
		LootManager.Instance.setLoot (Crews.Side.Player, currentData.playerLoot);

		yield return new WaitForEndOfFrame ();

		// gold
		GoldManager.Instance.GoldAmount = CurrentData.playerGold;

		yield return new WaitForEndOfFrame ();

		WeatherManager.Instance.LoadWeather ();

		Transitions.Instance.ScreenTransition.Switch ();

	}
	public void SaveGame (int index) {

		// player crew
		Crews.Instance.SavePlayerCrew ();

		// boat position
		MapManager.Instance.SaveBoatPosition ();

		// island ids
		// island datas
		// special island positions
		MapGenerator.Instance.SaveIslandsData ();

		// player loot
		currentData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		// gold
		CurrentData.playerGold = GoldManager.Instance.GoldAmount;

		WeatherManager.Instance.SaveWeather ();

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
	public Crew playerCrew;

	public List<IslandData> islandsData = new List<IslandData> ();
	public byte[] islandIDs;

	public int[] clueIslandsXPos;
	public int[] clueIslandsYPos;

	public int treasureIslandXPos = 0;
	public int treasureIslandYPos = 0;

	public int homeIslandXPos = 0;
	public int homeIslandYPos = 0;

	public int boatPosX = 0;
	public int boatPosY = 0;

	public Loot playerLoot;

	public int playerWeight = 0;
	public int playerGold = 0;

	public bool raining = false;
	public int currentRain = 0;

	public bool night = false;
	public int currentNight = 0;

	public GameData()
	{
		// islands ids
	}
}