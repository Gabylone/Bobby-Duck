using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

		// the coords of the story being read.
	private int index = 0;
	private int decal = 0;

	private int currentStoryLayer = 0;
	private int previousStoryLayer = 0;

	private bool waitToNextCell = false;
	float timer = 0f;

	void Awake () {
		Instance = this;
	}

	void Update () {
		if ( waitToNextCell )
			WaitForNextCell_Update ();
	}


	#region story flow
	public void Reset () {
		index = 0;
		decal = 0;
	}
	#endregion

	#region navigation
	public void NextCell () {
		++index;
	}
	public void UpdateStory () {

		string content = GetContent;

		if ( content == null) {
			Debug.LogError ( " no function at index : " + index.ToString () + " / decal : " + decal.ToString () );
		}

		StoryFunctions.Instance.Read ( content );

	}
	#endregion

	#region node & switch
	public void Node () {

		string text = StoryFunctions.Instance.CellParams;
		string nodeName = text.Remove (0, 2);
		Node node = GetNodeFromText (nodeName);
		GoToNode (node);

	}

	public void GoToNode (Node node) {

		StoryReader.Instance.Decal = node.x;
		StoryReader.Instance.Index = node.y;

		StoryReader.Instance.NextCell ();

		if (node.switched) {
			StoryReader.Instance.SetDecal (1);
		}

		StoryReader.Instance.UpdateStory ();

	}

	public void Switch () {

		string text = StoryFunctions.Instance.CellParams;

		string nodeName = text.Remove (0, 2);

		Node node = GetNodeFromText (nodeName);

		node.switched = true;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();

	}

	public Node GetNodeFromText ( string text ) {
		Node node = CurrentStory.nodes.Find ( x => x.name == text);

		if ( node == null ) {
			Debug.LogError ("couldn't find node " + text + " // story : " + CurrentStory.name);
			return null;
		}
		return node;
	}
	#endregion

	#region decal
	public void SetDecal ( int steps ) {

		while (steps > 0) {

			++decal;
			string content = GetContent;

			if (content.Length > 0) {
				--steps;
			}
		}

	}

	public int SaveDecal {
		get {
			return MapData.Instance.currentChunk.IslandData.Story.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index];
		}
		set {
			MapData.Instance.currentChunk.IslandData.Story.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index] = value;
		}
	}

	public string ReadDecal (int decal) {

		return MapData.Instance.currentChunk.IslandData.Story.content
			[decal]
			[StoryReader.Instance.Index]; 

	}
	#endregion

	#region story layers
	public void ChangeStory () {

		string text = StoryFunctions.Instance.CellParams;

		string storyName = text.Remove (0, 2);
		storyName = storyName.Remove (storyName.IndexOf ('['));

		// extract nodes
		string nodes = text.Remove (0,text.IndexOf ('[')+1);

		// set nodes
		string targetNode = nodes.Split ('/') [0];
		string fallbackNode = nodes.Split ('/') [1].TrimEnd(']');

		// get second story
		Story secondStory = StoryLoader.Instance.Stories.Find ( x => x.name == storyName);

		if (secondStory == null) {
			Debug.LogError ("pas trouvé second story : " + storyName);
			IslandManager.Instance.Leave ();
			return;
		}

		int storyIndex = CurrentStories.FindIndex (x => x.name == secondStory.name);

			// is the story already in the island ?
		if (storyIndex < 0 ) {
			CurrentStories.Add (secondStory);
			storyIndex = CurrentStories.Count - 1;
			secondStory.fallbackStoryName = CurrentStory.name;
			secondStory.fallbackNode = fallbackNode;
		}

		currentStoryLayer = storyIndex;

		Node node = GetNodeFromText (targetNode);
		GoToNode (node);
	}

	public void FallBackToPreviousStory () {
		string fallbackNode = MapData.Instance.currentChunk.IslandData.Story.fallbackNode;

		currentStoryLayer = MapData.Instance.currentChunk.IslandData.Stories.FindIndex (x => x.name == MapData.Instance.currentChunk.IslandData.Story.fallbackStoryName);
		if (currentStoryLayer < 0) {
			Debug.LogError ("pas trouvé de fall back story");
			currentStoryLayer = 0;
			IslandManager.Instance.Leave ();
			return;
		}

		Node node = StoryReader.Instance.GetNodeFromText (fallbackNode);
		StoryReader.Instance.GoToNode (node);
	}
	#endregion

	public void Wait ( float duration ) {
		waitToNextCell = true;
		timer = duration;
	}

	private void WaitForNextCell_Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {

			waitToNextCell = false;
			StoryReader.Instance.UpdateStory ();
		}
	}
	// ce truc n'a rien à foutre ici mais il y est
	[Header("Input")]
	[SerializeField]
	private GameObject inputButton;

	private bool waitForInput = false;

	[SerializeField]
	private AudioClip pressInputButton;
	#region input & wait
	public void WaitForInput () {
		waitForInput = true;
		inputButton.SetActive (true);
	}
	public void PressInput () {

		SoundManager.Instance.PlaySound (pressInputButton);

		DialogueManager.Instance.HideNarrator ();

		waitForInput = false;
		inputButton.SetActive (false);

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}
	#endregion

	#region properties
	public int Index {
		get {
			return index;
		}
		set {
			index = value;
		}
	}

	public int Decal {
		get {
			return decal;
		}
		set {
			decal = value;
		}
	}



	public string GetContent {
		get {
			if ( Decal >= CurrentStory.content.Count ) {

				Debug.LogError ("DECAL is outside of story << " + CurrentStory.name + " >> content : DECAL : " + Decal + " /// COUNT : " + CurrentStory.content.Count);

				return CurrentStory.content
					[0]
					[0];

			}

			if ( Index >= CurrentStory.content [Decal].Count ) {

				Debug.LogError ("INDEX is outside of story content : INDEX : " + Index + " /// COUNT : " + CurrentStory.content[Decal].Count);

				return CurrentStory.content
					[Decal]
					[0]; 
			}

			return CurrentStory.content
				[Decal]
				[Index];
		}
	}
	#endregion

	private Story CurrentStory {
		get {

			return MapData.Instance.currentChunk.IslandData.Story;
		}
	}

	private List<Story> CurrentStories {
		get {
			return MapData.Instance.currentChunk.IslandData.Stories;
		}
	}


	public int CurrentStoryLayer {
		get {
			return currentStoryLayer;
		}
	}
}
