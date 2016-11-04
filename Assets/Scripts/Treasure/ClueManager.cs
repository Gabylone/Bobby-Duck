using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueManager : MonoBehaviour {

	public static ClueManager Instance;

	private int clueAmount = 2;

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
}
