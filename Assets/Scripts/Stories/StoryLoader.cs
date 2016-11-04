using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLoader : MonoBehaviour {

	public static StoryLoader Instance;
	StoryFunctions storyFunction;

	Story[,] islandStories;

	Story[] stories;
//	private int[] storyIDs = 0;
	private int frequencyCount = 0;

	List<List<string>> content = new List<List<string>>();

	[SerializeField]
	private string pathToCSVs = "Data/Stories/CSVs";
	private TextAsset[] storyFiles;
	private int currentFile = 0;

	[SerializeField]
	private Text storyVisualizer;

	void Awake () {
		
		Instance = this;

		storyFunction = GetComponent<StoryFunctions> ();

		storyFiles = new TextAsset[Resources.LoadAll ("Stories/CSVs", typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll ("Stories/CSVs", typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}

		stories = new Story[ storyFiles.Length ];
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
				stories [currentFile] = new Story (currentFile, rowContent [0], int.Parse (rowContent [1]));

				foreach (string cellContent in rowContent) {
					stories [currentFile].content.Add (new List<string> ());
				}

			}
			else
			{

				foreach (string cellContent in rowContent) {

					stories [currentFile].content [collumnIndex].Add (cellContent);
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

	public Story[,] IslandStories {
		get {
			return islandStories;
		}
		set {
			islandStories = value;
		}
	}

	public Story CurrentIslandStory {
		get {
			return IslandStories [MapManager.Instance.PosX, MapManager.Instance.PosY];
		}
		set {
			IslandStories [MapManager.Instance.PosX, MapManager.Instance.PosY] = value;
		}
	}
}

[System.Serializable]
public class Story {

	public int 		storyID 	= 0;
	public string 	name 	= "";

	public int frequency = 0;

	public List<List<string>> content = new List<List<string>>();

	public Story (
		int _storyID,
		string _name,
		int _freq
	)
	{
		storyID = _storyID;
		name = _name;

		frequency = _freq;
	}

}