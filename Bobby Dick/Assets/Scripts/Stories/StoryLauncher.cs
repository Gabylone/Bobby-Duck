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

	void Awake() {
		Instance = this;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.PageDown)) {
			print ("quittage de force");
			PlayingStory = false;
		}
	}

	#region propeties
	public void PlayStory (StoryManager storyManager , StoryLauncher.StorySource source) {
		StoryReader.Instance.CurrentStoryManager = storyManager;
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

				return;
			}

			playingStory = value;

			NavigationManager.Instance.NavigationTriggers.SetActive (!playingStory);

			Transitions.Instance.ActionTransition.Fade = playingStory;

			PlayerLoot.Instance.CanOpen = !value;

			// place captain
			Crews.PlacingType pT = playingStory ? Crews.PlacingType.Discussion : Crews.PlacingType.Map;
			Crews.playerCrew.captain.Icon.MoveToPoint (pT, 0.2f);

			// lower volume
			SoundManager.Instance.AmbianceSource.volume = playingStory ? SoundManager.Instance.AmbianceSource.volume / 2 : SoundManager.Instance.AmbianceSource.volume * 2;

			WeatherManager.Instance.PlaySound ();

			MapImage.Instance.CloseMap ();

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
					MapData.Instance.currentChunk.State = ChunkState.VisitedIsland;
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