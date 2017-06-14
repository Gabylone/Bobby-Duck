using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLauncher : MonoBehaviour {

	public static StoryLauncher Instance;

	private bool playingStory = false;

	public enum StorySource {

		none,

		island,
		boat,
	}

	private StorySource currentStorySource;

	[SerializeField]
	private UIButton mapButton;

	void Awake() {
		Instance = this;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			print ("quittage de force");
			PlayingStory = false;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			print ("quittage de force");
			mapButton.Opened = false;
		}
	}

	#region propeties
	public void PlayStory (StoryManager storyHandlers , StoryLauncher.StorySource source) {
		StoryReader.Instance.StoryManager = storyHandlers;
		CurrentStorySource = source;
		PlayingStory = true;
	}

	public bool PlayingStory {
		get {
			return playingStory;
		}
		set {

			if (playingStory == value)
				return;

			if ( StoryReader.Instance.CurrentStoryLayer > 0 && value == false) {
				StoryReader.Instance.FallBackToPreviousStory ();
				Crews.enemyCrew.Hide ();
				return;
			}

			playingStory = value;

			NavigationManager.Instance.NavigationTriggers.SetActive (!playingStory);

			Transitions.Instance.ActionTransition.Fade = playingStory;

			// place captain
			Crews.PlacingType pT = playingStory ? Crews.PlacingType.Discussion : Crews.PlacingType.Map;
			Crews.playerCrew.captain.Icon.MoveToPoint (pT, Transitions.Instance.ActionTransition.Duration);

			// lower volume
			SoundManager.Instance.AmbianceSource.volume = playingStory ? SoundManager.Instance.AmbianceSource.volume / 2 : SoundManager.Instance.AmbianceSource.volume * 2;

			WeatherManager.Instance.PlaySound ();


			mapButton.Opened = false;
			mapButton.Locked = value;
//			MapImage.Instance.Opened = false;


			if (value == true) {
				// set story
				StoryReader.Instance.Reset ();
				StoryReader.Instance.UpdateStory ();
			} else {
				Crews.enemyCrew.Hide ();
				switch (CurrentStorySource) {
				case StorySource.none:
					// kek
					break;
				case StorySource.island:
					MapData.Instance.currentChunk.state = State.VisitedIsland;
					break;
				case StorySource.boat:
					Boats.Instance.OtherBoat.Leave ();
					break;
				default:
					break;
				}


			}

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