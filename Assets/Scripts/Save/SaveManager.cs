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

		Transitions.Instance.ScreenTransition.Switch ();

		Invoke ("LoadGameDelay" , 1);
	}

	private void LoadGameDelay () {
		if (loadData != null)
			loadData ();

		Transitions.Instance.ScreenTransition.Switch ();

	}
	public void SaveGame (int index) {

		if (saveData != null)
			saveData ();

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

	public GameData()
	{
		
	}
}