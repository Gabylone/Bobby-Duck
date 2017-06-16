using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	private int clueIndex = 0;
	private int clueAmount = 2;

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

	}

	public void StartClue () {
		clueBubble.SetActive (true);
		StoryReader.Instance.NextCell ();
	}

	public void CheckClue () {

		string stringToCheck = inputField.text;

		stringToCheck = stringToCheck.ToLower ();

		inputField.text = "";
		clueBubble.SetActive (false);

		int index = 0;

		foreach ( string clue in clues ) {
			
			if ( clue.ToLower() == stringToCheck ) {
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

		if ( MapData.Instance.currentChunk.IslandData.gaveClue == true) {
			return Clues [Random.Range (0,ClueIndex)];
		}

		MapData.Instance.currentChunk.IslandData.gaveClue = true;
		++ClueIndex;
		return Clues[clueIndex];
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
