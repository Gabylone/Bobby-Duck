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
	private float timer = 0f;

	private StoryManager currentStoryManager;

	private bool waitForInput = false;

	[SerializeField]
	private AudioClip pressInputButton;

	void Awake () {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
		StoryInput.onPressInput += HandleOnPressInput;
	}

	void HandleOnPressInput ()
	{
		NextCell ();
		UpdateStory ();
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.ChangeStory:
			ChangeStory ();
			break;
		case FunctionType.Node:
			Node ();
			break;
		case FunctionType.Switch:
			Switch ();
			break;
		}
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

		if (node.decal > 0) {
			StoryReader.Instance.SetDecal (node.decal);
		}

		StoryReader.Instance.UpdateStory ();

	}

	public void Switch () {

		string text = StoryFunctions.Instance.CellParams;

		int decal = 1;

		if ( text[text.Length-2] == '/' ) {

			decal = int.Parse (text.Remove(0,text.Length - 1));

			text = text.Remove (text.Length - 2);

		}

		string nodeName = text.Remove (0, 2);

		Node node = GetNodeFromText (nodeName);

		node.decal = decal;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();

	}

	public Node GetNodeFromText ( string text ) {
		return GetNodeFromText (CurrentStoryHandler.Story, text);
	}

	public Node GetNodeFromText ( Story story, string text ) {

		text = text.TrimEnd ('\r', '\n', '\t');

		Node node = story.nodes.Find ( x => x.name == text);

		if ( node == null ) {
			Debug.LogError ("couldn't find node " + text + " // story : " + story.name);
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

	public string ReadDecal (int decal) {
		return CurrentStoryHandler.Story.content
			[decal]
			[StoryReader.Instance.Index]; 

	}
	#endregion

	#region story layers
	public void ChangeStory () {

			// extract story informations
		string text = StoryFunctions.Instance.CellParams;

			// get story
		string storyName = text.Remove (0, 2);
		storyName = storyName.Remove (storyName.IndexOf ('['));

		// get second story
		Story secondStory = StoryLoader.Instance.FindByName (storyName,IslandType.Normal);

		// extract nodes
		string nodes = text.Remove (0,text.IndexOf ('[')+1);

		// get fallback nodes
		string fallbackNodeTXT = nodes.Split ('/') [1].TrimEnd(']');
		Node fallBackNode = GetNodeFromText (fallbackNodeTXT);

			// get target node
		string targetNodeTXT = nodes.Split ('/') [0];
		Node targetNode = GetNodeFromText (secondStory,targetNodeTXT);

		SetNewStory (secondStory, IslandType.Normal, targetNode, fallBackNode);
	}

	public void SetNewStory (Story story, IslandType storyType , Node targetNode , Node fallbackNode) {

		int secondStoryID = StoryLoader.Instance.FindIndexByName (story.name,storyType);

		int targetStoryLayer = CurrentStoryManager.storyHandlers.FindIndex (handler => (handler.storyID == secondStoryID) );
		if ( targetStoryLayer >= 0 ) {
			targetStoryLayer = CurrentStoryManager.storyHandlers.FindIndex (handler => (handler.decal == decal) && (handler.index == index));
		}

		// si la story n'apparait pas dans le truc
		if ( targetStoryLayer < 0 ) {
			
			StoryHandler newHandler = new StoryHandler ( secondStoryID,storyType);
			newHandler.fallBackLayer = CurrentStoryLayer;
			newHandler.decal = decal;
			newHandler.index = index;
			newHandler.fallbackNode = fallbackNode;

			CurrentStoryManager.AddStoryHandler (newHandler);

			targetStoryLayer = CurrentStoryManager.storyHandlers.Count - 1;
		}


		currentStoryLayer = targetStoryLayer;

		GoToNode (targetNode);
	}

	public void FallBackToPreviousStory () {



		Node fallbackNode = CurrentStoryHandler.fallbackNode;

		currentStoryLayer = CurrentStoryHandler.fallBackLayer;
		StoryReader.Instance.GoToNode (fallbackNode);
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
			if ( Decal >= CurrentStoryHandler.Story.content.Count ) {

				Debug.LogError ("DECAL is outside of story << " + CurrentStoryHandler.Story.name + " >> content : DECAL : " + Decal + " /// COUNT : " + CurrentStoryHandler.Story.content.Count);

				return CurrentStoryHandler.Story.content
					[0]
					[0];

			}

			if ( Index >= CurrentStoryHandler.Story.content [Decal].Count ) {

				Debug.LogError ("INDEX is outside of story content : INDEX : " + Index + " /// COUNT : " + CurrentStoryHandler.Story.content[Decal].Count);

				return CurrentStoryHandler.Story.content
					[Decal]
					[0]; 
			}

			return CurrentStoryHandler.Story.content
				[Decal]
				[Index];
		}
	}
	#endregion

	public int CurrentStoryLayer {
		get {
			return currentStoryLayer;
		}
	}

	public StoryManager CurrentStoryManager {
		get {
			return currentStoryManager;
		}
		set {
			currentStoryManager = value;
		}
	}

	public StoryHandler CurrentStoryHandler {
		get {
			return CurrentStoryManager.storyHandlers[CurrentStoryLayer];
		}
		set {
			CurrentStoryManager.storyHandlers[CurrentStoryLayer] = value;
		}
	}
}
