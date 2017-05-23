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
	}

	#region propeties
	public bool PlayingStory {
		get {
			return playingStory;
		}
		set {

			if (playingStory == value)
				return;

			if (value == true) {
				// set story
				StoryReader.Instance.Reset ();
				StoryReader.Instance.UpdateStory ();
			} else {

				if ( StoryReader.Instance.CurrentStoryLayer > 0 ) {
					StoryReader.Instance.FallBackToPreviousStory ();
					return;
				}

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

				Crews.enemyCrew.Hide ();
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

			mapButton.Locked = playingStory;
			mapButton.Opened = false;



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