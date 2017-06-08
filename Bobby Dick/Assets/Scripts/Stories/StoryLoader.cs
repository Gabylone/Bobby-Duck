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

	private TextAsset[] storyFiles;
	[SerializeField]
	private TextAsset functionFile;

	private float minFreq = 0f;

	[SerializeField]
	private StoryFunctions storyFunctions;

	int kek = 0;

	void Awake () {
		
		Instance = this;

		LoadFunctions ();
		LoadStories ();

	}


	public void LoadStories ()
	{
		LoadSheets (	islandStories	, 		"Stories/CSVs/IslandStories"	);
		LoadSheets (	boatStories		, 		"Stories/CSVs/BoatStories"		);
		LoadSheets (	homeStories		, 		"Stories/CSVs/HomeStories"		);
		LoadSheets (	clueStories		, 		"Stories/CSVs/ClueStories"		);
		LoadSheets (	treasureStories	, 		"Stories/CSVs/TreasureStories"	);
	}


	private void LoadSheets (List<Story> storyList , string path)
	{
		minFreq = 0f;

		GetFiles (path);
		for (int i = 0; i < storyFiles.Length; ++i )
			storyList.Add(LoadSheet (i));

		++kek;
	}

	private void GetFiles (string path)
	{
		storyFiles = new TextAsset[Resources.LoadAll (path, typeof(TextAsset)).Length];

		int index = 0;
		foreach ( TextAsset textAsset in Resources.LoadAll (path, typeof(TextAsset) )) {
			storyFiles[index] = textAsset;
			++index;
		}
	}


	
	private Story LoadSheet (int index)
	{
		string[] rows = storyFiles[index].text.Split ('\n');

		int collumnIndex 	= 0;

		Story newStory = new Story ("name");

		for (int rowIndex = 1; rowIndex < rows.Length; ++rowIndex ) {

			string[] rowContent = rows[rowIndex].Split (';');

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

				// story visials
//				VisualStory (newStory);

				minFreq += newStory.freq;

				newStory.spriteID = int.Parse (rowContent [2]);

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

		return newStory;
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
			return islandStories;
		}
		set {
			islandStories = value;
		}
	}
	#endregion

	public Story RandomStory (int x , int y) {

			// check if treasure island
		if (x == MapData.Instance.treasureIslandXPos &&
			y == MapData.Instance.treasureIslandXPos ) {

			if (treasureStories.Count == 0)
				Debug.LogError ("no treasure stories");
			
			return getStoryFromPercentage (treasureStories);

		}

			// check for home island
		if (x == MapData.Instance.homeIslandXPos &&
			y == MapData.Instance.homeIslandYPos ) {

			if (homeStories.Count == 0)
				Debug.LogError ("no home stories");
			
			return getStoryFromPercentage (homeStories);

		}

		// check if clue island
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

			if (x == MapData.Instance.clueIslandsXPos[i] &&
				y == MapData.Instance.clueIslandsYPos[i] ) {

				if (clueStories.Count == 0)
					Debug.LogError ("no clue stories");
				
				return getStoryFromPercentage (clueStories);

			}
		}

		return getStoryFromPercentage (islandStories);

	}

	public Story getStoryFromPercentage ( List<Story> stories ) {

		float random = Random.value * 100f;

		foreach (Story story in stories) {
			if (random < story.rangeMax && random > story.rangeMin) {
				return story;
			}

		}
		return stories [Random.Range (0,stories.Count)];
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


	void VisualStory (Story newStory)
	{
		float scale = 1f;
		//
		GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj.transform.localScale = new Vector3 (newStory.freq, 1f, 1f);
		obj.transform.position = new Vector3 ( minFreq + (newStory.freq/2), kek , 0f );
		obj.GetComponent<Renderer> ().material.color = Random.ColorHSV ();
		obj.name = newStory.name;
	}
}
