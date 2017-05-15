using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

		// the story being read.
	private int index = 0;
	private int decal = 0;

	[Header("Input")]
	[SerializeField]
	private GameObject inputButton;

	private bool waitForInput = false;
	private bool waitToNextCell = false;
	float timer = 0f;

	[SerializeField]
	private AudioClip pressInputButton;



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
	public void UpdateStory () {

		string content = GetContent;

		if ( content == null) {
			Debug.LogError ( " no function at index : " + index.ToString () + " / decal : " + decal.ToString () );
		}

		StoryFunctions.Instance.Read ( content );

	}
	public void NextCell () {
		++index;
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
		Node node = IslandManager.Instance.CurrentIsland.Story.nodes.Find ( x => x.name == text);

		if ( node == null ) {
			Debug.LogError ("couldn't find node " + text + " // story : " + IslandManager.Instance.CurrentIsland.Story.name);
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
			return IslandManager.Instance.CurrentIsland.Story.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index];
		}
		set {
			IslandManager.Instance.CurrentIsland.Story.contentDecal [StoryReader.Instance.Decal] [StoryReader.Instance.Index] = value;
		}
	}

	public string ReadDecal (int decal) {

		return IslandManager.Instance.CurrentIsland.Story.content
			[decal]
			[StoryReader.Instance.Index]; 

	}
	#endregion

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

		NextCell ();
		UpdateStory ();
	}

	public void Wait ( float duration ) {
		waitToNextCell = true;
		timer = duration;
	}

	private void WaitForNextCell_Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {

			waitToNextCell = false;
			UpdateStory ();
		}
	}
	#endregion

	#region story management
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

		int storyIndex = IslandManager.Instance.CurrentIsland.Stories.FindIndex (x => x.name == secondStory.name);

			// is the story already in the island ?
		if (storyIndex < 0 ) {
			IslandManager.Instance.CurrentIsland.Stories.Add (secondStory);
			storyIndex = IslandManager.Instance.CurrentIsland.Stories.Count - 1;
			secondStory.fallbackStory = IslandManager.Instance.CurrentIsland.Story.name;
			secondStory.fallbackNode = fallbackNode;
		}

		IslandManager.Instance.StoryLayer = storyIndex;

		Node node = GetNodeFromText (targetNode);
		GoToNode (node);
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
			Story targetStory = IslandManager.Instance.CurrentIsland.Story;

			if ( Decal >= targetStory.content.Count ) {

				Debug.LogError ("DECAL is outside of story << " + targetStory.name + " >> content : DECAL : " + Decal + " /// COUNT : " + targetStory.content.Count);

				return targetStory.content
					[0]
					[0];

			}

			if ( Index >= targetStory.content [Decal].Count ) {

				Debug.LogError ("INDEX is outside of story content : INDEX : " + Index + " /// COUNT : " + targetStory.content[Decal].Count);

				return targetStory.content
					[Decal]
					[0]; 
			}

			return targetStory.content
				[Decal]
				[Index];
		}
	}
	#endregion


}
