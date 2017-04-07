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
//				newStory.freq = int.TryParse (rowContent [1],);

				int frequence = 0;

				bool canParse = int.TryParse (rowContent [1], out frequence);

				if ( canParse== false){ 
					print ("ne peut pas parse la freq dans : " + newStory.name);
				}

				newStory.freq = frequence;

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
						newStory.nodes.Add (new Node (markName, collumnIndex, (rowIndex-2)));


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
