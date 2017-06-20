using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugMessage : MonoBehaviour {

	public static DebugMessage Instance;

	public GameObject debugObject;

	public Text text;

	void Awake () {
		Instance = this;
	}

	public void Open (string message) {
		text.text = message;	
		Debug.LogError (message);
	}

	public void Close () {
		debugObject.SetActive (false);
		StoryLauncher.Instance.PlayingStory = false;
	}
}
