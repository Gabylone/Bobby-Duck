using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;

	private List<Story> stories 		= new List<Story> ();
	private List<Story> clueStories 	= new List<Story> ();
	private List<Story> treasureStories = new List<Story> ();
	private List<Story> homeStories 	= new List<Story> ();
	private List<int> storyPercents 	= new List<int> ();

	[SerializeField]
	private string pathToCSVs = "Stories/CSVs";
	private TextAsset[] storyFiles;
	private int currentFile = 1;

	[SerializeField]
	private Text storyVisualizer;

	[SerializeField]
	private StoryFunctions storyFunctions;

	void Awake () {
		
		Instance = this;

		storyFiles = new TextAsset[Resources.LoadAll ("Stories/CSVs", typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (pathToCSVs, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}

		for (int i = 1; i < storyFiles.Length; ++i ) {
			if (currentFile == 1) {
				LoadFunctions ();
			} else {
				LoadStories ();
			}

			++currentFile;
		}
	}

	void LoadStories ()
	{
		string[] rows = storyFiles[currentFile].text.Split ('\n');

		int collumnIndex 	= 0;

		Story newStory = new Story (0, "name");

		for (int rowIndex = 1; rowIndex < rows.Length; ++rowIndex ) {

			string[] rowContent = rows[rowIndex].Split (';');

			// create story
			if (rowIndex == 1) 
			{
				newStory.storyID = currentFile;
				newStory.name = rowContent [0];
				newStory.freq = int.Parse (rowContent [1]);

				foreach (string cellContent in rowContent) {

					newStory.content.Add (new List<string> ());
					newStory.contentDecal.Add (new List<int> ());
				}
			}
			else
			{
				foreach (string cellContent in rowContent) {

					if ( cellContent.Length > 0 && cellContent[0] == '[' ) {
						string markName = cellContent.Remove (0, 1).Remove (cellContent.IndexOf (']')-1);
						newStory.marks.Add (new Mark (markName, collumnIndex, (rowIndex-2)));


					}

					newStory.content [collumnIndex].Add (cellContent);
					newStory.contentDecal [collumnIndex].Add (-1);

					++collumnIndex;

				}
			}

			collumnIndex = 0;

		}

		if ( newStory.name.Contains ("Indice") ) {
			clueStories.Add (newStory);
			return;
		}

		if ( newStory.name.Contains ("Trésor") ) {
			treasureStories.Add (newStory);
			return;
		}

		if ( newStory.name.Contains ("Maison") ) {
			homeStories.Add (newStory);
			return;
		}

		stories.Add (newStory);
		storyPercents.Add (newStory.freq);
	}

	void LoadFunctions () {
		
		string[] rows = storyFiles[currentFile].text.Split ( '\n' );

		storyFunctions.FunctionNames = new string[rows.Length-1];

		for (int row = 0; row < storyFunctions.FunctionNames.Length; ++row ) {
			storyFunctions.FunctionNames [row] = rows [row].Split (';') [0];

		}
	}

	#region properties
	public string GetContent {
		get {

			Story targetStory = IslandManager.Instance.CurrentIsland.Story;

			if ( StoryReader.Instance.Decal >= targetStory.content.Count ) {

				Debug.LogError ("DECAL is outside of story << " + targetStory.name + " >> content : DECAL : " + StoryReader.Instance.Decal + " /// COUNT : " + targetStory.content.Count);

				return targetStory.content
				[0]
				[0];

			}

			if ( StoryReader.Instance.Index >= targetStory.content [StoryReader.Instance.Decal].Count ) {

				Debug.LogError ("INDEX is outside of story content : INDEX : " + StoryReader.Instance.Index + " /// COUNT : " + targetStory.content[StoryReader.Instance.Decal].Count);

				return targetStory.content
				[StoryReader.Instance.Decal]
				[0]; 
			}

			return targetStory.content
				[StoryReader.Instance.Decal]
				[StoryReader.Instance.Index];
		}
	}
	public List<Story> Stories {
		get {
			return stories;
		}
		set {
			stories = value;
		}
	}
	#endregion

	public Story RandomStory {

		// IDEE : Pick story: 
		// chaque catégorie à un range ( et la fréquence s'applque au trésor et aux indices )

		get {

				// check if treasure island
			if (MapManager.Instance.PosX == IslandManager.Instance.TreasureIslandXPos &&
				MapManager.Instance.PosY == IslandManager.Instance.TreasureIslandYPos ) {

//				Debug.Log ("treasure island");

				if (treasureStories.Count == 0)
					Debug.LogError ("no treasure stories");
				return treasureStories [Random.Range (0, treasureStories.Count)];

			}

				// check for home island
			if (MapManager.Instance.PosX == IslandManager.Instance.HomeIslandXPos &&
				MapManager.Instance.PosY == IslandManager.Instance.HomeIslandYPos ) {

//				Debug.Log ("home island");

				if (homeStories.Count == 0)
					Debug.LogError ("no home stories");
				
				return homeStories [Random.Range (0,homeStories.Count)];

			}

			// check if clue island
			for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

				if (MapManager.Instance.PosX == IslandManager.Instance.ClueIslandsXPos[i] &&
					MapManager.Instance.PosY == IslandManager.Instance.ClueIslandsYPos[i] ) {

					if (clueStories.Count == 0)
						Debug.LogError ("no clue stories");
					
					return clueStories[Random.Range (0,clueStories.Count)];

				}
			}

			// set random story
			int storyIndex = Percentage.getRandomIndex (storyPercents.ToArray());
//			Debug.Log ("index chosen from % : " + storyIndex);
			return Stories [storyIndex];
		}
	}

	public List<Story> TreasureStories {
		get {
			return treasureStories;
		}
	}
}

[System.Serializable]
public class Story {

	public int 		storyID 	= 0;
	public string 	name 		= "";
	public int 		freq 		= 0;

	public List<List<string>> content = new List<List<string>>();
	public List<List<int>> contentDecal = new List<List<int>>();
	public List<Mark> marks = new List<Mark> ();

	public Story ()
	{
		
	}

	public Story (
		int _storyID,
		string _name
	)
	{
		storyID = _storyID;
		name = _name;
	}

}

[System.Serializable]
public class Mark {

	public string name;
	public int x, y;

	public bool switched;

	public Mark ( string n, int p1 , int p2 ) {
		name = n;
		x = p1;
		y = p2;
		switched = false;
	}

}