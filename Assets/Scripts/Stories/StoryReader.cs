using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

		// the story being read.
	private Story currentStory;

	private int index = 0;
	private int decal = 0;

	bool waitForInput = false;
	bool waitToNextCell = false;
	float timer = 0f;

	[SerializeField]
	private GameObject inputButton;

	[SerializeField]
	private AudioClip pressInputButton;

	void Awake () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if ( waitToNextCell )
			WaitForNextCell_Update ();
	}

	#region wait for input
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
	#endregion
}
