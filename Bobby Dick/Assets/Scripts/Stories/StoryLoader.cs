using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;

	private List<Story> islandStories 	= new List<Story> ();
	private List<Story> clueStories 	= new List<Story> ();
	private List<Story> treasureStories = new List<Story> ();
	private List<Story> homeStories 	= new List<Story> ();
	private List<Story> boatStories 	= new List<Story> ();
	private List<Story> quests 	= new List<Story> ();

	public bool checkNodes = false;

	private TextAsset[] storyFiles;
	[SerializeField]
	private TextAsset functionFile;

	private float minFreq = 0f;

	[SerializeField]
	private StoryFunctions storyFunctions;

	void Awake () {
		
		Instance = this;

		LoadStories ();

	}

	#region load
	private void GetFiles (string path)
	{
		storyFiles = new TextAsset[Resources.LoadAll (path, typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (path, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}
	}

	public void LoadStories ()
	{
		LoadSheets (	islandStories	, 		"Stories/CSVs/IslandStories"	);
		LoadSheets (	boatStories		, 		"Stories/CSVs/BoatStories"		);
		LoadSheets (	homeStories		, 		"Stories/CSVs/HomeStories"		);
		LoadSheets (	clueStories		, 		"Stories/CSVs/ClueStories"		);
		LoadSheets (	treasureStories	, 		"Stories/CSVs/TreasureStories"	);
		LoadSheets (	quests			, 		"Stories/CSVs/Quests"			);
	}

	private void LoadSheets (List<Story> storyList , string path)
	{
		minFreq = 0f;

		GetFiles (path);

		for (int i = 0; i < storyFiles.Length; ++i )
			storyList.Add(LoadSheet (i));
	}
 	#endregion

	#region sheet
	private Story LoadSheet (int index)
	{
		string[] rows = storyFiles[index].text.Split ('\n');

		int collumnIndex 	= 0;

		Story newStory = new Story ("name");

		for (int rowIndex = 1; rowIndex < rows.Length; ++rowIndex ) {

			string[] rowContent = rows [rowIndex].Split (';');

			// create story
			if (rowIndex == 1) 
			{
				newStory.name = rowContent [0];

				float frequence = 0f;

				bool canParse = float.TryParse (rowContent [1] ,out frequence);

				if ( canParse== false){ 
					print ("ne peut pas parse la freq dans : " + newStory.name + " TRY PARSE : " + rowContent[1]);
				}

				frequence = (frequence/100);

					// set story frequence
				newStory.freq = frequence;
				newStory.rangeMin = minFreq;
				newStory.rangeMax = minFreq + newStory.freq;

				minFreq += newStory.freq;

				bool containsParam = int.TryParse (rowContent [2], out newStory.param);
				if (containsParam == false ) {
					
					print("sprite id pas parcable : (" + rowContent[2] + ") dans l'histoire " + newStory.name);

					print (rowContent [0]);
					print (rowContent [1]);
					print (rowContent [2]);
					print (rowContent [3]);

					print (index);

				}

//				newStory.spriteID = int.Parse (rowContent [2]);

				foreach (string cellContent in rowContent) {
					newStory.content.Add (new List<string> ());
				}
			}
			else
			{
				foreach (string cellContent in rowContent) {

					string txt = cellContent;

					if ( collumnIndex == rowContent.Length - 1) {
						txt = txt.TrimEnd ('\r', '\n', '\t');
					}

					if ( txt.Length > 0 && txt[0] == '[' ) {
						string markName = txt.Remove (0, 1).Remove (txt.IndexOf (']')-1);
						newStory.nodes.Add (new Node (markName, collumnIndex, (rowIndex-2)));
					}

					newStory.content [collumnIndex].Add (txt);

					++collumnIndex;

				}
			}

			collumnIndex = 0;

		}

		return newStory;
	}
	#endregion
//
	#region random story from position
	public Story RandomStory (Coords c) {
		return IslandStories[RandomStoryIndex (c)];
	}
	public StoryType GetTypeFromPos (Coords coords)
	{
		if (coords == MapData.Instance.treasureIslandCoords ) {
			return StoryType.Treasure;
		}

		// check for home island
		if (coords == MapData.Instance.homeIslandCoords ) {
			return StoryType.Home;
		}

		// check if clue island
		foreach (var formula in FormulaManager.Instance.formulas) {
			if ( coords == formula.coords){
				return StoryType.Clue;
			}
		}

		return StoryType.Normal;
	}
	public int RandomStoryIndex (Coords c)
	{
		if (c == MapData.Instance.treasureIslandCoords ) {

			if (treasureStories.Count == 0)
				Debug.LogError ("no treasure stories");

			return getStoryIndexFromPercentage(treasureStories);

		}

		// check for home island
		if (c == MapData.Instance.homeIslandCoords ) {

			if (homeStories.Count == 0)
				Debug.LogError ("no home stories");

			return getStoryIndexFromPercentage (homeStories);

		}

		foreach (var formula in FormulaManager.Instance.formulas) {
			if ( c == formula.coords){
				if (clueStories.Count == 0)
					Debug.LogError ("no clue stories");

				return getStoryIndexFromPercentage (clueStories);
			}
		}

		return getStoryIndexFromPercentage (islandStories);
	}
	#endregion

	#region percentage
	public List<Story> getStories ( StoryType storyType ) {

		switch (storyType) {
		case StoryType.Normal:
			return IslandStories;
			break;
		case StoryType.Treasure:
			return TreasureStories;
			break;
		case StoryType.Home:
			return HomeStories;
			break;
		case StoryType.Clue:
			return ClueStories;
			break;
		case StoryType.Boat:
			return BoatStories;
			break;
		case StoryType.Quest:
			return Quests;
			break;
		default:
			return IslandStories;
			break;
		}
	}
	public int getStoryIndexFromPercentage ( StoryType type ) {
		return getStoryIndexFromPercentage (getStories(type));
	}
	public int getStoryIndexFromPercentage ( List<Story> stories ) {

		float random = Random.value * 100f;

		int a = 0;

		foreach (Story story in stories) {
			if (random < story.rangeMax && random >= story.rangeMin) {
				return a;
			}

			++a;
		}

		return Random.Range (0,stories.Count);
	}
	#endregion



	public Story FindByName (string storyName, StoryType type)
	{
		int index = FindIndexByName (storyName,type);

		if (index < 0)
			return null;

		return getStories(type)[index];
	}

	public int FindIndexByName (string storyName,StoryType storyType)
	{
		int storyIndex = getStories (storyType).FindIndex (x => x.name == storyName);

		if (storyIndex < 0) {
			Debug.LogError ("coun't find story / " + storyName + " /, returning null");
		}

		return storyIndex;
	}


	#region story getters
	public List<Story> IslandStories {
		get {
			return islandStories;
		}
		set {
			islandStories = value;
		}
	}
	public List<Story> TreasureStories {
		get {
			return treasureStories;
		}
	}

	public List<Story> BoatStories {
		get {
			return boatStories;
		}
	}
	public List<Story> ClueStories {
		get {
			return clueStories;
		}
	}

	public List<Story> HomeStories {
		get {
			return homeStories;
		}
	}


	public List<Story> Quests {
		get {
			return quests;
		}
	}
	#endregion
}
