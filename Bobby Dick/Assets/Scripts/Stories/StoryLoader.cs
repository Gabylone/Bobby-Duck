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
	private List<Story> boatStories 	= new List<Story> ();
	private List<float> storyPercents 	= new List<float> ();

	[SerializeField]
	private string pathToCSVs = "Stories/CSVs";
	private TextAsset[] storyFiles;
	[SerializeField]
	private TextAsset functionFile;
	private int currentFile = 1;


	[SerializeField]
	private StoryFunctions storyFunctions;

	void Awake () {
		
		Instance = this;

		LoadFunctions ();
		LoadStories ();

	}


	public void LoadStories ()
	{

		GetFiles ();
		LoadSheets ();
	}

	private void GetFiles ()
	{
		currentFile = 1;

		storyFiles = new TextAsset[Resources.LoadAll ("Stories/CSVs", typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (pathToCSVs, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}
	}

	float minFreq = 0f;

	private void LoadSheets ()
	{
		stories.Clear ();
		clueStories.Clear ();
		treasureStories.Clear ();
		homeStories.Clear ();
		storyPercents.Clear ();

		for (int i = 1; i < storyFiles.Length; ++i ) {

			LoadSheet ();

			++currentFile;
		}
	}



	private void LoadSheet ()
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

				float frequence = 0f;

//				System.Globalization.NumberStyles n = ;
				bool canParse = float.TryParse (rowContent [1] ,out frequence);

				if ( canParse== false){ 
					print ("ne peut pas parse la freq dans : " + newStory.name + " TRY PARSE : " + rowContent[1]);
				}

				newStory.freq = frequence;
				newStory.rangeMin = minFreq;
				newStory.rangeMax = minFreq + frequence;

				minFreq += frequence;

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

		if ( newStory.name.Contains ("Bateau") ) {
			boatStories.Add (newStory);
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

	private void LoadFunctions () {
		
		string[] rows = functionFile.text.Split ( '\n' );

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

	public Story RandomStory (int x , int y) {

			// check if treasure island
		if (x == MapData.Instance.treasureIslandXPos &&
			y == MapData.Instance.treasureIslandXPos ) {

			if (treasureStories.Count == 0)
				Debug.LogError ("no treasure stories");
			
			return treasureStories [Random.Range (0, treasureStories.Count)];

		}

			// check for home island
		if (x == MapData.Instance.homeIslandXPos &&
			y == MapData.Instance.homeIslandYPos ) {

			if (homeStories.Count == 0)
				Debug.LogError ("no home stories");
			
			return homeStories [Random.Range (0,homeStories.Count)];

		}

		// check if clue island
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

			if (x == MapData.Instance.clueIslandsXPos[i] &&
				y == MapData.Instance.clueIslandsYPos[i] ) {

				if (clueStories.Count == 0)
					Debug.LogError ("no clue stories");
				
				return clueStories[Random.Range (0,clueStories.Count)];

			}
		}


		float random = Random.value * 100f;

		foreach (Story story in stories) {
			if (random < story.rangeMax && random > story.rangeMin) {

//				print ("RANDOM : " + random);
//				print ("story range max : " + story.rangeMax);
//				print ("story range min : " + story.rangeMin);
//
//				print (story.name);
				return story;

			}

		}
		return Stories [Random.Range (0,stories.Count)];
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


}
