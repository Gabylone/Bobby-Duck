using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiscussionManager : MonoBehaviour {

	public static DiscussionManager Instance;

	[SerializeField]
	private GameObject[] choiceButtons;

	void Awake () {
		Instance = this;
	}

	public void SetChoices (int amount, string[] content) {

		for (int i = 0; i < amount ; ++i ) {
			choiceButtons[i].SetActive (true);
			choiceButtons [i].GetComponentInChildren<Text> ().text = content [i];
		}
	}

	public void Choose (int i) {

		StoryReader.Instance.SetDecal (i);

		foreach ( GameObject button in choiceButtons ) {
			button.SetActive (false);
		}

		Debug.Log ("next in choose");
		
		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}


}
