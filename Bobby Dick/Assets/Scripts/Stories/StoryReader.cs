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

	private StoryManager storyManager;

	private bool waitForInput = false;

	[SerializeField]
	private AudioClip pressInputButton;

	void Awake () {
		Instance = this;
	}

	void Update () {
		if ( waitToNextCell )
			WaitForNextCell_Update ();

		if ( waitForInput ) {
			if (InputManager.Instance.OnInputDown ()) {
				PressInput ();
			}
		}
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

		text = text.TrimEnd ('\r', '\n');

		Node node = CurrentStoryHandler.Story.nodes.Find ( x => x.name == text);

		if ( node == null ) {
			Debug.LogError ("couldn't find node " + text + " // story : " + CurrentStoryHandler.Story.name);
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

		string storyName = text.Remove (0, 2);
		storyName = storyName.Remove (storyName.IndexOf ('['));

		// get second story
		Story secondStory = StoryLoader.Instance.FindByName (storyName);

		if (secondStory == null) {
			Debug.LogError ("pas trouvé second story : " + storyName);
			StoryLauncher.Instance.PlayingStory = false;
			return;
		}

		// extract nodes
		string nodes = text.Remove (0,text.IndexOf ('[')+1);

		// set nodes
		string fallbackNodeTXT = nodes.Split ('/') [1].TrimEnd(']');
		Node fallBackNode = GetNodeFromText (fallbackNodeTXT);


		int decal = StoryReader.Instance.Decal;
		int index = StoryReader.Instance.Index;

		int secondStoryID = StoryLoader.Instance.FindIndexByName (secondStory.name);

		int targetStoryLayer = StoryManager.storyHandlers.FindIndex (handler => (handler.storyID == secondStoryID) );

		// si le story ID apparrait déjà dans la liste handler
		if ( targetStoryLayer < 0 ) {
			// vérifier s'il est à sa bonne position

			targetStoryLayer = StoryManager.storyHandlers.FindIndex (handler => (handler.decal == decal) && (handler.index == index));

			if ( targetStoryLayer < 0 ) {
				

				StoryHandler newHandler = new StoryHandler ( secondStoryID,StoryType.Island);
				newHandler.fallBackLayer = CurrentStoryLayer;
				newHandler.decal = decal;
				newHandler.index = index;

				StoryManager.AddStory (newHandler);

				targetStoryLayer = StoryManager.storyHandlers.Count - 1;

				// fall back nodes
				CurrentStoryHandler.fallbackNode = fallBackNode;
			}
		}

		currentStoryLayer = targetStoryLayer;

			// go to next node
		string targetNode = nodes.Split ('/') [0];
		Node node = GetNodeFromText (targetNode);
		GoToNode (node);
	}

	public void FallBackToPreviousStory () {
		
		currentStoryLayer = CurrentStoryHandler.fallBackLayer;
		StoryReader.Instance.GoToNode (CurrentStoryHandler.fallbackNode);
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
	#region input & wait
	public void WaitForInput () {
		waitForInput = true;
	}
	public void PressInput () {

		SoundManager.Instance.PlaySound (pressInputButton);

		DialogueManager.Instance.HideNarrator ();
		DialogueManager.Instance.EndDialogue ();

		waitForInput = false;

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

	public StoryManager StoryManager {
		get {
			return storyManager;
		}
		set {
			storyManager = value;
		}
	}

	public StoryHandler CurrentStoryHandler {
		get {
			return StoryManager.storyHandlers[CurrentStoryLayer];
		}
		set {
			StoryManager.storyHandlers[CurrentStoryLayer] = value;
		}
	}
}
