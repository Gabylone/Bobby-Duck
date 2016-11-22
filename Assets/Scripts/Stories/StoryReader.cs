using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

	private Story currentStory;

	private int index = 0;
	private int decal = 0;

	bool waitForInput = false;
	bool waitToNextCell = false;
	float timer = 0f;

	bool fallBackStory = false;
	int fallBackCoordX = 0;
	int fallBackCoordY = 0;

	[SerializeField]
	private AudioClip pressInputButton;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if ( waitToNextCell ) {
			timer -= Time.deltaTime;

			if (timer <= 0) {

				waitToNextCell = false;
				UpdateStory ();
			}
		}

	}

	#region wait for input
	[SerializeField]
	private GameObject inputButton;

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
	#endregion

	public void Wait ( float duration ) {
		waitToNextCell = true;
		timer = duration;
	}

	#region navigation
	public void SetStory ( Story newStory ) {

		currentStory = newStory;

		index = 0;
		decal = 0;
	}
	public void UpdateStory () {

		if ( StoryLoader.Instance.GetContent == null) {
			Debug.LogError ( " no function at index : " + index.ToString () + " / decal : " + decal.ToString () );
		}

		StoryFunctions.Instance.Read ( StoryLoader.Instance.GetContent );

	}
	public void NextCell () {
		++index;
	}
	#endregion

	#region decal
	public void SetDecal ( int steps ) {

		while (steps > 0) {

			++decal;
			string content = StoryLoader.Instance.GetContent;

			if (content.Length > 0) {
				--steps;
			}
		}

	}
	#endregion

	#region properties
	public Story CurrentStory {
		get {
			return currentStory;
		}
		set {
			currentStory = value;
		}
	}

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
	#endregion

	#region fall back story
	public bool FallBackStory {
		get {
			return fallBackStory;
		}
		set {
			fallBackStory = value;
		}
	}

	public int FallBackCoordX {
		get {
			return fallBackCoordX;
		}
		set {
			fallBackCoordX = value;
		}
	}

	public int FallBackCoordY {
		get {
			return fallBackCoordY;
		}
		set {
			fallBackCoordY = value;
		}
	}
	#endregion
}
