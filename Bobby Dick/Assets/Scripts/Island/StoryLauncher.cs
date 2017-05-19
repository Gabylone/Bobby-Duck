using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryLauncher : MonoBehaviour {

	public static StoryLauncher Instance;

	private bool playingStory = false;

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

			if ( StoryReader.Instance.CurrentStoryLayer > 0 ) {
				StoryReader.Instance.FallBackToPreviousStory ();
				return;
			}

			playingStory = value;

			NavigationManager.Instance.NavigationTriggers.SetActive (!playingStory);

			Transitions.Instance.ActionTransition.Fade = playingStory;

			// place captain
			Crews.PlacingType pT = playingStory ? Crews.PlacingType.Discussion : Crews.PlacingType.Map;
			Crews.playerCrew.captain.Icon.MoveToPoint (pT, Transitions.Instance.ActionTransition.Duration);

			// set story
			StoryReader.Instance.Reset ();
			StoryReader.Instance.UpdateStory ();

			// lower volume
			SoundManager.Instance.AmbianceSource.volume = playingStory ? SoundManager.Instance.AmbianceSource.volume / 2 : SoundManager.Instance.AmbianceSource.volume * 2;

			WeatherManager.Instance.PlaySound ();

			mapButton.Locked = playingStory;
			mapButton.Opened = false;

			if (playingStory == false) {
				Crews.enemyCrew.Hide ();
				MapData.Instance.currentChunk.state = State.VisitedIsland;
			}

		}
	}
	#endregion
}