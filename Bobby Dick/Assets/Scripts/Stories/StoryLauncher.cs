using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLauncher : MonoBehaviour {

	public static StoryLauncher Instance;

	private bool playingStory = false;

	public delegate void PlayStoryEvent ();
	public PlayStoryEvent playStoryEvent;
	public delegate void EndStoryEvent ();
	public EndStoryEvent endStoryEvent;

	public enum StorySource {

		none,

		island,
		boat,
	}

	private StorySource currentStorySource;

	void Awake() {
		Instance = this;
	}

	void Start () {
		StoryFunctions.Instance.getFunction += HandleGetFunction;
	}

	void HandleGetFunction (FunctionType func, string cellParameters)
	{
		switch (func) {
		case FunctionType.Leave:
			EndStory ();
			break;

		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.PageDown)) {
			print ("quittage de force");
			EndStory ();
		}
	}

	#region propeties
	public void PlayStory ( StoryManager storyManager , StoryLauncher.StorySource source) {

		if (playingStory)
			return;

		StoryReader.Instance.CurrentStoryManager = storyManager;

		CurrentStorySource = source;

		playingStory = true;

		Transitions.Instance.ActionTransition.Fade = true;

		// place captain
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Discussion, 0.2f);

		MapImage.Instance.CloseMap ();

		StoryReader.Instance.Reset ();
		StoryReader.Instance.UpdateStory ();

		if (playStoryEvent != null)
			playStoryEvent ();
	}

	public void EndStory () {
		if ( StoryReader.Instance.CurrentStoryLayer > 0 ) {
			StoryReader.Instance.FallBackToPreviousStory ();
			return;
		}

		playingStory = false;

		Transitions.Instance.ActionTransition.Fade = false;

		// place captain
		Crews.playerCrew.captain.Icon.MoveToPoint (Crews.PlacingType.Map, 0.2f);

		Crews.enemyCrew.Hide ();
		switch (CurrentStorySource) {
		case StorySource.none:
			// kek
			break;
		case StorySource.island:
			MapGenerator.Instance.CurrentChunk.State = ChunkState.VisitedIsland;
			break;
		case StorySource.boat:
			Boats.Instance.OtherBoat.Leave ();
			break;
		default:
			break;
		}

		if (endStoryEvent != null)
			endStoryEvent ();
	}

	public bool PlayingStory {
		get {
			return playingStory;
		}
	}
	#endregion

	public StorySource CurrentStorySource {
		get {
			return currentStorySource;
		}
		set {
			currentStorySource = value;
		}
	}
}