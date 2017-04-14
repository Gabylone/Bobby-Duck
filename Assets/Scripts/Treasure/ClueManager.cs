using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	private int clueIndex = 0;
	private int clueAmount = 2;

	private int[] clueIslands;
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

		clueIslands = new int[clues.Length];

		for (int i = 0; i < clueIslands.Length; ++i)
			clueIslands [i] = -1;
		
	}

	#region clues
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

	public void GiveClue() {

		string formula = getFormula ();

		Debug.Log (formula);

		if ( Crews.enemyCrew.CrewMembers.Count == 0 ) {
			DialogueManager.Instance.ShowNarrator (formula);
		} else {
			Crews.enemyCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion);
			DialogueManager.Instance.SetDialogue (formula, Crews.enemyCrew.captain);

		}

		StoryReader.Instance.WaitForInput ();

	}
	public void GiveDirectionToClue () {

		Directions dir = NavigationManager.Instance.getDirectionToPoint (ClueManager.Instance.GetNextClueIslandPos);
		string directionPhrase = NavigationManager.Instance.getDirName (dir);

		if ( StoryFunctions.Instance.CellParams.Length == 0 ) {
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.enemyCrew.captain);
		} else {
			DialogueManager.Instance.SetDialogue (directionPhrase, Crews.playerCrew.captain);
		}

		StoryReader.Instance.WaitForInput ();
	}

	string getFormula () {

		int clueIndex = ClueManager.Instance.ClueIndex;

		string clue = "";

		bool clueAlreadyFound = false;

		int a = 0;

		foreach ( int i in ClueManager.Instance.ClueIslands ) {

			if ( i == MapManager.Instance.IslandID ) {
				Debug.Log ("already found clue in island");
				clue = ClueManager.Instance.Clues [a];
				clueIndex = a;
				clueAlreadyFound = true;
			}

			++a;

		}

		if ( clueAlreadyFound == false ) {
			Debug.Log ("first time gave clue");
			clue = ClueManager.Instance.Clues[clueIndex];
			ClueManager.Instance.ClueIndex += 1;
		}

		ClueManager.Instance.ClueIslands [clueIndex] = MapManager.Instance.IslandID;

		return clue;
	}

	public Vector2 GetNextClueIslandPos {
		get {
			if ( clueIndex == clues.Length ) 
				return new Vector2 ( IslandManager.Instance.TreasureIslandXPos , IslandManager.Instance.TreasureIslandYPos );

			return new Vector2 ( IslandManager.Instance.ClueIslandsXPos[ClueIndex] , IslandManager.Instance.ClueIslandsYPos[ClueIndex] );
		}
	}

	public string[] Clues {
		get {
			return clues;
		}
	}

	public int[] ClueIslands {
		get {
			return clueIslands;
		}
		set {
			clueIslands = value;
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
