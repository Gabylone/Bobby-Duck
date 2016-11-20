using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	private int currentClue = 0;
	private int clueAmount = 2;

	private int treasureIslandX = 0;
	private int treasureIslandY = 0;

	private int[] clue_XPos;
	private int[] clue_YPos;

	private int[] clueIslands;
	private string[] clues = new string[2] {
		"Bonjour",
		"Connard"
	};

	[SerializeField]
	private GameObject clueBubble;

	[SerializeField]
	private InputField inputField;

	void Awake () {
		Instance = this;
	}

	void Start () {
		clueIslands = new int[clues.Length];
		for (int i = 0; i < clueIslands.Length; ++i)
			clueIslands [i] = -1;
	}

	public int ClueAmount {
		get {
			return clueAmount;
		}
		set {
			clueAmount = value;
		}
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

	public int[] Clue_XPos {
		get {
			return clue_XPos;
		}
		set {
			clue_XPos = value;
		}
	}

	public int[] Clue_YPos {
		get {
			return clue_YPos;
		}
		set {
			clue_YPos = value;
		}
	}

	public int TreasureIslandX {
		get {
			return treasureIslandX;
		}
		set {
			treasureIslandX = value;
		}
	}

	public int TreasureIslandY {
		get {
			return treasureIslandY;
		}
		set {
			treasureIslandY = value;
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

	public int CurrentClue {
		get {
			return currentClue;
		}
		set {
			currentClue = Mathf.Clamp (value, 0, clues.Length);
		}
	}
}
