using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

	public delegate void OnLoad ();
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
		SaveGameData ();
	}

	#region load game data
	public void LoadGame () {

		LoadGameData ();

		LoadAllIslands ();

		if (onLoad != null)
			onLoad ();
		
		NavigationManager.Instance.ChangeChunk (Directions.None);

	}

	public void LoadGameData () {


		// GAME DATA
		gameData = SaveTool.Instance.LoadFromPath ("game data.xml" , "GameData") as GameData;

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

	#region save game data
	public void SaveGameData () {

		FormulaManager.Instance.SaveFormulas ();

		Crews.Instance.SavePlayerCrew ();

		gameData.playerLoot = LootManager.Instance.getLoot (Crews.Side.Player);

		gameData.currentQuests = QuestManager.Instance.currentQuests;
		gameData.finishedQuests = QuestManager.Instance.finishedQuests;

		gameData.globalID = Member.globalID;

		GameData.playerGold = GoldManager.Instance.goldAmount;

		// karma
		Karma.Instance.SaveKarma ();

		TimeManager.Instance.Save ();

		SaveTool.Instance.SaveToPath ("game data",gameData);

		SaveTool.Instance.SaveToPath ("discovered coords", MapGenerator.Instance.discoveredCoords);

	}
	#endregion



	/// <summary>
	/// load
	/// </summary>
	#region Load island data
	public void LoadAllIslands () {

		LoadAllIslandCoroutine ();
	}

	void LoadAllIslandCoroutine () {
		//	IEnumerator LoadAllIslandCoroutine () {

		MapGenerator.Instance.LoadMap ();

		string pathToFolder = SaveTool.Instance.GetSaveFolderPath() + "/Islands";

		var folder = new DirectoryInfo (pathToFolder);
		var files = folder.GetFiles ();

		foreach (var item in files) {

			string pathToFile = "Islands/"+item.Name;
			if (pathToFile [pathToFile.Length - 1] == 'a') {
				continue;
			}

			Chunk chunkToLoad = SaveTool.Instance.LoadFromPath (pathToFile,"Chunk") as Chunk;


			Coords chunkCoords = GetCoordsFromFile (item.Name.Remove ( item.Name.Length - 4 ));

			// attation aux choses qui se passent dans "set island data"
			Chunk.SetChunk(chunkCoords,chunkToLoad);

			//			yield return new WaitForEndOfFrame ();

			//			++l;

			//			LoadingScreen.Instance.Push (l);
		}

		//		yield return new WaitForEndOfFrame ();

	}

	public void SaveAllIslands () {
		//		StartCoroutine (SaveAllIslandsCoroutine ());
		SaveAllIslandsCoroutine ();
	}
	void SaveAllIslandsCoroutine () {

		SaveTool.Instance.ResetIslandFolder ();

		for ( int y = 0; y < MapGenerator.Instance.MapScale ; ++y ) {

			for (int x = 0; x < MapGenerator.Instance.MapScale; ++x ) {

				Coords c = new Coords ( x , y );

				Chunk targetChunk = Chunk.GetChunk (c);

				if (targetChunk.IslandData == null)
					continue;

				string fileName = "chk" + "x" + c.x + "y" + c.y;
				string path = "Islands/" + fileName;

				Coords pathedCoords = GetCoordsFromFile (fileName);

				SaveTool.Instance.SaveToPath (path,targetChunk);
			}

		}

	}
	public void SaveCurrentIsland () {
		//
	}
	public Coords GetCoordsFromFile ( string str ) {

		string s = str.Remove (0, 4);

		string xString = s.Remove(s.IndexOf ('y'));
//		Debug.LogError(" trying x string : " + xString);
		string yString = s.Remove (0, s.IndexOf ('y') + 1);
//		Debug.LogError(" trying prout : " + yString);
//		yString = yString.Remove (yString.IndexOf('.'));

		return new Coords (int.Parse (xString), int.Parse (yString));
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

	public Formula[] 			formulas;

	public PlayerBoatInfo 		playerBoatInfo;

	public Coords treasureCoords;
	public Coords homeCoords;

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