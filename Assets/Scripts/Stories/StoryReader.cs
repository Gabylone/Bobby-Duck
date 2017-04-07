using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryReader : MonoBehaviour {

	public static StoryReader Instance;

		// the story being read.
	private int index = 0;
	private int decal = 0;

	[Header("Input")]
	[SerializeField]
	private GameObject inputButton;

	private bool waitForInput = false;
	private bool waitToNextCell = false;
	float timer = 0f;

	[SerializeField]
	private AudioClip pressInputButton;



	void Awake () {
		Instance = this;
	}
	
	void Update () {
		if ( waitToNextCell )
			WaitForNextCell_Update ();
	}

	#region navigation
	public void Reset () {
		index = 0;
		decal = 0;
	}
	public void UpdateStory () {

		string content = GetContent;

		if ( content == null) {
			Debug.LogError ( " no function at index : " + index.ToString () + " / decal : " + decal.ToString () );
		}

		StoryFunctions.Instance.Read ( content );

	}
	public void NextCell () {
		++index;
	}

	public string GetContent {
		get {
			Story targetStory = IslandManager.Instance.CurrentIsland.Story;

			if ( Decal >= targetStory.content.Count ) {

				Debug.LogError ("DECAL is outside of story << " + targetStory.name + " >> content : DECAL : " + Decal + " /// COUNT : " + targetStory.content.Count);

				return targetStory.content
					[0]
					[0];

			}

			if ( Index >= targetStory.content [Decal].Count ) {

				Debug.LogError ("INDEX is outside of story content : INDEX : " + Index + " /// COUNT : " + targetStory.content[Decal].Count);

				return targetStory.content
					[Decal]
					[0]; 
			}

			return targetStory.content
				[Decal]
				[Index];
		}
	}
	#endregion

	#region decal
	public void SetDecal ( int steps ) {

		while (steps > 0) {

			++decal;
			string content = GetContent;

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

	#region input
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
