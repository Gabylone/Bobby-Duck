using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;
	StoryFunctions storyFunction;

	Story[] stories;
	int[] storyRate;

	List<List<string>> content = new List<List<string>>();

	[SerializeField]
	private string pathToCSVs = "Stories/CSVs";
	private TextAsset[] storyFiles;
	private int currentFile = 0;

	[SerializeField]
	private Text storyVisualizer;

	void Awake () {
		
		Instance = this;

		storyFunction = GetComponent<StoryFunctions> ();

		storyFiles = new TextAsset[Resources.LoadAll ("Stories/CSVs", typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (pathToCSVs, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}

		stories = new Story[ storyFiles.Length ];
		storyRate = new int[storyFiles.Length];


		for (int i = 0; i < storyFiles.Length; ++i ) {
			if (currentFile == 0) {
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
		for (int rowIndex = 1; rowIndex < rows.Length; ++rowIndex ) {

			string[] rowContent = rows[rowIndex].Split (';');

			// create story
			if (rowIndex == 1) 
			{
				stories [currentFile] = new Story (currentFile, rowContent [0]);
				storyRate [currentFile] = int.Parse (rowContent [1]);

				foreach (string cellContent in rowContent) {
					stories [currentFile].content.Add (new List<string> ());
					stories [currentFile].contentDecal.Add (new List<int> ());
				}

			}
			else
			{

				foreach (string cellContent in rowContent) {

					stories [currentFile].content [collumnIndex].Add (cellContent);
					stories [currentFile].contentDecal [collumnIndex].Add (-1);

					++collumnIndex;

				}
			}

			collumnIndex 	= 0;

		}
	}

	void LoadFunctions () {
		
		string[] rows = storyFiles[currentFile].text.Split ( '\n' );

		storyFunction.FunctionNames = new string[rows.Length-1];

		for (int row = 0; row < storyFunction.FunctionNames.Length; ++row ) {

			storyFunction.FunctionNames [row] = rows [row].Split (';') [0];

		}
	}

	#region properties
	public string GetContent {
		get {
			return CurrentIslandStory.content
				[StoryReader.Instance.Decal]
				[StoryReader.Instance.Index];
		}
	}
	public int SaveDecal {
		get {
			return CurrentIslandStory.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index];
		}
		set {
			CurrentIslandStory.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index] = value;
		}
	}
	public string ReadDecal (int decal) {

		return CurrentIslandStory.content
			[decal]
			[StoryReader.Instance.Index]; 

	}
	public Story[] Stories {
		get {
			return stories;
		}
		set {
			stories = value;
		}
	}
	#endregion

	public Story CurrentIslandStory {
		get {
			int id = MapGenerator.Instance.IslandIds [MapManager.Instance.PosX, MapManager.Instance.PosY];
			return MapGenerator.Instance.IslandDatas [id].Story;
		}
		set {
			int id = MapGenerator.Instance.IslandIds [MapManager.Instance.PosX, MapManager.Instance.PosY];
			if ( id > MapGenerator.Instance.IslandDatas.Count || id < 0 ) {
				Debug.LogError ( "Merde : " + id.ToString () + " Longueur " + MapGenerator.Instance.IslandDatas.Count );
			}
			MapGenerator.Instance.IslandDatas [id].Story = value;
		}
	}

	public Story RandomStory {

		// IDEE : Pick story: 
		// chaque catégorie à un range ( et la fréquence s'applque au trésor et aux ineices )

		get {

				// check if treasure island
			if (MapManager.Instance.PosX == IslandManager.Instance.TreasureIslandXPos &&
				MapManager.Instance.PosY == IslandManager.Instance.TreasureIslandYPos ) {

				return stories [2];

			}

				// check for home island
			if (MapManager.Instance.PosX == IslandManager.Instance.HomeIslandXPos &&
				MapManager.Instance.PosY == IslandManager.Instance.HomeIslandYPos ) {

				return stories [1];

			}

			// check if clue island
			for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

				if (MapManager.Instance.PosX == IslandManager.Instance.ClueIslandsXPos[i] &&
					MapManager.Instance.PosY == IslandManager.Instance.ClueIslandsYPos[i] ) {

					return stories [3];

				}
			}

			// set random story
			int storyIndex = Random.Range (4, StoryLoader.Instance.Stories.Length);

			if ( storyRate[storyIndex] == 0 ) {
				int i = storyIndex + 1;

				while ( storyRate[i] == 0 ) {

					if (i == storyIndex) {
						Debug.LogError ("No more story frequency");
						break;
					}

					if ( storyRate[i] > 0 ) {
						storyIndex = i;
						break;
					}

					++i;

					if (i == StoryLoader.Instance.stories.Length)
						i = 0;
				}
			}

			storyRate [storyIndex]--;

			return Stories [storyIndex];
		}
	}
}

[System.Serializable]
public class Story {

	public int 		storyID 	= 0;
	public string 	name 	= "";

	public List<List<string>> content = new List<List<string>>();
	public List<List<int>> contentDecal = new List<List<int>>();

	public Story (
		int _storyID,
		string _name
	)
	{
		storyID = _storyID;
		name = _name;
	}

}