using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiscussionManager : MonoBehaviour {

	public static DiscussionManager Instance;

	[SerializeField]
	private GameObject[] choiceButtons;

	[SerializeField]
	private Color[] statColor;

	void Awake () {
		Instance = this;
	}

	public void SetChoices (int amount, string[] content) {

		for (int i = 0; i < amount ; ++i ) {
			choiceButtons[i].SetActive (true);
			choiceButtons [i].GetComponentInChildren<Text> ().text = content [i];
		}
	}

	public void ResetColors () {
		foreach ( GameObject buttonObj in choiceButtons )
			buttonObj.GetComponentInChildren<Image> ().color = Color.white;
	}

	public void TaintChoice (int buttonIndex , int statIndex) {

		choiceButtons [buttonIndex].GetComponentInChildren<Image> ().color = statColor [statIndex];

	}

	public void Choose (int i) {

		StoryReader.Instance.SetDecal (i);

		foreach ( GameObject button in choiceButtons ) {
			button.SetActive (false);
		}

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}


	public GameObject[] ChoiceButtons {
		get {
			return choiceButtons;
		}
	}
}
