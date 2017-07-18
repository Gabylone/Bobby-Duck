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

		LoadFunctions ();
		LoadStories ();

	}

	void Update () {
		if ( Input.GetKeyDown(KeyCode.Insert) ) {
			StartCoroutine (CheckAllNodes ());
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

				// story visials
//				VisualStory (newStory);

				minFreq += newStory.freq;

				newStory.spriteID = int.Parse (rowContent [2]);

				foreach (string cellContent in rowContent) {
					newStory.content.Add (new List<string> ());
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

					++collumnIndex;

				}
			}

			collumnIndex = 0;

		}

		return newStory;
	}

	private void LoadFunctions () {
		
		string[] rows = functionFile.text.Split ( '\n' );

		storyFunctions.FunctionNames = new string[rows.Length-2];

		int functionIndex = 0;

		for (int row = 1; row < rows.Length-1; ++row ) {

			string function = rows [row].Split (';') [0];
			storyFunctions.FunctionNames [functionIndex] = function;

			++functionIndex;

		}
	}

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
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {
			if (coords == MapData.Instance.clueIslandsCoords[i] ) {
				return StoryType.Clue;
			}
		}

		return StoryType.Island;
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

		// check if clue island
		for( int i = 0; i < ClueManager.Instance.ClueAmount ; ++i ) {

			if (c == MapData.Instance.clueIslandsCoords[i] ) {

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
		case StoryType.Island:
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

//		Debug.LogError ("out of percentage, returning random story index : (random : " + random + ")");

		return Random.Range (0,stories.Count);
	}
	#endregion
	void VisualStory (Story newStory)
	{
		float scale = 1f;
		//
		GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj.transform.localScale = new Vector3 (newStory.freq, 1f, 1f);
		obj.transform.position = new Vector3 ( minFreq + (newStory.freq/2), 0 , 0f );
		obj.GetComponent<Renderer> ().material.color = Random.ColorHSV ();
		obj.name = newStory.name;
	}

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

	#region check nodes
	int checkNodes_Decal = 0;
	int checkNodes_Index = 0;
	string alphabet = "abcdefghijklmnopqrstuvwxyz";
	bool checkNodes_Incorrect = false;
	IEnumerator CheckAllNodes ()
	{
		CheckNodes (islandStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (boatStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (homeStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (clueStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (treasureStories);
		yield return new WaitForEndOfFrame ();
		CheckNodes (quests);

		if (!checkNodes_Incorrect) {
			Debug.Log ("Stories & Quests are perfect");
		}
	}

	Story storyToCheck;

	void CheckNodes (List<Story> stories)
	{
		foreach (Story story in stories) {
			storyToCheck = story;
			CheckNodes_Story (story);
		}
	}

	void CheckNodes_Story ( Story story ) {

		checkNodes_Decal = 0;

		foreach (List<string> contents in story.content) {

			CheckNodes_CheckCells (contents);

			++checkNodes_Decal;

		}
	}

	void CheckNodes_CheckCells (List<string> contents)
	{
		checkNodes_Index = 3;

		foreach (string content in contents) {

			CheckNodes_CheckCell (content);

			++checkNodes_Index;

		}
	}

	void CheckNodes_CheckCell (string cellContent)
	{
			// check if empty
		if (cellContent.Length < 2)
			return;

			// check if node
		if ( cellContent[0] == '[' ) {
			return;
		}

			// check if choice
		if ( cellContent.Contains ("Choice") ) {
			return;
		}

			// CHECK FOR FUNCTION
		if (ContainedFunction (cellContent) == false) {
			CheckNodes_Error ("Cell doesn't contain function",cellContent);
			return;
		}

		// CHECK NODE
		if (cellContent.Contains ("Node") ) {

			string nodeName = cellContent.Remove (0, 6);

			if ( LinkedToNode (nodeName) == false ) {
				
				CheckNodes_Error ("There's a node function, but the node has no link",cellContent);

				return;


			}
		}

		if (cellContent.Contains ("ChangeStory") ) {

			// get second story name
			string storyName = cellContent.Remove (0, 13);
			storyName = storyName.Remove (storyName.IndexOf ('['));

			string[] nodes = cellContent.Remove (0, cellContent.IndexOf ('[') + 1).TrimEnd (']').Split ('/');

			if ( LinkedToNode (nodes[1]) == false ) {
				CheckNodes_Error ("the fallback node has no link",cellContent);
			}

			Story secondStory = StoryLoader.Instance.FindByName (storyName,StoryType.Island);
			if ( secondStory == null ) {
				CheckNodes_Error ("Story doesn't exist",cellContent);
				return;
			}

			if ( LinkedToNode (nodes[0],secondStory) == false ) {
				CheckNodes_Error ("the target node has no link",cellContent);
			}


		}
	
	}

	void CheckNodes_Error (string str,string content)
	{
		
		checkNodes_Incorrect = true;

		Debug.Log (storyToCheck.name);
		Debug.LogError (str);
		Debug.LogError ("CELL CONTENT : " + content);
		Debug.LogError ("ROW : " + alphabet [checkNodes_Decal] + " / COLL " + checkNodes_Index);
	}

	private bool LinkedToNode ( string nodeName ) {
		return LinkedToNode (nodeName, storyToCheck);
	}
	private bool LinkedToNode ( string nodeName , Story targetStory ) {

		nodeName = nodeName.TrimEnd ('\r', '\n');

		foreach (Node node in targetStory.nodes) {
			if (nodeName == node.name) {
				return true;
			}
		}

		return false;
	}

	private bool ContainedFunction ( string str ) {

		foreach (string functionName in storyFunctions.FunctionNames) {

			if (str.Contains (functionName)) {
				return true;
			}

		}

		return false;
	}
	#endregion

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
