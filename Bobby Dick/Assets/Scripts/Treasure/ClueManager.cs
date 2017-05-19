using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	private int clueIndex = 0;
	private int clueAmount = 2;

	private Chunk[] clueChunks;

	private string[] clues = new string[2] {
		"formule1",
		"formule2"
	};

	[SerializeField]
	private GameObject clueBubble;

	[SerializeField]
	private InputField inputField;

	void Awake () {
		Instance = this;
	}

	public void Init () {
		// randomize clues
		for (int i = 0; i < clues.Length; ++i )
			clues[i] = NameGeneration.Instance.randomWord.ToUpper ();

		clueChunks = new Chunk[clues.Length];
		
	}

	public void StartClue () {
		clueBubble.SetActive (true);
		StoryReader.Instance.NextCell ();
	}

	public void CheckClue () {

		string stringToCheck = inputField.text;

		inputField.text = "";
		clueBubble.SetActive (false);

		int index = 0;

		foreach ( string clue in clues ) {
			
			if ( clue == stringToCheck ) {
				DeleteClue (index);
				return;
			}

			++index;
		}

		StoryReader.Instance.UpdateStory ();
	}

	private void DeleteClue ( int index ) {

		string [] newClues = new string[clues.Length-1];

		int a = 0;
		for (int i = 0; i < clues.Length; ++i ) {
			if (i != index) {
				newClues [a] = clues [i];
				++a;
			}
		}

		clues = newClues;

		if ( clues.Length == 0 ) {
			StoryReader.Instance.SetDecal (2);
			StoryReader.Instance.UpdateStory ();
		} else {
			StoryReader.Instance.SetDecal (1);
			StoryReader.Instance.UpdateStory ();
		}
	}

	public string getDirectionToFormula () {
		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		return directionPhrase;
	}

	public string getFormula () {

		for (int index = ClueIndex; clueIndex < Clues.Length; clueIndex++) {
			
			if ( MapData.Instance.currentChunk == clueChunks[index] ) {
				
				Debug.Log ("already found clue in island");

				return Clues [index];
			}
		}

		Debug.Log ("first time gave clue");

		// set clue phrase
		string clue = Clues[clueIndex];

		// set clue island
		clueChunks [ClueIndex] = MapData.Instance.currentChunk;

		// go to next clue
		++ClueIndex;

		return clue;
	}

	public Vector2 GetNextClueIslandPos {
		get {
			if ( clueIndex == clues.Length ) 
				return new Vector2 ( MapData.Instance.treasureIslandXPos , MapData.Instance.treasureIslandYPos );

			return new Vector2 ( MapData.Instance.clueIslandsXPos[ClueIndex] , MapData.Instance.clueIslandsYPos[ClueIndex] );
		}
	}

	public string[] Clues {
		get {
			return clues;
		}
	}

	public int ClueIndex {
		get {
			return clueIndex;
		}
		set {
			clueIndex = Mathf.Clamp (value, 0, clues.Length);
		}
	}

	public int ClueAmount {
		get {
			return clueAmount;
		}
		set {
			clueAmount = value;
		}
	}
}
