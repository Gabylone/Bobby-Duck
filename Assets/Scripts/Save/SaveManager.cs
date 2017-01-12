using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;

	public delegate void LoadGameData ();
	public event LoadGameData loadData;

	void Awake () {
		Instance = this;

		currentData = new GameData ();
	}


	#region action
	public void LoadGame (int index) {
		currentData = SaveTool.Instance.Load (index);

		if (loadData != null)
			loadData ();
	}
	#endregion

	GameData currentData;

	public GameData CurrentData {
		get {
			return currentData;
		}
	}

}

[System.Serializable]
public class GameData
{
	public Crew playerCrew;

	public GameData()
	{
//		guyName = "aucune";
//		guyColor = Color.black;
	}
}